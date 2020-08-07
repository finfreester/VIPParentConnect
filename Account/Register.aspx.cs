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
    public partial class Register : VIPDayCarePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            UserDTO user = this.LoggedInUserData();

            if (this.IsPostBack)
                return;

            this.gvUsers.Visible = ((user != null) && (user.Role.ToUpper() == "ADMIN"));

            this.populateRoleDDL();

            this.hidvipUserId.Value = string.Empty;

            this.bindData();

        }

        private void clearFields()
        {

            this.FirstName.Text = string.Empty;
            this.LastName.Text = string.Empty;
            this.UserName.Text = string.Empty;
            this.Email.Text = string.Empty;
            this.Role.SelectedIndex = 0;
            this.chkIsActive.Checked = true;

            this.UserName.Enabled = true;
            this.butSave.Text = "Add";

        }

        private void bindData()
        {
            var repo = new DataRepo();

            this.gvUsers.DataSource = repo.getAllUsers();
            this.gvUsers.DataBind();

            this.clearFields();
        }

        private void populateRoleDDL()
        {
            var repo = new DataRepo();

            this.Role.DataSource = repo.getRoles();
            this.Role.DataTextField = "Value";
            this.Role.DataValueField = "Value";
            this.Role.DataBind();

            //Remove Admin and Manager Roles
            if (!this.gvUsers.Visible)
            {
                this.Role.Items.RemoveAt(0);
                this.Role.Items.RemoveAt(0);
            }

            this.Role.Items.Insert(0, new ListItem("[-- select one --]", string.Empty));
        }

        protected void butSave_Click(object sender, EventArgs e)
        {

            try
            {

                var repo = new DataRepo();

                var user = new UserDTO()
                {
                    FirstName = this.FirstName.Text,
                    LastName = this.LastName.Text,
                    UserName = this.UserName.Text,
                    Email = this.Email.Text,
                    isActive = this.chkIsActive.Checked,
                    Role = this.Role.SelectedValue
                };

                int vipUserId = 0;

                int.TryParse(this.hidvipUserId.Value, out vipUserId);

                if (vipUserId == 0)
                {
                    string username = repo.postUser(user);

                    if (string.Compare(username, user.UserName, true) != 0)
                    {
                        this.writeMessage(string.Format("The user was created successfully.  However, the user name {0} was already assigned, so the new user was assigned the similar user name, {1}.", this.UserName.Text, username), Enumerations.enumPageMessageTypes.ERROR);
                    }
                    else
                    {
                        this.writeMessage("The user was created successfully.");
                    }
                }
                else
                {
                    repo.putUser(vipUserId, user);
                    this.writeMessage("The user was updated successfully.");
                }

                this.UserName.Enabled = true;
                this.butSave.Text = "Add";
                this.hidvipUserId.Value = string.Empty;

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

        protected void butReset_Click(object sender, EventArgs e)
        {
            this.clearFields();
        }
        
        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                // Convert the row index stored in the CommandArgument
                // property to an Integer.
                int index = int.Parse(e.CommandArgument.ToString());

                // Retrieve the row that contains the button clicked 
                // by the user from the Rows collection.
                GridViewRow row = this.gvUsers.Rows[index];

                this.hidvipUserId.Value = this.gvUsers.DataKeys[index].Values["Id"].ToString();

                this.chkIsActive.Checked = ((CheckBox)row.Cells[2].Controls[0]).Checked;
                this.FirstName.Text = row.Cells[3].Text;
                this.LastName.Text = row.Cells[4].Text;
                this.Role.SelectedValue = row.Cells[5].Text;
                this.UserName.Text = row.Cells[6].Text;
                this.Email.Text = row.Cells[7].Text;

                this.UserName.Enabled = false;
                this.butSave.Text = "Save";

                e.Handled = true;
            }
        }

        protected void gvUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvUsers.PageIndex = e.NewPageIndex;
            this.bindData();
        }

        protected void gvUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int vipUserId = int.Parse(this.gvUsers.DataKeys[e.RowIndex].Values["Id"].ToString());

            var repo = new DataRepo();

            repo.deleteUser(vipUserId);

            this.writeMessage("The user was deleted successfully.");

            this.bindData();

        }

    }
}
