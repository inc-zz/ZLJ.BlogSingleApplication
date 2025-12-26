using Blogs.AppServices.ModelValidator.Admin.Menu;
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
            Type = request.Type;
            Url = request.Url;
            Icon = request.Icon;
            Status = (int)ApproveStatusEnum.Normal;
            Buttons = request.Buttons;
           
        }

        /// <summary>
        /// 数据验证
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            ValidationResult = new CreateMenuCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
