using System.Web.Mvc;
using System.Web.Routing;

namespace SocialNetwork_Img_Resizer
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Upload",
                url: "upload",
                defaults: new { controller = "Resizer", action = "Index" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Resizer", action = "Index", id = UrlParameter.Optional }
            );


        }
    }
}
