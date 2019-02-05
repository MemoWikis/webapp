<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AnswerQuestionModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="well" id="answerQuestionDetails" style="background-color: white; padding-bottom: 10px; min-height: 175px;">
    <div class="row">
        <div class="col-xs-6 xxs-stack">
            <p>
                von: <a href="<%= Links.UserDetail(Model.Creator) %>"><%= Model.CreatorName %></a><%= Model.Visibility != QuestionVisibility.All ? " <i class='fa fa-lock show-tooltip' title='Private Frage'></i>" : "" %><br />
                vor <span class="show-tooltip" title="erstellt am <%= Model.CreationDate %>" ><%= Model.CreationDateNiceText %></span> <br />
            </p>
            
            <div id="QuestionHistory">
                <a href="<%= Links.QuestionHistory(Model.QuestionId) %>" class="TextLinkWithIcon">
                    <i class="fa fa-list-ul"></i>
                    <span class="TextSpan">Bearbeitungshistorie</span>
                </a>
            </div>

            <% if (Model.IsOwner)
               { %>
                <%--<div class="navLinks">--%>
                <div id="EditQuestion">
                    <a href="<%= Links.EditQuestion(Url, Model.QuestionText, Model.QuestionId) %>" class="TextLinkWithIcon">
                        <i class="fa fa-pencil"></i>
                        <span class="TextSpan">Frage bearbeiten</span>
                    </a>
                </div>
            
                <div id="DeleteQuestion">
                    <a class="TextLinkWithIcon" data-toggle="modal" data-questionId="<%= Model.QuestionId %>" href="#modalDeleteQuestion">
                        <i class="fa fa-trash-o"></i> <span class="TextSpan">Frage löschen</span>
                    </a>
                </div>
                <%--</div>--%>
            <% } %>
        
            <% if (Model.Categories.Count > 0)
               { %>
                <p style="padding-top: 10px;">
                    <% Html.RenderPartial("CategoriesOfQuestion", Model.Question); %>
                </p>
            <% } %>
        
            <% if (Model.SetMinis.Count > 0)
               { %>
                <% foreach (var setMini in Model.SetMinis)
                   { %>
                    <a href="<%= Links.SetDetail(Url, setMini) %>"><span class="label label-set"><%: setMini.Name %></span></a>
                <% } %>
        
                <% if (Model.SetCount > 5)
                   { %>
                    <div style="margin-top: 3px;">
                        <a href="#" popover-all-sets-for="<%= Model.QuestionId %>">+  <%= Model.SetCount - 5 %> weitere </a>
                    </div>
                <% } %>

            <% } %>
        </div>
        <div class="col-xs-6 xxs-stack">
            <div style="padding-bottom: 20px;" id="answerHistory">
                <% Html.RenderPartial("HistoryAndProbability", Model.HistoryAndProbability); %>
            </div>
        
            <p>
                <span class="show-tooltip" title="Die Frage wurde <%= Model.TotalRelevancePersonalEntries %>x zum Wunschwissen hinzugefügt.">
                    <i class="fa fa-heart greyed"></i> 
                    <span id="sideWishKnowledgeCount"><%= Model.TotalRelevancePersonalEntries %>x</span><br />
                </span>                
                <span class="show-tooltip" title="Die Frage wurde <%= Model.TotalViews %>x mal gesehen.">
                    <i class="fa fa-eye"></i> <%= Model.TotalViews %>x
                </span><br />
            </p>

            <p style="width: 150px;">                    
                <div class="fb-share-button" style="margin-right: 10px; margin-bottom: 5px; float: left; " data-href="<%= Settings.CanonicalHost %><%= Links.AnswerQuestion(Model.Question) %>" data-layout="button" data-size="small" data-mobile-iframe="true"><a class="fb-xfbml-parse-ignore" target="_blank" href="https://www.facebook.com/sharer/sharer.php?u=https%3A%2F%2Fdevelopers.facebook.com%2Fdocs%2Fplugins%2F&amp;src=sdkpreparse">Teilen</a></div>
                    
                <div style="margin-top: 5px">
                    <a style="white-space: nowrap" href="#" data-action="embed-question"><i class="fa fa-code" aria-hidden="true">&nbsp;</i>Einbetten</a>
                </div>
            </p>
        </div>
    </div>
</div>