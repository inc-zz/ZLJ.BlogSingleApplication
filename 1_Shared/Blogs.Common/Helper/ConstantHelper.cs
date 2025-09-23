using System;
using System.Collections.Generic;
using System.Reflection;

namespace Blogs.Core
{
    /// <summary>
    /// 枚举扩展属性
    /// </summary>
    public static class ConstantHelper
    { 
        /// <summary>
        /// 获取类上的自定义属性值和类属性值集合 
        /// </summary>
        /// <returns>字典格式 key=>注释，value=>属性值</returns>
        public static Dictionary<string, string> GetAttrNameAndValue<T>() where T : class, new()
        {
            var result = new Dictionary<string, string>();
            try
            {
                Type objType = typeof(T);

                foreach (PropertyInfo property in objType.GetProperties())
                {
                    var objValue = property.GetValue(objType, null);
                    var value = objValue == null ? "" : objValue.ToString();
                    object[] objAttrs = property.GetCustomAttributes(typeof(ConstantTextAttribute), true);
                    if (objAttrs.Length == 0)
                        continue;
                    ConstantTextAttribute nextAttr = objAttrs[0] as ConstantTextAttribute;
                    if (nextAttr == null)
                        continue;
                    var attrName = nextAttr.ColumnName; //列名
                    if (!string.IsNullOrWhiteSpace(attrName) && !result.ContainsKey(attrName))
                        result.Add(attrName, value);

                }
            }
            catch
            {
                throw; 
            }
            return result;
        }

        /// <summary>
        /// 获取类的属性名和属性值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Dictionary<string, string> GetFieldAndValue<T>()
        {
            Type typeInfo = typeof(T);
            FieldInfo[] fields = typeInfo.GetFields();
            var result = new Dictionary<string, string>();
            foreach (var item in fields)
            {
                result.Add(item.Name, item.GetRawConstantValue().ToString());
            }
            return result;
        }

        /// <summary>
        /// 获取实体上的自定义特性名+特性值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetConstantTextAttribute<T>()
        {
            var result = new Dictionary<string, string>();

            var type = typeof(T);
            var fields = type.GetFields();
            foreach (var item in fields)
            {
                var attrs = item.GetCustomAttributes(typeof(ConstantTextAttribute), true);
                if (attrs.Length == 1)
                {
                    var enumValue = item.GetValue(item.Name).ToString();
                    var enumText = ((ConstantTextAttribute)attrs[0]).ColumnName;
                    if (!result.ContainsKey(enumValue))
                    {
                        result.Add(enumValue, enumText);
                    }
                }
            }
            return result;
        }


        /// <summary>
        /// 根据常量Value获取常量类自定义Text的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ConstantTextValue<T>(string value)
        {
            var dicAttr = GetConstantTextAttribute<T>();
            if (dicAttr.ContainsKey(value))
                return dicAttr[value].ToString();
            return string.Empty;
        }
    }
}