using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Blogs.Core
{
    /// <summary>
    /// 枚举扩展属性
    /// </summary>
    public static class EnumHelper
    {
        private static Dictionary<string, Dictionary<string, string>> enumCache;

        private static Dictionary<string, Dictionary<string, string>> EnumCache
        {
            get
            {
                if (enumCache == null)
                {
                    enumCache = new Dictionary<string, Dictionary<string, string>>();
                }
                return enumCache;
            }
            set { enumCache = value; }
        }

        /// <summary>
        /// 获得枚举提示文本
        /// </summary>
        /// <param name="en"></param>
        /// <returns></returns>
        public static string GetEnumText(this Enum en)
        {
            string enString = string.Empty;
            if (null == en) return enString;
            var type = en.GetType();
            enString = en.ToString();
            if (!EnumCache.ContainsKey(type.FullName))
            {
                var fields = type.GetFields();
                Dictionary<string, string> temp = new Dictionary<string, string>();
                foreach (var item in fields)
                {
                    var attrs = item.GetCustomAttributes(typeof(EnumTextAttribute), false);
                    if (attrs.Length == 1)
                    {
                        var v = ((EnumTextAttribute)attrs[0]).Value;
                        temp.Add(item.Name, v);
                    }
                }

                EnumCache.Add(type.FullName, temp);
            }
            if (EnumCache[type.FullName].ContainsKey(enString))
            {
                return EnumCache[type.FullName][enString];
            }
            return enString;
        }
        /// <summary>
        /// 根据枚举值获取枚举自定义特性
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumText(this Enum en, int? value)
        {
            string enString = string.Empty;
            if (null == en || null == value) return enString;
            var type = en.GetType();
            enString = en.ToString();

            var fields = type.GetFields();
            foreach (var item in fields)
            {
                var attrs = item.GetCustomAttributes(typeof(EnumTextAttribute), false);
                if (attrs.Length == 1)
                {
                    var enumValue = (int)item.GetValue(item.Name);//根据名称获取，枚举项的值
                    var enumText = ((EnumTextAttribute)attrs[0]).Value;
                    if (enumValue == value)
                    {
                        return enumText;
                    }
                }
            }

            return enString;
        }
        /// <summary>
        /// 获取实体上的自定义属性名+特性值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Dictionary<int, string> GetTextAttribute<T>()
        {
            var result = new Dictionary<int, string>();

            var type = typeof(T);
            var fields = type.GetFields();
            foreach (var item in fields)
            {
                var attrs = item.GetCustomAttributes(typeof(EnumTextAttribute), true);
                if (attrs.Length == 1)
                {
                    var enumValue = (int)item.GetValue(item.Name);
                    var enumText = ((EnumTextAttribute)attrs[0]).Value;
                    if (!result.ContainsKey(enumValue))
                    {
                        result.Add(enumValue, enumText);
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 获取类属性上的自定义Text
        /// </summary>
        /// <typeparam name="T"></typeparam> 
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static string GetConstantText<T>(string fieldName) where T : class, new()
        {
            var result = string.Empty;
            try
            {
                Type objType = typeof(T);

                MemberInfo[] minfos = objType.GetMembers(BindingFlags.Static | BindingFlags.Public);
                foreach (MemberInfo item in minfos)
                {
                    if (fieldName == item.Name)
                    {
                        var attrInfos = item.GetCustomAttributes(typeof(EnumTextAttribute), true);
                        EnumTextAttribute attr = attrInfos[0] as EnumTextAttribute;
                        result = attr == null ? "" : attr.Value;
                        break;
                    }
                }
            }
            catch
            {
                throw;
            }
            return result;
        }

    }
}