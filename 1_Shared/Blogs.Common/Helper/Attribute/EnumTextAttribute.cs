using System;
using System.Collections.Generic;

namespace  Blogs.Core;

/// <summary>
/// 枚举类自定义特性
/// </summary>
public class EnumTextAttribute : Attribute
{
    public EnumTextAttribute(string value)
    {
        Value = value;
    }
    public string Value { get; set; }
}
