namespace OSS.StorageApiService
{
    /// <summary>
    /// 操作类型
    /// </summary>
    public sealed class OssObjectHandleConstant
    {
        /// <summary>
        /// 只读
        /// </summary>
        public const string READ = "ReadOnly";
        /// <summary>
        /// 只写
        /// </summary>
        public const string WRITE = "WriteOnly";
        /// <summary>
        /// 读写
        /// </summary>
        public const string READ_WRITE = "ReadWrite";
        /// <summary>
        /// 完全控制
        /// </summary>
        public const string OWINER_CONTROL = "FurrControl";

    }
}
