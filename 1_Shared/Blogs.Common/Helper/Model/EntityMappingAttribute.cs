using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Blogs.Core;

/// <summary>
/// Excel导出类中的实体标注
/// </summary>
public class EntityMappingAttribute :Attribute
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="_columnName"></param>
    public EntityMappingAttribute(string _columnName)
    {
        this.columnName = _columnName;
    }

    /// <summary>
    /// 自定义特性上的属性名
    /// </summary>
    public string ColumnName
    {
        get
        {
            return this.columnName;
        }
        set
        {
            this.columnName = value;
        }
    }
    private string columnName;

	}
