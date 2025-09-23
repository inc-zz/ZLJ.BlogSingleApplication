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
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public string UpdateUser { get; set; }

    }
}
