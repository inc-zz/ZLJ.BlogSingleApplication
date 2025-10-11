using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.Domain.ServiceValidator.Admin
{

    /// <summary>
    /// 
    /// </summary>
    public interface IUserValidatorService
    {
        /// <summary>
        /// 验证用户是否存在
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        bool ExistsUser(string account); 

    }
}
