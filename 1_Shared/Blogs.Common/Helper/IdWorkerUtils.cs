using System;

namespace NCD.Common
{
    /// <summary>
    /// 精简版雪花ID生成器，生成50位ID，输出为最长15位的十进制数
    /// </summary>
    public class IdWorkerUtils
    {
        private const int SequenceBits = 4;
        private const int WorkerIdBits = 5;
        private const int TimestampBits = 41;

        private const long MaxSequenceId = -1L ^ (-1L << SequenceBits); // 15
        private const long MaxWorkerId = -1L ^ (-1L << WorkerIdBits);   // 31

        private const int TimestampShift = SequenceBits + WorkerIdBits; // 9
        private const int WorkerIdShift = SequenceBits;                 // 4

        // 自定义纪元时间（2024-01-01 00:00:00 UTC）
        private static readonly DateTime CustomEpoch = new(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly long EpochMillis = (long)(CustomEpoch - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;

        private readonly long _workerId = 1;
        private long _lastTimestamp = -1L;
        private long _sequence = 0L;
        private readonly object _lock = new object();

        public IdWorkerUtils()
        {
        }

        public long NextId()
        {
            lock (_lock)
            {
                var timestamp = GetCurrentTimestamp();

                if (timestamp < _lastTimestamp)
                    throw new InvalidOperationException("Clock moved backwards!");

                if (timestamp == _lastTimestamp)
                {
                    _sequence = (_sequence + 1) & MaxSequenceId;
                    if (_sequence == 0)
                    {
                        // 阻塞到下一毫秒
                        timestamp = WaitNextMillis(_lastTimestamp);
                    }
                }
                else
                {
                    _sequence = 0;
                }

                _lastTimestamp = timestamp;

                var id = ((timestamp - EpochMillis) << TimestampShift)
                         | (_workerId << WorkerIdShift)
                         | _sequence;

                return id;
            }
        }

        private long GetCurrentTimestamp()
        {
            return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }

        private long WaitNextMillis(long lastTimestamp)
        {
            var timestamp = GetCurrentTimestamp();
            while (timestamp <= lastTimestamp)
            {
                timestamp = GetCurrentTimestamp();
            }
            return timestamp;
        }
    }
}