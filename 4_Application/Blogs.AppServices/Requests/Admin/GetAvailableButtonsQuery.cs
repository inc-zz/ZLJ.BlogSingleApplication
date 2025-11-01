using Blogs.AppServices.Queries.ResponseDto.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Requests.Admin
{
    /// <summary>
    /// 获取所有可用按钮查询（用于下拉选择）
    /// </summary>
    public class GetAvailableButtonsQuery : IRequest<List<ButtonListDto>>
    {
        public string Position { get; set; }

        public GetAvailableButtonsQuery(string position = null)
        {
            Position = position;
        }
    }
}
