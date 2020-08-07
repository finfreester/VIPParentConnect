using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.Security;
using VIPDayCareCenters.Models;
using VIPDayCareCenters.Repository;
using System.Configuration;

namespace VIPDayCareCenters
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {

        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        protected void Page_Init(object sender, EventArgs e)
        {

            var url = this.ResolveUrl("~/");

            this.createScriptControl(url + "Scripts/bootstrap.min.js");
            this.createScriptControl(url + "Scripts/jquery-1.10.2.min.js");

            if (ScriptManager.GetCurrent(this.Page) != null)
            {

                ScriptManager.RegisterClientScriptInclude(this.Page, this.GetType(), "jquery", url + "Scripts/jquery-1.10.2.min.js");
                ScriptManager.RegisterClientScriptInclude(this.Page, this.GetType(), "bootstrap", url + "Scripts/bootstrap.min.js");

            }

            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {

            ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name;

            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }

        private void createScriptControl(string script)
        {

            var jqueryInclude = new HtmlGenericControl("script");
            jqueryInclude.Attributes.Add("type", "text/javascript");
            jqueryInclude.Attributes.Add("src", script);
            Page.Header.Controls.AddAt(0, jqueryInclude);

        }

        protected void LoginStatus_LoggingOut(object sender, LoginCancelEventArgs e)
        {

            var contentPage = ((VIPDayCarePage)this.Page);

            UserDTO user = contentPage.LoggedInUserData();

            if ((user != null) && (user.Role.ToLower() == "teacher"))
            {

                int dailyGroupId = int.Parse( contentPage._Session.GetVar("DAILY_GROUP_ID", 0).ToString());

                if (dailyGroupId > 0)
                {

                    var repo = new DataRepo();
                    repo.logoutDailyGroups(dailyGroupId);

                }

            }
            
            Session.Clear();
            FormsAuthentication.SignOut();
        }

    }
}
