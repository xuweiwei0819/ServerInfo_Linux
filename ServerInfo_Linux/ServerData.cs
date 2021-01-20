using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerInfo_Linux
{
    public class ServiceData
    {
        /// <summary>
        /// Cpu使用率
        /// </summary>
        /// <returns></returns>
        public string Cpu { get; set; }

        /// <summary>
        /// Memory使用率
        /// </summary>
        /// <returns></returns>
        public string Memory { get; set; }
        /// <summary>
        /// Disk总大小b
        /// </summary>
        /// <returns></returns>
        public string DiskTotal { get; set; }
        /// <summary>
        /// Disk已使用大小b
        /// </summary>
        /// <returns></returns>
        public string DiskUsed { get; set; }
        /// <summary>
        /// Disk未使用大小b
        /// </summary>
        /// <returns></returns>
        public string DiskUnUsed { get; set; }
        public DateTime CurrentTime { get; set; }

        public ServiceData()
        {
        }
    }
}
