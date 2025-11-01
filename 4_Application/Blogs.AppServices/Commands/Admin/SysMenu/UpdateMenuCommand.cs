using Blogs.AppServices.Requests.Admin;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Commands.Admin.SysMenu
{

    /// <summary>
    /// 修改菜单数据验证
    /// </summary>
    public class UpdateMenuCommand : MenuCommand
    {
        /// <summary>
        /// 菜单对象初始化
        /// </summary>
        public UpdateMenuCommand(UpdateMenuRequest request)
        {
            Id = request.Id;
            ParentId = request.ParentId;
            Buttons = request.Buttons;
            Name = request.Name;
            Type = request.Type;
            Url = request.Url;
            Icon = request.Icon;
            Sort = request.Sort;
        }

        /// <summary>
        /// 数据验证
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            //ValidationResult = new UpdateMenuCommandValidation().Validate(this);
            //if (DbContext.Queryable<SysMenu>().Any(x => x.Name == this.Name && x.Id != this.Id && x.TenantId == this.TenantId))
            //{
            //    ValidationResult.Errors.Add(new FluentValidation.Results.ValidationFailure("", $"菜单名称{this.Name}重复!"));
            //}
            //return ValidationResult.IsValid;
            return true;
        }
    }
}
