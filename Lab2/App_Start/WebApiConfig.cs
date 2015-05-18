using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Newtonsoft.Json;

namespace Lab2
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
        //    config.Services.Replace(typeof(IHttpControllerSelector), new NamespaceHttpControllerSelector(config)); //Funkar ej med HttpAttributesRouting
            // Web API routes
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{namespace}/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);


            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        }
    }
}
