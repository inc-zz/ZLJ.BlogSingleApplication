using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Core.Enums
{
    public enum DbTypeNameEnum
    {
        /// <summary>
        /// MySQL数据库
        /// </summary>
        MySQL = 0,
        /// <summary>
        /// SQL Server数据库
        /// </summary>
        MsSql = 1,
        /// <summary>
        /// PostgreSQL数据库
        /// </summary>
        PgSql = 2,
        /// <summary>
        /// Oracle数据库
        /// </summary>
        Oracle = 3
    }
}
