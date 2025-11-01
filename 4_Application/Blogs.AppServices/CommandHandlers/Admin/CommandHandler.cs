using Blogs.Domain;
using Blogs.Domain.Notices;
using Microsoft.Extensions.Logging;

namespace Blogs.AppServices.CommandHandlers.Admin
{
    /// <summary>
    /// 命令处理程序基类 - 优化版本
    /// </summary>
    public abstract class CommandHandler : SqlSugarDbContext
    {
        protected readonly ILogger _logger;
        private readonly DomainNotificationHandler _notificationHandler;

        /// <summary>
        /// 构造函数注入
        /// </summary>
        protected CommandHandler(
            DomainNotificationHandler notificationHandler,
            ILogger logger)
        {
            _notificationHandler = notificationHandler;
            _logger = logger;
        }

        /// <summary>
        /// 验证命令并收集错误信息
        /// </summary>
        protected bool ValidateCommand(Command command)
        {
            if (!command.IsValid())
            {
                NotifyValidationErrors(command);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 将领域命令中的验证错误信息收集
        /// </summary>
        protected void NotifyValidationErrors(Command command)
        {
            foreach (var error in command.ValidationResult.Errors)
            {
                NotifyError(error.ErrorMessage).Wait(); // 同步等待，因为这是void方法
            }
        }

        /// <summary>
        /// 自定义通知 - 直接使用通知处理器，确保中间件能收集到
        /// </summary>
        protected async Task NotifyError(string errorMessage)
        {
            await _notificationHandler.Handle(
                new DomainNotification("domainNotice", errorMessage),
                CancellationToken.None
            );
        }

        /// <summary>
        /// 执行数据库操作并处理异常
        /// </summary>
        protected async Task<bool> ExecuteDbOperationAsync(Func<Task<bool>> operation, string operationName)
        {
            try
            {
                return await operation();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{OperationName} 执行失败", operationName);
                await NotifyError($"{operationName}失败，请稍后重试");
                return false;
            }
        }

        /// <summary>
        /// 检查实体是否存在
        /// </summary>
        protected async Task<bool> CheckEntityExistsAsync<T>(long id, string entityName) where T : class, new()
        {
            var exists = await DbContext.Queryable<T>()
                .Where($"Id = {id} AND IsDeleted = 0")
                .AnyAsync();

            if (!exists)
            {
                await NotifyError($"{entityName}不存在，ID: {id}");
                return false;
            }

            return true;
        }
    }
}