using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerInfo_Linux
{
    public class MemInfo
    {

        /// <summary>
        /// 总计内存大小
        /// </summary>
        public string Total { get; set; }
        /// <summary>
        /// 可用内存大小
        /// </summary>
        public string Available { get; set; }
    }
}
