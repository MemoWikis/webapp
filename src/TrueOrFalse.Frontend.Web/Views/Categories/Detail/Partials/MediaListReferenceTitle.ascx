<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<Category>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<%
    object type = Model.GetTypeModel();
    DateTime date;
    switch (Model.Type)
    {
        case CategoryType.Book:
            var book = (CategoryTypeBook) type;
%>
            <div class="ReferenceTitle Book">
                <% if (!String.IsNullOrEmpty(book.Author)) {
                    var htmlAuthors = book.Author
                        .Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                        .Aggregate((a, b) => (a + ";&nbsp" + b)); %>
                    <div class="Author"><%= htmlAuthors %></div><%= String.IsNullOrEmpty(book.PublicationYear) ? "" : " ("+book.PublicationYear+")" %>:
                <% } %>
                
                <% if (!String.IsNullOrEmpty(book.Title)) { %>
                    <div class="Title">
                        <%= book.Title %><span class="subtitle"><%= String.IsNullOrEmpty(book.Subtitle) ? "" : " - " + book.Subtitle %></span>
                    </div>
                <% } %>
            </div>
            
        <% break;

        case CategoryType.Daily:
            var daily = (CategoryTypeDaily) type;
            %>
            <div class="ReferenceTitle Daily">
                <% if (!String.IsNullOrEmpty(Model.Name)){ %>
                    <div class="Title">
                        <%= Model.Name %>.
                    </div>
               <% } if (!String.IsNullOrEmpty(daily.Publisher)) { %>
                    <div class="Publisher"><%= daily.Publisher %></div>
               <% } %>

            </div>
            <%
            break;

        case CategoryType.DailyArticle:
            var dailyArticle = (CategoryTypeDailyArticle) type;
            %>
            
            <div class="ReferenceTitle DailyArticle">
                <% if (!String.IsNullOrEmpty(dailyArticle.Author)) {
                        var htmlAuthors = dailyArticle.Author
                            .Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                            .Aggregate((a, b) => (a + ";&nbsp" + b)); 
                        %><div class="Author"><%= htmlAuthors %>: </div>
                <% }
                if (!String.IsNullOrEmpty(dailyArticle.Title)) { %>
                   <div class="Title">
                        <%= dailyArticle.Title %><span class="subtitle"><%= String.IsNullOrEmpty(dailyArticle.Subtitle) ? "" : " - " + dailyArticle.Subtitle %></span>.
                   </div>
                <%}
                
                if (!String.IsNullOrEmpty(dailyArticle.DailyIssue.Name)) { %>
                    <div class="ParentIssue">
                        In: <span><%= dailyArticle.DailyIssue.Name %></span>
                    </div>
                <% } %>

            </div>
            <%
            break;

        case CategoryType.DailyIssue:
            var dailyIssue = (CategoryTypeDailyIssue) type;
%>
           <div class="ReferenceTitle DailyIssue">
                <% if (!String.IsNullOrEmpty(Model.Name)) { %>
                    <div class="Title">
                        <%= Model.Name %>
                    </div><%
                }
            %></div>
<%
            break;
        
        case CategoryType.Magazine:
            var magazine = (CategoryTypeMagazine) type;
%>
            <div class="ReferenceTitle Magazine">
                <% if (!String.IsNullOrEmpty(Model.Name)) { %>
                    <div class="Title"><%= Model.Name %>.</div>
                <% }
                if (!String.IsNullOrEmpty(magazine.Publisher)) { %>
                    <div class="Publisher"><%= magazine.Publisher %></div>
                <% } %>
            </div>
<%
            break;
            
        case CategoryType.MagazineArticle:
            var magazineArticle = (CategoryTypeMagazineArticle) type;
%>
           <div class="ReferenceTitle MagazineArticle">
                <% if (!String.IsNullOrEmpty(magazineArticle.Author)) {
                    var htmlAuthors = magazineArticle.Author
                        .Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                        .Aggregate((a, b) => (a + ";&nbsp" + b)); %>
                       <div class="Author"><%= htmlAuthors %>:</div>
               <% }
                if (!String.IsNullOrEmpty(magazineArticle.Title)) { %>
                    <div class="Title">
                        <%= magazineArticle.Title %><span class="subtitle"><%= String.IsNullOrEmpty(magazineArticle.Subtitle) ? "" : " - " + magazineArticle.Subtitle %></span>.
                    </div>
                <%}
                if (!String.IsNullOrEmpty(magazineArticle.MagazineIssue.Name)) { %>
                    <div class="ParentIssue">
                        In: <span><%= magazineArticle.MagazineIssue.Name %></span>
                    </div>
               <% } %>
           </div>
<%
            break;
            
        case CategoryType.MagazineIssue:
            var magazineIssue = (CategoryTypeMagazineIssue) type;
%>
           <div class="ReferenceTitle MagazineIssue">
                <% 
                if (!String.IsNullOrEmpty(Model.Name)) { %>
                    <div class="Name"><%= Model.Name %></div>
                <% } %>
           </div>
<%
            break;
            
        case CategoryType.VolumeChapter:
            var volumeChapter = (CategoryTypeVolumeChapter) type;
%>
            <div class="ReferenceTitle VolumeChapter">
                <% 
                if (!String.IsNullOrEmpty(volumeChapter.Author))
                {
                    var htmlAuthors = volumeChapter.Author
                        .Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                        .Aggregate((a, b) => (a + ";&nbsp" + b)); %>
                    <div class="Author"><%= htmlAuthors %></div><%= String.IsNullOrEmpty(volumeChapter.PublicationYear) ? "" : " ("+volumeChapter.PublicationYear+")" %>:
                <% }
                if (!String.IsNullOrEmpty(volumeChapter.Title))
                {  %>
                    <div class="Title">
                        <%= volumeChapter.Title %><span class="subtitle"><%= String.IsNullOrEmpty(volumeChapter.Subtitle) ? "" : " - " + volumeChapter.Subtitle %></span>.
                    </div>
                <%}
                if (!String.IsNullOrEmpty(volumeChapter.TitleVolume))
                {
                    %><div class="PublishedIn">Erschienen in: </div><%
                }
                if (!String.IsNullOrEmpty(volumeChapter.Editor))
                   {
                       var htmlEditors = volumeChapter.Editor
                           .Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                           .Aggregate((a, b) => (a + ";&nbsp" + b));
                        %><div class="Editor"><span><%= htmlEditors %></span> (Hrsg.):</div><%
                }
                if (!String.IsNullOrEmpty(volumeChapter.TitleVolume)) { %>
                    <div class="TitleVolume">
                        <%= volumeChapter.TitleVolume %><span class="subtitle"><%= String.IsNullOrEmpty(volumeChapter.SubtitleVolume) ? "" : " - " + volumeChapter.SubtitleVolume %></span>
                    </div>
                <% } %>

            </div>
<%
            break;
            
        case CategoryType.WebsiteArticle:
            var websiteArticle = (CategoryTypeWebsiteArticle) type;
%>
           <div class="ReferenceTitle WebsiteArticle">
                <% if (!String.IsNullOrEmpty(websiteArticle.Author)) {
                    var htmlAuthors = websiteArticle.Author
                        .Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                        .Aggregate((a, b) => (a + ";&nbsp" + b)); 
                    %><div class="Author"><%= htmlAuthors %></div><%
                    if (DateTime.TryParse(websiteArticle.PublicationDateYear + "-" + websiteArticle.PublicationDateMonth + "-" + websiteArticle.PublicationDateDay, out date)) { 
                    %> (<%= date.ToString("dd.MM.yyyy")%>):
                    <% }
                }
                if (!String.IsNullOrEmpty(websiteArticle.Title)) { %>
                    <div class="Title">
                        <%= websiteArticle.Title %><span class="subtitle"><%= String.IsNullOrEmpty(websiteArticle.Subtitle) ? "" : " - " + websiteArticle.Subtitle %></span>
                    </div>
                <%}
               
                //if (!String.IsNullOrEmpty(Model.Url)) //{ %>
                    <%--<div class="Url"><a href="<%= Model.Url %>"><span><%= Model.Url.Truncate(30,true) %></span></a></div>--%>
               <%// }
            %></div>
<%
            break;
        case CategoryType.Website:
%>
            <div class="ReferenceTitle Website">
                <div class="Title">
                    <%= Model.Name %>
                </div>
                <%//if (!String.IsNullOrEmpty(Model.Url)) //{ %>
                <%--<div class="Url"><a href="<%= Model.Url %>"><span><%= Model.Url.Truncate(30,true) %></span></a></div>--%>
                <%// } %>
            </div>
<%
            break;
    }
%> 
