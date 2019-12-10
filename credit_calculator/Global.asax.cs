using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace credit_calculator
{
    public class MvcApplication : System.Web.HttpApplication
    {

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            System.Globalization.CultureInfo nf_info = new System.Globalization.CultureInfo("en-US", false);
            nf_info.NumberFormat.NumberDecimalSeparator = ".";
            nf_info.NumberFormat.NumberGroupSeparator = " ";
            nf_info.NumberFormat.CurrencyDecimalSeparator = ".";
            nf_info.NumberFormat.CurrencyGroupSeparator = " ";
            System.Threading.Thread.CurrentThread.CurrentCulture = nf_info;

        }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
