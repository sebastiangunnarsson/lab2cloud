using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Newtonsoft.Json.Linq;

namespace Lab2.Helpers
{
    public class CachingAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);

            if (actionContext.Request.Method == HttpMethod.Get)
            {
                var json = CacheHelper.Get<string>(actionContext.Request.RequestUri.ToString());
                
                if (json != null)
                {
                    
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK);
                    actionContext.Response.Content = new StringContent(json);
                    actionContext.Response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    actionContext.Response.Headers.Age = TimeSpan.FromSeconds(22);
                }
            }
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
            string routeprefix = GetRoutePrefix(actionExecutedContext.ActionContext);
            var method = actionExecutedContext.Request.Method;
            var url = actionExecutedContext.Request.RequestUri.ToString();
            if(method == HttpMethod.Get)
                CacheHelper.Save(url, actionExecutedContext.Response.Content.ReadAsStringAsync().Result);
            
            if (method == HttpMethod.Post || method == HttpMethod.Put || method.Equals(new HttpMethod("PATCH")) ||
                method == HttpMethod.Delete)
            {
                CacheHelper.Delete(url);
                CacheHelper.Delete(routeprefix);
            }
            
        }

        private string GetRoutePrefix(HttpActionContext ac)
        {
            var controllerCtx = ac.ControllerContext;
            var requestUri = ac.Request.RequestUri;
            Type controllerType = controllerCtx.Controller.GetType();

            var routePrefixAttribute = (RoutePrefixAttribute)Attribute.GetCustomAttribute(
            controllerType, typeof(RoutePrefixAttribute));
            var prefix = routePrefixAttribute.Prefix;
            if (routePrefixAttribute.Prefix.Contains("vacationId"))
                prefix = routePrefixAttribute.Prefix.Replace("{vacationId:int}",
                    ac.RequestContext.RouteData.Values["vacationId"].ToString());

            if(requestUri.IsDefaultPort)
                return string.Format("{0}://{1}/{2}", requestUri.Scheme, requestUri.Host, prefix);
            return string.Format("{0}://{1}:{2}/{3}",requestUri.Scheme,requestUri.Host,requestUri.Port,prefix);
        }
    }
}