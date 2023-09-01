using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace MVC_PhotoAlbum
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var formsIdentity = HttpContext.Current.User.Identity as FormsIdentity;

                if (formsIdentity != null)
                {
                    // 解析使用者票證中的角色資訊
                    string[] roles = formsIdentity.Ticket.UserData.Split(';');

                    // 建立使用者主體
                    HttpContext.Current.User = new GenericPrincipal(formsIdentity, roles);
                }
            }
        }
    }
}
