using Blogs.AppServices.Requests.Admin;
using Blogs.Core.Enums;
using NCD.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Commands.Admin.SysMenu
{

    /// <summary>
    /// 创建菜单命令
    /// </summary>
    public class CreateMenuCommand : MenuCommand
    {
        /// <summary>
        /// 菜单对象初始化
        /// </summary>
        public CreateMenuCommand(AddMenuRequest request)
        {
            ParentId = request.ParentId;
            Name = request.Name;
            this.Type = request.Type;
            Url = request.Url;
            Icon = request.Icon;
            Status = (int)ApproveStatusEnum.Normal;
           
        }

        /// <summary>
        /// 数据验证
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            //数据库验证需要提升到应用层
            //ValidationResult = new CreateMenuCommandValidation().Validate(this);
            //if (DbContext.Queryable<SysMenu>().Any(x => x.Name == this.Name && x.TenantId != this.Id))
            //{   
            //    ValidationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("", $"菜单名称{this.Name}重复!"));
            //}
            //return ValidationResult.IsValid;
            return true;
        }

    }
}
