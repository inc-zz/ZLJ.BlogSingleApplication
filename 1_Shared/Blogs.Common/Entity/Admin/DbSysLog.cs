using System;
using SqlSugar;

namespace Blogs.Core.Entity.Admin
{
    /// <summary>
    /// 操作日志
    /// </summary> 
    [SugarTable("sys_log")]
    public class DbSysLog : BaseEntity
    {
        ///<summary>
        ///  请求Action
        ///</summary>
        public string? Action { set; get; }
        ///<summary>
        ///  执行动作
        ///</summary>
        public string? Operating { set; get; }
        ///<summary>
        ///  请求IP
        ///</summary>
        public string? IP { set; get; }
        ///<summary>
        ///  日志信息
        ///</summary>
        public string? Message { set; get; }
        ///<summary>
        ///  请求地址
        ///</summary>
        public string? RequestUrl { set; get; }
        ///<summary>
        ///  请求参数
        ///</summary>
        public string? RequestParam { set; get; }
        ///<summary>
        ///  返回信息
        ///</summary>
        public string? Result { set; get; }
        ///<summary>
        ///  客户端信息
        ///</summary>
        public string? ClientInfo { set; get; }
    }
}
