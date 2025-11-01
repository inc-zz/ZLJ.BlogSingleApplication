using Blogs.AppServices.Commands.Admin.SysUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.ModelValidator.Admin.User
{
    public class BatchDeleteUserCommandValidation : UserValidatorCommand<UserCommand>
    {
        /// <summary>
        /// 
        /// </summary>
        public BatchDeleteUserCommandValidation()
        {
            ValidateIds();
        }
    }
}
