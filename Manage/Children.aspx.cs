using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using VIPDayCareCenters.Models;
using VIPDayCareCenters.Repository;
using VIPDayCareCenters;
using CodeBridgeSoftware.CoreApps;
using VIPDayCareCenters.Helpers;

namespace VIPDayCareCenters.Manage
{
    public partial class Children : VIPDayCarePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            if (this.IsPostBack)
                return;

            this.populateStateDDL();
            this.populateCenterDDL();

            this.hidChildId.Value = string.Empty;

            this.ddlGroup.Enabled = false;

            ViewState["SortExpression"] = "FirstName";
            this.populateChildrenGrid(ViewState["SortExpression"].ToString(), this.gridViewSortDirection);

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

        private SortDirection gridViewSortDirection
        {
            get
            {
                if (ViewState["sortDirection"] == null)
                    ViewState["sortDirection"] = SortDirection.Ascending;
                return (SortDirection)ViewState["sortDirection"];
            }
            set { ViewState["sortDirection"] = value; }
        }

        private void clearFields()
        {

            this.ddlCenter.SelectedValue = "0";
            ddlCenter_SelectedIndexChanged(null, new EventArgs());

            this.FirstName.Text = string.Empty;
            this.LastName.Text = string.Empty;
            this.DOB.Text = string.Empty;
            this.lblDOB.Text = "DOB (Age: -)";
            
            this.chklstScheduledDays.Items[0].Selected = true;
            this.chklstScheduledDays.Items[1].Selected = true;
            this.chklstScheduledDays.Items[2].Selected = true;
            this.chklstScheduledDays.Items[3].Selected = true;
            this.chklstScheduledDays.Items[4].Selected = true;

            this.EnrollmentDate.Text = string.Empty;
            this.DismisalDate.Text = string.Empty;
            this.chkIsActive.Checked = true;

            this.ParentFirstName.Text = string.Empty;
            this.ParentLastName.Text = string.Empty;
            this.Address.Text = string.Empty;
            this.City.Text = string.Empty;
            this.State.SelectedIndex = 0;
            this.Zip.Text = string.Empty;
            this.Phone.Text = string.Empty;
            this.Email.Text = string.Empty;
            this.chkNotify.Checked = true;

            this.butSave.Text = "Add";

        }

        private void populateStateDDL()
        {
            var repo = new DataRepo();

            this.State.DataSource = repo.getStatesLookup();
            this.State.DataTextField = "Value";
            this.State.DataValueField = "Key";
            this.State.DataBind();

            this.State.Items.Insert(0, new ListItem("[-- select one --]", "0"));

            this.State.SelectedIndex = 0;
        }

        private string getExcludedScheduleDayNumbers()
        {

            string excludedDays = string.Empty;

            foreach (ListItem li in this.chklstScheduledDays.Items)
            {
                if (!li.Selected)
                    excludedDays += li.Value + ",";
            }

            return (excludedDays.Length > 0) ? excludedDays.Remove(excludedDays.Length - 1, 1) : excludedDays;

        }

        private void setExcludedScheduleDays(string excludedDates)
        {

            this.chklstScheduledDays.Items[0].Selected = true;
            this.chklstScheduledDays.Items[1].Selected = true;
            this.chklstScheduledDays.Items[2].Selected = true;
            this.chklstScheduledDays.Items[3].Selected = true;
            this.chklstScheduledDays.Items[4].Selected = true;

            excludedDates = excludedDates.Trim();

            if (string.IsNullOrEmpty(excludedDates))
                return;

            foreach (var day in excludedDates.Split(",".ToCharArray()))
            {
                var d = short.Parse(day);
                this.chklstScheduledDays.Items[d - 1].Selected = false;
            }

        }

        private void populateChildrenGrid(string sortExpression, SortDirection sortDirection)
        {
            var repo = new DataRepo();

            var children = repo.getChildren();

            switch (sortExpression.ToLower())
            {
                case "isactive":
                    children = (sortDirection == SortDirection.Ascending) ? children.OrderBy(o => o.isActive).ToList() : children.OrderByDescending(o => o.isActive).ToList();
                    break;

                case "center":
                    children = (sortDirection == SortDirection.Ascending) ? children.OrderBy(o => o.Center).ToList() : children.OrderByDescending(o => o.Center).ToList();
                    break;

                case "group":
                    children = (sortDirection == SortDirection.Ascending) ? children.OrderBy(o => o.Group).ToList() : children.OrderByDescending(o => o.Group).ToList();
                    break;

                case "firstname":
                    children = (sortDirection == SortDirection.Ascending) ? children.OrderBy(o => o.FirstName).ToList() : children.OrderByDescending(o => o.FirstName).ToList();
                    break;

                case "lastname":
                    children = (sortDirection == SortDirection.Ascending) ? children.OrderBy(o => o.LastName).ToList() : children.OrderByDescending(o => o.LastName).ToList();
                    break;

                case "dob":
                    children = (sortDirection == SortDirection.Ascending) ? children.OrderBy(o => o.DOB).ToList() : children.OrderByDescending(o => o.DOB).ToList();
                    break;

                case "enrollmentdate":
                    children = (sortDirection == SortDirection.Ascending) ? children.OrderBy(o => o.EnrollmentDate).ToList() : children.OrderByDescending(o => o.EnrollmentDate).ToList();
                    break;

                case "dismisaldate":
                    children = (sortDirection == SortDirection.Ascending) ? children.OrderBy(o => o.DismisalDate).ToList() : children.OrderByDescending(o => o.DismisalDate).ToList();
                    break;
            }

            this.gvChildren.DataSource = children;
            this.gvChildren.DataBind();

            this.clearFields();
        }

        protected void ddlCenter_SelectedIndexChanged(object sender, EventArgs e)
        {

            int centerId = int.Parse(this.ddlCenter.SelectedValue);
            this.ddlGroup.Enabled = (centerId > 0);

            this.populateGroupDDL(centerId);

        }
        
        protected void butSave_Click(object sender, EventArgs e)
        {
            try
            {

                var repo = new DataRepo();

                var child = new ChildrenDTO()
                {
                    CenterId = int.Parse(this.ddlCenter.SelectedValue),
                    GroupId = int.Parse(this.ddlGroup.SelectedValue),
                    FirstName = this.FirstName.Text,
                    LastName = this.LastName.Text,
                    DOB = DateTime.Parse(this.DOB.Text),
                    ExcludedDays = this.getExcludedScheduleDayNumbers(),
                    EnrollmentDate = DateTime.Parse(this.EnrollmentDate.Text),
                    DismisalDate = (string.IsNullOrEmpty(this.DismisalDate.Text)) ? (DateTime?)null : DateTime.Parse(this.DismisalDate.Text),
                    isActive = this.chkIsActive.Checked,
                    ParentFirstName = this.ParentFirstName.Text,
                    ParentLastName = this.ParentLastName.Text,
                    Address = this.Address.Text,
                    City = this.City.Text,
                    State = (this.State.SelectedIndex == 0) ? null : this.State.SelectedValue,
                    Zip = this.Zip.Text,
                    Phone = this.Phone.Text,
                    Email = this.Email.Text,
                    Notify = this.chkNotify.Checked
                };

                int childId = 0;

                int.TryParse(this.hidChildId.Value, out childId);

                if (childId == 0)
                {
                    repo.postChild(child);
                    this.writeMessage("The child and their supporting contact was created successfully.");
                }
                else
                {
                    repo.putChild(childId, child);
                    this.writeMessage("The child and their supporting contact was updated successfully.");
                }

                this.butSave.Text = "Add";
                this.hidChildId.Value = string.Empty;

                this.populateChildrenGrid(ViewState["SortExpression"].ToString(), this.gridViewSortDirection);

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

        protected void gvChildren_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            try
            {

                int childId = int.Parse(this.gvChildren.DataKeys[e.RowIndex].Values["Id"].ToString());

                var repo = new DataRepo();

                repo.deleteChild(childId);

                this.writeMessage("The child and their supporting contact was deactivated successfully.");

                this.populateChildrenGrid(ViewState["SortExpression"].ToString(), this.gridViewSortDirection);

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
        
        protected void gvChildren_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                // Convert the row index stored in the CommandArgument
                // property to an Integer.
                int index = int.Parse(e.CommandArgument.ToString());

                // Retrieve the row that contains the button clicked 
                // by the user from the Rows collection.
                GridViewRow row = this.gvChildren.Rows[index];

                this.hidChildId.Value = this.gvChildren.DataKeys[index].Values["Id"].ToString();

                var repo = new DataRepo();

                ChildrenDTO dataItem = repo.getChild(int.Parse(this.hidChildId.Value));

                this.chkIsActive.Checked = dataItem.isActive;

                this.ddlCenter.SelectedValue = dataItem.CenterId.ToString();
                ddlCenter_SelectedIndexChanged(null, new EventArgs());

                this.ddlGroup.SelectedValue = dataItem.GroupId.ToString();

                this.FirstName.Text = dataItem.FirstName;
                this.LastName.Text = dataItem.LastName;
                this.lblDOB.Text = string.Format("DOB (Age: {0:0#})", VIPHelpers.calculateAge(dataItem.DOB));
                this.DOB.Text = dataItem.DOB.ToString("MM/dd/yyyy");

                this.setExcludedScheduleDays(dataItem.ExcludedDays);

                this.EnrollmentDate.Text = dataItem.EnrollmentDate.ToString("MM/dd/yyyy");
                this.DismisalDate.Text = (dataItem.DismisalDate.HasValue) ? ((DateTime)dataItem.DismisalDate).ToString("MM/dd/yyyy") : string.Empty;

                this.ParentFirstName.Text = (string.IsNullOrEmpty(dataItem.ParentFirstName)) ? string.Empty : dataItem.ParentFirstName;
                this.ParentLastName.Text = (string.IsNullOrEmpty(dataItem.ParentLastName)) ? string.Empty : dataItem.ParentLastName;
                this.Address.Text = (string.IsNullOrEmpty(dataItem.Address)) ? string.Empty : dataItem.Address;
                this.City.Text = (string.IsNullOrEmpty(dataItem.City)) ? string.Empty : dataItem.City;
                this.State.SelectedValue = dataItem.State;
                this.Zip.Text = (string.IsNullOrEmpty(dataItem.Zip)) ? string.Empty : dataItem.Zip;
                this.Phone.Text = dataItem.Phone;
                this.Email.Text = dataItem.Email;
                this.chkNotify.Checked = dataItem.Notify;

                this.butSave.Text = "Save";

                e.Handled = true;

                this.FirstName.Focus();
            }
        }

        protected void gvChildren_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvChildren.PageIndex = e.NewPageIndex;
            this.populateChildrenGrid(ViewState["SortExpression"].ToString(), this.gridViewSortDirection);
        }

        protected void gvChildren_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            ViewState["SortExpression"] = sortExpression;
            if (this.gridViewSortDirection == SortDirection.Ascending)
            {
                this.gridViewSortDirection = SortDirection.Descending;
            }
            else
            {
                this.gridViewSortDirection = SortDirection.Ascending;
            }

            this.gvChildren.PageIndex = 0;
            this.populateChildrenGrid(sortExpression, this.gridViewSortDirection);
        }

        protected void gvChildren_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType ==  DataControlRowType.DataRow)
            {
                ChildrenDTO dataItem = e.Row.DataItem as ChildrenDTO;

                e.Row.Cells[7].Text = string.Format("{0:MM/dd/yyyy}", dataItem.DOB);
                e.Row.Cells[10].Text = string.Format("{0:MM/dd/yyyy}", dataItem.EnrollmentDate);
                e.Row.Cells[11].Text = string.Format("{0:MM/dd/yyyy}", dataItem.DismisalDate);

                Label lbl;
               
                lbl = e.Row.FindControl("lblScheduledDays") as Label;

                if (lbl != null)
                    lbl.Text = VIPHelpers.formatScheduledDates(dataItem.ExcludedDays);

                lbl = e.Row.FindControl("lblParentName") as Label;

                if (lbl != null)
                    lbl.Text = dataItem.ParentFirstName + " " + dataItem.ParentLastName;
            }
        }
                
        protected void butReset_Click(object sender, EventArgs e)
        {
            this.clearFields();
        }

    }
}
