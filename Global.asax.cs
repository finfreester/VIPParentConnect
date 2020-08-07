using System;
using System.Web;
using System.Net.Configuration;
using System.Configuration;
using CodeBridgeSoftware.Infrastructure.Email;
using System.IO;

namespace VIPDayCareCenters
{
    public class Global : System.Web.HttpApplication
    {

        void Application_BeginRequest(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now);
            Response.Cache.SetNoStore();
            Response.Cache.SetMaxAge(new TimeSpan(0, 0, 30));
        }

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup

        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }

        void Session_Start(object sender, EventArgs e)
        {
        }

        void Session_End(object sender, EventArgs e)
        {
        }

    }
}
