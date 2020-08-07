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

namespace VIPDayCareCenters.Manage
{
    public partial class Centers : VIPDayCarePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            if (this.IsPostBack)
                return;

            this.hidCenterId.Value = string.Empty;

            this.populateManagerDDL();

            this.bindData();

        }

        private void clearFields()
        {

            this.CenterName.Text = string.Empty;
            this.ddlManager.SelectedIndex = 0;
            this.Address.Text = string.Empty;
            this.City.Text = string.Empty;
            this.State.SelectedIndex = 0;
            this.Zip.Text = string.Empty;
            this.Phone.Text = string.Empty;

            this.butSave.Text = "Add";
          
        }

        private void bindData()
        {
            var repo = new DataRepo();

            this.gvCenters.DataSource = repo.getCenters();
            this.gvCenters.DataBind();

            this.clearFields();
        }

        private void populateManagerDDL()
        {
            var repo = new DataRepo();

            this.ddlManager.DataSource = repo.getManagersAndAdminsLookup();
            this.ddlManager.DataTextField = "Value";
            this.ddlManager.DataValueField = "Key";
            this.ddlManager.DataBind();
            this.ddlManager.Items.Insert(0, new ListItem("[-- select one --]", string.Empty));
        }

        protected void butSave_Click(object sender, EventArgs e)
        {
            try
            {

                var repo = new DataRepo();

                var center = new CenterDTO()
                {
                    Name = this.CenterName.Text,
                    ManagerId = int.Parse(this.ddlManager.SelectedValue),
                    Address = this.Address.Text,
                    City = this.City.Text,
                    State = this.State.SelectedValue,
                    Zip = this.Zip.Text,
                    Phone = this.Phone.Text
                };

                int centerId = 0;

                int.TryParse(this.hidCenterId.Value, out centerId);

                if (centerId == 0)
                {
                    repo.postCenter(center);
                    this.writeMessage("The center was created successfully.");
                }
                else
                {
                    repo.putCenter(centerId, center);
                    this.writeMessage("The center was updated successfully.");
                }

                this.butSave.Text = "Add";
                this.hidCenterId.Value = string.Empty;

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

        protected void gvCenters_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            try
            {

                int centerId = int.Parse(this.gvCenters.DataKeys[e.RowIndex].Values["Id"].ToString());

                var repo = new DataRepo();

                repo.deleteCenter(centerId);

                this.writeMessage("The center was deleted successfully.");

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
        
        protected void gvCenters_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                // Convert the row index stored in the CommandArgument
                // property to an Integer.
                int index = int.Parse(e.CommandArgument.ToString());

                // Retrieve the row that contains the button clicked 
                // by the user from the Rows collection.
                GridViewRow row = this.gvCenters.Rows[index];

                this.hidCenterId.Value = this.gvCenters.DataKeys[index].Values["Id"].ToString();

                this.CenterName.Text = row.Cells[2].Text;
                this.ddlManager.SelectedValue = (this.gvCenters.DataKeys[index].Values["ManagerId"] == null) ? string.Empty : this.gvCenters.DataKeys[index].Values["ManagerId"].ToString(); //If no manager, should be "" and should select 1st item in list
                this.Address.Text = row.Cells[4].Text;
                this.City.Text = row.Cells[5].Text;
                this.State.SelectedValue = row.Cells[6].Text;
                this.Zip.Text = row.Cells[7].Text;
                this.Phone.Text = cStringHelper.StripStringofChars(row.Cells[8].Text, " -()", string.Empty);

                this.butSave.Text = "Save";

                e.Handled = true;
            }
        }

        protected void gvCenters_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvCenters.PageIndex = e.NewPageIndex;
            this.bindData();
        }

        protected void gvCenters_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[7].Text.Length > 5)
                    e.Row.Cells[7].Text = e.Row.Cells[7].Text.Substring(0, 5) + "-" + e.Row.Cells[7].Text.Substring(6, 4);

                e.Row.Cells[8].Text = string.Format("{0:(###) ###-####}", long.Parse(e.Row.Cells[8].Text));
            }  
        }
                
        protected void butReset_Click(object sender, EventArgs e)
        {
            this.clearFields();
        }

    }
}
