using Blogs.Core.Constant;

namespace Blogs.Infrastructure;

/// <summary>
/// 操作类型
/// </summary>
[ConstantName(TableName ="操作类型")]
public sealed class OperationTypeConstant
{
    [ConstantName(ColumnName = "创建")]
    public const string Create = "CREATE";
    [ConstantName(ColumnName = "更新")]
    public const string Update = "UPDATE";
    [ConstantName(ColumnName ="删除")]
    public const string Delete = "DELETE";
    [ConstantName(ColumnName ="查看")]
    public const string Retrieve = "RETRIEVE";
    [ConstantName(ColumnName ="导出")]
    public const string Export = "EXPORT";
    [ConstantName(ColumnName ="导入")]
    public const string Import = "IMPORT";
}
