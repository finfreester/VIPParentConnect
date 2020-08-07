using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using VIPDayCareCenters.Models;
using VIPDayCareCenters.Repository;
using VIPDayCareCenters;
using CodeBridgeSoftware.CoreApps;

namespace VIPDayCareCenters.Account
{
    public partial class Login : VIPDayCarePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (this.IsPostBack)
                return;

            this.pnlLogin.Visible = true;
            this.pnlTeacherLogin.Visible = false;
            this.butLogin.Text = "Log in";

            //RegisterHyperLink.NavigateUrl = "Register.aspx";
            //// Enable this once you have account confirmation enabled for password reset functionality
            ////ForgotPasswordHyperLink.NavigateUrl = "Forgot";

            //var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
            //if (!String.IsNullOrEmpty(returnUrl))
            //{
            //    RegisterHyperLink.NavigateUrl += "?ReturnUrl=" + returnUrl;
            //}
        }

        protected void ddlCenter_SelectedIndexChanged(object sender, EventArgs e)
        {

            int centerId = int.Parse(this.ddlCenter.SelectedValue);

            this.ddlGroup.Enabled = (centerId > 0);
            this.populateGroupDDL(centerId);

        }

        private void populateCenterDDL()
        {
            var repo = new DataRepo();

            this.ddlCenter.DataSource = repo.getCenterLookup();
            this.ddlCenter.DataTextField = "Value";
            this.ddlCenter.DataValueField = "Key";
            this.ddlCenter.DataBind();

            this.ddlCenter.Items.Insert(0, new ListItem("[-- select one --]", "0"));
        }

        private void populateGroupDDL(int centerId)
        {
            var repo = new DataRepo();

            var user = (UserDTO)_Session.GetVar("User", null);

            this.ddlGroup.DataSource = repo.getGroupsNotInUseByCenterIdLookup(centerId, user.Id);
            this.ddlGroup.DataTextField = "Value";
            this.ddlGroup.DataValueField = "Key";
            this.ddlGroup.DataBind();

            this.ddlGroup.Items.Insert(0, new ListItem("[-- select one --]", "0"));

            this.ddlGroup.SelectedIndex = 0;
        }

        protected void butLogin_Click(object sender, EventArgs e)
        {
            try
            {

                UserDTO user;

                var repo = new DataRepo();

                if (this.pnlLogin.Visible)
                {

                    _Session.RemoveVar("DAILY_GROUP_ID");

                    user = new UserDTO()
                    {
                        UserName = this.UserName.Text,
                        Password = this.Password.Text
                    };

                    user = repo.authenticateUser(user);

                    this._Session.SetVar("User", user);

                    if (user.Role.ToLower() == "teacher")
                    {

                        this.pnlLogin.Visible = false;
                        this.pnlTeacherLogin.Visible = true;
                        this.butLogin.Text = "Go";

                        this.populateCenterDDL();

                        this.ddlGroup.Enabled = false;

                        return;

                    }

                }
                else //Teacher Login so create or update DailyGroup record and redirect to menu
                {

                    user = (UserDTO)_Session.GetVar("User", null);

                    int dailyGroupId = int.Parse(_Session.GetVar("DAILY_GROUP_ID", 0).ToString());

                    DailyGroupDTO dailygroup = new DailyGroupDTO()
                    {
                        Id = dailyGroupId,
                        CenterId = int.Parse(this.ddlCenter.SelectedValue),
                        GroupId = int.Parse(this.ddlGroup.SelectedValue),
                        TeacherId = user.Id
                    };

                    dailyGroupId = repo.postDailyGroups(dailygroup);

                    _Session.SetVar("DAILY_GROUP_ID", dailyGroupId);

                }

                FormsAuthentication.RedirectFromLoginPage(this.UserName.Text, this.RememberMe.Checked);

            }
            catch (CBSException ex)
            {
                this.writeMessage(ex);
            }
            catch (Exception ex)
            {
                this.writeMessage(ex);
            }
        }
        
    }
}
