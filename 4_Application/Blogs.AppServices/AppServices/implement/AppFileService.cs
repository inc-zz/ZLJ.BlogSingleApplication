﻿using Blogs.AppServices.AppServices.Interface;
using Blogs.Core;
using Blogs.Core.Config;
using Blogs.Domain.Entity.Blogs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Blogs.AppServices.AppServices.implement
{
    /// <summary>
    /// 
    /// </summary>
    public class AppFileService: IAppFileService
    {
        private readonly SqlSugarDbContext _db;
        private readonly IWebHostEnvironment _environment;
        private readonly SemaphoreSlim _semaphore;

        /// <summary>
        /// 文件上传配置
        /// </summary>
        /// <param name="db"></param>
        /// <param name="environment"></param>
        public AppFileService( IWebHostEnvironment environment)
        {
            _db = new SqlSugarDbContext();
            _environment = environment; 
            _semaphore = new SemaphoreSlim(AppConfig.Instance.FileStoreConfig.MaxConcurrentUploads);
        }

        public async Task<FileUploadResult> UploadSingleFileAsync(IFormFile file, string businessType, string? description = null, string? userId = null)
        {
            // 并发控制
            await _semaphore.WaitAsync();
            try
            {
                // 验证文件
                var validationResult = ValidateFile(file, businessType);
                if (!validationResult.Success)
                {
                    return validationResult;
                }

                // 获取业务目录配置
                var directoryConfig = AppConfig.Instance.FileStoreConfig.DirectoryConfig.FirstOrDefault(d => d.BusinessType == businessType);
                if (directoryConfig == null)
                {
                    return new FileUploadResult { Success = false, Message = $"未找到业务类型 '{businessType}' 的目录配置" };
                }

                // 确保目录存在
                EnsureDirectoryExists(directoryConfig.PhysicalPath);

                // 生成存储文件名
                var storedFileName = GenerateStoredFileName(file.FileName);
                var filePath = Path.Combine(directoryConfig.PhysicalPath, storedFileName);
                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

                // 计算文件MD5（针对大文件使用流式处理）
                string md5Hash;
                using (var stream = file.OpenReadStream())
                {
                    md5Hash = await ComputeMD5HashAsync(stream);
                }

                // 检查文件是否已存在（秒传功能）
                var existingFile = await _db.DbContext.Queryable<BlogsFileRecord>()
                    .Where(f => f.Md5Hash == md5Hash && f.BusinessType == businessType)
                    .FirstAsync();

                if (existingFile != null)
                {
                    return new FileUploadResult
                    {
                        Success = true,
                        Message = "文件已存在",
                        FileRecord = existingFile,
                        FileUrl = $"{directoryConfig.UrlPath}/{existingFile.StoredFileName}"
                    };
                }

                // 保存文件（区分大文件和小文件）
                if (file.Length > AppConfig.Instance.FileStoreConfig.LargeFileThreshold)
                {
                    await SaveLargeFileAsync(file, filePath);
                }
                else
                {
                    await SaveSmallFileAsync(file, filePath);
                }

                // 创建文件记录
                var fileRecord = new BlogsFileRecord
                {
                    OriginalFileName = file.FileName,
                    StoredFileName = storedFileName,
                    FilePath = filePath,
                    FileSize = file.Length,
                    ContentType = file.ContentType,
                    FileExtension = fileExtension,
                    BusinessType = businessType,
                    UploadUserId = userId,
                    Description = description,
                    Md5Hash = md5Hash,
                    UploadTime = DateTime.Now
                };

                var recordId = await _db.DbContext.Insertable(fileRecord).ExecuteReturnBigIdentityAsync();
                fileRecord.Id = recordId;

                return new FileUploadResult
                {
                    Success = true,
                    Message = "文件上传成功",
                    FileRecord = fileRecord,
                    FileUrl = $"{directoryConfig.UrlPath}/{storedFileName}"
                };
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<IEnumerable<FileUploadResult>> UploadMultipleFilesAsync(IEnumerable<IFormFile> files, string businessType, string? description = null, string? userId = null)
        {
            var tasks = files.Select(file => UploadSingleFileAsync(file, businessType, description, userId));
            return await Task.WhenAll(tasks);
        }

        public async Task<BlogsFileRecord?> GetFileRecordAsync(long fileId)
        {
            return await _db.DbContext.Queryable<BlogsFileRecord>().Where(f => f.Id == fileId).FirstAsync();
        }

        public async Task<bool> DeleteFileAsync(long fileId)
        {
            var fileRecord = await GetFileRecordAsync(fileId);
            if (fileRecord == null) return false;

            // 删除物理文件
            if (File.Exists(fileRecord.FilePath))
            {
                File.Delete(fileRecord.FilePath);
            }

            // 删除数据库记录
            return await _db.DbContext.Deleteable<BlogsFileRecord>().Where(f => f.Id == fileId).ExecuteCommandHasChangeAsync();
        }

        private FileUploadResult ValidateFile(IFormFile file, string businessType)
        {
            // 检查文件大小
            if (file.Length == 0)
            {
                return new FileUploadResult { Success = false, Message = "文件为空" };
            }

            var maxFileSize = AppConfig.Instance.FileStoreConfig.MaxFileSize;
            if (file.Length > maxFileSize)
            {
                return new FileUploadResult { Success = false, Message = $"文件大小超过限制，最大允许 {maxFileSize / 1024 / 1024}MB" };
            }

            // 检查文件扩展名
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var fileExtensionList = AppConfig.Instance.FileStoreConfig.AllowedFileExtensions;
            if (!fileExtensionList.Contains(fileExtension))
            {
                return new FileUploadResult { Success = false, Message = $"不支持的文件格式：{fileExtension}，允许的格式：{string.Join(", ", fileExtensionList)}" };
            }

            // 检查业务类型
            var fileDirectories = AppConfig.Instance.FileStoreConfig.DirectoryConfig;
            if (!fileDirectories.Any(d => d.BusinessType == businessType))
            {
                return new FileUploadResult { Success = false, Message = $"无效的业务类型：{businessType}" };
            }

            // 验证文件内容类型（MIME类型）
            if (!IsValidContentType(file.ContentType, fileExtension))
            {
                return new FileUploadResult { Success = false, Message = "文件内容类型与扩展名不匹配" };
            }

            return new FileUploadResult { Success = true };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentType"></param>
        /// <param name="fileExtension"></param>
        /// <returns></returns>
        private bool IsValidContentType(string contentType, string fileExtension)
        {
            var allowedTypes = new Dictionary<string, string[]>
            {
                [".jpg"] = new[] { "image/jpeg", "image/jpg" },
                [".jpeg"] = new[] { "image/jpeg", "image/jpg" },
                [".png"] = new[] { "image/png" },
                [".gif"] = new[] { "image/gif" },
                [".pdf"] = new[] { "application/pdf" },
                [".doc"] = new[] { "application/msword" },
                [".docx"] = new[] { "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
                [".txt"] = new[] { "text/plain" }
            };

            if (allowedTypes.TryGetValue(fileExtension, out var validContentTypes))
            {
                return validContentTypes.Contains(contentType.ToLowerInvariant());
            }

            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        private void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="originalFileName"></param>
        /// <returns></returns>
        private string GenerateStoredFileName(string originalFileName)
        {
            var extension = Path.GetExtension(originalFileName);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(originalFileName);
            var safeFileName = Regex.Replace(fileNameWithoutExtension, @"[^a-zA-Z0-9\u4e00-\u9fa5-]", "_");
            return $"{safeFileName}_{DateTime.Now:yyyyMMddHHmmssfff}_{Guid.NewGuid():N}{extension}";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private async Task<string> ComputeMD5HashAsync(Stream stream)
        {
            using var md5 = MD5.Create();
            stream.Position = 0; // 重置流位置
            var hashBytes = await md5.ComputeHashAsync(stream);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private async Task SaveSmallFileAsync(IFormFile file, string filePath)
        {
            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private async Task SaveLargeFileAsync(IFormFile file, string filePath)
        {
            const int bufferSize = 81920; // 80KB缓冲区
            using var sourceStream = file.OpenReadStream();
            using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize, useAsync: true);

            var buffer = new byte[bufferSize];
            int bytesRead;
            while ((bytesRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                await fileStream.WriteAsync(buffer, 0, bytesRead);
            }
        }
    }
}
