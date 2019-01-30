<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<TopicNavigationModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<li class="module" markdown="<%: Model.Markdown %>">
    <content-module inline-template >
    
        <div @mouseenter="updateHoverState(true)" @mouseleave="updateHoverState(false)" class="ContentModule" v-if="!isDeleted" >
            <div class="ModuleBorder" :class="{ active : hoverState }">
            <% if (Model.CategoryList.Any()) {
                    if(!String.IsNullOrEmpty(Model.Title)){%>
                        <h2><%: Model.Title %></h2>
                    <% }
                    if(!String.IsNullOrEmpty(Model.Text)){%>
                         <p><%: Model.Text %></p>
                    <% } %>

                <div class="topicNavigation row" style= <%= Model.CategoryList.Count == 1 ? " \"justify-content: start;\" " : "" %>>
                    <% foreach (var category in Model.CategoryList)
                        { %>
                            <% if(Model.GetTotalSetCount(category) > 0 || Model.GetTotalQuestionCount(category) > 0 || Model.IsInstallationAdmin)
                               { %>
                                <div class="col-xs-6 topic">
                                    <div class="row">
                                        <div class="col-xs-3">
                                            <div class="ImageContainer">
                                                <%= Model.GetCategoryImage(category).RenderHtmlImageBasis(128, true, ImageType.Category, linkToItem: Links.CategoryDetail(category.Name, category.Id)) %>
                                            </div>
                                        </div>
                                        <div class="col-xs-9">
                                            <a class="topic-name" href="<%= Links.GetUrl(category) %>">
                                                <div class="topic-name">
                                                    <% if (Model.GetTotalSetCount(category) < 1 && Model.GetTotalQuestionCount(category) < 1 && Model.IsInstallationAdmin) { %>
                                                        <i class="fa fa-user-secret show-tooltip" data-original-title="Thema ist leer und wird daher nur Admins angezeigt"></i>
                                                    <% } %>
                                                    <%= category.Type.GetCategoryTypeIconHtml() %><%: category.Name %>
                                                </div>
                                            </a>
                                            <div class="set-question-count">
                                                <%: Model.GetTotalSetCount(category) %> Lernset<% if(Model.GetTotalSetCount(category) != 1){ %>s&nbsp;<% } else { %>&nbsp;<% } %>
                                                <%: Model.GetTotalQuestionCount(category) %> Frage<% if(Model.GetTotalQuestionCount(category) != 1){ %>n<% } %>
                                            </div>
                                            <div class="KnowledgeBarWrapper">
                                                <% Html.RenderPartial("~/Views/Categories/Detail/CategoryKnowledgeBar.ascx", new CategoryKnowledgeBarModel(category)); %>
                                                <div class="KnowledgeBarLegend">Dein Wissensstand</div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            <% } %>
                    <% } %>
                </div>
            <% } else { %>
                <div class="hidden">&nbsp;</div><% //if empty, templateparser throws error %>
            <% } %>
            
                <div v-if="canBeSorted">
                    <div class="Button Handle" v-if="hoverState">
                        <i class="fa fa-bars"></i>
                    </div>

                    <div class="Button dropdown" v-if="hoverState">
                        <a href="#" id="Dropdown" class="dropdown-toggle btn btn-link btn-sm ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true" >
                            <i class="fa fa-ellipsis-v"></i>
                        </a>
                        <ul class="dropdown-menu dropdown-menu-right" aria-labelledby="Dropdown">
                            <li><a href="" data-allowed="logged-in"><i class="fa fa-copy"></i> Duplizieren</a></li>
                            <li><a href="" data-allowed="logged-in"><i class="fa fa-caret-up"></i> Inhalt oben einfügen</a></li>
                            <li><a href="" data-allowed="logged-in"><i class="fa fa-caret-down"></i> Inhalt unten einfügen</a></li>
                            <li><a href="" data-allowed="logged-in"><i class="fa fa-code"></i> Als HTML bearbeiten</a></li>
                            <li class="delete"><a href="" data-allowed="logged-in" @click.prevent="deleteModule()"><i class="fa fa-trash"></i> Löschen</a></li>
                        </ul>
                    </div>
                </div>

            
            </div>
        </div>
   
    </content-module>    
</li>
