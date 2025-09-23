using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.Core.DtoModel.ResponseDto
{

    /// <summary>
    /// 管理员Dto
    /// </summary>
    public class SysAdminDto
    {
        /// <summary>
        /// 管理员Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string? Account { get; set; }
        /// <summary>
        ///  真实姓名
        /// </summary>
        public string? TrueName { set; get; }
        /// <summary>
        /// 租户Id
        /// </summary>
        public long TenantId { get; set; }
        /// <summary>
        ///  租户
        /// </summary>
        public string? TenantName { set; get; }
        /// <summary>
        ///  状态
        /// </summary>
        public int Status { set; get; }
        /// <summary>
        ///  状态名称
        /// </summary>
        public string? StatusName { set; get; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string? CreateUser { get; set; }
        /// <summary>
        /// 创建人名称
        /// </summary>
        public string? CreateName { get; set; }

    }
}
