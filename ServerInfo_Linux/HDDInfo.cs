using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerInfo_Linux
{
    public class HDDInfo
    {
        /// <summary>
        /// 硬盘大小
        /// </summary>
        public string Size { get; set; }

        /// <summary>
        /// 已使用大小
        /// </summary>
        public string Used { get; set; }

        /// <summary>
        /// 可用大小
        /// </summary>
        public string Avail { get; set; }

        /// <summary>
        /// 使用率
        /// </summary>
        public string Usage { get; set; }
    }
}
