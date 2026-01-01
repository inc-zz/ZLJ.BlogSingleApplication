using Blogs.Core.Models;
using Blogs.Domain.EventBus;
using Blogs.Domain.EventNotices;
using Blogs.Domain.Notices;
using System.Text.Json;

namespace Blogs.WebApi.Middleware;
/// <summary>
/// 统一响应处理中间件 - 修正版本
/// 只有业务错误和系统异常才格式化为ResultObject，成功响应保持原样
/// </summary>
public class UnifiedResponseMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<UnifiedResponseMiddleware> _logger;
    private readonly IWebHostEnvironment _environment;

    public UnifiedResponseMiddleware(
        RequestDelegate next,
        ILogger<UnifiedResponseMiddleware> logger,
        IWebHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(
        HttpContext context,
        DomainNotificationHandler notificationHandler)
    {
        // 保存原始响应流
        var originalBodyStream = context.Response.Body;

        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        try
        {
            // 执行后续中间件和控制器
            await _next(context);

            // 重置流位置以便读取响应内容
            responseBody.Seek(0, SeekOrigin.Begin);
            var responseContent = await new StreamReader(responseBody).ReadToEndAsync();

            // 1. 首先检查领域通知（业务错误）
            var notifications = notificationHandler.GetNotifications();
            if (notifications != null && notifications.Any())
            {
                _logger.LogWarning("发现业务错误: {Error}", notifications.First().Value);
                await HandleDomainNotifications(context, notifications, originalBodyStream);
                return;
            }

            // 2. 如果是200状态码，直接返回原始响应（不包装）
            if (context.Response.StatusCode == 200)
            {
                await ReturnOriginalResponse(context, responseContent, originalBodyStream);
                return;
            }

            // 3. 处理其他HTTP错误状态码（非200）
            if (context.Response.StatusCode >= 400)
            {
                await HandleHttpError(context, originalBodyStream);
                return;
            }

            // 4. 其他状态码也返回原始响应
            await ReturnOriginalResponse(context, responseContent, originalBodyStream);
        }
        catch (Exception ex)
        {
            // 5. 处理系统异常
            await HandleException(context, ex, originalBodyStream);
        }
        finally
        {
            context.Response.Body = originalBodyStream;
        }
    }

    /// <summary>
    /// 处理领域通知（业务错误）- 强制覆盖为错误响应
    /// </summary>
    private async Task HandleDomainNotifications(
        HttpContext context,
        IEnumerable<DomainNotification> notifications,
        Stream originalBodyStream)
    {
        var primaryNotification = notifications.First();
        _logger.LogWarning("业务验证失败: {Message}", primaryNotification.Value);

        // 强制设置为错误响应
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = 400; // 业务错误返回400

        var resultObject = ResultObject.FromDomainNotifications(notifications);
        resultObject.traceId = context.TraceIdentifier;

        await WriteJsonResponse(context, resultObject, originalBodyStream);
    }

    /// <summary>
    /// 返回原始响应（不包装）
    /// </summary>
    private async Task ReturnOriginalResponse(
        HttpContext context,
        string responseContent,
        Stream originalBodyStream)
    {
        //_logger.LogDebug("返回原始响应，状态码: {StatusCode}", context.Response.StatusCode);

        // 直接将内存流内容写回原始流
        context.Response.Body = originalBodyStream;

        if (!string.IsNullOrEmpty(responseContent))
        {
            await context.Response.WriteAsync(responseContent);
        }
    }

    /// <summary>
    /// 处理HTTP错误状态码
    /// </summary>
    private async Task HandleHttpError(
        HttpContext context,
        Stream originalBodyStream)
    {
        var message = GetHttpStatusMessage(context.Response.StatusCode);
        var resultObject = ResultObject.Error(message, context.Response.StatusCode);
        resultObject.traceId = context.TraceIdentifier;

        await WriteJsonResponse(context, resultObject, originalBodyStream);
    }

    /// <summary>
    /// 处理系统异常
    /// </summary>
    private async Task HandleException(
        HttpContext context,
        Exception exception,
        Stream originalBodyStream)
    {
        _logger.LogError(exception, "系统异常: {Message}", exception.Message);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = GetStatusCodeFromException(exception);

        var resultObject = CreateErrorResult(exception, context.TraceIdentifier);

        await WriteJsonResponse(context, resultObject, originalBodyStream);
    }

    /// <summary>
    /// 写入JSON响应
    /// </summary>
    private async Task WriteJsonResponse(
        HttpContext context,
        ResultObject resultObject,
        Stream originalBodyStream)
    {
        var json = JsonSerializer.Serialize(resultObject, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = _environment.IsDevelopment()
        });

        context.Response.Body = originalBodyStream;
        await context.Response.WriteAsync(json);
    }

    /// <summary>
    /// 获取HTTP状态码对应的消息
    /// </summary>
    private string GetHttpStatusMessage(int statusCode)
    {
        return statusCode switch
        {
            400 => "请求参数错误",
            401 => "未授权访问",
            403 => "访问被禁止",
            404 => "资源未找到",
            405 => "请求方法不允许",
            500 => "服务器内部错误",
            _ => "请求失败"
        };
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
            _ => 500
        };
    }

    /// <summary>
    /// 创建错误响应
    /// </summary>
    private ResultObject CreateErrorResult(Exception exception, string traceId)
    {
        if (_environment.IsDevelopment())
        {
            var errorDetails = new
            {
                exceptionType = exception.GetType().Name,
                message = exception.Message,
                stackTrace = exception.StackTrace,
                innerException = exception.InnerException?.Message
            };

            return new ResultObject<dynamic>
            {
                code = 500,
                success = false,
                message = $"系统异常: {exception.Message}",
                data = errorDetails,
                traceId = traceId
            };
        }
        else
        {
            return new ResultObject
            {
                code = 500,
                success = false,
                message = "系统繁忙，请稍后重试",
                traceId = traceId
            };
        }
    }
}