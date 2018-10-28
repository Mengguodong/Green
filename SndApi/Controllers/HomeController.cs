using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SndApi.Controllers
{
    public class HomeController : ApiController
    {
        [HttpGet]
        public bool Index() 
        {
            bool isTrue = false;
            return true;
        }
    }
}
