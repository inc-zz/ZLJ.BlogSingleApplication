using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Domain.ServiceValidator.App
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAppUserValidatorService
    {
        /// <summary>
        /// 验证用户是否存在
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        bool ExistsUser(string account);

    }
}
