using Blogs.Core.Config;
using Blogs.Core.Entity.Admin;
using Blogs.Core.Entity.Blogs;
using Blogs.Domain.Entity.Admin;
using Blogs.Domain.Entity.Blogs;
using SqlSugar;
using System.Reflection;

namespace Blogs.Infrastructure;


/// <summary>
/// 数据库上下文
/// </summary>
public class SqlSugarDbContext
{
    public SqlSugarClient DbContext;

    public SqlSugarDbContext()
    {

        var connectionString = AppConfig.GetSettingString("ConnectionStrings:MySqlConnectionWrite");
        var connectionRead = AppConfig.GetSettingString("ConnectionStrings:MySqlConnectionWrite");

        DbContext = new SqlSugarClient(new ConnectionConfig()
        {
            DbType = SqlSugar.DbType.MySql,
            InitKeyType = InitKeyType.Attribute,
            IsAutoCloseConnection = true,
            ConnectionString = connectionString,
            SlaveConnectionConfigs = new List<SlaveConnectionConfig>()
            {
             new SlaveConnectionConfig() { HitRate=10, ConnectionString = connectionRead }
            },
            ConfigureExternalServices = new ConfigureExternalServices
            {
                EntityService = (c, p) =>
                {
                    if (p.IsPrimarykey == false && new NullabilityInfoContext()
                    .Create(c).WriteState is NullabilityState.Nullable)
                    {
                        p.IsNullable = true;
                    }
                }
            }
        });
    }

    /// <summary>
    /// 数据库迁移-初始化表
    /// </summary>
    public void InitTables()
    {
        Type[] types = new Type[]
            {
                //typeof(DbSysDepartment),
                //typeof(DbSysLog),
                //typeof(DbSysMenu),
                //typeof(DbSysPermissions),
                //typeof(DbSysRole),
                typeof(DbSysUser),
                //typeof(DbSysUserGroup),
                //typeof(DbSysUserGroupRelation),
                //typeof(DbSysUserRoleRelation),

                //typeof(DbBlogsUser),
                //typeof(DbBlogsArticle),
                //typeof(DbBlogsArticleTag),
                //typeof(DbBlogsCategory),
                //typeof(DbBlogsTag),
                //typeof(DbBlogsComment)

            };
        try
        {
            //一次生成所有相关的表
            Console.WriteLine("===========================================================");

            var conn = AppConfig.GetSettingString("ConnectionStrings:MySqlConnectionWrite");
            Console.WriteLine($"连接字符串: {conn}");
            DbContext.CodeFirst.InitTablesWithAttr(types);
        }
        catch (Exception ex)
        {
            Console.WriteLine("-----------------------执行数据库表迁移出现异常：-----------------------");
            Console.WriteLine(ex.Message, ex);
        }
    }

}
