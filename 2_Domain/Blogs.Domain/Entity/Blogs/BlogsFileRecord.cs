using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Domain.Entity.Blogs
{
    /// <summary>
    /// 文件记录实体
    /// </summary>
    [SugarTable("blogs_file_record")]
    public class BlogsFileRecord
    {
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true, IsIdentity = true)]
        public long Id { get; set; }

        public string OriginalFileName { get; set; } = string.Empty;

        public string StoredFileName { get; set; } = string.Empty;

        public string FilePath { get; set; } = string.Empty;

        public long FileSize { get; set; }

        public string ContentType { get; set; } = string.Empty;

        public string FileExtension { get; set; } = string.Empty;

        public string BusinessType { get; set; } = string.Empty;

        public long BusinessId { get; set; }

        public DateTime UploadTime { get; set; } = DateTime.Now;

        public string? UploadUserId { get; set; }

        public string? Description { get; set; }

        public string? Md5Hash { get; set; }
    }
}
