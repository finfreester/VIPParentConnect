<%@ Page Title="Daily Group Activities" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TrackGroupActivities.aspx.cs" Inherits="VIPDayCareCenters.Manage.TrackGroupActivities" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

    <h2><%: Page.Title %>.</h2>
    <p class="text-danger">
        <asp:Literal runat="server" ID="ErrorMessage" />
    </p>

    <div class="form-horizontal">
        <h4>Track Daily Group Activities</h4>
        <hr />
        <asp:ValidationSummary ID="vsChildren" runat="server" CssClass="text-danger" />

        <asp:Panel id="pnlCenter" runat="server" CssClass="form-group">
            <asp:Label ID="lblCenter" runat="server" AssociatedControlID="ddlCenter" CssClass="col-md-2 control-label">Select a Center</asp:Label>
            <div class="col-md-10">
                <asp:DropDownList ID="ddlCenter" runat="server" CssClass="form-control selector" AutoPostBack="true" OnSelectedIndexChanged="ddlCenter_SelectedIndexChanged" />
            </div>
        </asp:Panel>

        <asp:Panel id="pnlGroup" runat="server" CssClass="form-group">
            <asp:Label ID="lblGroup" runat="server" AssociatedControlID="ddlGroup" CssClass="col-md-2 control-label">Select a Group</asp:Label>
            <div class="col-md-10">
                <asp:DropDownList ID="ddlGroup" runat="server" CssClass="form-control selector" AutoPostBack="true" OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged" />
            </div>
        </asp:Panel>

        <div class="form-group">

            <asp:Panel ID="pnlGroupActivity" runat="server" CssClass="col-md-offset-0 col-md-10">
            
                <asp:Repeater ID="rptGroupActivities" runat="server" OnItemCreated="rptGroupActivities_ItemCreated" OnItemDataBound="rptGroupActivities_ItemDataBound" EnableViewState="true">
                    
                    <HeaderTemplate>
                        <table id="tblGroupActivity" class="table">
                            <tr class="HeaderGrid">
                                <td style="white-space: nowrap;">Child Name</td>
                                <asp:PlaceHolder ID="phHeaderCell" runat="server"></asp:PlaceHolder>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr class="Grid">
                            <asp:HiddenField ID="hidChildId" runat="server" />
                            <td><asp:Label ID="lblName" runat="server"></asp:Label></td>
                            <asp:PlaceHolder ID="phCell" runat="server"></asp:PlaceHolder>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="AlternatingGrid">
                            <asp:HiddenField ID="hidChildId" runat="server" />
                            <td><asp:Label ID="lblName" runat="server"></asp:Label></td>
                            <asp:PlaceHolder ID="phCell" runat="server"></asp:PlaceHolder>
                        </tr>
                    </AlternatingItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>

                </asp:Repeater>

            </asp:Panel>

        </div>
   
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Button ID="butSave" runat="server" OnClick="butSave_Click" Text="Save" CssClass="btn btn-default submit" CausesValidation="true" />
                <asp:Button ID="butSubmit" runat="server" OnClick="butSubmit_Click" Text="Submit" CssClass="btn btn-default selector" CausesValidation="false" />
                <asp:Button ID="butApprove" runat="server" OnClick="butApprove_Click" Text="Approve" CssClass="btn btn-default selector" CausesValidation="false" />
            </div>
        </div>

       <%-- <script type="text/javascript">

            $(document).ready(function () {

                $('.HoverSelectAll').hover(function () {
                    $(this).find('.ButtonSelectAll').stop(true, true).fadeIn(400);
                }, function () {
                    $(this).find('.ButtonSelectAll').stop(true, true).fadeOut(400);
                });

            });

        </script>--%>
            
    </div>

</asp:Content>
