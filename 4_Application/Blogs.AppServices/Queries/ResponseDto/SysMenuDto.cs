using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.Core.DtoModel.ResponseDto
{

    /// <summary>
    /// 菜单返回类
    /// </summary>
    public class SysMenuDto
    {
        
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 菜单标识
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 菜单地址
        /// </summary>
        public string Url { get; set; } 
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 状态名称
        /// </summary>
        public string StatusName { get; set; } 

    }
}
