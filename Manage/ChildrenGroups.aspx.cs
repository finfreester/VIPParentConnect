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
    public partial class ChildrenGroups : VIPDayCarePage
    {

        UserDTO _user;

        protected void Page_Load(object sender, EventArgs e)
        {

            _user = this.LoggedInUserData();

            if (this.IsPostBack)
                return;

            this.populateCenterDDL();

            ViewState["SortExpression"] = "FirstName";
            this.populateMasterChildrenGrid(ViewState["SortExpression"].ToString(), this.gridViewSortDirection);

            this.ddlGroup.Enabled = false;
            this.butAdd.Enabled = false;
            this.butRemove.Enabled = false;
            this.gvMasterChildren.Enabled = false;
            this.lstGroupChildren.Enabled = false;

            this.clearFields();

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

            for (int i = 0; i < this.gvMasterChildren.Rows.Count; i++)
            {
                CheckBox chk = (CheckBox)this.gvMasterChildren.Rows[i].Cells[0].FindControl("chk");
                if (chk != null) 
                {
                    chk.Checked = false;
                }
            }

            this.lstGroupChildren.SelectedIndex = -1;
        }

        private void populateMasterChildrenGrid(string sortExpression, SortDirection sortDirection)
        {
            var repo = new DataRepo();

            var children = repo.getChildren();

            switch (sortExpression.ToLower())
            {
                case "isactive":
                    children = (sortDirection == SortDirection.Ascending) ? children.OrderBy(o => o.isActive).ToList() : children.OrderByDescending(o => o.isActive).ToList();
                    break;

                case "firstname":
                    children = (sortDirection == SortDirection.Ascending) ? children.OrderBy(o => o.FirstName).ToList() : children.OrderByDescending(o => o.FirstName).ToList();
                    break;

                case "lastname":
                    children = (sortDirection == SortDirection.Ascending) ? children.OrderBy(o => o.LastName).ToList() : children.OrderByDescending(o => o.LastName).ToList();
                    break;
            }

            this.gvMasterChildren.DataSource = children;
            this.gvMasterChildren.DataBind();
        }

        private void populateCenterDDL()
        {
            var repo = new DataRepo();

            //Keep Centers only where user is the manager
            if (_user.Role.ToUpper() == "ADMIN")
            {
                this.ddlCenter.DataSource = repo.getCenterLookup();
            }
            else //MANAGER
            {
                this.ddlCenter.DataSource = repo.getCentersByManagerIdLookup(_user.Id);
            }

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

        private void populateGroupChildren()
        {
            var repo = new DataRepo();

            if (this.ddlGroup.SelectedIndex < 1)
            {
                this.lstGroupChildren.Items.Clear();
                return;
            }

            this.lstGroupChildren.DataSource = repo.getChildrenInGroupLookup(int.Parse(this.ddlGroup.SelectedValue));
            this.lstGroupChildren.DataTextField = "Value";
            this.lstGroupChildren.DataValueField = "Key";
            this.lstGroupChildren.DataBind();
        }
                        
        protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.gvMasterChildren.Enabled = true;
            this.lstGroupChildren.Enabled = true;
            this.butAdd.Enabled = true;
            this.butRemove.Enabled = true;
                       
            this.populateGroupChildren();

            this.clearFields();
        }

        protected void ddlCenter_SelectedIndexChanged(object sender, EventArgs e)
        {

            int centerId = int.Parse(this.ddlCenter.SelectedValue);

            this.ddlGroup.Enabled = (centerId > 0);
            
            this.gvMasterChildren.Enabled = false;
            this.lstGroupChildren.Enabled = false;
            this.butAdd.Enabled = false;
            this.butRemove.Enabled = false;

            this.populateGroupDDL(centerId);

            this.clearFields();

            this.populateGroupChildren();

        }

        protected void butAdd_Click(object sender, EventArgs e)
        {

            try
            {

                var repo = new DataRepo();

                List<ChildGroupDTO> childgroups = new List<ChildGroupDTO>();

                for (int i = 0; i < this.gvMasterChildren.Rows.Count; i++)
                {
                    CheckBox chk = (CheckBox)this.gvMasterChildren.Rows[i].Cells[0].FindControl("chk");
                    if ((chk != null) && (chk.Checked))
                    {

                        string childId = this.gvMasterChildren.DataKeys[i].Values["Id"].ToString();

                        if (lstGroupChildren.Items.FindByValue(childId) == null)
                        {

                            var childGroup = new ChildGroupDTO()
                            {
                                GroupId = int.Parse(this.ddlGroup.SelectedValue),
                                ChildId = int.Parse(childId)
                            };

                            childgroups.Add(childGroup);

                        }

                    }
                }

                repo.postChildGroups(childgroups);

                this.populateGroupChildren();

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

        protected void butRemove_Click(object sender, EventArgs e)
        {

            try
            {

                if (this.lstGroupChildren.GetSelectedIndices().Length == 0)
                     return;

                var repo = new DataRepo();

                List<ChildGroupDTO> childgroups = new List<ChildGroupDTO>();

                foreach (int index in this.lstGroupChildren.GetSelectedIndices())
                {

                    var childGroup = new ChildGroupDTO()
                    {
                        GroupId = int.Parse(this.ddlGroup.SelectedValue),
                        ChildId = int.Parse(this.lstGroupChildren.Items[index].Value)
                    };

                    childgroups.Add(childGroup);

                }

                repo.deleteChildGroups(childgroups);

                this.populateGroupChildren();

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

        protected void gvMasterChildren_Sorting(object sender, GridViewSortEventArgs e)
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

            this.gvMasterChildren.PageIndex = 0;
            this.populateMasterChildrenGrid(sortExpression, this.gridViewSortDirection);
        }

    }
}
