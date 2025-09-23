using Blogs.Core;

namespace Blogs.Core.Enums
{

    /// <summary>
    /// 数据审批状态枚举
    /// </summary>
    public enum ApproveStatusEnum
    {
        /// <summary>
        /// 已删除
        /// </summary>
        [EnumText("已删除")]
        Delete = -1,
        /// <summary>
        /// 禁用
        /// </summary>
        [EnumText("禁用")]
        Disable = 0,
        /// <summary>
        /// 正常/审批通过
        /// </summary>
        [EnumText("正常")]
        Normal

    }
}
