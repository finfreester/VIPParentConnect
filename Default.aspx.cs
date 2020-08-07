using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VIPDayCareCenters.Models;

namespace VIPDayCareCenters
{
    public partial class _Default : VIPDayCarePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            bool isLoggedIn = this.isUserLoggedIn();

            if (!isLoggedIn)
            {
                this.pnlLogin.Visible = true;
                //this.pnlUsers.Visible = this.pnlCenters.Visible = this.pnlGroups.Visible = this.pnlChildren.Visible = this.pnlChildrenGroups.Visible = this.pnlTrackGroupActivities.Visible = false;
                this.pnlUsers.Visible = this.pnlCenters.Visible = this.pnlGroups.Visible = this.pnlChildren.Visible = this.pnlTrackGroupActivities.Visible = false;
            }
            else
            {

                UserDTO user = this.LoggedInUserData();

                user.Role = user.Role.ToUpper();

                this.pnlLogin.Visible = false;
                this.pnlUsers.Visible = (user.Role == "ADMIN");
                this.pnlCenters.Visible = (user.Role == "ADMIN");
                this.pnlGroups.Visible = (user.Role == "ADMIN");
                this.pnlChildren.Visible = ((user.Role == "MANAGER") || (user.Role == "ADMIN"));
                //this.pnlChildrenGroups.Visible = ((user.Role == "MANAGER") || (user.Role == "ADMIN"));
                this.pnlTrackGroupActivities.Visible = true;    
            }

        }
    }
}
