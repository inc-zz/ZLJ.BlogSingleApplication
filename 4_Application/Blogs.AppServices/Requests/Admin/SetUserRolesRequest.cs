﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Requests.Admin
{

    /// <summary>
    /// 用户授权角色
    /// </summary>
    public class SetUserRolesRequest
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 角色Id
        /// </summary>
        public long RoleId { get; set; }


    }
}
