<%@ Page Title="Children" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Children.aspx.cs" Inherits="VIPDayCareCenters.Manage.Children" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2><%: Page.Title %>.</h2>
    <p class="text-danger">
        <asp:Literal runat="server" ID="ErrorMessage" />
    </p>

    <div class="form-horizontal">
        <h4>Maintain Children</h4>
        <hr />
        <asp:ValidationSummary ID="vsChildren" runat="server" CssClass="text-danger" />

        <div class="form-group">
            <div class="col-md-offset-1 col-md-10">
                <asp:GridView runat="server" ID="gvChildren" EmptyDataText="No children exist."
                    AutoGenerateColumns="False" OnRowDeleting="gvChildren_OnRowDeleting" OnRowDataBound="gvChildren_RowDataBound" OnRowCommand="gvChildren_RowCommand" OnPageIndexChanging="gvChildren_OnPageIndexChanging" OnSorting="gvChildren_Sorting"
                    DataKeyNames="Id" CellPadding="4" CellSpacing="1" AllowPaging="true" AllowSorting="true"
                    BorderStyle="Solid"
                    RowStyle-CssClass="Grid" AlternatingRowStyle-CssClass="AlternatingGrid" HeaderStyle-CssClass="HeaderGrid SortableHeaderGrid"
                    PagerStyle-HorizontalAlign="Left"
                    PagerSettings-Position="TopAndBottom">
                    <EmptyDataRowStyle Wrap="false" />
                    <AlternatingRowStyle CssClass="AlternatingGrid"></AlternatingRowStyle>
                    <Columns>
                        <asp:CommandField ShowDeleteButton="True" ControlStyle-CssClass="selector" CausesValidation="false">
                            <ControlStyle CssClass="selector"></ControlStyle>
                        </asp:CommandField>
                        <asp:ButtonField ButtonType="Link" CommandName="Edit" Text="Edit" ControlStyle-CssClass="selector" CausesValidation="false">
                            <ControlStyle CssClass="selector"></ControlStyle>
                        </asp:ButtonField>
                        <asp:CheckBoxField HeaderText="is Active?" DataField="isActive" SortExpression="isActive" />
                        <asp:BoundField HeaderText="Center" DataField="Center" SortExpression="Center" />
                        <asp:BoundField HeaderText="Group" DataField="Group" SortExpression="Group" />
                        <asp:BoundField HeaderText="First Name" DataField="FirstName" SortExpression="FirstName" />
                        <asp:BoundField HeaderText="Last Name" DataField="LastName" SortExpression="LastName" />
                        <asp:BoundField HeaderText="Date of Birth" DataField="DOB" SortExpression="DOB" ItemStyle-CssClass="hidden-sm hidden-xs" HeaderStyle-CssClass="hidden-sm hidden-xs">
                            <HeaderStyle CssClass="hidden-sm hidden-xs"></HeaderStyle>

                            <ItemStyle CssClass="hidden-sm hidden-xs"></ItemStyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Scheduled Days" ItemStyle-CssClass="hidden-sm hidden-xs" HeaderStyle-CssClass="hidden-sm hidden-xs">
                            <ItemTemplate>
                                <asp:Label ID="lblScheduledDays" runat="Server"></asp:Label>
                            </ItemTemplate>

                            <HeaderStyle CssClass="hidden-sm hidden-xs"></HeaderStyle>

                            <ItemStyle CssClass="hidden-sm hidden-xs"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Parent Name" ItemStyle-CssClass="hidden-sm hidden-xs" HeaderStyle-CssClass="hidden-sm hidden-xs">
                            <ItemTemplate>
                                <asp:Label ID="lblParentName" runat="Server"></asp:Label>
                            </ItemTemplate>

                            <HeaderStyle CssClass="hidden-sm hidden-xs"></HeaderStyle>

                            <ItemStyle CssClass="hidden-sm hidden-xs"></ItemStyle>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Enrollment Date" DataField="EnrollmentDate" ItemStyle-CssClass="hidden-sm hidden-xs" HeaderStyle-CssClass="hidden-sm hidden-xs" SortExpression="EnrollmentDate">
                            <HeaderStyle CssClass="hidden-sm hidden-xs"></HeaderStyle>

                            <ItemStyle CssClass="hidden-sm hidden-xs"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Dismisal Date" DataField="DismisalDate" ItemStyle-CssClass="hidden-sm hidden-xs" HeaderStyle-CssClass="hidden-sm hidden-xs" SortExpression="DismisalDate">
                            <HeaderStyle CssClass="hidden-sm hidden-xs"></HeaderStyle>

                            <ItemStyle CssClass="hidden-sm hidden-xs"></ItemStyle>
                        </asp:BoundField>
                    </Columns>

                    <HeaderStyle CssClass="HeaderGrid SortableHeaderGrid"></HeaderStyle>

                    <PagerSettings Position="TopAndBottom"></PagerSettings>

                    <PagerStyle HorizontalAlign="Left"></PagerStyle>

                    <RowStyle CssClass="Grid"></RowStyle>
                </asp:GridView>
            </div>
        </div>

        <asp:HiddenField ID="hidChildId" runat="server" />

        <div class="form-group">
            <asp:Label ID="lblFirstName" runat="server" AssociatedControlID="FirstName" CssClass="col-md-2 control-label">First Name</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="FirstName" CssClass="form-control" MaxLength="25" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="FirstName"
                    CssClass="text-danger" ErrorMessage="The first name field is required." Text="*" SetFocusOnError="true" EnableClientScript="true" />
            </div>
        </div>

        <div class="form-group">
            <asp:Label ID="lblLastName" runat="server" AssociatedControlID="LastName" CssClass="col-md-2 control-label">Last Name</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="LastName" CssClass="form-control" MaxLength="50" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="LastName"
                    CssClass="text-danger" ErrorMessage="The last name field is required." Text="*" SetFocusOnError="true" EnableClientScript="true" />
            </div>
        </div>

        <div class="form-group">
            <asp:Label ID="lblCenter" runat="server" AssociatedControlID="ddlCenter" CssClass="col-md-2 control-label">Center</asp:Label>
            <div class="col-md-10">
                <asp:DropDownList ID="ddlCenter" runat="server" CssClass="form-control selector" AutoPostBack="true" OnSelectedIndexChanged="ddlCenter_SelectedIndexChanged" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="ddlCenter" InitialValue="0"
                    CssClass="text-danger" ErrorMessage="The center field is required." Text="*" SetFocusOnError="true" EnableClientScript="true" />
            </div>
        </div>

        <div class="form-group">
            <asp:Label ID="lblGroup" runat="server" AssociatedControlID="ddlGroup" CssClass="col-md-2 control-label">Group</asp:Label>
            <div class="col-md-10">
                <asp:DropDownList ID="ddlGroup" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlGroup" InitialValue="0"
                    CssClass="text-danger" ErrorMessage="The group field is required." Text="*" SetFocusOnError="true" EnableClientScript="true" />
            </div>
        </div>

        <div class="form-group">
            <asp:Label ID="lblDOB" runat="server" AssociatedControlID="DOB" CssClass="col-md-2 control-label">DOB</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="DOB" CssClass="form-control" MaxLength="10" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="DOB"
                    CssClass="text-danger" ErrorMessage="The date of birth field is required." Text="*" SetFocusOnError="true" EnableClientScript="true" />
                <asp:RangeValidator ID="rvDOB" runat="server" CssClass="text-danger" ControlToValidate="DOB" Type="Date" MinimumValue="01/01/2010" MaximumValue="01/01/9999" ErrorMessage="The DOB Date field is invalid." Text="*" SetFocusOnError="true" EnableClientScript="true"></asp:RangeValidator>
            </div>
        </div>

        <div class="form-group">
            <asp:Label ID="lblScheduledDays" runat="server" AssociatedControlID="chklstScheduledDays" CssClass="col-md-2 control-label">Scheduled Days</asp:Label>
            <div class="col-md-10">
                <asp:CheckBoxList ID="chklstScheduledDays" runat="server" RepeatDirection="Horizontal" TextAlign="Left">
                    <asp:ListItem Selected="True" Text="Mon" Value="1"></asp:ListItem>
                    <asp:ListItem Selected="True" Text="Tue" Value="2"></asp:ListItem>
                    <asp:ListItem Selected="True" Text="Wed" Value="3"></asp:ListItem>
                    <asp:ListItem Selected="True" Text="Thu" Value="4"></asp:ListItem>
                    <asp:ListItem Selected="True" Text="Fri" Value="5"></asp:ListItem>
                </asp:CheckBoxList>
            </div>
        </div>

        <div class="form-group">
            <asp:Label ID="lnlEnrollmentDate" runat="server" AssociatedControlID="EnrollmentDate" CssClass="col-md-2 control-label">Enrollment Date</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="EnrollmentDate" CssClass="form-control" MaxLength="10" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="EnrollmentDate"
                    CssClass="text-danger" ErrorMessage="The Enrollment Date field is required." Text="*" SetFocusOnError="true" EnableClientScript="true" />
                <asp:RangeValidator ID="rvEnrollmentDate" runat="server" CssClass="text-danger" ControlToValidate="EnrollmentDate" Type="Date" MinimumValue="01/01/2010" MaximumValue="01/01/9999" ErrorMessage="The Enrollment Date field is invalid." Text="*" SetFocusOnError="true" EnableClientScript="true"></asp:RangeValidator>
            </div>
        </div>

        <div class="form-group">
            <asp:Label ID="lblDismisalDate" runat="server" AssociatedControlID="DismisalDate" CssClass="col-md-2 control-label">Dismisal Date</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="DismisalDate" CssClass="form-control" MaxLength="10" />
                <asp:RangeValidator ID="rvDismisalDate" runat="server" CssClass="text-danger" ControlToValidate="DismisalDate" Type="Date" MinimumValue="01/01/2010" MaximumValue="01/01/9999" ErrorMessage="The DismisalDate Date field is invalid." Text="*" SetFocusOnError="true" EnableClientScript="true"></asp:RangeValidator>
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
            <asp:Label ID="lblParentFirstName" runat="server" AssociatedControlID="ParentFirstName" CssClass="col-md-2 control-label">Parent First Name</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="ParentFirstName" CssClass="form-control" MaxLength="25" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ParentFirstName"
                    CssClass="text-danger" ErrorMessage="The parent first name field is required." Text="*" SetFocusOnError="true" EnableClientScript="true" />
            </div>
        </div>

        <div class="form-group">
            <asp:Label ID="lblParentLastName" runat="server" AssociatedControlID="ParentLastName" CssClass="col-md-2 control-label">Parent Last Name</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="ParentLastName" CssClass="form-control" MaxLength="50" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ParentLastName"
                    CssClass="text-danger" ErrorMessage="The parent last name field is required." Text="*" SetFocusOnError="true" EnableClientScript="true" />
            </div>
        </div>

        <div class="form-group">
            <asp:Label ID="lblAddress" runat="server" AssociatedControlID="Address" CssClass="col-md-2 control-label">Address</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="Address" CssClass="form-control" MaxLength="50" />
            </div>
        </div>

        <div class="form-group">
            <asp:Label ID="lblCity" runat="server" AssociatedControlID="City" CssClass="col-md-2 control-label">City</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="City" CssClass="form-control" MaxLength="50" />
            </div>
        </div>

        <div class="form-group">
            <asp:Label ID="lblState" runat="server" AssociatedControlID="State" CssClass="col-md-2 control-label">State</asp:Label>
            <div class="col-md-10">
                <asp:DropDownList ID="State" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="State" InitialValue="0"
                    CssClass="text-danger" ErrorMessage="The state field is required." Text="*" SetFocusOnError="true" EnableClientScript="true" />
            </div>
        </div>

        <div class="form-group">
            <asp:Label ID="lblZip" runat="server" AssociatedControlID="Zip" CssClass="col-md-2 control-label">Zip</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="Zip" CssClass="form-control" MaxLength="10" />
            </div>
        </div>

        <div class="form-group">
            <asp:Label ID="lblPhone" runat="server" AssociatedControlID="Phone" CssClass="col-md-2 control-label">Phone</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="Phone" CssClass="form-control" MaxLength="10" TextMode="Phone" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="Phone"
                    CssClass="text-danger" ErrorMessage="The phone field is required." Text="*" SetFocusOnError="true" EnableClientScript="true" />
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="Phone" CssClass="text-danger" ErrorMessage="The phone is invalid and should be 10 digits." Text="*" SetFocusOnError="true" EnableClientScript="true" ValidationExpression="^\d{10}$"></asp:RegularExpressionValidator>
            </div>
        </div>

        <div class="form-group">
            <asp:Label ID="lblEmail" runat="server" AssociatedControlID="Email" CssClass="col-md-2 control-label">Email</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="Email" CssClass="form-control" TextMode="Email" />
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="Email" CssClass="text-danger" ErrorMessage="The email address is invalid." Text="*" SetFocusOnError="true" EnableClientScript="true" ValidationExpression="^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"></asp:RegularExpressionValidator>
            </div>
        </div>

        <div class="form-group">
            <div class="col-md-offset-1 col-md-10">
                <div class="checkbox">
                    <asp:CheckBox runat="server" ID="chkNotify" Checked="true" />
                    <asp:Label ID="lblNotify" runat="server" AssociatedControlID="chkNotify" Font-Bold="true">Notify?</asp:Label>
                </div>
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
