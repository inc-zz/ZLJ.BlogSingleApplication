using System;
using System.Collections.Generic;
using System.Text;

namespace Blogs.Core;

/// <summary>
/// 常量类自定义特性
/// </summary>
public class ConstantTextAttribute : Attribute
{
    /// <summary>
    /// 类上的名称
    /// </summary>
    public string? TableName { get; set; }
    /// <summary>
    /// 列上的自定义名称
    /// </summary>
    public string? ColumnName { get; set; }
    /// <summary>
    /// 数据类型
    /// </summary>
    public Type? Type { get; set; }

}
