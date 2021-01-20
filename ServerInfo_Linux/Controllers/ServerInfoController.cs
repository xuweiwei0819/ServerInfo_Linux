using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ServerInfo_Linux.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ServerInfoController : ControllerBase
    {
        private ServerSystemInfoApp _app;
        public ServerInfoController(ServerSystemInfoApp app)
        {
            _app = app;
        }

        /// <summary>
        /// 获取服务器数据
        /// </summary>
        /// <param name="hostid"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<ServiceData>> GetServerInfo()
        {
            var result = new Response<ServiceData>();
            try
            {
                result.Result =  _app.Get();
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return result;
        }


    }
}