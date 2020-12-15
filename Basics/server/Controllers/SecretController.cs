using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Controllers
{
    public class SecretController:Controller
    {
        public string Index()
        {
            return "secret message";
        }
    }
}
