<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="ViewPage<SetModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<asp:Content ID="ContentHeadSEO" ContentPlaceHolderID="HeadSEO" runat="server">
    <% Title = "Fragesatz: " + Model.Name; %>

    <%var canonicalUrl = Settings.CanonicalHost + Links.SetDetail(Model.Name, Model.Id); %>    
    <link rel="canonical" href="<%= canonicalUrl %>">
    <meta name="description" content="<%= Model.Name.Replace("\"", "'").Replace("„", "'").Replace("“", "'").Truncate(40, true) %> (<%=Model.QuestionCount %> Fragen)<%= String.IsNullOrEmpty(Model.Text) ? "" : ": "+Model.Text.Replace("\"", "'").Replace("„", "'").Replace("“", "'").Truncate(74, true) %> - Lerne mit memucho!">
    
    <meta property="og:url" content="<%= canonicalUrl %>" />
    <meta property="og:type" content="article" />
    <meta property="og:image" content="<%= Model.ImageFrontendData.GetImageUrl(350, false, imageTypeForDummy: ImageType.QuestionSet).Url %>" />
</asp:Content>

<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/Views/Sets/Detail/Set.css") %>
    <%= Scripts.Render("~/bundles/Set") %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <input type="hidden" id="hhdSetId" value="<%= Model.Set.Id %>"/>
    <div class="row">
        
        <%--<div class="col-xs-3 col-md-2 xxs-stack col-xs-push-9 col-md-push-10">--%>
        <div class="col-xs-12 col-md-2 col-md-push-10">
            <div class="navLinks">
                <a href="<%= Links.SetsAll() %>"><i class="fa fa-list">&nbsp;</i>Zur Übersicht</a>
                <% if(Model.IsOwner){ %>
                    <a href="<%= Links.QuestionSetEdit(Url, Model.Name, Model.Id) %>"><i class="fa fa-pencil">&nbsp;</i>Bearbeiten</a> 
                <% } %>
                
                <% if (Model.QuestionCount > 0) { %>
                    <%--<a class="btn btn-primary btn-sm" href="<%= Links.TestSessionStartForSet(Model.Name, Model.Id) %>" rel="nofollow" style="margin: 4px 0; margin-left: -15px;">
                        <i class="fa fa-play-circle">&nbsp;</i>Jetzt testen
                    </a>--%>
                    <%--<a style="font-size: 12px;" data-allowed="logged-in" data-allowed-type="learning-session" href="" rel="nofollow" class="show-tooltip" data-original-title="Übungssitzung zu diesem Fragesatz starten." >
                        <i class="fa fa-line-chart">&nbsp;</i>Jetzt üben
                    </a>--%>
                    <%--<a style="font-size: 12px;" href="<%= Links.GameCreateFromSet(Model.Id) %>" class="show-tooltip" rel="nofollow" data-original-title="Spiel mit Fragen aus diesem Fragesatz starten." >
                        <i class="fa fa-gamepad">&nbsp;</i>Spiel starten--%>
                    <%--</a>--%>
                    <%--<a style="font-size: 12px;" href="<%= Links.DateCreate(Model.Id) %>" class="show-tooltip" rel="nofollow" data-original-title="Termin mit diesem Fragesatz erstellen." >
                        <i class="fa fa-calendar">&nbsp;</i>Termin lernen
                    </a>--%>
                <% } %>
            </div>
        </div>
       <%-- <div class="xxs-stack col-xs-9 col-md-10 col-xs-pull-3 col-md-pull-2">--%>
        <div class="col-xs-12 col-md-10 col-md-pull-2">
            <div id="SetInfo" class="Box">
                <div class="">
                    <div class="row">
                        <div class="col-xs-12 col-sm-3">
                            <div class="ImageContainer">
                                <%= Model.ImageFrontendData.RenderHtmlImageBasis(350, false, ImageType.QuestionSet, "ImageContainer") %>
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-9">
                            <header>
                                <div>
                                    Fragesatz mit <%= Model.QuestionCount %> Frage<%= StringUtils.PluralSuffix(Model.QuestionCount, "n") %>
                                </div>
                                <h1 style="margin-top: 5px; font-size: 26px;">
                                    <%= Model.Name %>
                                </h1>
                            </header>
                            <div>
                                <%= Model.Text %>
                            </div>
                            <div style="display: inline-block; margin-top: 20px;">
                                <% foreach (var category in Model.Set.Categories){ %>
                                    <a href="<%= Links.CategoryDetail(category) %>"><span class="label label-category"><%= category.Name %></span></a>    
                                <% } %>
                            </div>        
                    
                            <div style="margin-top:6px;">
                                <span style="display: inline-block; font-size: 16px; font-weight: normal;" class="Pin" data-set-id="<%= Model.Id %>">
                                    <a href="#" class="noTextdecoration" style="font-size: 22px; height: 10px;">
                                        <i class="fa fa-heart show-tooltip iAdded <%= Model.IsInWishknowledge ? "" : "hide2" %>" style="color:#b13a48;" title="Aus deinem Wunschwissen entfernen"></i>
                                        <i class="fa fa-heart-o show-tooltip iAddedNot <%= Model.IsInWishknowledge ? "hide2" : "" %>" style="color:#b13a48;" title="Zu deinem Wunschwissen hinzuzufügen"></i>
                                        <i class="fa fa-spinner fa-spin  hide2 iAddSpinner" style="color:#b13a48;"></i>
                                    </a>
                                    <span class="show-tooltip" id="totalPins" title="Ist bei <%= Model.TotalPins%> Personen im Wunschwissen"><%= Model.TotalPins %>x</span>

                                    <span class="show-tooltip" title="<%= Model.ActiveMemory.TotalInActiveMemory %> von <%= Model.ActiveMemory.TotalQuestions%> Fragen 
                                        aus diesem Fragesatz <br> sind in deinem aktiven Wissen. <br><br> Im 'aktiven Wissen' ist eine Frage, wenn die<br> Antwortwahrscheinlichkeit über 90% liegt." 
                                        data-html="true" data-placement="bottom">
                                        <i class="fa fa-tachometer" style="margin-left: 20px; color: #69D069;"></i> 
                                        <%= Model.ActiveMemory.TotalInActiveMemory %>/<%= Model.ActiveMemory.TotalQuestions %>
                                    </span>
                                </span>
                            </div>
                            <div style="margin-top: 10px; margin-bottom: 10px; color: silver; font-size: 11px;">
                                erstellt von <a href="<%= Links.UserDetail(Model.Creator) %>" ><%= Model.CreatorName %></a> 
                                <span class="show-tooltip" title="erstellt am <%= Model.CreationDate %>">vor <%= Model.CreationDateNiceText%> <i class="fa fa-info-circle"></i></span> <br />
                            </div>
                            <div class="Divider" style="margin-top: 10px; margin-bottom: 5px;"></div>
                            <div class="BottomBar">
                                <div style="float: left; padding-top: 3px;">
                                    <div class="fb-share-button" style="width: 100%" data-href="<%= Settings.CanonicalHost + Links.SetDetail(Model.Name, Model.Id) %>" data-layout="button" data-size="small" data-mobile-iframe="true"><a class="fb-xfbml-parse-ignore" target="_blank" href="https://www.facebook.com/sharer/sharer.php?u=https%3A%2F%2Fdevelopers.facebook.com%2Fdocs%2Fplugins%2F&amp;src=sdkpreparse">Teilen</a></div> 
                                </div>
                                <%--<div class="dropdown">
                                    <% var buttonId = Guid.NewGuid(); %>
                                    <a href="#" id="<%=buttonId %>" class="dropdown-toggle  btn btn-link btn-sm ButtonOnHover ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                                        <i class="fa fa-ellipsis-v"></i>
                                    </a>
                                    <ul class="dropdown-menu" aria-labelledby="<%=buttonId %>">
                                        <li><a href="<%= Links.StartSetLearningSession(Model.Id) %>" data-allowed="logged-in" data-allowed-type="learning-session" rel="nofollow">Jetzt üben</a></li>
                                        <li><a href="<%= Links.GameCreateFromSet(Model.Id) %>"> Spiel starten</a></li>
                                        <li><a href="<%= Links.DateCreate(Model.Id) %>"> Termin anlegen</a></li>
                                    </ul>
                                </div>--%>
                                
                                <a href="<%= Links.StartSetLearningSession(Model.Id) %>" class="btn btn-link btn-sm" data-allowed="logged-in" data-allowed-type="learning-session" rel="nofollow">Jetzt üben</a>
                                <a href="<%= Links.GameCreateFromSet(Model.Id) %>" class="btn btn-link btn-sm"> Spiel starten</a>
                                <a href="<%= Links.DateCreate(Model.Id) %>" class="btn btn-link btn-sm"> Termin anlegen</a>
                            </div>
                            <div class="BottomBar">
                                <a href="<%= Links.TestSessionStartForSet(Model.Name, Model.Id) %>" class="btn btn-primary btn-sm " role="button" rel="nofollow">
                                    &nbsp;JETZT TESTEN
                                </a>
                            </div>
                        </div>
                    </div>
                </div>    
            </div>
            <h4>Fragesätze im Fragesatz</h4>
            <div id="rowContainer">
                <%  foreach(var questionRow in Model.QuestionsInSet){ %>
                    <% Html.RenderPartial("/Views/Sets/Detail/SetQuestionRowResult.ascx", questionRow); %>
                <% } %>
            </div>

            <div class="row">
                <div class="col-md-12">
                    <% if (Model.QuestionsInSet.Any()){ %>
                        <div class="pull-right">
                            <a href="<%= Links.GameCreateFromSet(Model.Id) %>" class="show-tooltip" data-original-title="Spiel mit Fragen aus diesem Termin starten." style="display: inline-block; padding-right: 15px; margin-top: 29px;">
                                <i class="fa fa-gamepad" style="font-size: 18px;">&nbsp;</i>Spiel starten
                            </a>
                            <a class="btn <%= Model.IsLoggedIn ? "btn-primary" : "btn-link" %>" data-btn="startLearningSession" data-allowed="logged-in" data-allowed-type="learning-session" href="<%= Links.StartSetLearningSession(Model.Id) %>" rel="nofollow">
                                <i class="fa fa-line-chart">&nbsp;</i>Jetzt üben
                            </a>
                            <a class="btn btn-primary" href="<%= Links.StartSetLearningSession(Model.Id) %>" rel="nofollow">
                                <i class="fa fa-play-circle">&nbsp;</i>Jetzt testen
                            </a>
                        </div>
                    <% } %>
                </div>
            </div> 

            <% if (Model.ContentRecommendationResult != null) { %>
                <h4 style="margin-top: 40px;">Lust auf mehr? Andere Nutzer lernen auch:</h4>
                <div class="row CardsLandscape" id="contentRecommendation">
                    <% foreach (var set in Model.ContentRecommendationResult.Sets)
                       {
                            Html.RenderPartial("~/Views/Welcome/WelcomeBoxSingleSet.ascx", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(set.Id));
                       } %>
                    <% foreach (var category in Model.ContentRecommendationResult.Categories)
                       {
                            Html.RenderPartial("Cards/CardSingleCategory", CardSingleCategoryModel.GetCardSingleCategoryModel(category.Id));
                       } %>
                    <% foreach (var set in Model.ContentRecommendationResult.PopularSets)
                       { 
                            Html.RenderPartial("~/Views/Welcome/WelcomeBoxSingleSet.ascx", WelcomeBoxSingleSetModel.GetWelcomeBoxSetSingleModel(set.Id));
                       } %>
                </div>
            <% } %>

        </div>
        
        
        <div class="col-lg-10 col-xs-9 xxs-stack">
            
        </div>
    
    </div>

</asp:Content>