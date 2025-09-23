using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.Core.DtoModel
{

    /// <summary>
    /// 分组数据Dto
    /// </summary>
    public class GroupListDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 上级id
        /// </summary>
        public long ParentId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
    }
}
