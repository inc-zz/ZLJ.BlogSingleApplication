using Autofac;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using System.Reflection;
using System.Runtime.Loader;

namespace OSS.StorageApiService;

/// <summary>
/// 
/// </summary>
public static class AutofacContainerModuleExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="builder"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddProductServiceModule(this IServiceCollection services, ContainerBuilder builder, IConfiguration configuration)
    {

        Type baseType = typeof(IDependency);
        var compilationLibrary = DependencyContext.Default
            .RuntimeLibraries
            .Where(x => !x.Serviceable
            && x.Type == "project")
            .ToList();

        var count1 = compilationLibrary.Count;
        List<Assembly> assemblyList = new List<Assembly>();

        try
        {
            assemblyList.Add(AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("ZLJ.Storage.ApiService")));

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        builder.RegisterAssemblyTypes(assemblyList.ToArray())
         .Where(type => baseType.IsAssignableFrom(type) && !type.IsAbstract)
         .Where(it => it.Name.EndsWith("Service") || it.Name.EndsWith("Repository"))
         .AsSelf()
         .AsImplementedInterfaces()
         .InstancePerLifetimeScope();

        builder.RegisterType<UserContext>().InstancePerLifetimeScope();
        builder.RegisterType<ActionObserver>().InstancePerLifetimeScope();
        //model校验结果
        builder.RegisterType<ObjectModelValidatorState>().InstancePerLifetimeScope();
        string connectionString = DBServerProvider.GetConnectionString(null);

        
        return services;
    }

}
