using System;

namespace NCD.Common
{
    /// <summary>
    /// 生成器（分布式唯一 ID）
    /// 64 位 ID 结构：
    /// [1位符号位] + [41位时间戳] + [5位数据中心ID] + [5位机器ID] + [12位序列号]
    /// 可保证全局唯一、时间有序、高并发（每秒约 4096 x 1000 = 400万 ID）
    /// </summary>
    public class IdWorkerUtils
    {
        // ====================== 配置参数 ======================
        private const int TimestampBits = 41;   // 时间戳位数（支持约69年）
        private const int DatacenterIdBits = 5; // 数据中心ID位数
        private const int WorkerIdBits = 5;     // 机器ID位数
        private const int SequenceBits = 12;    // 序列号位数

        private const long MaxDatacenterId = -1L ^ (-1L << DatacenterIdBits);
        private const long MaxWorkerId = -1L ^ (-1L << WorkerIdBits);
        private const long SequenceMask = -1L ^ (-1L << SequenceBits);

        private const int WorkerIdShift = SequenceBits;
        private const int DatacenterIdShift = SequenceBits + WorkerIdBits;
        private const int TimestampLeftShift = SequenceBits + WorkerIdBits + DatacenterIdBits;

        // 起始时间戳（2021-01-01T00:00:00Z），可根据项目需要调整
        private const long Twepoch = 1609459200000L; // 2021-01-01 00:00:00 UTC

        // ====================== 实例字段 ======================
        private readonly long _workerId;
        private readonly long _datacenterId;
        private long _sequence = 0L;
        private long _lastTimestamp = -1L;

        // 静态实例用于单例模式
        private static IdWorkerUtils _instance;
        private static readonly object _initLock = new object();

        // ====================== 构造函数 ======================
        /// <summary>
        /// 创建 Snowflake ID 生成器
        /// </summary>
        /// <param name="workerId">机器ID (0 <= workerId <= 31)</param>
        /// <param name="datacenterId">数据中心ID (0 <= datacenterId <= 31)</param>
        public IdWorkerUtils(long workerId = 0, long datacenterId = 0)
        {
            // 从环境变量获取或使用默认值
            _workerId = GetConfigValue("SNOWFLAKE_WORKER_ID", workerId);
            _datacenterId = GetConfigValue("SNOWFLAKE_DATACENTER_ID", datacenterId);

            if (_workerId > MaxWorkerId || _workerId < 0)
                throw new ArgumentException($"Worker ID 不能大于 {MaxWorkerId} 或小于 0，当前值: {_workerId}");

            if (_datacenterId > MaxDatacenterId || _datacenterId < 0)
                throw new ArgumentException($"Datacenter ID 不能大于 {MaxDatacenterId} 或小于 0，当前值: {_datacenterId}");

            Console.WriteLine($"Snowflake ID 生成器初始化完成: workerId={_workerId}, datacenterId={_datacenterId}");
        }

        // ====================== 静态方法 ======================
        /// <summary>
        /// 初始化ID生成器（单例模式）
        /// </summary>
        /// <param name="workerId">机器ID，默认为0</param>
        /// <param name="datacenterId">数据中心ID，默认为0</param>
        /// <returns>ID生成器实例</returns>
        public static IdWorkerUtils Initialize(long workerId = 0, long datacenterId = 0)
        {
            if (_instance != null)
                return _instance;

            lock (_initLock)
            {
                if (_instance != null)
                    return _instance;

                _instance = new IdWorkerUtils(workerId, datacenterId);
                return _instance;
            }
        }

        /// <summary>
        /// 获取ID生成器实例（必须先调用Initialize）
        /// </summary>
        public static IdWorkerUtils Instance
        {
            get
            {
                if (_instance == null)
                    throw new InvalidOperationException("请先调用IdWorkerUtils.Initialize()方法初始化");

                return _instance;
            }
        }

        // ====================== 核心方法 ======================
        /// <summary>
        /// 生成下一个唯一 ID
        /// </summary>
        /// <returns>64位唯一 long 类型 ID</returns>
        public long NextId()
        {
            lock (this)
            {
                long timestamp = GetCurrentTimestamp();

                // 时钟回拨处理
                if (timestamp < _lastTimestamp)
                {
                    throw new InvalidOperationException(
                        $"时钟回拨：无法生成 ID，当前时间戳 {timestamp} 小于上次生成时间 {_lastTimestamp}。回拨差值: {_lastTimestamp - timestamp}ms");
                }

                // 同一毫秒内生成多个 ID
                if (timestamp == _lastTimestamp)
                {
                    _sequence = (_sequence + 1) & SequenceMask;
                    if (_sequence == 0)
                    {
                        // 当前毫秒序列已满，等待下一毫秒
                        timestamp = WaitNextMillis(_lastTimestamp);
                    }
                }
                else
                {
                    // 新的一毫秒，序列重置
                    _sequence = 0L;
                }

                _lastTimestamp = timestamp;

                return ((timestamp - Twepoch) << TimestampLeftShift) |
                       (_datacenterId << DatacenterIdShift) |
                       (_workerId << WorkerIdShift) |
                       _sequence;
            }
        }

        /// <summary>
        /// 批量生成多个ID
        /// </summary>
        /// <param name="count">要生成的ID数量</param>
        /// <returns>ID数组</returns>
        public long[] NextIds(int count)
        {
            if (count <= 0)
                throw new ArgumentException("生成数量必须大于0", nameof(count));

            if (count > 10000)
                throw new ArgumentException("单次生成数量不能超过10000", nameof(count));

            var ids = new long[count];
            for (int i = 0; i < count; i++)
            {
                ids[i] = NextId();
            }
            return ids;
        }

        /// <summary>
        /// 等待直到下一毫秒
        /// </summary>
        private static long WaitNextMillis(long lastTimestamp)
        {
            long timestamp;
            do
            {
                timestamp = GetCurrentTimestamp();
                Thread.Sleep(0); // 让出时间片，避免忙等待
            } while (timestamp <= lastTimestamp);

            return timestamp;
        }

        /// <summary>
        /// 获取当前时间戳（毫秒）
        /// </summary>
        private static long GetCurrentTimestamp() => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        /// <summary>
        /// 从环境变量获取配置值，如果没有则使用默认值
        /// </summary>
        private static long GetConfigValue(string envVarName, long defaultValue)
        {
            var envValue = Environment.GetEnvironmentVariable(envVarName);
            if (!string.IsNullOrEmpty(envValue) && long.TryParse(envValue, out long result))
                return result;

            return defaultValue;
        }

        /// <summary>
        /// 解析雪花ID，获取其组成部分
        /// </summary>
        /// <param name="id">雪花ID</param>
        /// <returns>包含各组成部分的对象</returns>
        public static (long Timestamp, long DatacenterId, long WorkerId, long Sequence) ParseId(long id)
        {
            long timestamp = (id >> TimestampLeftShift) + Twepoch;
            long datacenterId = (id >> DatacenterIdShift) & MaxDatacenterId;
            long workerId = (id >> WorkerIdShift) & MaxWorkerId;
            long sequence = id & SequenceMask;

            return (timestamp, datacenterId, workerId, sequence);
        }

        /// <summary>
        /// 获取当前配置的WorkerId和DatacenterId
        /// </summary>
        public (long WorkerId, long DatacenterId) GetCurrentConfig()
        {
            return (_workerId, _datacenterId);
        }
    }
}