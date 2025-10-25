using Blogs.Domain.AggregateRoot;
using SqlSugar.DistributedSystem.Snowflake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Core.Entity
{
    /// <summary>
    /// 公共字段
    /// </summary>
    public abstract class BaseEntity 
    {
        protected BaseEntity()
        {
            
        }

        protected BaseEntity(long id)
        {
            this.Id = id;
        }

        /// <summary>
        /// 主键
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public virtual long Id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get;  set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string? CreatedBy { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? ModifiedAt { get;  set; }
        /// <summary>
        /// 最后修改人
        /// </summary>
        public string? ModifiedBy { get;  set; }
        /// <summary>
        /// 是否删除(针对树形表需要直接操作)
        /// </summary>
        public int IsDeleted { get;  set; } = 0;

        /// <summary>
        /// 创建信息
        /// </summary>
        /// <param name="byUser"></param>
        public void MarkAsCreated(string byUser)
        {
            CreatedAt = DateTime.Now;
            CreatedBy = byUser;
            IsDeleted = 0;
        }
        /// <summary>
        /// 修改信息
        /// </summary>
        /// <param name="byUser"></param>
        public void MarkAsModified(string byUser)
        {
            ModifiedAt = DateTime.Now;
            ModifiedBy = byUser;
        }
        /// <summary>
        /// 软删除
        /// </summary>
        public void SoftDelete()
        {
            IsDeleted = 1;
        }
    }
}
