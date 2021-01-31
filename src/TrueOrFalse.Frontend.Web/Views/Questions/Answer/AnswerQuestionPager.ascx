<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AnswerQuestionModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div id="AnswerQuestionPager">
    <div class="Current">
        <% 
        if (!String.IsNullOrEmpty(Model.PageCurrent) && !String.IsNullOrEmpty(Model.PagesTotal)){
            if (Model.SourceIsCategory){ %>
                Frage <%= Model.PageCurrent %> von <%= Model.PagesTotal %> im Thema
            <% if(Model.SourceCategory.IsSpoiler(Model.Question)){ %>
                <a href="#" onclick="location.href='<%= Links.CategoryDetail(Model.SourceCategory) %>'" style="height: 30px">
                    <span class="label label-category" data-isSpolier="true" style="position: relative; top: -1px;">Spoiler</span>
                </a>
            <% } else { %>
                <a href="<%= Links.CategoryDetail(Model.SourceCategory) %>" style="height: 30px">
                    <span class="label label-category" style="position: relative; top: -1px;"><%= Model.SourceCategory.Name %></span>
                </a>
            <% } %>
        <% }
        } %>
    </div>
    <div class="Previous" style="padding-right: 5px;">
        <% if (Model.HasPreviousPage) { %>
            <a id="PreviousQuestionLink" class="btn btn-sm btn-default" href="<%= Model.PreviousUrl(Url) %>" rel="nofollow"><i class="fa fa-chevron-left"></i><span class="NavButtonText"> vorherige Frage</span></a>
        <% } %>
    </div>
    <div class="Next" style="padding-left: 5px;">
        <% if (Model.HasNextPage) { %>
            <a id="NextQuestionLink" class="btn btn-sm btn-default" href="<%= Model.NextUrl(Url) %>" rel="nofollow"><span class="NavButtonText" id="NextQuestion">nächste Frage </span><i class="fa fa-chevron-right"></i></a>
        <% } %>
    </div>
</div>