using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Queries.ResponseDto.Admin
{
    /// <summary>
    /// 按钮详情DTO
    /// </summary>
    public class ButtonDetailDto : ButtonListDto
    {
        public List<ButtonMenuDto> RelatedMenus { get; set; } = new();
    }
}
