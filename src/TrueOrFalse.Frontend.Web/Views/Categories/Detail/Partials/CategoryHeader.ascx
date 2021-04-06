<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div id="CategoryHeader">
    
    <% var buttonId = Guid.NewGuid(); %>
    <div id="HeadingSection">
        <div class="ImageContainer">
            <%= Model.ImageFrontendData.RenderHtmlImageBasis(128, true, ImageType.Category, linkToItem: Links.CategoryDetail(Model.Category)) %>
        </div>
        <div id="HeadingContainer">
            <h1 style="margin-bottom: 0">
                <%= Model.Name %>
                <%if (Model.Category.Visibility == CategoryVisibility.Owner) {%><i class="fas fa-lock header-icon"></i>
                <%} %>
            </h1>
            <div>
                <div class="greyed">
                    
                    <% if (!Model.Category.IsHistoric) { %>
                        <div class="Button Pin mobileHeader" data-category-id="<%= Model.Id %>">
                            <a href="#" class="noTextdecoration" style="font-size: 22px; height: 10px;">
                                <%= Html.Partial("AddToWishknowledge", new AddToWishknowledge(Model.IsInWishknowledge, displayAdd: false)) %>
                            </a>
                        </div>
                    <% } %>

                    <%= Model.Category.Type == CategoryType.Standard ? "Thema" : Model.Type %> mit <% if (Model.AggregatedTopicCount == 1)
                                                                                                      { %> 1 Unterthema und <% }
                                                                                                      if (Model.AggregatedTopicCount > 1)
                                                                                                      { %> <%= Model.AggregatedTopicCount %> Unterthemen und <% } %><%= Model.CountAggregatedQuestions %> Frage<%= StringUtils.PluralSuffix(Model.CountAggregatedQuestions, "n") %>
                    <% if (Model.IsInstallationAdmin) { %>
                        <a href="#" id="jsAdminStatistics">
                            <span style="margin-left: 10px; font-size: smaller;" class="show-tooltip" data-placement="right" data-original-title="Nur von admin sichtbar">
                                (<i class="fas fa-user-cog" data-details="<%= Model.GetViewsPerDay() %>">&nbsp;</i><%= Model.GetViews() %> views)
                            </span>
                        </a>
                        <div id="last60DaysViews" style="display: none"></div>
                    <% } %>
                </div>
            </div>
            <% if (!Model.Category.IsHistoric) { %>
                <div class="KnowledgeBarWrapper mobileHeader">
                    <% Html.RenderPartial("~/Views/Categories/Detail/CategoryKnowledgeBar.ascx", new CategoryKnowledgeBarModel(Model.Category)); %>
                </div>
            <% } %>
            <%if (Model.Category.Visibility == CategoryVisibility.Owner) {%>
                <% Html.RenderPartial("~/Views/Categories/Detail/Partials/PublishCategory/PublishCategory.vue.ascx"); %>
            <%} %>
        </div>
    </div>

    <% if (!Model.Category.IsHistoric) { %>
        <div id="TabsBar">
            <div id="CategoryTabsApp" class="Tabs">
                <div id="TopicTab" class="Tab" data-url="<%=Links.CategoryDetail(Model.Name, Model.Id) %>" >
                    <div class="center-tab">
                        <a href="">
                            <%= Model.Category.Type == CategoryType.Standard ? "Thema" : "Übersicht" %>
                        </a>
                    </div>

                </div>
                <div id="LearningTabWithOptions" class="Tab">
                    <div id="LearningTab" class="Tab" data-url="<%=Links.CategoryDetailLearningTab(Model.Name, Model.Id) %>">
                        <a href="" >
                            Fragen
                        </a>
                        <div id="LearnOptionsHeaderContainer">
                            <i id="LearnOptionsHeader" class="fa fa-cog disable" aria-hidden="true" data-toggle="tooltip" data-html="true" title="<p style='width: 200px'><b>Persönliche Filter helfen Dir</b>. Nutze die Lernoptionen und entscheide welche Fragen Du lernen möchtest.</p>">
                            </i>
                            <% if (!Model.ShowLearningSessionConfigurationMessageForTab)
                               { %>
                                <div id="SessionConfigReminderHeader" class="hide">
                                    <span>
                                        <img src="/Images/Various/SessionConfigReminder.svg" class="session-config-reminder-header">
                                    </span>
                                    <span class="far fa-times-circle"></span>
                                </div>
                            <% } %>
                        </div>


                    </div>
                </div>
            </div>
            <div id="Management">
                <div class="Border hide-sm"></div>
                <div class="KnowledgeBarWrapper col-md-3 hide-sm">
                    <% Html.RenderPartial("~/Views/Categories/Detail/CategoryKnowledgeBar.ascx", new CategoryKnowledgeBarModel(Model.Category)); %>
                    <%--<div class="KnowledgeBarLegend">Dein Wissensstand</div>--%>
                </div>
                <div class="Border hide-sm"></div>
                <div class="Buttons">
                    <div class="PinContainer hide-sm">
                        <div class="Button Pin pinHeader" data-category-id="<%= Model.Id %>">
                            <a href="#" class="noTextdecoration" style="font-size: 22px;">
                                <%= Html.Partial("AddToWishknowledge", new AddToWishknowledge(Model.IsInWishknowledge, isHeader: true)) %>
                            </a>
                        </div>
                    </div>

                    <div id="MyWorldToggleApp" :class="{'active': showMyWorld}">
                        <div class="toggle-label">
                            <div>
                                Zeige nur mein
                                <br/>
                                <b>Wunschwissen</b>
                            </div>
                        </div>
                        <% Html.RenderPartial("~/Views/Shared/MyWorldToggle/MyWorldToggleComponent.vue.ascx"); %>
                    </div>
                    <div class="Button dropdown DropdownButton">
                        <% buttonId = Guid.NewGuid(); %>
                        <a href="#" id="<%= buttonId %>" class="dropdown-toggle  btn btn-link btn-sm ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                            <i class="fa fa-ellipsis-v"></i>
                        </a>
                        <ul class="dropdown-menu dropdown-menu-right" aria-labelledby="<%= buttonId %>">
                            <li><a href="<%= Links.CategoryHistory(Model.Id) %>"><i class="fa fa-code-fork"></i>&nbsp;Bearbeitungshistorie</a></li>
                            <li><a href="<%= Links.CreateQuestion(categoryId: Model.Id) %>" data-allowed="logged-in"><i class="fa fa-plus-circle"></i>&nbsp;Frage hinzufügen</a></li>
                            <li><a href="<%= Links.CategoryCreate(Model.Id) %>" data-allowed="logged-in"><i class="fa fa-plus-circle"></i>&nbsp;Unterthema hinzufügen</a></li>
                            <li><a href="<%= Links.CategoryEdit(Url, Model.Name, Model.Id) %>" data-allowed="logged-in"><i class="fa fa-pencil"></i>&nbsp;bearbeiten (Expertenmodus)</a></li>
                            <li><a href="" id="AnalyticsTab" data-url="<%=Links.CategoryDetailAnalyticsTab(Model.Name, Model.Id) %>" data-allowed="logged-in" class="Tab" ><i class="fas fa-project-diagram"></i>&nbsp;Wissensnetz anzeigen</a></li>
                        </ul>
                    </div>
                </div>
                

            </div>
        </div>
    <% } %>
    
</div>
<%= Scripts.Render("~/bundles/js/PublishCategory") %>
<%= Scripts.Render("~/bundles/js/MyWorldToggle") %>
