<%@ Page Title="Centers" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Centers.aspx.cs" Inherits="VIPDayCareCenters.Manage.Centers" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2><%: Page.Title %>.</h2>
    <p class="text-danger">
        <asp:Literal runat="server" ID="ErrorMessage" />
    </p>

    <div class="form-horizontal">
        <h4>Maintain Centers</h4>
        <hr />
        <asp:ValidationSummary ID="vsCenters" runat="server" CssClass="text-danger" />
       
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                 <asp:GridView runat="server" ID="gvCenters" EmptyDataText="No centers exist."
                    AutoGenerateColumns="False"  OnRowDeleting="gvCenters_OnRowDeleting" OnRowDataBound="gvCenters_RowDataBound" OnRowCommand="gvCenters_RowCommand" OnPageIndexChanging="gvCenters_OnPageIndexChanging"
                    DataKeyNames="Id, ManagerId" CellPadding="4" CellSpacing="1" AllowPaging="true"
                    BorderStyle="Solid"
                    RowStyle-CssClass="Grid" AlternatingRowStyle-CssClass="AlternatingGrid" HeaderStyle-CssClass="HeaderGrid"
                    PagerStyle-HorizontalAlign="Right"
                    PagerSettings-Position="TopAndBottom">
                    <EmptyDataRowStyle Wrap="false" />
                    <Columns>
                        <asp:CommandField ShowDeleteButton="True" ControlStyle-CssClass="selector" CausesValidation="false" />
                        <asp:buttonfield buttontype="Link" commandname="Edit" text="Edit" ControlStyle-CssClass="selector" CausesValidation="false"/>
                        <asp:BoundField HeaderText="Name" DataField="Name"  />
                        <asp:BoundField HeaderText="Manager" DataField="ManagerName" />
                        <asp:BoundField HeaderText="Address" DataField="Address" ItemStyle-CssClass="hidden-sm hidden-xs" HeaderStyle-CssClass="hidden-sm hidden-xs" />
                        <asp:BoundField HeaderText="City" DataField="City" ItemStyle-CssClass="hidden-sm hidden-xs" HeaderStyle-CssClass="hidden-sm hidden-xs" />
                        <asp:BoundField HeaderText="State" DataField="State" ItemStyle-CssClass="hidden-sm hidden-xs" HeaderStyle-CssClass="hidden-sm hidden-xs" />
                        <asp:BoundField HeaderText="Zip" DataField="Zip" ItemStyle-CssClass="hidden-sm hidden-xs" HeaderStyle-CssClass="hidden-sm hidden-xs" />
                        <asp:BoundField HeaderText="Phone" DataField="Phone" ItemStyle-CssClass="hidden-sm hidden-xs" HeaderStyle-CssClass="hidden-sm hidden-xs" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>

        <asp:HiddenField ID="hidCenterId" runat="server" />

         <div class="form-group">
            <asp:Label ID="lblCenterName" runat="server" AssociatedControlID="CenterName" CssClass="col-md-2 control-label">Name</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="CenterName" CssClass="form-control" MaxLength="50" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="CenterName"
                    CssClass="text-danger" ErrorMessage="The name field is required." Text="*" SetFocusOnError="true" EnableClientScript="true" />
            </div>
         </div>

         <div class="form-group">
            <asp:Label ID="lblManager" runat="server" AssociatedControlID="ddlManager" CssClass="col-md-2 control-label">Manager</asp:Label>
            <div class="col-md-10">
                <asp:DropDownList ID="ddlManager" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlManager" InitialValue=""
                    CssClass="text-danger" ErrorMessage="The manager field is required."  Text="*" SetFocusOnError="true" EnableClientScript="true"/>
            </div>
          </div>

         <div class="form-group">
            <asp:Label ID="lblAddress" runat="server" AssociatedControlID="Address" CssClass="col-md-2 control-label">Address</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="Address" CssClass="form-control" MaxLength="50" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="Address"
                    CssClass="text-danger" ErrorMessage="The address field is required." Text="*" SetFocusOnError="true" EnableClientScript="true" />
            </div>
         </div>

         <div class="form-group">
            <asp:Label ID="lblCity" runat="server" AssociatedControlID="City" CssClass="col-md-2 control-label">City</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="City" CssClass="form-control" MaxLength="50" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="City"
                    CssClass="text-danger" ErrorMessage="The city field is required." Text="*" SetFocusOnError="true" EnableClientScript="true" />
            </div>
         </div>

          <div class="form-group">
            <asp:Label ID="lblState" runat="server" AssociatedControlID="State" CssClass="col-md-2 control-label">State</asp:Label>
            <div class="col-md-10">
                <asp:DropDownList ID="State" runat="server" CssClass="form-control">
                    <asp:ListItem Value="NY" Text="New York"></asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="State" InitialValue="NY" Enabled="false"
                    CssClass="text-danger" ErrorMessage="The state field is required."  Text="*" SetFocusOnError="true" EnableClientScript="true"/>
            </div>
          </div>

         <div class="form-group">
            <asp:Label ID="lblZip" runat="server" AssociatedControlID="Zip" CssClass="col-md-2 control-label">Zip</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="Zip" CssClass="form-control" MaxLength="10" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="Zip"
                    CssClass="text-danger" ErrorMessage="The zip field is required."  Text="*" SetFocusOnError="true" EnableClientScript="true"/>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="Zip" CssClass="text-danger" ErrorMessage="The zip is invalid and should be in the format 99999 or 99999-9999." Text="*" SetFocusOnError="true" EnableClientScript="true" ValidationExpression="^\d{5}(-\d{4})?$"></asp:RegularExpressionValidator>
            </div>
        </div>

         <div class="form-group">
            <asp:Label ID="lblPhone" runat="server" AssociatedControlID="Phone" CssClass="col-md-2 control-label">Phone</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="Phone" CssClass="form-control" MaxLength="10" TextMode="Phone"/>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="Phone"
                    CssClass="text-danger" ErrorMessage="The phone field is required."  Text="*" SetFocusOnError="true" EnableClientScript="true"/>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="Phone" CssClass="text-danger" ErrorMessage="The phone is invalid and should be 10 digits." Text="*" SetFocusOnError="true" EnableClientScript="true" ValidationExpression="^\d{10}$"></asp:RegularExpressionValidator>
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
