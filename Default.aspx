<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="VIPDayCareCenters._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Welcome to VIP Daycare Center Parent Connect</h1>
        <p class="lead">Track, maintain and report children's day-to-day status to parents.</p>
    </div>

    <div class="row">
         <asp:Panel id="pnlLogin" runat="server" CssClass="col-md-4">
            <h2>Login</h2>
            <p>
                Login to begin using from the system.
            </p>
            <p>
                <a class="btn btn-default selector" href="Account/Login.aspx">Go</a>
            </p>
        </asp:Panel>
        <asp:Panel id="pnlUsers" runat="server" CssClass="col-md-4">
            <h2>Manage Users</h2>
            <p>
                Add, Modify or Remove Users from the system.
            </p>
            <p>
                <a class="btn btn-default selector" href="Account/Register.aspx">Go</a>
            </p>
        </asp:Panel>
        <asp:Panel id="pnlCenters" runat="server" CssClass="col-md-4">
            <h2>Manage Centers</h2>
            <p>
                Add, Modify or Remove Centers from the system.
            </p>
            <p>
                <a class="btn btn-default selector" href="Manage/Centers.aspx">Go</a>
            </p>
        </asp:Panel>
          <asp:Panel id="pnlGroups" runat="server" CssClass="col-md-4">
            <h2>Manage Groups</h2>
            <p>
                Add, Modify or Remove Groups from the system and assign to Centers.
            </p>
            <p>
                <a class="btn btn-default selector" href="Manage/Groups.aspx">Go</a>
            </p>
        </asp:Panel>
        <asp:Panel id="pnlChildren" runat="server" CssClass="col-md-4">
            <h2>Manage Children</h2>
            <p>
                Add, Modify or Remove children from the system.
            </p>
            <p>
                <a class="btn btn-default selector" href="Manage/Children.aspx">Go</a>
            </p>
        </asp:Panel>
       <%-- <asp:Panel id="pnlChildrenGroups" runat="server" CssClass="col-md-4">
            <h2>Manage Children Groups</h2>
            <p>
                Add or Remove children to and from a Center Group.
            </p>
            <p>
                <a class="btn btn-default selector" href="Manage/ChildrenGroups.aspx">Go</a>
            </p>
        </asp:Panel>--%>
         <asp:Panel id="pnlTrackGroupActivities" runat="server" CssClass="col-md-4">
            <h2>Track Daily Group Activities</h2>
            <p>
                Track and report groups' daily activities, Approve a groups' daily activities or send parents' report.
            </p>
            <p>
                <a class="btn btn-default selector" href="Manage/TrackGroupActivities.aspx">Go</a>
            </p>
        </asp:Panel>
    </div>

</asp:Content>
