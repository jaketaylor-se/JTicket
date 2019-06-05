/*
 *  Configurations for the JTicket RESTful Web API.
 */

using System.Web.Http;
using Newtonsoft.Json.Serialization;

namespace JTicket
{
    /// <summary>
    /// Class 
    /// <c>WebApiConfig</c> 
    /// Configuration for the RESTful Web API for JTicket.
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Method 
        /// <c>Register</c> 
        /// Register HTTP routes for Web API.
        /// </summary>
        public static void Register(HttpConfiguration config)
        {
            // Formatting settings. The goal is to make the JSON 
            // returned from the Web API more digestable for JavaScript.
            var settings = config.Formatters.JsonFormatter.SerializerSettings;
            settings.ContractResolver =    // Format to Camel Case 
                new CamelCasePropertyNamesContractResolver();
            settings.Formatting = Newtonsoft.Json.Formatting.Indented;

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute
            (
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new
                {
                    id = RouteParameter.Optional
                }
            );
        }
    }
}
