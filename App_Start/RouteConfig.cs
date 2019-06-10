/*
 *  Route Configurations. 
 */

using System.Web.Mvc;
using System.Web.Routing;

namespace JTicket
{
    /// <summary>
    /// Class 
    /// <c>RouteConfig</c> 
    /// Route Configuration class. 
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        /// Method 
        /// <c>RegisterRoutes</c> 
        /// Register the routes. Method is called upon application start.
        /// </summary>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute    // Default route
            (
                name: "ViewResolvedTickets",
                url: "ResolvedTickets/",
                defaults: new
                {
                    controller = "Ticket",          // Default to Home Controller
                    action = "ViewResolvedTickets",
                }
            );

            routes.MapRoute    // View open tickets
            (
                name: "ViewOpenTickets",
                url: "OpenTickets/",
                defaults: new
                {
                    controller = "Ticket",          // Default to Home Controller
                    action = "ViewOpenTickets",
                }
            );

            routes.MapRoute    // Default route
            (
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new
                {
                  controller = "Home",          // Default to Home Controller
                  action = "Index",
                  id = UrlParameter.Optional    // Id is optional
                }
            );
        }
    }
}
