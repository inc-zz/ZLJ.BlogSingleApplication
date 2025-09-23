using System.Data;
using System.Reflection;

namespace Blogs.Core
{
    /// <summary>
    /// Table-实体转换工具类
    /// </summary>
    public sealed class TableEntityHelper
    {
        /// <summary>
        /// DataTable转指定格式List
        /// </summary>
        public IList<T> GetList<T>(DataTable table) where T : new()
        {
            if (table is null || table.Rows.Count == 0)
                return new List<T>();

            var list = new List<T>(table.Rows.Count);
            var properties = typeof(T).GetProperties();

            foreach (DataRow row in table.Rows)
            {
                var item = CreateItemFromRow<T>(row, properties);
                list.Add(item);
            }

            return list;
        }

        /// <summary>
        /// DataRow转指定格式实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static T CreateItemFromRow<T>(DataRow row, PropertyInfo[] properties) where T : new()
        {
            var item = new T();

            foreach (var prop in properties)
            {
                if (!row.Table.Columns.Contains(prop.Name))
                    continue;

                try
                {
                    var value = row[prop.Name];
                    if (value == DBNull.Value || value is null)
                        continue;

                    if (IsNullableType(prop.PropertyType))
                    {
                        var underlyingType = Nullable.GetUnderlyingType(prop.PropertyType);
                        prop.SetValue(item, Convert.ChangeType(value, underlyingType!));
                    }
                    else
                    {
                        prop.SetValue(item, Convert.ChangeType(value, prop.PropertyType));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Property '{prop.Name}' processing failed", ex);
                }
            }

            return item;
        }

        private static bool IsNullableType(Type type) =>
            type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

        /// <summary>
        /// List转DataTable
        /// </summary>
        public DataTable ListToDataTable<T>(IEnumerable<T> varlist)
        {
            var dtReturn = new DataTable();
            var properties = typeof(T).GetProperties();

            // 创建列
            foreach (var pi in properties)
            {
                var colType = pi.PropertyType;
                if (IsNullableType(colType))
                {
                    colType = Nullable.GetUnderlyingType(colType)!;
                }
                dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
            }

            // 填充数据
            foreach (var rec in varlist)
            {
                var dr = dtReturn.NewRow();
                foreach (var pi in properties)
                {
                    var value = pi.GetValue(rec, null);
                    dr[pi.Name] = value ?? DBNull.Value;
                }
                dtReturn.Rows.Add(dr);
            }

            return dtReturn;
        }

        /// <summary>
        /// DataTable内存分页
        /// </summary>
        public DataTable GetPagedTable(DataTable dt, int pageIndex, int pageSize)
        {
            if (pageIndex <= 0 || dt is null || dt.Rows.Count == 0)
                return dt.Clone();

            int rowbegin = (pageIndex - 1) * pageSize;
            if (rowbegin >= dt.Rows.Count)
                return dt.Clone();

            int rowend = Math.Min(pageIndex * pageSize, dt.Rows.Count);

            var newdt = dt.Clone();
            for (int i = rowbegin; i < rowend; i++)
            {
                newdt.ImportRow(dt.Rows[i]);
            }

            return newdt;
        }

        /// <summary>
        /// DataSet分页
        /// </summary>
        public DataSet GetPagedDataSet(DataSet ds, int pageIndex, int pageSize)
        {
            var newDs = new DataSet();
            foreach (DataTable table in ds.Tables)
            {
                DataTable newDt = GetPagedTable(table, pageIndex, pageSize);
                newDs.Tables.Add(newDt);
            }
            return newDs;
        }

        /// <summary>
        /// 取DataTable的列名
        /// </summary>
        public List<string> GetDataTableColumns(DataTable dt) =>
            dt?.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToList() ?? new List<string>();

        /// <summary>
        /// 转换DataTable的列名
        /// </summary>
        public void ConvertTableColumnName(DataTable source, Dictionary<string, string> columnList)
        {
            if (source is null || columnList is null || !columnList.Any())
                return;

            // 确保至少有一行
            if (source.Rows.Count == 0)
                source.Rows.Add(source.NewRow());

            // 重命名列
            foreach (var item in columnList)
            {
                if (source.Columns.Contains(item.Key))
                {
                    source.Columns[item.Key].ColumnName = item.Value;
                }
            }
        }

        /// <summary>
        /// 移除DataTable指定列名
        /// </summary>
        public DataTable DeleteDataTableColumns(DataTable dt, List<string> dtColumnList)
        {
            if (dt is null || dtColumnList is null)
                return dt;

            foreach (var columnName in dtColumnList.Where(name => dt.Columns.Contains(name)))
            {
                dt.Columns.Remove(columnName);
            }

            return dt;
        }

        /// <summary>
        /// 获取实体上的自定义特性名
        /// </summary>
        public Dictionary<string, string> GetAttributeName<T>() where T : class, new()
        {
            var result = new Dictionary<string, string>();
            var properties = typeof(T).GetProperties();

            foreach (var propInfo in properties)
            {
                var attr = propInfo.GetCustomAttribute<EntityMappingAttribute>();
                if (attr is not null)
                {
                    string columnName = string.IsNullOrWhiteSpace(attr.ColumnName) ?
                                        propInfo.Name : attr.ColumnName;
                    result[propInfo.Name] = columnName;
                }
            }

            return result;
        }
    }
}