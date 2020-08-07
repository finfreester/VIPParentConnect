using System;
using System.Configuration;
using System.Web;
using System.Net;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Web.Security;
using VIPDayCareCenters.Models;
using VIPDayCareCenters.Repository;
using VIPDayCareCenters.Enumerations;
using CodeBridgeSoftware.Infrastructure.Email;
using CodeBridgeSoftware.WebApps;
using CodeBridgeSoftware.CoreApps;
using System.Collections.Generic;

public class VIPDayCarePage : System.Web.UI.Page
{
    #region Fields

    public cSession _Session;

    #endregion Fields

    #region Ctor

    public VIPDayCarePage()
    {
        _Session = new cSession();
    }

    #endregion Ctor

    protected override void OnLoad(EventArgs e)
    {
        this.Page.ClientTarget = "uplevel";
        base.OnLoad(e);
    }

    #region Helper methods

    private void generateMessage(string message,
                                 string title = "",
                                 enumPageMessageTypes messageType = enumPageMessageTypes.INFO,
                                 bool sendEmail = false)
    {


        string friendlymessage = "Sorry for the inconvenience, but the application has encountered an unknown error. It doesn't appear to have affected your data, but our technical staff have been automatically notified and will be looking into this with the utmost urgency.  Thank you for your patience.";
        string jsFunction = string.Empty;
        string platform = ConfigurationManager.AppSettings["PlatformName"].ToUpper().Trim();
        bool inTestMode = (ConfigurationManager.AppSettings["TestMode"] == "on");

        message = message.Trim();

        if (string.IsNullOrEmpty(message))
            return;

        var emailMgr = new Emailer(ConfigurationManager.AppSettings["SMTPServer"],
                                   int.Parse(ConfigurationManager.AppSettings["MailPort"]),
                                   ConfigurationManager.AppSettings["notificationemailaddress"],
                                   new NetworkCredential(ConfigurationManager.AppSettings["MailUserId"], ConfigurationManager.AppSettings["MailPassword"]));

        emailMgr.isSecure = bool.Parse(ConfigurationManager.AppSettings["SendSecureMail"]);

        if ((emailMgr != null) && (sendEmail) && (messageType != enumPageMessageTypes.INFO))
        {

            emailMgr.priority = EmailConstants.enumMailPriority.High;
                                    
            emailMgr.Send(
                            ((!string.IsNullOrEmpty(platform)) ? "[" + platform + "]" : string.Empty) + title,
                            message,
                            ConfigurationManager.AppSettings["notificationemailaddress"]
                          );

        }

        message = message.Replace("\r\n", "<br />").Trim();

        if (messageType == enumPageMessageTypes.INFO)
        {
            jsFunction = string.Format("showInfoPopup(\"{0}\");", message);
        }
        else
        {
            if (!inTestMode && (messageType == enumPageMessageTypes.ERROR)) //means we are in Prod, so show friendly message
            {
                message = friendlymessage;
            }

            jsFunction = string.Format("showErrorPopup(\"{0}\", \"{1}\");", title, message);
        }

        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "ShowMessage", "$(document).ready(function() { " + jsFunction + " });", true);

    }

    protected void writeMessage(Exception ex)
    {

        //Do logging of exception details
        if (ex is CBSException) //Should be warning (logical or validation error), not a fatal error
        {
            this.generateMessage(ex.Message, string.Empty, enumPageMessageTypes.WARNING);
            return;
        }

        string szErrorMsg = string.Empty;
        string szMsg = string.Empty;

        if (ex.InnerException != null)
        {
            szErrorMsg = ex.Message + ": " + ex.InnerException.Message;
        }
        else
        {
            szErrorMsg = ex.Message;
        }

        szMsg = string.Format("Process Failed at {0:yyyy-MM-dd hh:mm:ss tt} in {1}\r\nException in {2} - [{3}]\r\n\r\n{4}", DateTime.Now, ex.TargetSite.Name, ex.Source, szErrorMsg, ex.StackTrace);

        this.generateMessage(szMsg, "VIP Parent Connect Exception Occurred", enumPageMessageTypes.ERROR, true);

    }

    protected void writeMessage(string message, enumPageMessageTypes messageType = enumPageMessageTypes.INFO)
    {
        this.generateMessage(message, string.Empty, messageType);
    }

    public bool isUserLoggedIn()
    {

        return (HttpContext.Current.User != null) && HttpContext.Current.User.Identity.IsAuthenticated;

    }

    public UserDTO LoggedInUserData()
    {

        if (!this.isUserLoggedIn())
            return null;

        var member = Membership.GetUser(HttpContext.Current.User.Identity.Name);

        var repo = new DataRepo();

        return repo.getUserByUserId(new Guid(member.ProviderUserKey.ToString()));

    }

    #endregion Helper methods
}
