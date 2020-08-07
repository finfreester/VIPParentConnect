<%@ Page Title="Children Groups" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChildrenGroups.aspx.cs" Inherits="VIPDayCareCenters.Manage.ChildrenGroups" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

    <h2><%: Page.Title %>.</h2>
    <p class="text-danger">
        <asp:Literal runat="server" ID="ErrorMessage" />
    </p>

    <div class="form-horizontal">
        <h4>Maintain Children Groups</h4>
        <hr />
        <asp:ValidationSummary ID="vsChildren" runat="server" CssClass="text-danger" />

        <div class="form-group">
            <asp:Label ID="lblCenter" runat="server" AssociatedControlID="ddlCenter" CssClass="col-md-2 control-label">Select a Center</asp:Label>
            <div class="col-md-10">
                <asp:DropDownList ID="ddlCenter" runat="server" CssClass="form-control selector" AutoPostBack="true" OnSelectedIndexChanged="ddlCenter_SelectedIndexChanged" />
            </div>
        </div>

        <div class="form-group">
            <asp:Label ID="lblGroup" runat="server" AssociatedControlID="ddlGroup" CssClass="col-md-2 control-label">Select a Group</asp:Label>
            <div class="col-md-10">
                <asp:DropDownList ID="ddlGroup" runat="server" CssClass="form-control selector" AutoPostBack="true" OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged" />
            </div>
        </div>

        <div class="form-group">

            <div class="container">
                <div class="row">
                    <div class="col-md-5">

                        <label>Select checkbox(s) and click Add to add children to Group, select children from Group and click Remove to delete children.</label>
                        <br />
                        <label class="col-md-2 control-label">All Children</label>
                        <asp:Button ID="butAdd" runat="server" OnClick="butAdd_Click" Text="Add" CssClass="col-md-3 btn pull-right btn-default selector" CausesValidation="false" />
                        <asp:GridView runat="server" ID="gvMasterChildren" EmptyDataText="No children exist."
                            AutoGenerateColumns="False" OnSorting="gvMasterChildren_Sorting"
                            DataKeyNames="Id" CellPadding="4" CellSpacing="1" AllowPaging="false" AllowSorting="true"
                            BorderStyle="Solid"
                            RowStyle-CssClass="Grid" AlternatingRowStyle-CssClass="AlternatingGrid" HeaderStyle-CssClass="HeaderGrid SortableHeaderGrid"
                            PagerStyle-HorizontalAlign="Right"
                            PagerSettings-Position="TopAndBottom">
                            <EmptyDataRowStyle Wrap="false" />
                            <Columns>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <label>De/Select</label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:CheckBoxField HeaderText="is Active?" DataField="isActive" SortExpression="isActive" />
                                <asp:BoundField HeaderText="First Name" DataField="FirstName" SortExpression="FirstName" />
                                <asp:BoundField HeaderText="Last Name" DataField="LastName" SortExpression="LastName" />
                            </Columns>
                        </asp:GridView>
                    </div>

                    <div class="col-md-5">
                        <label>
                            Select children using mouse click or Up/Down arrow keys and spacebar. To multi-select, hold CTRL key down.
                        </label>
                        <br />
                        <label class="col-md-2 control-label">Group Children</label>
                        <asp:Button ID="butRemove" runat="server" OnClick="butRemove_Click" Text="Remove" CssClass="col-md-3 btn pull-right btn-default selector" CausesValidation="false" />
                        <br />
                        <asp:ListBox runat="server" ID="lstGroupChildren" CssClass="policyDropDown" SelectionMode="Multiple" Width="400px" Height="300"></asp:ListBox><br />
                        <br />

                    </div>
                </div>

            </div>

        </div>

        <div class="form-group">
        </div>

    </div>

</asp:Content>
