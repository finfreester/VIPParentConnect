<%@ Page Title="Groups" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Groups.aspx.cs" Inherits="VIPDayCareCenters.Manage.Groups" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2><%: Page.Title %>.</h2>
    <p class="text-danger">
        <asp:Literal runat="server" ID="ErrorMessage" />
    </p>

    <div class="form-horizontal">
        <h4>Maintain Groups</h4>
        <hr />
        <asp:ValidationSummary ID="vsGroups" runat="server" CssClass="text-danger" />
       
         <div class="form-group">
            <asp:Label ID="lblCenter" runat="server" AssociatedControlID="ddlCenter" CssClass="col-md-2 control-label">Select a Center</asp:Label>
            <div class="col-md-10">
                <asp:DropDownList ID="ddlCenter" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCenter_SelectedIndexChanged" />
            </div>
          </div>

        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                 <asp:GridView runat="server" ID="gvGroups" EmptyDataText="No Groups exist."
                    AutoGenerateColumns="False"  OnRowDeleting="gvGroups_OnRowDeleting" OnRowCommand="gvGroups_RowCommand" OnPageIndexChanging="gvGroups_OnPageIndexChanging"
                    DataKeyNames="Id, AgeCategoryId" CellPadding="4" CellSpacing="1" AllowPaging="true"
                    BorderStyle="Solid"
                    RowStyle-CssClass="Grid" AlternatingRowStyle-CssClass="AlternatingGrid" HeaderStyle-CssClass="HeaderGrid"
                    PagerStyle-HorizontalAlign="Right"
                    PagerSettings-Position="TopAndBottom">
                    <EmptyDataRowStyle Wrap="false" />
                    <Columns>
                        <asp:CommandField ShowDeleteButton="True" ControlStyle-CssClass="selector" CausesValidation="false" />
                        <asp:buttonfield buttontype="Link" commandname="Edit" text="Edit" ControlStyle-CssClass="selector" CausesValidation="false"/>
                        <asp:BoundField HeaderText="Group Name" DataField="Name"  />
                        <asp:BoundField HeaderText="Age Category" DataField="AgeCategoryTitle" />
                        <asp:BoundField HeaderText="Age Range" DataField="AgeRange" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>

        <asp:HiddenField ID="hidGroupId" runat="server" />

         <div class="form-group">
            <asp:Label ID="lblGroupName" runat="server" AssociatedControlID="GroupName" CssClass="col-md-2 control-label">Name</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="GroupName" CssClass="form-control" MaxLength="50" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="GroupName"
                    CssClass="text-danger" ErrorMessage="The name field is required." Text="*" SetFocusOnError="true" EnableClientScript="true" />
            </div>
         </div>

          <div class="form-group">
            <asp:Label ID="lblAgeCategory" runat="server" AssociatedControlID="ddlAgeCategory" CssClass="col-md-2 control-label">Age Category</asp:Label>
            <div class="col-md-10">
                <asp:DropDownList ID="ddlAgeCategory" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlAgeCategory" InitialValue=""
                    CssClass="text-danger" ErrorMessage="The age category field is required."  Text="*" SetFocusOnError="true" EnableClientScript="true"/>
            </div>
          </div>
       
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Button ID="butSave" runat="server" OnClick="butSave_Click" Text="Add" CssClass="btn btn-default submit" CausesValidation="true" />
                <asp:Button ID="butReset" runat="server" OnClick="butReset_Click" Text="Clear" CssClass="btn btn-default selector" CausesValidation="false" />
            </div>
        </div>

    </div>
</asp:Content>
