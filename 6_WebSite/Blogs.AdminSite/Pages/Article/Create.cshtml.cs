using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Blogs.AdminSite.Pages.Article
{
    public class Create : PageModel
    {
        private readonly ILogger<Create> _logger;
        private readonly IWebHostEnvironment _environment;

        public Create(ILogger<Create> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        [BindProperty]
        public string Title { get; set; }

        [BindProperty]
        public int CategoryId { get; set; }

        [BindProperty]
        public string Tags { get; set; } // 存储JSON格式的标签数据

        [BindProperty]
        public string AccessPermission { get; set; } = "Public";

        [BindProperty]
        public IFormFile CoverImage { get; set; }

        [BindProperty]
        public string Status { get; set; } = "Draft";

        [BindProperty]
        public string Content { get; set; }

        public void OnGet()
        {
        }

        // 处理图片上传
        public async Task<IActionResult> OnPostUploadImageAsync()
        {
            try
            {
                var file = Request.Form.Files[0];
                if (file == null || file.Length == 0)
                {
                    return BadRequest(new { success = false, message = "没有选择文件" });
                }

                // 验证文件类型
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!allowedExtensions.Contains(extension))
                {
                    return BadRequest(new { success = false, message = "不支持的文件类型" });
                }

                // 生成唯一文件名
                var fileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{DateTime.Now.Ticks}{extension}";
                var uploadPath = Path.Combine(_environment.WebRootPath, "uploads", "images");
                
                // 确保目录存在
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                // 保存文件
                var filePath = Path.Combine(uploadPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // 返回文件URL
                var fileUrl = $"/uploads/images/{fileName}";
                return StatusCode(200, new { success = true, url = fileUrl });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "图片上传失败");
                return StatusCode(500, new { success = false, message = "上传失败，请重试" });
            }
        }

        // 处理表单提交
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // 处理标签数据
                List<string> selectedTags = new List<string>();
                if (!string.IsNullOrEmpty(Tags))
                {
                    try
                    {
                        selectedTags = JsonSerializer.Deserialize<List<string>>(Tags);
                    }
                    catch (JsonException ex)
                    {
                        _logger.LogError(ex, "标签数据解析失败");
                    }
                }

                // 处理封面图片
                string coverImageUrl = null;
                if (CoverImage != null && CoverImage.Length > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var extension = Path.GetExtension(CoverImage.FileName).ToLower();
                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("CoverImage", "不支持的文件类型");
                        return Page();
                    }

                    // 生成唯一文件名
                    var fileName = $"cover_{DateTime.Now.Ticks}{extension}";
                    var uploadPath = Path.Combine(_environment.WebRootPath, "uploads", "covers");
                    
                    // 确保目录存在
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    // 保存文件
                    var filePath = Path.Combine(uploadPath, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await CoverImage.CopyToAsync(stream);
                    }

                    coverImageUrl = $"/uploads/covers/{fileName}";
                }

                // 这里应该将文章数据保存到数据库
                // 为了演示，这里仅记录日志
                _logger.LogInformation("文章保存成功: {Title}, 访问权限: {Permission}", Title, AccessPermission);
                _logger.LogInformation("选择的标签数量: {TagCount}", selectedTags?.Count ?? 0);

                // 重定向到文章列表页
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存文章失败");
                ModelState.AddModelError(string.Empty, "保存失败，请重试");
                return Page();
            }
        }
    }
}