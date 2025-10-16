using Blogs.AppServices.Commands.Blogs.Settings;
using Blogs.Domain.Entity.Blogs;
using Microsoft.AspNetCore.Http;

namespace Blogs.AppServices.CommandHandlers.App
{
    public class AppSettingCommandHandler : AppCommandHandler,
        IRequestHandler<CreateBlogSettingCommand, bool>,
        IRequestHandler<UpdateBlogSettingCommand, bool>,
        IRequestHandler<DeleteBlogSettingCommand, bool>
    {
        private readonly IMediatorHandler _eventBus;
        private readonly IHttpContextAccessor _httpContext;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventBus"></param>
        public AppSettingCommandHandler(IMediatorHandler eventBus) : base(eventBus)
        {
            _eventBus = eventBus;
        }

        /// <summary>
        /// 创建配置
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(CreateBlogSettingCommand request, CancellationToken cancellationToken)
        {
            var settings = request.Adapt<BlogsSettings>();
            var result = await DbContext.Insertable(settings).ExecuteCommandAsync();
            return result > 0;
        }
        /// <summary>
        /// 更新配置
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(UpdateBlogSettingCommand request, CancellationToken cancellationToken)
        {
            var settings = await DbContext.Queryable<BlogsSettings>().Where(it => it.Id == request.Id).FirstAsync();
            if (settings == null)
            {
                return false;
            }
            settings.SetEntity(request.Title, request.Summary, request.Url,
                request.Tags, request.BusType, request.Content, request.Status);
            var result = await DbContext.Updateable(settings).ExecuteCommandAsync();

            return result > 0;
        }

        /// <summary>
        /// 删除配置
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(DeleteBlogSettingCommand request, CancellationToken cancellationToken)
        {
            var result = await DbContext.Deleteable<BlogsSettings>(it => it.Id == request.Id).ExecuteCommandAsync();
            return result > 0;
        }
    }
}
