using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.Core.DtoModel.ResponseDto
{

    /// <summary>
    /// 角色
    /// </summary>
    public class SysRoleDto
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public long Id { get; set; } 
        /// <summary>
        /// 角色名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 角色标识
        /// </summary>
        public string? Code { get; set; } 
        /// <summary>
        /// 角色描述
        /// </summary>
        public string? Summary { get; set; } 

    }
}
