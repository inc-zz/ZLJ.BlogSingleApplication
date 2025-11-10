//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Blogs.AppServices.Queries.ResponseDto.Admin
//{
//   public class BlogsCommentDto
//    {
//        public long Id { get; set; }
//        public long ArticleId { get; set; }
//        public string ArticleTitle { get; set; } = string.Empty;
//        public string Content { get; set; } = string.Empty;
//        public string Author { get; set; } = string.Empty;
//        public string? Email { get; set; }
//        public string? Website { get; set; }
//        public long? ParentId { get; set; }
//        public string? ParentAuthor { get; set; }
//        public int Status { get; set; }
//        public string? IpAddress { get; set; }
//        public int LikeCount { get; set; }
//        public DateTime CreatedAt { get; set; }
//        public string StatusName => Status == 1 ? "正常" : "隐藏";
//    }
//}
