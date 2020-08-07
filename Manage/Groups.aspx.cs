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
using System.Drawing;

namespace VIPDayCareCenters.Manage
{
    public partial class Groups : VIPDayCarePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            if (this.IsPostBack)
                return;

            this.hidGroupId.Value = string.Empty;

            this.populateCenterDDL();
            this.populateAgeCategoryDDL();

            this.ddlCenter.SelectedIndex = 0;

            this.bindData();

        }

        private void clearFields()
        {

            this.GroupName.Text = string.Empty;
            this.ddlAgeCategory.SelectedIndex = 0;

            this.butSave.Text = "Add";

        }

        private void bindData()
        {
            var repo = new DataRepo();

            var centerId = int.Parse(this.ddlCenter.SelectedValue);

            this.clearFields();
            this.enableControls(centerId > 0);

            this.gvGroups.DataSource = repo.getGroups(centerId);
            this.gvGroups.DataBind();
        }

        private void enableControls(bool enable) {

            this.GroupName.Enabled = this.ddlAgeCategory.Enabled = this.butSave.Enabled = enable;

            this.GroupName.BackColor = (enable) ? SystemColors.Window : SystemColors.ControlLight;
            this.ddlAgeCategory.BackColor = (enable) ? SystemColors.Window : SystemColors.ControlLight;
            
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

        private void populateAgeCategoryDDL()
        {
            var repo = new DataRepo();

            this.ddlAgeCategory.DataSource = repo.getAgeCategoryLookup();
            this.ddlAgeCategory.DataTextField = "Value";
            this.ddlAgeCategory.DataValueField = "Key";
            this.ddlAgeCategory.DataBind();
            this.ddlAgeCategory.Items.Insert(0, new ListItem("[-- select one --]", string.Empty));
        }

        protected void butSave_Click(object sender, EventArgs e)
        {
            try
            {

                var repo = new DataRepo();

                var Group = new GroupDTO()
                {
                    Name = this.GroupName.Text,
                    CenterId = int.Parse(this.ddlCenter.SelectedValue),
                    AgeCategoryId = int.Parse(this.ddlAgeCategory.SelectedValue),
                };

                int GroupId = 0;

                int.TryParse(this.hidGroupId.Value, out GroupId);

                if (GroupId == 0)
                {
                    repo.postGroup(Group);
                    this.writeMessage("The Group was created successfully.");
                }
                else
                {
                    repo.putGroup(GroupId, Group);
                    this.writeMessage("The Group was updated successfully.");
                }

                this.butSave.Text = "Add";
                this.hidGroupId.Value = string.Empty;

                this.bindData();

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

        protected void gvGroups_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            try
            {

                int GroupId = int.Parse(this.gvGroups.DataKeys[e.RowIndex].Values["Id"].ToString());

                var repo = new DataRepo();

                repo.deleteGroup(GroupId);

                this.writeMessage("The Group was deleted successfully.");

                this.bindData();

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
        
        protected void gvGroups_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                // Convert the row index stored in the CommandArgument
                // property to an Integer.
                int index = int.Parse(e.CommandArgument.ToString());

                // Retrieve the row that contains the button clicked 
                // by the user from the Rows collection.
                GridViewRow row = this.gvGroups.Rows[index];

                this.hidGroupId.Value = this.gvGroups.DataKeys[index].Values["Id"].ToString();

                this.GroupName.Text = row.Cells[2].Text;
                this.ddlAgeCategory.SelectedValue = this.gvGroups.DataKeys[index].Values["AgeCategoryId"].ToString();

                this.butSave.Text = "Save";

                e.Handled = true;
            }
        }

        protected void gvGroups_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvGroups.PageIndex = e.NewPageIndex;
            this.bindData();
        }
                
        protected void butReset_Click(object sender, EventArgs e)
        {
            this.clearFields();
        }

        protected void ddlCenter_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.bindData();
        }

    }
}
