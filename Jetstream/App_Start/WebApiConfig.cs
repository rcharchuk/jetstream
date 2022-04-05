﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Jetstream
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //Return JSON by default
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.MediaTypeMappings
                                .Add(new System.Net.Http.Formatting.RequestHeaderMapping("Accept",
                                "text/html",
                                StringComparison.InvariantCultureIgnoreCase,
                                true,
                                "application/json"));

            //Enable CORS
            config.EnableCors(new EnableCorsAttribute("*", "*", "GET"));
          
            // Web API routes
            config.MapHttpAttributeRoutes();
        }
    }
}
