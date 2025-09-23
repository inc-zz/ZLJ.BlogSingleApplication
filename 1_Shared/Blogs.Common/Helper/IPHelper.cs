using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace  Blogs.Core
{
    public class IPHelper
    {
        /// <summary>
        /// 获得IP地址
        /// </summary>
        /// <returns>字符串数组</returns>
        public static string GetIp()
        {
            HttpContextAccessor _context = new HttpContextAccessor();
            if (_context.HttpContext == null)
                return string.Empty;

            var ip = _context.HttpContext.Request.Headers["X-Forwarded-For"].ToString();
            if (string.IsNullOrEmpty(ip))
            {
                ip = _context.HttpContext.Connection.RemoteIpAddress.ToString();
            }
            if (ip == "::1")
            {
                ip = "127.0.0.1";
            }
            return ip;
        }

        /// <summary>
        /// 获取Mac地址
        /// </summary>
        /// <returns></returns>
        public static string GetMacAddress()
        {
            NetworkInterface[] networks = NetworkInterface.GetAllNetworkInterfaces();
            if (networks.Length == 0)
            {
                return string.Empty;
            }
            var macAddress = string.Empty;
            var effectiveNetworks = networks.Where(it => it.NetworkInterfaceType == NetworkInterfaceType.Loopback && it.OperationalStatus == OperationalStatus.Up).ToList();
            foreach (NetworkInterface adapter in effectiveNetworks)
            {
                PhysicalAddress address1 = adapter.GetPhysicalAddress();
                macAddress = address1?.ToString();
                if (string.IsNullOrWhiteSpace(macAddress))
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    var unicastAddress = properties.UnicastAddresses;
                    if (unicastAddress.Any(it => it.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork))
                    {
                        var address2 = adapter.GetPhysicalAddress().ToString();
                        macAddress = address2?.ToString();
                    }
                }
            }
            return macAddress;
        }

    }
}
