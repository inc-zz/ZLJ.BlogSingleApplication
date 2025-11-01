using Blogs.AppServices.Queries.Admin;
using Blogs.AppServices.Queries.ResponseDto.Admin;
using Blogs.Core.Entity.Blogs;
using Blogs.Core.Models;
using Blogs.Domain.Entity.Blogs;
using Blogs.Infrastructure.OpenIdDict;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.QueryHandlers.Admin
{
    public class SysStatisticsHandler : SqlSugarDbContext,
        IRequestHandler<GetSysStatisticsQuery, ResultObject<HomeStatisticsDto>>
    {
        private readonly IDatabase _redisCache;

        public SysStatisticsHandler(IConnectionMultiplexer redis)
        {
            _redisCache = redis.GetDatabase();
        }

        /// <summary>
        /// 获取首页统计数据
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResultObject<HomeStatisticsDto>> Handle(GetSysStatisticsQuery request, CancellationToken cancellationToken)
        {
            //从Redis获取统计，可以使用后台任务Hangfire或者Quartz.NET库定时统计，也可以使用RabbitMQ实时统计
            var cacheData = await GetHomeStatisticsAsync();
            return ResultObject<HomeStatisticsDto>.Success(cacheData, "");
        }

        /// <summary>
        /// 执行统计与缓存
        /// </summary>
        /// <returns></returns>
        private async Task<HomeStatisticsDto> GetHomeStatisticsAsync()
        {
            var cacheKey = "Statistics:HomeStatistics";
            var cacheValue = await _redisCache.StringGetAsync(cacheKey);
            if (!string.IsNullOrWhiteSpace(cacheValue))
            {
                var cacheData = JsonConvert.DeserializeObject<HomeStatisticsDto>(cacheValue);
                return cacheData;
            }

            // 添加分布式锁Key
            var lockKey = cacheKey + ":Lock";
            // 申请锁
            var lockId = Guid.NewGuid().ToString();
            // 尝试获取锁，设置锁的过期时间为10秒，防止死锁
            bool isLocked = await _redisCache.LockTakeAsync(lockKey, lockId, TimeSpan.FromSeconds(10));
            if (!isLocked)
            {
                //如果没有获取到锁，等待一段时间后重试、或者直接返还旧数据
                await Task.Delay(100);//等待100毫秒
                await GetHomeStatisticsAsync();
            }
            try
            {
                cacheValue = await _redisCache.StringGetAsync(cacheKey);
                if (!string.IsNullOrWhiteSpace(cacheValue))
                {
                    var cacheData = JsonConvert.DeserializeObject<HomeStatisticsDto>(cacheValue);
                    return cacheData;
                }

                //获取统计数据
                var execSql = @"SELECT 
                (SELECT COUNT(*) FROM blogs_user WHERE IsDeleted = 0) as UserCount,
                (SELECT COUNT(*) FROM blogs_article WHERE IsDeleted = 0) as ArticleCount,
                (SELECT COUNT(*) FROM blogs_comment WHERE IsDeleted = 0) as ArticleCommentCount,
                (SELECT COALESCE(SUM(ViewCount), 0) FROM blogs_article WHERE IsDeleted = 0) as ArticleViewCount";

                var data = await DbContext.Ado.SqlQuerySingleAsync<HomeStatisticsDto>(execSql);
                //缓存数据：缓存30分钟
                await _redisCache.StringSetAsync(cacheKey, JsonConvert.SerializeObject(data), TimeSpan.FromMinutes(30));
                return data;
            }
            finally
            {
                //释放锁
                await _redisCache.LockReleaseAsync(lockKey, lockId);
            }
        }
    }
}
