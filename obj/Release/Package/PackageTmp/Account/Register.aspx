<%@ Page Title="Users" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="VIPDayCareCenters.Account.Register" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2><%: Page.Title %>.</h2>
    <p class="text-danger">
        <asp:Literal runat="server" ID="ErrorMessage" />
    </p>

    <div class="form-horizontal">
        <h4>Maintain Users</h4>
        <hr />
        <asp:ValidationSummary ID="vsCreateUser" runat="server" CssClass="text-danger" />
       
          <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                 <asp:GridView runat="server" ID="gvUsers" EmptyDataText="No users exist." 
                    AutoGenerateColumns="False"  OnRowDeleting="gvUsers_RowDeleting"  OnRowCommand="gvUsers_RowCommand" OnPageIndexChanging="gvUsers_PageIndexChanging"
                    DataKeyNames="Id, UserId" CellPadding="4" CellSpacing="1" AllowPaging="true"
                    BorderStyle="Solid"
                    RowStyle-CssClass="Grid" AlternatingRowStyle-CssClass="AlternatingGrid" HeaderStyle-CssClass="HeaderGrid"
                    PagerStyle-HorizontalAlign="Right"
                    PagerSettings-Position="TopAndBottom">
                    <EmptyDataRowStyle Wrap="false" />
                    <Columns>
                        <asp:CommandField ShowDeleteButton="True" ControlStyle-CssClass="selector" CausesValidation="false" />
                        <asp:buttonfield buttontype="Link" commandname="Edit" text="Edit" ControlStyle-CssClass="selector" CausesValidation="false"/>
                        <asp:CheckBoxField HeaderText="is Active?" DataField="isActive"/>
                        <asp:BoundField HeaderText="First Name" DataField="FirstName"  />
                        <asp:BoundField HeaderText="Last Name" DataField="LastName" />
                        <asp:BoundField HeaderText="Role" DataField="Role"  />
                        <asp:BoundField HeaderText="User Name" DataField="UserName" ItemStyle-CssClass="hidden-sm hidden-xs" HeaderStyle-CssClass="hidden-sm hidden-xs" />
                        <asp:BoundField HeaderText="Email" DataField="Email" ItemStyle-CssClass="hidden-sm hidden-xs" HeaderStyle-CssClass="hidden-sm hidden-xs" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>

        <asp:HiddenField ID="hidvipUserId" runat="server" />

         <div class="form-group">
            <asp:Label ID="lblFirstName" runat="server" AssociatedControlID="FirstName" CssClass="col-md-2 control-label">First Name</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="FirstName" CssClass="form-control" MaxLength="25" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="FirstName"
                    CssClass="text-danger" ErrorMessage="User first name field is required." Text="*" SetFocusOnError="true" EnableClientScript="true" />
            </div>
         </div>

         <div class="form-group">
            <asp:Label ID="lblLastName" runat="server" AssociatedControlID="LastName" CssClass="col-md-2 control-label">Last Name</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="LastName" CssClass="form-control" MaxLength="50" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="LastName"
                    CssClass="text-danger" ErrorMessage="User last name is required." Text="*" SetFocusOnError="true" EnableClientScript="true" />
            </div>
         </div>

         <div class="form-group">
            <asp:Label ID="lblUserName" runat="server" AssociatedControlID="UserName" CssClass="col-md-2 control-label">UserName</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="UserName" CssClass="form-control" MaxLength="256" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="UserName"
                    CssClass="text-danger" ErrorMessage="The username field is required." Text="*" SetFocusOnError="true" EnableClientScript="true" />
            </div>
         </div>

         <div class="form-group">
            <asp:Label ID="lblEmail" runat="server" AssociatedControlID="Email" CssClass="col-md-2 control-label">Email</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="Email" CssClass="form-control" TextMode="Email" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="Email"
                    CssClass="text-danger" ErrorMessage="The email field is required."  Text="*" SetFocusOnError="true" EnableClientScript="true"/>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="Email" CssClass="text-danger" ErrorMessage="The email address is invalid." Text="*" SetFocusOnError="true" EnableClientScript="true" ValidationExpression="^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"></asp:RegularExpressionValidator>
            </div>
        </div>

        <div class="form-group">
            <asp:Label ID="lblRole" runat="server" AssociatedControlID="Role" CssClass="col-md-2 control-label">Role</asp:Label>
            <div class="col-md-10">
                <asp:DropDownList ID="Role" runat="server" CssClass="form-control"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="Role" InitialValue=""
                    CssClass="text-danger" ErrorMessage="The role field is required."  Text="*" SetFocusOnError="true" EnableClientScript="true"/>
            </div>
        </div>

        <div class="form-group">
            <div class="col-md-offset-1 col-md-10">
                <div class="checkbox">
                    <asp:CheckBox runat="server" ID="chkIsActive" Checked="true" />
                    <asp:Label ID="lblIsActive" runat="server" AssociatedControlID="chkIsActive">Is Active?</asp:Label>
                </div>
            </div>
        </div>
        
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Button ID="butSave" runat="server" OnClick="butSave_Click" Text="Create" CssClass="btn btn-default submit" CausesValidation="true" />&nbsp;
                <asp:Button ID="butReset" runat="server" OnClick="butReset_Click" Text="Clear" CssClass="btn btn-default selector" CausesValidation="false" />
            </div>
        </div>
    </div>
</asp:Content>
