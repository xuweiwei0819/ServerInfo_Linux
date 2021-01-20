using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace ServerInfo_Linux
{
    /// <summary>
    /// 非Zabbix
    /// </summary>
    public class ServerSystemInfoApp
    {
        public ServiceData Get()
        {
            var serviceData = new ServiceData();
            serviceData.Cpu = ReadCpuUsage().ToString();
            serviceData.DiskTotal = ReadHddInfo().Size.ToString();
            serviceData.DiskUsed = ReadHddInfo().Used.ToString();
            serviceData.DiskUnUsed =(decimal.Parse( ReadHddInfo().Size)- decimal.Parse(ReadHddInfo().Used)).ToString();
            serviceData.Memory = ((1-decimal.Parse(ReadMemInfo().Available)/decimal.Parse(ReadMemInfo().Total))*100).ToString("f2");
            serviceData.CurrentTime = DateTime.Now;
            return serviceData;
        }


        /// <summary>
        /// 获取ip地址
        /// </summary>
        /// <returns></returns>
        public static string IpAddress()
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo("top", "-b -n1")
            };
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.Start();
            var ip = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            process.Dispose();
            return ip;
        }

        /// <summary>
        /// 获取内存信息：linux
        /// </summary>
        /// <returns></returns>
        public static MemInfo ReadMemInfo()
        {
            MemInfo memInfo = new MemInfo();
            const string CPU_FILE_PATH = "/proc/meminfo";
            var mem_file_info = File.ReadAllText(CPU_FILE_PATH);
            var lines = mem_file_info.Split(new[] { '\n' });
            mem_file_info = string.Empty;

            int count = 0;
            foreach (var item in lines)
            {
                if (item.StartsWith("MemTotal:"))
                {
                    count++;
                    var tt = item.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    memInfo.Total = tt[1].Trim().Substring(0, tt[1].Trim().IndexOf("kB") - 1);
                }
                else if (item.StartsWith("MemAvailable:"))
                {
                    count++;
                    var tt = item.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    memInfo.Available = tt[1].Trim().Substring(0,tt[1].Trim().IndexOf("kB")-1);
                }
                if (count >= 2) break;
            }
            return memInfo;
        }

        /// <summary>
        /// 读取CPU使用率信息：linux
        /// </summary>
        /// <returns></returns>
        public static decimal ReadCpuUsage()
        {
            decimal value = 0;
            var process = new Process
            {
                StartInfo = new ProcessStartInfo("top", "-b -n1")
            };
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.Start();
            var cpuInfo = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            process.Dispose();

            var lines = cpuInfo.Split('\n');
            bool flags = false;
            foreach (var item in lines)
            {
                if (!flags)
                {
                    if (item.Contains("PID USER"))
                    {
                        flags = true;
                    }
                }
                else
                {
                    var li = item.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < li.Length; i++)
                    {
                        if (li[i] == "R" || li[i] == "S")
                        {
                            value += decimal.Parse(li[i + 1]);
                            break;
                        }
                    }
                }
            }
            decimal r = Math.Round(value / 4 , 2);     //4核
            if (r > 100) r = 100;
            return r;
        }

        /// <summary>
        /// 读取硬盘信息：linux
        /// </summary>
        /// <returns></returns>
        public static HDDInfo ReadHddInfo()
        {
            HDDInfo hdd = null;
            var process = new Process
            {
                StartInfo = new ProcessStartInfo("df", "-h /")
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                }
            };
            process.Start();
            var hddInfo = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            process.Dispose();

            var lines = hddInfo.Split('\n');
            foreach (var item in lines)
            {
                if (item.Contains("/dev/vda1") || item.Contains("/dev/sda4") || item.Contains("/dev/mapper/cl-root") || item.Contains("/dev/mapper/centos-root"))
                {
                    var li = item.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < li.Length; i++)
                    {
                        if (li[i].Contains("%"))
                        {
                            hdd = new HDDInfo()
                            {
                                Size = li[i - 3].Substring(0, li[i - 3].IndexOf("G")),
                                Used = li[i - 2].Substring(0, li[i - 2].IndexOf("G")),
                                Avail = li[i - 1].Substring(0, li[i - 1].IndexOf("G")),
                                Usage = li[i]
                            };
                            break;
                        }
                    }
                }
            }
            return hdd;
        }
    }
}
