using Blogs.Core.DtoModel.ResponseDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Queries.ResponseDto.Admin
{

    /// <summary>
    /// 系统按钮
    /// </summary>
    public class SysButtonDto : BaseDto
    { 
        /// <summary>
        /// 按钮名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 按钮编码
        /// </summary>
        public string? Code { get; set; }
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
        /// <summary>
        /// 描述
        /// </summary>
        public string Summary { get; set; }
      
    }
}
