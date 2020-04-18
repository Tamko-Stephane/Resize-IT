using System.Web;
using System.Web.Mvc;

namespace SocialNetwork_Img_Resizer
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
