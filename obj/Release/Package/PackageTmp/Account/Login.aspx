<%@ Page Title="Log into VIP DayCare Parent Connect" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="VIPDayCareCenters.Account.Login" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2><%: Page.Title %></h2>

    <div class="row">
        <div class="col-md-8">
            <section id="loginForm">
                <div class="form-horizontal">
                    <asp:Panel ID="pnlLogin" runat="server">

                        <h4>Please sign in.</h4>
                        <hr />
                        <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                            <p class="text-danger">
                                <asp:Literal runat="server" ID="FailureText" />
                            </p>
                        </asp:PlaceHolder>

                        <div class="form-group">
                            <asp:Label ID="Label1" runat="server" AssociatedControlID="UserName" CssClass="col-md-2 control-label">UserName</asp:Label>
                            <div class="col-md-10">
                                <asp:TextBox runat="server" ID="UserName" CssClass="form-control" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="UserName"
                                    CssClass="text-danger" ErrorMessage="The username field is required." Text="*" SetFocusOnError="true" EnableClientScript="true" />
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label2" runat="server" AssociatedControlID="Password" CssClass="col-md-2 control-label">Password</asp:Label>
                            <div class="col-md-10">
                                <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="Password" CssClass="text-danger" ErrorMessage="The password field is required." Text="*" SetFocusOnError="true" EnableClientScript="true" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-offset-2 col-md-10">
                                <div class="checkbox">
                                    <asp:CheckBox runat="server" ID="RememberMe" />
                                    <asp:Label ID="Label3" runat="server" AssociatedControlID="RememberMe">Remember me?</asp:Label>
                                </div>
                            </div>
                        </div>

                    </asp:Panel>

                    <asp:Panel ID="pnlTeacherLogin" runat="server">

                        <h4>Please select the Group you are assigned to teach today:</h4>

                        <div class="form-group">
                            <asp:Label ID="lblCenter" runat="server" AssociatedControlID="ddlCenter" CssClass="col-md-2 control-label">Select a Center</asp:Label>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlCenter" runat="server" CssClass="form-control" AutoPostBack="true" CausesValidation="false" OnSelectedIndexChanged="ddlCenter_SelectedIndexChanged" />
                            </div>
                        </div>

                        <div class="form-group">
                            <asp:Label ID="lblGroup" runat="server" AssociatedControlID="ddlGroup" CssClass="col-md-2 control-label">Select a Group</asp:Label>
                            <div class="col-md-10">
                                <asp:DropDownList ID="ddlGroup" runat="server" CssClass="form-control" />
                            </div>
                        </div>

                    </asp:Panel>

                    <div class="form-group">
                        <div class="col-md-offset-1 col-md-10">
                            <asp:Button ID="butLogin" runat="server" Text="Log in" CssClass="btn btn-default submit" CausesValidation="true" OnClick="butLogin_Click" />
                        </div>
                    </div>

                </div>
                <%--<p>
                    <<asp:HyperLink runat="server" ID="RegisterHyperLink" ViewStateMode="Disabled">Register a new user</asp:HyperLink>
                </p>--%>
              <%--  <p>
                     Enable this once you have account confirmation enabled for password reset functionality
                    <asp:HyperLink runat="server" ID="ForgotPasswordHyperLink" ViewStateMode="Disabled">Forgot your password?</asp:HyperLink>
                    
                </p>--%>
            </section>
        </div>

    </div>
</asp:Content>
