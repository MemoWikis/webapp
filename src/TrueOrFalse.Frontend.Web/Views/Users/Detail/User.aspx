<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master"
    Inherits="System.Web.Mvc.ViewPage<UserModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="ContentHeadSEO" ContentPlaceHolderID="HeadSEO" runat="server">
    <% Title = "Nutzer: " + Model.Name; %>
    <link rel="canonical" href="<%= Settings.CanonicalHost %><%= Links.UserDetail(Model.User) %>">
</asp:Content>

<asp:Content ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/bundles/User") %>
    <%= Scripts.Render("~/bundles/Js/User") %>
    <% Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem{Text = "Nutzer", Url = "/Nutzer", ToolTipText = Links.Users()});
        Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem{Text = "Profilseite", Url = Url.Action(Links.UserAction, Links.UserController, new { name = Model.User.Name, id = Model.User.Id}), ToolTipText = "Profilseite"});
        Model.TopNavMenu.IsCategoryBreadCrumb = false; %>

</asp:Content>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="xxs-stack col-xs-12">
            <div class="row">
                <div class="col-xs-9 xxs-stack" style="margin-bottom: 10px;">
                    <h1 class="pull-left ColoredUnderline User" style="margin-bottom: 10px; margin-top: 0px;  font-size: 30px;">
                        <% if (Model.IsMember) { %>
                            <i class="fa fa-star show-tooltip" style="color: #afd534;" title="<%= Model.Name %> unterstützt memucho als Fördermitglied. Danke!"></i>
                        <% } %>
                        <%= Model.Name %>
                        <span style="display: inline-block; font-size: 20px; font-weight: normal;">
                            &nbsp;(Reputation: <%=Model.ReputationTotal %> - Rang <%= Model.ReputationRank %>)
                        </span>
                    </h1>
                </div>
                <div class="col-xs-3 xxs-stack">
                    <div class="navLinks">
                        <a href="<%= Url.Action("Users", "Users")%>" style="font-size: 12px; margin: 0px;"><i class="fa fa-list"></i>&nbsp;zur Übersicht</a>
                        <% if (Model.IsCurrentUser) { %>
                            <a href="<%= Url.Action(Links.UserSettingsAction, Links.UserSettingsController) %>" style="font-size: 12px; margin: 0px;"><i class="fa fa-pencil"></i>&nbsp;bearbeiten</a> 
                        <% } %>
                    </div>
                </div>
            </div>
        </div>
    
        <div class="col-lg-10 col-xs-9 xxs-stack">
            <div class="box-content" style="min-height: 120px; clear: both; ">
            
                <div class="column">
                    <h4 style="margin-top: 0px;">Reputation</h4>
                    <div>- <%= Model.Reputation.ForQuestionsCreated %> für erstellte Fragen</div>
                    <div>- <%= Model.Reputation.ForSetsCreated %> für erstellte Lernsets</div>
                    <div>- <%= Model.User.Id != -1 ? Model.Reputation.ForQuestionsInOtherWishknowledge : 0 %> für eigene Fragen im Wunschwissen anderer </div>
                    <div>- <%= Model.User.Id != -1 ? Model.Reputation.ForSetsInOtherWishknowledge: 0 %> für eigene Lernsets im Wunschwissen anderer</div>
                    <div>- <%= Model.User.Id != -1 ? Model.Reputation.ForDatesCreatedVisible : 0 %> für erstellte Termine (sichtbar im Netzwerk)</div>
                    <div>- <%= Model.User.Id != -1 ? Model.Reputation.ForDatesCopied : 0 %> für eigene Termine, die von anderen übernommen wurden</div>
                    <div>- <%= Model.User.Id != -1 ? Model.Reputation.ForPublicWishknowledge : 0 %> für die Veröffentlichung des eigenen Wunschwissens</div>
                    <div>- <%= Model.Reputation.ForUsersFollowingMe %> für folgende Nutzer</div>
                </div>
                <div class="column" >
                    <h4 style="margin-top: 0px;">Erstellte Inhalte</h4>
                    <div><a href="<%= Links.QuestionWithCreatorFilter(Url, Model.User.User) %>"><%= Model.AmountCreatedQuestions %> öffentliche Fragen erstellt</a></div>
                    <div><%= Model.AmountCreatedSets %> Lernsets erstellt</div>
                    <div><%= Model.AmountCreatedCategories %>  Themen erstellt</div>
                </div>
            
                <div class="column">
                    <h4 style="margin-top: 0px;">Wunschwissen</h4>
                    <div><%= Model.AmountWishCountQuestions %> Fragen gemerkt</div>
                    <div><%= Model.AmountWishCountSets %> Lernsets gemerkt</div>
                    <div></div>
                </div>

            </div>
        </div>
        
        <div class="col-lg-2 col-xs-3 xxs-stack">
            <img style="width:100%; border-radius:5px;" src="<%=Model.ImageUrl_250 %>" /><br/>
            <div style="text-align: center; margin-top: 10px;" data-userid="<%=Model.UserIdProfile %>">
                <% if(!Model.IsCurrentUser && Model.IsMember){ %>
                    <button class="btn btn-default btn-sm" type="button" data-type="btn-follow"
                        style="<%= Html.CssHide(Model.DoIFollow) %> ">
                        <i class="fa fa-user-plus"></i>
                        Folgen
                    </button>
                    
                    <i class='fa fa-spinner fa-pulse' data-type="btnFollowSpinner" style="display:none"></i>
                
                    <button class="btn btn-warning btn-sm " type="button" data-type="btn-unfollow"
                        style="<%= Html.CssHide(!Model.DoIFollow) %>">
                        <i class="fa fa-user-times"></i>
                        Entfolgen
                    </button>
                <% } %>
            </div>
        </div>
        
        <% if (Model.IsCurrentUser) { %>
            <div class="col-xs-12" style="margin-top: 20px; margin-bottom: 20px;">
                <a href="<%= Links.WidgetStats() %>" class="btn btn-default">Zur Widget-Statistik</a>
            </div>
        <% } %>
    </div>
    
    <div class="row" id="user-main">
        
        <div id="MobileSubHeader" class="MobileSubHeader DesktopHide" style="margin-top: 20px;">
            <div class="MainFilterBarWrapper">
                <div id="MainFilterBarBackground" class="btn-group btn-group-justified">
                    <div class="btn-group">
                        <a class="btn btn-default disabled">.</a>
                    </div>
                </div>
                <div class="container">
                    <div id="MainFilterBar" class="btn-group btn-group-justified JS-Tabs">

                        <div class="btn-group <%= Model.IsActiveTabKnowledge? "active" : "" %>">
                            <a  href="<%= Links.UserDetail(Model.User) %>" type="button" class="btn btn-default">
                                Wunsch<span class="hidden-xxs">wissen</span>
                            </a>
                        </div>
                    
                        <div class="btn-group  <%= Model.IsActiveTabBadges  ? "active" : "" %>">
                            <a  href="<%= Links.UserDetailBadges(Model.User.User) %>" type="button" class="btn btn-default">
                                Badges
                            </a>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-lg-12">
            <div class="boxtainer-outlined-tabs" style="margin-top: 20px;">
                <div class="boxtainer-header MobileHide">
                    <ul class="nav nav-tabs">
                        <li class="<%= Html.IfTrue(Model.IsActiveTabKnowledge, "active") %>">
                            <a href="<%= Links.UserDetail(Model.User) %>" >
                                Wunschwissen
                            </a>
                        </li>
                        <li class="<%= Html.IfTrue(Model.IsActiveTabBadges, "active") %>">
                            <a href="<%= Links.UserDetailBadges(Model.User.User) %>">
                                Badges (0 von <%= BadgeTypes.All().Count %>)
                            </a>
                        </li>
                    </ul>
                </div>
                <div class="boxtainer-content">
                    <% if(Model.IsActiveTabKnowledge) { %>
                        <% Html.RenderPartial("~/Views/Users/Detail/TabKnowledge.ascx", new TabKnowledgeModel(Model)); %>
                    <% } %>
                    <% if(Model.IsActiveTabBadges) { %>
                        <% Html.RenderPartial("~/Views/Users/Detail/TabBadges.ascx", new TabBadgesModel(Model)); %>
                    <% } %>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
