using Blogs.Core.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Blogs.WebApi.Middleware
{
    // <summary>
    /// 全局异常处理过滤器
    /// 捕获所有未处理的异常并格式化为统一响应
    ///</summary>
    public class GlobalExceptionFilter : IAsyncExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;
        private readonly IWebHostEnvironment _environment;

        public GlobalExceptionFilter(
            ILogger<GlobalExceptionFilter> logger,
            IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            // 记录异常
            _logger.LogError(context.Exception, "全局异常过滤器捕获异常: {Message}", context.Exception.Message);

            // 创建统一的错误响应
            var resultObject = CreateErrorResult(context.Exception, context.HttpContext.TraceIdentifier);

            // 设置响应
            context.Result = new ObjectResult(resultObject)
            {
                StatusCode = GetStatusCodeFromException(context.Exception)
            };

            // 标记异常已处理
            context.ExceptionHandled = true;

            return Task.CompletedTask;
        }

        /// <summary>
        /// 创建错误响应
        /// </summary>
        private ResultObject CreateErrorResult(Exception exception, string traceId)
        {
            if (_environment.IsDevelopment())
            {
                // 开发环境返回详细错误信息
                var errorDetails = new
                {
                    exceptionType = exception.GetType().Name,
                    message = exception.Message,
                    stackTrace = exception.StackTrace,
                    innerException = exception.InnerException?.Message,
                    source = exception.Source
                };

                return new ResultObject
                {
                    code = 500,
                    success = false,
                    message = $"系统异常: {exception.Message}",
                    traceId = traceId
                };
            }
            else
            {
                // 生产环境返回通用错误信息
                return new ResultObject
                {
                    code = 500,
                    success = false,
                    message = "系统繁忙，请稍后重试",
                    traceId = traceId
                };
            }
        }

        /// <summary>
        /// 根据异常类型获取HTTP状态码
        /// </summary>
        private int GetStatusCodeFromException(Exception exception)
        {
            return exception switch
            {
                UnauthorizedAccessException => 401,
                KeyNotFoundException => 404,
                ArgumentException => 400,
                InvalidOperationException => 400,
                NotImplementedException => 501,
                TimeoutException => 408,
                _ => 500
            };
        }
    }
}
