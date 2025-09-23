using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Core.Constant;

/// <summary>
/// 常量类名称特性
/// </summary>
public class ConstantNameAttribute : Attribute
{
    /// <summary>
    /// 类名
    /// </summary>
    public string? TableName { get; set; }
    /// <summary>
    /// 属性名称
    /// </summary>
    public string? ColumnName { get; set; }
    /// <summary>
    /// 数据类型
    /// </summary>
    public Type? Type { get; set; }

}
