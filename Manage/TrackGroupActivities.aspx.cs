using System;
using System.Configuration;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using VIPDayCareCenters;
using VIPDayCareCenters.Models;
using VIPDayCareCenters.Repository;
using CodeBridgeSoftware.CoreApps;
using CodeBridgeSoftware.WebApps;
using VIPDayCareCenters.Helpers;
using System.Web.UI.HtmlControls;
using CodeBridgeSoftware.Infrastructure.Email;
using System.Threading;

namespace VIPDayCareCenters.Manage
{
    public partial class TrackGroupActivities : VIPDayCarePage
    {

        #region Fields

        protected UserDTO _user;
        protected DailyGroupActivitiesDTO _dailyGroupActivities = null;

        #endregion Fields

        #region Properties

        protected int dailyGroupId
        {
            get
            {
                return (this.ViewState["DailyGroupId"] == null) ? 0 : int.Parse(this.ViewState["DailyGroupId"].ToString());
            }
            set { this.ViewState["DailyGroupId"] = value.ToString(); }
        }

        #endregion Properties

        #region Methods

        private void getGroupActivities()
        {

            var repo = new DataRepo();

            if (this.dailyGroupId > 0)
            {
                _dailyGroupActivities = repo.getAllDailyGroupData(this.dailyGroupId);
            }
            else //Manager or Admin
            {
                int centerid = int.Parse(this.ddlCenter.SelectedValue);
                int groupId = 0;

                int.TryParse(this.ddlGroup.SelectedValue, out groupId);

                if ((centerid > 0) && (groupId > 0))
                {
                    _dailyGroupActivities = repo.getDailyGroupActivitiesByCenterIdGroupId(centerid, groupId);
                    this.dailyGroupId = _dailyGroupActivities.DailyGroup.Id;
                }
                else
                {
                    _dailyGroupActivities = null;
                    this.dailyGroupId = 0;
                }

            }

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

            this.ddlGroup.DataSource = repo.getGroupsByCenterIdLookup(centerId);
            this.ddlGroup.DataTextField = "Value";
            this.ddlGroup.DataValueField = "Key";
            this.ddlGroup.DataBind();

            this.ddlGroup.Items.Insert(0, new ListItem("[-- select one --]", "0"));

            this.ddlGroup.SelectedIndex = 0;
        }

        private void bindData()
        {
            this.getGroupActivities();

            if (_dailyGroupActivities != null)
            {

                string role = _user.Role.ToLower();

                this.butApprove.Visible = ((role != "teacher") && (this.dailyGroupId > 0) && (_dailyGroupActivities.GroupChildren.Count > 0));
                this.butApprove.Enabled = (this.butApprove.Visible && (!_dailyGroupActivities.DailyGroup.IsApproved));
                this.butSave.Enabled = (
                                        ((role == "teacher") && (!_dailyGroupActivities.DailyGroup.IsSubmitted)) ||
                                        ((role != "teacher") && (!_dailyGroupActivities.DailyGroup.IsApproved))
                                       );
                this.butSubmit.Enabled = ((!_dailyGroupActivities.DailyGroup.IsSubmitted) && (!_dailyGroupActivities.DailyGroup.IsApproved));

                this.pnlGroupActivity.Enabled = this.butSave.Enabled;

            }

            this.rptGroupActivities.DataSource = (_dailyGroupActivities != null) ? _dailyGroupActivities.GroupChildren : null;
            this.rptGroupActivities.DataBind();
        }

        private void saveData()
        {
            int dailygroupId = this.dailyGroupId;

            var repo = new DataRepo();

            List<DailyGroupActivityDTO> dailyGroupActivity = repo.getDailyGroupActivitiesByDailyGroupId(dailygroupId);

            foreach (RepeaterItem item in this.rptGroupActivities.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {

                    PlaceHolder ph = (PlaceHolder)item.FindControl("phCell");
                    HiddenField hid = (HiddenField)item.FindControl("hidChildId");

                    int childId = int.Parse(hid.Value);

                    LinkedList<Control> llControls = new LinkedList<Control>();
                    cWebHelper.FindControlRecursive(ph, "ddl", ref llControls, false, false);

                    foreach (var ctl in llControls)
                    {

                        DropDownList ddl = (DropDownList)ctl;

                        int activityId = int.Parse(ddl.ID.Split("_".ToCharArray())[1]);
                        int activityValueId = int.Parse(ddl.SelectedValue);

                        DailyGroupActivityDTO dailyGroupactivity = (from dga in dailyGroupActivity
                                                                    where dga.ChildId == childId && dga.ActivityId == activityId
                                                                    select dga).FirstOrDefault();

                        if (dailyGroupactivity == null)
                        {

                            dailyGroupactivity = new DailyGroupActivityDTO()
                            {
                                DailyGroupActivityId = 0,
                                DailyGroupId = dailygroupId,
                                ChildId = childId,
                                ActivityId = activityId,
                                ActivityValueId = activityValueId
                            };

                            repo.postDailyGroupActivity(dailyGroupactivity);

                        }
                        else
                        {
                            repo.putDailyGroupActivity(dailyGroupactivity.DailyGroupActivityId, activityValueId);
                        }

                    }

                }
            }
        }

        #endregion Methods

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {

            _user = this.LoggedInUserData();

            if (this.IsPostBack)
                return;

            this.ddlGroup.Enabled = false;

            this.butApprove.Enabled = false;
            this.butSave.Enabled = false;
            this.butSubmit.Enabled = false;

            this.populateCenterDDL();

            var repo = new DataRepo();

            if (this.ClientQueryString.Contains("dg") && (this.dailyGroupId == 0)) //Manager coming from Approval Email
            {
                int dailyGroupId = int.Parse(cQCryptographyHelper.DecryptString(this.Request.QueryString["dg"]));
                _Session.SetVar("DAILY_GROUP_ID", dailyGroupId);
            }

            this.dailyGroupId = int.Parse(_Session.GetVar("DAILY_GROUP_ID", 0).ToString());

            string role = _user.Role.ToLower();

            if (this.dailyGroupId > 0) //Admin or Manager coming from Approval Email
            {

                DailyGroupDTO dailyGroup = repo.getDailyGroup(this.dailyGroupId);

                this.ddlCenter.SelectedValue = dailyGroup.CenterId.ToString();

                this.populateGroupDDL(dailyGroup.CenterId);
                this.ddlGroup.SelectedValue = dailyGroup.GroupId.ToString();

                this.bindData();

                this.pnlCenter.Enabled = false;
                this.ddlGroup.Enabled = ((role == "admin" || role == "manager") && (!this.ClientQueryString.Contains("dg")));

            }
            else
            {
                switch (role)
                {

                    case "manager":
                        var centerId = repo.getCentersByManagerIdLookup(_user.Id).FirstOrDefault().Key;

                        this.ddlCenter.SelectedValue = centerId.ToString();
                        this.pnlCenter.Enabled = false;

                        this.populateGroupDDL(centerId);
                        this.ddlGroup.Enabled = true;

                        break;
                }

            }

        }

        protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            _dailyGroupActivities = null;
            this.dailyGroupId = 0;

            this.bindData();
        }

        protected void ddlCenter_SelectedIndexChanged(object sender, EventArgs e)
        {

            int centerId = int.Parse(this.ddlCenter.SelectedValue);

            this.ddlGroup.Enabled = (centerId > 0);

            this.populateGroupDDL(centerId);

            _dailyGroupActivities = null;
            this.dailyGroupId = 0;

            this.bindData();
        }

        protected void rptGroupActivities_ItemCreated(object sender, RepeaterItemEventArgs e)
        {

            if (_dailyGroupActivities == null || _dailyGroupActivities.ChildActivityValues == null)
                this.getGroupActivities();

            if (e.Item.ItemType == ListItemType.Header)
            {

                PlaceHolder ph = (PlaceHolder)e.Item.FindControl("phHeaderCell");

                foreach (var activityName in _dailyGroupActivities.ChildActivityValues.Select(cav => cav.ActivityName).Distinct())
                {

                    TableCell tc = new TableCell();
                    tc.Text = activityName;
                    ph.Controls.Add(tc);

                }

                return;
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                
                PlaceHolder ph = (PlaceHolder)e.Item.FindControl("phCell");

                foreach (var activityId in _dailyGroupActivities.ChildActivityValues.Select(cav => cav.ActivityId).Distinct())
                {

                    TableCell tc = new TableCell();
                    tc.ID = "tc_" + activityId.ToString();
                    tc.CssClass = "HoverSelectAll";

                    DropDownList ddl = new DropDownList();
                    ddl.ID = "ddl_" + activityId.ToString();

                    var items = (from cav in _dailyGroupActivities.ChildActivityValues
                                 where cav.ActivityId == activityId
                                 orderby cav.ActivityValue ascending
                                 select cav).ToDictionary(k => k.ActivityValueId, v => v.ActivityValue);

                    ddl.DataSource = items;
                    ddl.DataTextField = "Value";
                    ddl.DataValueField = "Key";
                    ddl.DataBind();

                    ImageButton but = new ImageButton();
                    but.ID = "but_" + activityId.ToString();
                    but.ImageUrl = "~/images/selectall.png";
                    but.ImageAlign = ImageAlign.Middle;
                    but.CssClass = "ButtonSelectAll";
                    but.Click += but_Click;

                    tc.Controls.Add(ddl);
                    tc.Controls.Add(but);
                    ph.Controls.Add(tc);

                }
            }

        }

        protected void rptGroupActivities_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                ChildrenDTO child = (ChildrenDTO)e.Item.DataItem;

                int childId = child.Id;
                HiddenField hid = (HiddenField)e.Item.FindControl("hidChildId");
                hid.Value = childId.ToString();

                int DOW = (int)_dailyGroupActivities.DailyGroup.ClassDate.DayOfWeek;
                string childState = string.Empty;

                PlaceHolder ph = (PlaceHolder)e.Item.FindControl("phCell");

                LinkedList<Control> llControls = new LinkedList<Control>();
                cWebHelper.FindControlRecursive(ph, "ddl", ref llControls, false, false);

                foreach (var ctl in llControls)
                {

                    DropDownList ddl = (DropDownList)ctl;

                    int activityId = int.Parse(ddl.ID.Split("_".ToCharArray())[1]);

                    var selectedActivity = (from ca in _dailyGroupActivities.ChildDailyGroupActivities
                                            where ca.ActivityId == activityId && ca.ChildId == childId
                                            select ca.ActivityValueId.ToString()).FirstOrDefault();

                    if (string.IsNullOrEmpty(selectedActivity)) //No value selected, so use default
                    {

                        //Attendance In/Out?
                        if ((activityId == 8) && ((from gc in _dailyGroupActivities.GroupChildren
                                                   where gc.Id == childId && (gc.ExcludedDays.Contains(DOW.ToString()))
                                                   select gc.Id).Any()))
                        {

                            childState = "NotInClass";
                            ddl.SelectedValue = "26"; //Set Attendance value to out

                        }
                        else
                        {

                            ddl.SelectedValue = (from cav in _dailyGroupActivities.ChildActivityValues
                                                 where cav.ActivityId == activityId && cav.IsDefault
                                                 orderby cav.ActivityValue ascending
                                                 select cav.ActivityValueId.ToString()).FirstOrDefault();

                        }

                    }
                    else
                    {

                        ddl.SelectedValue = selectedActivity;

                        if ((from cga in _dailyGroupActivities.ChildDailyGroupActivities
                             where cga.ChildId == childId && cga.ActivityId == 8 && cga.ActivityValueId == 26
                             select cga.ChildId).Any())
                        {
                            childState = "NotInClass";
                        }

                    }

                    ddl.CssClass = childState;

                }

                Label lbl = (Label)e.Item.FindControl("lblName");
                lbl.Text = child.FirstName + " " + child.LastName.Substring(0, 1) + ".";
                lbl.CssClass = childState;

            }

        }

        protected void but_Click(object sender, EventArgs e)
        {

            try
            {

                ImageButton but = (ImageButton)sender;
                DropDownList ddl = null;

                var activityId = but.ID.Split("_".ToCharArray())[1];

                var ri = (RepeaterItem)but.NamingContainer;
                var ItemIndex = ri.ItemIndex;

                ddl = (DropDownList)ri.FindControl("ddl_" + activityId);
                var selectedValue = ddl.SelectedValue;

                for (int i = (ItemIndex + 1); i < this.rptGroupActivities.Items.Count; i++)
                {
                    ddl = (DropDownList)this.rptGroupActivities.Items[i].FindControl("ddl_" + activityId);
                    ddl.SelectedValue = selectedValue;
                }

                this.writeMessage("Select All applied successfully.");

            }
            catch (Exception ex)
            {
                this.writeMessage(ex);
            }
            finally
            {
                //this.bindData();
            }

        }

        protected void butApprove_Click(object sender, EventArgs e)
        {

            const int EMAIL_PAUSE = 10000; //10 seconds

            //Generate report and send it via email to parents if email on file
            try
            {

                //Save Submission
                this.saveData();

                this.getGroupActivities();

                string platform = ConfigurationManager.AppSettings["PlatformName"].ToUpper().Trim();
                bool inTestMode = (ConfigurationManager.AppSettings["TestMode"] == "on");
                string webAddress = ConfigurationManager.AppSettings["WebAddress"].ToString();
                
                var rf = new ReportFactory();

                var emailMgr = new Emailer(ConfigurationManager.AppSettings["SMTPServer"],
                                int.Parse(ConfigurationManager.AppSettings["MailPort"]),
                                ConfigurationManager.AppSettings["notificationemailaddress"],
                                new NetworkCredential(ConfigurationManager.AppSettings["MailUserId"], ConfigurationManager.AppSettings["MailPassword"]));

                emailMgr.isSecure = bool.Parse(ConfigurationManager.AppSettings["SendSecureMail"]);

                foreach (var child in (from gc in _dailyGroupActivities.GroupChildren
                                       orderby gc.Email ascending
                                       select gc).ToList())
                {

                    int childId = child.Id;

                    //Skip if child was marked absent or not in class today
                    if ((from cga in _dailyGroupActivities.ChildDailyGroupActivities
                         where cga.ChildId == childId && cga.ActivityId == 8 && cga.ActivityValueId == 26
                         select cga.ChildId).Any())
                    {
                        continue;
                    }

                    //Skip in production if parent did not provide email or does not want to be notified
                    if (!inTestMode && ((!child.Notify) || string.IsNullOrEmpty(child.Email)))
                        continue;

                    rf.generateReportCorrespondence("childactivity", "~/App_Data/CorrespondenceTemplate.xml", emailMgr, _dailyGroupActivities, child.Id, this.ddlCenter.SelectedItem.Text, this.ddlGroup.SelectedItem.Text, webAddress);

                }

                //Save Approval
                var repo = new DataRepo();

                _dailyGroupActivities.DailyGroup.IsSubmitted = true;
                _dailyGroupActivities.DailyGroup.IsApproved = true;
                _dailyGroupActivities.DailyGroup.TeacherId = _user.Id; //Should be a Manager or Admin id

                repo.postDailyGroups(_dailyGroupActivities.DailyGroup);

                this.writeMessage("Child Activity report(s) successfully sent to parent emails on file.");

            }
            catch (Exception ex)
            {
                this.writeMessage(ex);
            }
            finally
            {
                this.bindData();
            }

        }

        protected void butSave_Click(object sender, EventArgs e)
        {

            try
            {

                this.saveData();
                this.writeMessage("Group Activities successfully saved.");

            }
            catch (Exception ex)
            {
                this.writeMessage(ex);
            }
            finally
            {
                this.bindData();
            }

        }

        protected void butSubmit_Click(object sender, EventArgs e)
        {

            try
            {

                //Save Submission
                this.saveData();

                //Submit
                var repo = new DataRepo();

                _dailyGroupActivities.DailyGroup.IsSubmitted = true;

                repo.postDailyGroups(_dailyGroupActivities.DailyGroup);

                //Notify Manager via Email of submission

                //Get center in order to get manager; if no manager (rare), do not send email notification.
                CenterDTO center = repo.getCenterById(int.Parse(this.ddlCenter.SelectedValue));

                var emailMgr = new Emailer(ConfigurationManager.AppSettings["SMTPServer"],
                                  int.Parse(ConfigurationManager.AppSettings["MailPort"]),
                                  ConfigurationManager.AppSettings["notificationemailaddress"],
                                  new NetworkCredential(ConfigurationManager.AppSettings["MailUserId"], ConfigurationManager.AppSettings["MailPassword"]));

                emailMgr.isSecure = bool.Parse(ConfigurationManager.AppSettings["SendSecureMail"]);
                
                if ((emailMgr != null) && (center.ManagerId.HasValue))
                {

                    UserDTO mgr = repo.getUser((int)center.ManagerId);   

                    string platform = ConfigurationManager.AppSettings["PlatformName"].ToUpper().Trim();
                    bool inTestMode = (ConfigurationManager.AppSettings["TestMode"] == "on");
                    string subject = string.Format("VIP Parent Connect Daily Activities for Group {0} ready for review!", this.ddlGroup.SelectedItem.Text);
                    string queryString = Server.UrlEncode(cQCryptographyHelper.EncryptString(this.dailyGroupId.ToString()));
                    string message = string.Format("Hi {0},<br/><br/>Today’s ({1:dddd, MM/dd/yyyy}) Daily Activities for Group {2} are ready for managerial review <a href='{3}'>here</a>.<br/><br/><b>Issued from VIP Parent Connect</b>", mgr.FirstName, _dailyGroupActivities.DailyGroup.ClassDate, this.ddlGroup.SelectedItem.Text, ConfigurationManager.AppSettings["WebAddress"].ToString() + "/Manage/TrackGroupActivities.aspx?dg=" + queryString);
                    string recipient = (inTestMode) ? ConfigurationManager.AppSettings["TestModeEmail"] : mgr.Email;
                    
                    emailMgr.priority = EmailConstants.enumMailPriority.High;

                    emailMgr.Send(
                                    ((inTestMode) ? "[" + platform + "]" : string.Empty) + subject,
                                    message,
                                    recipient,
                                    null,
                                    (!inTestMode) ? ConfigurationManager.AppSettings["notificationemailaddress"] : null,
                                    true
                                  );

                }

                if (_user.Role.ToLower() == "teacher")
                {

                    repo.logoutDailyGroups(this.dailyGroupId);

                    Session.Clear();
                    FormsAuthentication.SignOut();

                    this.writeMessage("Group Activities successfully submitted.  You have been logged out.");

                }
                else
                {
                    this.writeMessage("Group Activities successfully submitted.");
                }

                this.bindData();

            }
            catch (Exception ex)
            {
                this.writeMessage(ex);
            }

        }

        #endregion Events

    }

}
