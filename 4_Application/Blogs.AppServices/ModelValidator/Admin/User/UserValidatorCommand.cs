using Blogs.AppServices.Commands.Admin.SysUser;
using Blogs.Domain.ServiceValidator.Admin;
using Blogs.Infrastructure.ValidationServer.Admin;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.ModelValidator.Admin.User
{

    /// <summary>
    /// 对象值校验
    /// </summary>
    public class UserValidatorCommand<T> : AbstractValidator<T> where T : UserCommand
    {
        private readonly IUserValidatorService _userValidatorService;

        /// <summary>
        /// 
        /// </summary>
        public UserValidatorCommand()
        {
            _userValidatorService = new UserValidatorService();
        }

        /// <summary>
        /// 验证Id
        /// </summary>
        protected void ValidateId()
        {
            RuleFor(x => x.Id).NotNull().NotEqual(0).WithMessage("用户Id");
        }

        /// <summary>
        /// 真实姓名
        /// </summary>
        protected void ValidateName()
        {
            RuleFor(x => x.RealName).NotEmpty().WithName("真实姓名");
        }
        /// <summary>
        /// 性别验证
        /// </summary>
        protected void ValidateSex()
        {
            RuleFor(x => x.Sex).InclusiveBetween(1, 2).WithMessage("性别取值错误，只能是{From}或者{To}");
        }
        /// <summary>
        /// 手机号验证
        /// </summary>
        protected void ValidateMobile()
        {
            RuleFor(x => x.PhoneNumber).Matches(@"^1(?:3\d|4[5-9]|5[0-35-9]|6[2567]|7[0-8]|8\d|9[0-35-9])\d{8}$").WithMessage("手机号不正确");
        }
        /// <summary>
        /// 密码验证
        /// </summary>
        protected void ValidatePassword()
        {
            RuleFor(x => x.Password).NotEmpty().WithMessage("密码不能为空");
            RuleFor(x => x.Password).MinimumLength(6);
        }
        /// <summary>
        /// 验证部门
        /// </summary>
        protected void ValidateDepartment()
        {
            RuleFor(v => v).Must(f => f.Department == null).WithMessage("部门不能为空");
        }
        /// <summary>
        /// 验证权限
        /// </summary>
        protected void ValidateAuthorizes()
        {
            RuleFor(v => v).Must(v => v.Authorizes == null).WithMessage("权限不存在");
        }

        /// <summary>
        /// 验证角色
        /// </summary>
        protected void ValidateRoleIds()
        {
            RuleFor(v => v.RoleId).Equal(0).WithMessage("用户角色不能为空");
        }

        /// <summary>
        /// 头像格式
        /// </summary>
        protected void ValidateHeadPic()
        {
            RuleFor(v => v.HeadPic).Matches("\b(([\\w-]+://?|www[.])[^\\s()<>]+(?:[\\w\\d]+|([^[:punct:]\\s]|/)))").WithMessage("头像格式不正确");
        }
        /// <summary>
        /// 验证账号
        /// </summary>
        protected void ValidateAccount()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("账号不能为空");
        }
        /// <summary>
        /// 验证账号是否存在
        /// </summary>
        protected void ValidateAccountExists()
        {
            RuleFor(x => x).Must(it => !_userValidatorService.ExistsUser(it.UserName)).WithMessage("该账号已被注册");
        }

    }
}
