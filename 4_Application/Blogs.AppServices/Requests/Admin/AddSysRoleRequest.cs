using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Blogs.AppServices.Requests.Admin
{

    /// <summary>
    /// 新增角色
    /// </summary>
    public class AddSysRoleRequest
    {
        ///<summary>
        ///  角色名称
        ///</summary>
        public string Name { set; get; }
        ///<summary>
        ///  角色编号
        ///</summary>
        public string Code { set; get; }
        ///<summary>
        ///  是否超管员
        ///</summary>
        public int IsSystem { set; get; }
        ///<summary>
        ///  排序
        ///</summary>
        public int Sort { set; get; }
        ///<summary>
        ///  描述
        ///</summary>
        public string Remark { set; get; }


    }
}
