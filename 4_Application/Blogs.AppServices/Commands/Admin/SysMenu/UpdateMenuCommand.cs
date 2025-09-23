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
        public UpdateMenuCommand(long id, long parentId, string name, string code, string url, string icon, List<MenuButtonCommand> buttons)
        {
            Id = id;
            ParentId = parentId;
            Name = name;
            Code = code;
            Url = url;
            Icon = icon;
            Buttons = buttons;
            //if (this.ParentId > 0)
            //{
            //    this.ParentIdList = 0;
            //}
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
