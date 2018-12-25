<%@ Page Title="Wissenszentrale" Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="ViewPage<KnowledgeModel>" %>

<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="ContentHeadSEO" ContentPlaceHolderID="HeadSEO" runat="server">
    <link rel="canonical" href="<%= Settings.CanonicalHost %><%= Links.Knowledge() %>">
</asp:Content>

<asp:Content runat="server" ID="header" ContentPlaceHolderID="Head">
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <% Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem { Text = "Nutzer", Url = Links.Users(), ToolTipText = "Nutzer" });
        Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem { Text = "Profilseite", Url = Url.Action(Links.UserAction, Links.UserController, new { name = Model.User.Name, id = Model.User.Id }), ToolTipText = "Profilseite" });
        Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem { Text = "Wissenszentrale", Url = Links.Knowledge(), ToolTipText = "Wissenszentrale" });
        Model.TopNavMenu.IsCategoryBreadCrumb = false; %>
    <%= Styles.Render("~/bundles/Knowledge") %>
    <%= Scripts.Render("~/bundles/js/Knowledge") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <% if (Model.IsLoggedIn){ %>
    <input type="hidden" id="hddNoQuestionUrl" value="<%= Links.NoQuestionUrl %>" />
    <input type="hidden" id="hddNoCategoryUrl" value="<%= Links.NoCategoryUrl %>" />
    <!-- Spinner-->
    <div class="spinner">
        <div class="double-bounce1"></div>
        <div class="double-bounce2"></div>
    </div>
    

    <input type="hidden" id="hddUrlAddTopic" value="<%= Url.Action("Create", "EditCategory") %>" />
    <input type="hidden" id="hddUrlAddQuestion" value="<%= Links.CreateQuestion() %>" />


    <script type="text/javascript">

        google.load("visualization", "1", { packages: ["corechart"] });

        var isGoogleApiInitialized = false;
        google.setOnLoadCallback(isApiInitialized);

        function isApiInitialized() {
            isGoogleApiInitialized = true;
        }
    </script>
    <div class="row">
        <div class="col-xs-12">
            <div id="CategoryHeader">
                <div id="ManagementMobile">
                    <div class="Buttons">
                        <div class="Button"><a class="LinkIsDirectedToPartialView" href="<%= Links.StartWishLearningSession() %>">Lernsitzung starten</a></div>
                    </div>
                </div>
                <div id="TabsBar">
                    <div class="Tabs">
                        <div class="Tab active"><a href="#" id="dashboard">Überblick</a></div>
                        <div class="Tab"><a href="#" id="topics">Themen</a></div>
                        <div class="Tab"><a href="#" id="questions">Fragen</a></div>
                    </div>
                    <div class="Border"></div>
                    <div id="Management">
                        <div class="Buttons">
                            <div class="Button"><a class="LinkIsDirectedToPartialView" href="<%= Links.StartWishLearningSession() %>">Lernsitzung starten</a></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12">
            <div class="content" style="margin-top: 2rem">
                <% Html.RenderPartial("~/Views/Knowledge/Partials/_Dashboard.ascx"); %>
            </div>
        </div>
    </div>
        <% }
       else
       {
           Response.Redirect(Links.Welcome(), true);
       }%>
    <%= Scripts.Render("~/bundles/js/Vue") %>

</asp:Content>

