using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.AppServices.Queries.ResponseDto.Admin
{

    /// <summary>
    /// 导航菜单树Dto
    /// </summary>
    public class MenuNavTreeDto
    {
        /// <summary>
        /// 菜单Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 父菜单
        /// </summary>
        public long ParentId { get; set; }
        /// <summary>
        /// 菜单Code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 菜单地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 子集
        /// </summary>
        public List<MenuNavTreeDto> Children { get; set; }

    }
}
