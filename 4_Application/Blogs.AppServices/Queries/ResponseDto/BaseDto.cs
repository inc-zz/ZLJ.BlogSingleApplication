using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.Core.DtoModel.ResponseDto
{
    /// <summary>
    /// 基础属性 
    /// </summary>
    public class BaseDto
    {
        /// <summary>
        /// 菜单Id
        /// </summary>
        public long Id { get; set; }
        public DateTime? CreatedAt { get; set; }

        public string? CreatedBy { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? ModifiedAt { get; protected set; }
        /// <summary>
        /// 最后修改人
        /// </summary>
        public string? ModifiedBy { get; protected set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; protected set; }

    }
}
