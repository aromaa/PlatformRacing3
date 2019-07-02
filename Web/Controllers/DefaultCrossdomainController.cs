using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platform_Racing_3_Web.Controllers
{
    [Route("crossdomain.xml")]
    public class DefaultCrossdomainController
    {
        [HttpGet]
        public string Get()
        {
            //Use proxies to server static page, for development purposes
            return @"<?xml version=""1.0""?><cross-domain-policy><allow-access-from domain=""*""/></cross-domain-policy>";
        }
    }
}
