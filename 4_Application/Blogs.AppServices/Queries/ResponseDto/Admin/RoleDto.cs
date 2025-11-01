using Blogs.Core.DtoModel.ResponseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.AppServices.Queries.ResponseDto.Admin
{
    /// <summary>
    /// 角色信息数据传输对象
    /// </summary>
    public class RoleDto
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 是否系统角色（1：是，0：否）
        /// </summary>
        public int IsSystem { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 角色编码（唯一标识）
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 角色描述
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 状态（1：启用，0：禁用）
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? ModifiedAt { get; set; }

        /// <summary>
        /// 最后修改人
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// 是否已删除
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 状态显示名称
        /// </summary>
        public string StatusName { get; set; }
    }
}
