<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Category>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<%
    object type = Model.GetTypeModel();
    DateTime date;
    switch (Model.Type)
    {
        case CategoryType.Book:
            var book = (CategoryTypeBook) type;
%>
            <div class="Reference Book">
                <div class="Icon show-tooltip" title="<%= CategoryType.Book.GetName() %>"><i class="fa fa-book"></i></div><% 
                
                if (!String.IsNullOrEmpty(book.Title))
                {%>
                    <div class="Name">
                        <a href="<%= Links.CategoryDetail(Model.Name, Model.Id) %>">
                            <span><%= book.Title %><%= String.IsNullOrEmpty(book.Subtitle) ? "" : " - " + book.Subtitle %></span>
                        </a>

                    </div><%
                }
                if (!String.IsNullOrEmpty(book.Author))
                   {
                       var htmlAuthors = book.Author
                           .Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                           .Aggregate((a, b) => (a + ";&nbsp" + b));
                        %><div class="Author"><span>von <%= htmlAuthors %></span></div><% 
                }
                if (!String.IsNullOrEmpty(book.PublicationCity) ||
                    !String.IsNullOrEmpty(book.Publisher) ||
                    !String.IsNullOrEmpty(book.PublicationYear))
                { 
                    %><div class="PublicationInfo"><%
                        if (!String.IsNullOrEmpty(book.PublicationCity))
                        {
                            if (!String.IsNullOrEmpty(book.Publisher))
                            { 
                            %><span class="PublicationCity"><%= book.PublicationCity %>: </span><%
                            }
                            else
                            {  
                            %><span class="PublicationCity"><%= book.PublicationCity %></span><%
                            }
                        }
                        if (!String.IsNullOrEmpty(book.Publisher))
                        { 
                        %><span class="Publisher"><%= book.Publisher %></span><%
                        }
                        if (!String.IsNullOrEmpty(book.PublicationYear))
                        {
                            if (!String.IsNullOrEmpty(book.PublicationCity) ||
                                !String.IsNullOrEmpty(book.Publisher))
                            {
                                %><span class="PublicationYear">, <%= book.PublicationYear %></span><%
                            }
                            else
                            {
                                %><span class="PublicationYear"><%= book.PublicationYear %></span><%
                            }
                        } 
                    %></div><% 
                }
                    
                if (!String.IsNullOrEmpty(book.ISBN))
                   {
                %><div class="Isbn"><span>ISBN: <%= book.ISBN %></span></div><% 
                }
                if (!String.IsNullOrEmpty(Model.WikipediaURL))
                {
                    %><br /><div class="WikiUrl"><a href="<%= Model.WikipediaURL %>" target="_blank"><span><%= Model.WikipediaURL %> <i class='fa fa-external-link'></i></span></a></div><%
                }
                if (!String.IsNullOrEmpty(Model.Url))
                {
                    %><br /><div class="Url"><a href="<%= Model.Url %>" target="_blank"><span><%= Model.Url %> <i class='fa fa-external-link'></i></span></a></div><%
                }
                %></div>
<%
            break;

        case CategoryType.Daily:
            var daily = (CategoryTypeDaily) type;
%>
           <div class="Reference Daily">
                <div class="Icon show-tooltip" title="<%= CategoryType.Daily.GetName() %>"><i class="fa fa-file-text-o"></i></div><% 
                if (!String.IsNullOrEmpty(Model.Name)){%>
                    <div class="Name">
                        <a href="<%= Links.CategoryDetail(Model.Name, Model.Id) %>">
                            <span><%= Model.Name %></span>
                        </a>
                    </div>
               <%}
                if (!String.IsNullOrEmpty(daily.Publisher)){
                    %><div class="Publisher"><span><%= daily.Publisher %></span></div><%
                }
                if (!String.IsNullOrEmpty(daily.ISSN)){
                    %><div class="Issn"><span>ISSN: <%= daily.ISSN %></span></div><%      
                }
                if (!String.IsNullOrEmpty(Model.Url)){
                    %><div class="Url"><a href="<%= Model.Url %>"><span><%= Model.Url %></span></a></div><%
                }
                if (!String.IsNullOrEmpty(Model.WikipediaURL)){
                    %><div class="WikiUrl"><a href="<%= Model.WikipediaURL %>"><span><%= Model.WikipediaURL %></span></a></div><%
                }
            %></div>
<%

            break;

        case CategoryType.DailyArticle:
            var dailyArticle = (CategoryTypeDailyArticle) type;
%>
           <div class="Reference DailyArticle">
                <div class="Icon show-tooltip" title="<%= CategoryType.DailyArticle.GetName() %>"><i class="fa fa-file-text-o"></i></div><% 
                if (!String.IsNullOrEmpty(dailyArticle.Title))
                {%>
                   <div class="Name">
                       <a href="<%= Links.CategoryDetail(Model.Name, Model.Id) %>">
                           <span><%= dailyArticle.Title %><%= String.IsNullOrEmpty(dailyArticle.Subtitle) ? "" : " - " + dailyArticle.Subtitle %></span>
                       </a>
                   </div>
                <%}
                if (!String.IsNullOrEmpty(dailyArticle.Author))
                {
                    var htmlAuthors = dailyArticle.Author
                        .Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                        .Aggregate((a, b) => (a + ";&nbsp" + b)); 
                    %><div class="Author"><span>von <%= htmlAuthors %></span></div><%
                }
                
                if (!String.IsNullOrEmpty(dailyArticle.DailyIssue.Name)){
                    %><div class="ParentIssue"><span><%= dailyArticle.DailyIssue.Name %></span><%
                        if (!String.IsNullOrEmpty(dailyArticle.PagesArticleFrom))
                        {
                        %> <span class="Pages">(S. <%= dailyArticle.PagesArticleFrom %><%
                             if (!String.IsNullOrEmpty(dailyArticle.PagesArticleTo))
                            {%>–<%= dailyArticle.PagesArticleTo %><%}
                         %>)</span><%
                        }
                    %></div><%
                }
                
                if (!String.IsNullOrEmpty(Model.Url)){
                    %><div class="Url"><a href="<%= Model.Url %>"><span><%= Model.Url %></span></a></div><%
                }
                
            %></div>
<%
            break;

        case CategoryType.DailyIssue:
            var dailyIssue = (CategoryTypeDailyIssue) type;
%>
           <div class="Reference DailyIssue">
                <div class="Icon show-tooltip" title="<%= CategoryType.DailyIssue.GetName() %>"><i class="fa fa-file-text-o"></i></div><% 
                if (!String.IsNullOrEmpty(Model.Name)){
                    %><div class="Name"><a href="<%= Links.CategoryDetail(Model.Name, Model.Id) %>"><span><%= Model.Name %></span></a></div><%
                }
                if (DateTime.TryParse(dailyIssue.PublicationDateYear + "-" + dailyIssue.PublicationDateMonth + "-" + dailyIssue.PublicationDateDay, out date))
                {
                    %><div class="PublicationDate"><span>erschienen am <%= date.ToString("dd.MM.yyyy")%></span></div><%
                }
                if (!String.IsNullOrEmpty(dailyIssue.Volume) || !String.IsNullOrEmpty(dailyIssue.No)){
                    %><div class="VolumeNo"><%
                        if (!String.IsNullOrEmpty(dailyIssue.Volume))
                        {
                            %><span>Jahrgang <%= dailyIssue.Volume %></span><%
                            if (!String.IsNullOrEmpty(dailyIssue.No))
                            {
                                %><span>, Nr. <%= dailyIssue.No %></span><%                                   
                            }
                        } else {%><span>Nr. <%= dailyIssue.No %></span><%}
                    %></div><%
                }
            %></div>
<%
            break;
        
        case CategoryType.Magazine:
            var magazine = (CategoryTypeMagazine) type;
%>
            <div class="Reference Magazine">
                <div class="Icon show-tooltip" title="<%= CategoryType.Magazine.GetName() %>"><i class="fa fa-file-text-o"></i></div><% 
                if (!String.IsNullOrEmpty(Model.Name)){
                    %><div class="Name"><a href="<%= Links.CategoryDetail(Model.Name, Model.Id) %>"><span><%= Model.Name %></span></a></div><%
                }
                if (!String.IsNullOrEmpty(magazine.Publisher)){
                    %><div class="Publisher"><span><%= magazine.Publisher %></span></div><%
                }
                if (!String.IsNullOrEmpty(magazine.ISSN)){
                    %><div class="Issn"><span>ISSN: <%= magazine.ISSN %></span></div><%
                }
                if (!String.IsNullOrEmpty(Model.Url)){
                    %><div class="Url"><a href="<%= Model.Url %>"><span><%= Model.Url %></span></a></div><%
                }
                if (!String.IsNullOrEmpty(Model.WikipediaURL)){
                    %><div class="WikiUrl"><a href="<%= Model.WikipediaURL %>"><span><%= Model.WikipediaURL %></span></a></div><%
                }
            %></div>
<%
            break;
            
        case CategoryType.MagazineArticle:
            var magazineArticle = (CategoryTypeMagazineArticle) type;
%>
           <div class="Reference MagazineArticle">
                <div class="Icon show-tooltip" title="<%= CategoryType.MagazineArticle.GetName() %>"><i class="fa fa-file-text-o"></i></div><% 
                if (!String.IsNullOrEmpty(magazineArticle.Title))
                {%>
                   <div class="Name">
                       <a href="<%= Links.CategoryDetail(Model.Name, Model.Id) %>">
                           <span><%= magazineArticle.Title %><%= String.IsNullOrEmpty(magazineArticle.Subtitle) ? "" : " - " + magazineArticle.Subtitle %></span>
                       </a>
                   </div>
                <%}
                if (!String.IsNullOrEmpty(magazineArticle.Author))
                   {
                       var htmlAuthors = magazineArticle.Author
                           .Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                           .Aggregate((a, b) => (a + ";&nbsp" + b)); %>
                        <div class="Author"><span>von <%= htmlAuthors %></span></div><% 
                }
                if (!String.IsNullOrEmpty(magazineArticle.MagazineIssue.Name))
                {
                %><div class="ParentIssue"><span><%= magazineArticle.MagazineIssue.Name %></span><%
                    if (!String.IsNullOrEmpty(magazineArticle.PagesArticleFrom))
                    {
                    %> <span class="Pages">(S. <%= magazineArticle.PagesArticleFrom %><%
                        if (!String.IsNullOrEmpty(magazineArticle.PagesArticleTo))
                        {%>–<%= magazineArticle.PagesArticleTo %><%}
                        %>)</span><%
                    }
                %></div><%
                }
                if (!String.IsNullOrEmpty(Model.Url))
                {
                    %><div class="Url"><a href="<%= Model.Url %>"><span><%= Model.Url %></span></a></div><%
                }
            %></div>
<%
            break;
            
        case CategoryType.MagazineIssue:
            var magazineIssue = (CategoryTypeMagazineIssue) type;
%>
           <div class="Reference MagazineIssue">
                <div class="Icon show-tooltip" title="<%= CategoryType.MagazineIssue.GetName() %>"><i class="fa fa-file-text-o"></i></div><% 
                if (!String.IsNullOrEmpty(Model.Name)){
                    %><div class="Name"><a href="<%= Links.CategoryDetail(Model.Name, Model.Id) %>"><span><%= Model.Name %></span></a></div><%
                }
                if (!String.IsNullOrEmpty(magazineIssue.Title))
                {
                    %><div class="Title"><span>Ausgabentitel: <%= magazineIssue.Title %></span></div><%
                }
                if (DateTime.TryParse(magazineIssue.PublicationDateYear + "-" + magazineIssue.PublicationDateMonth + "-" + magazineIssue.PublicationDateDay, out date))
                {
                    %><div class="PublicationDate"><span>erschienen am <%= date.ToString("dd.MM.yyyy")%></span></div><%
                }
                if (!String.IsNullOrEmpty(magazineIssue.Volume))
                {
                    %><div class="Volume"><span>Jahrgang <%= magazineIssue.Volume %></span></div><%
                }
            %></div>
<%
            break;
            
        case CategoryType.VolumeChapter:
            var volumeChapter = (CategoryTypeVolumeChapter) type;
%>
            <div class="Reference VolumeChapter">
                <div class="Icon show-tooltip" title="<%= CategoryType.VolumeChapter.GetName() %>"><i class="fa fa-book"></i></div><% 
                if (!String.IsNullOrEmpty(volumeChapter.Title))
                {  %>
                    <div class="Name">
                        <a href="<%= Links.CategoryDetail(Model.Name, Model.Id) %>">
                            <span><%= volumeChapter.Title %><%= String.IsNullOrEmpty(volumeChapter.Subtitle) ? "" : " - " + volumeChapter.Subtitle %></span>
                        </a>
                    </div>
                <%}
                if (!String.IsNullOrEmpty(volumeChapter.Author))
                   {
                       var htmlAuthors = volumeChapter.Author
                           .Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                           .Aggregate((a, b) => (a + ";&nbsp" + b));
                        %><div class="Author"><span>von <%= htmlAuthors %></span></div><% 
                }
                if (!String.IsNullOrEmpty(volumeChapter.TitleVolume))
                {
                    %><div class="NoSeperator">Erschienen in: </div><%
                }
                if (!String.IsNullOrEmpty(volumeChapter.Editor))
                   {
                       var htmlEditors = volumeChapter.Editor
                           .Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                           .Aggregate((a, b) => (a + ";&nbsp" + b));
                        %><div class="Editor"><span><%= htmlEditors %> (Hrsg.)</span></div><%
                }
                if (!String.IsNullOrEmpty(volumeChapter.TitleVolume))
                {
                    %><div class="TitleVolume"><%
                    if (!String.IsNullOrEmpty(volumeChapter.SubtitleVolume))
                    {
                        %><span><%= volumeChapter.TitleVolume %> – <%= volumeChapter.SubtitleVolume %></span><%
                    }
                    else
                    {
                        %><span><%= volumeChapter.TitleVolume %></span><% 
                    }
                    %></div><%
                }
                
                if (!String.IsNullOrEmpty(volumeChapter.PublicationCity) ||
                    !String.IsNullOrEmpty(volumeChapter.Publisher) ||
                    !String.IsNullOrEmpty(volumeChapter.PublicationYear))
                { 
                    %><div class="PublicationInfo"><%
                        if (!String.IsNullOrEmpty(volumeChapter.PublicationCity))
                            {
                                if (!String.IsNullOrEmpty(volumeChapter.Publisher))
                                {
                                %><span class="PublicationCity"><%= volumeChapter.PublicationCity %>: </span><%
                                }
                                else
                                { 
                                %><span class="PublicationCity"><%= volumeChapter.PublicationCity %></span><%
                                }
                            }
                            if (!String.IsNullOrEmpty(volumeChapter.Publisher))
                            { 
                            %><span class="Publisher"><%= volumeChapter.Publisher %></span><%
                            }
                            if (!String.IsNullOrEmpty(volumeChapter.PublicationYear))
                            {
                                if (!String.IsNullOrEmpty(volumeChapter.PublicationCity) ||
                                    !String.IsNullOrEmpty(volumeChapter.Publisher))
                                {
                                    %><span class="PublicationYear">, <%= volumeChapter.PublicationYear %></span><%
                                }
                                else
                                {
                                    %><span class="PublicationYear"><%= volumeChapter.PublicationYear %></span><%
                                }
                            } 
                    %></div><%
                }
                if (!String.IsNullOrEmpty(volumeChapter.ISBN))
                {
                    %><div class="Isbn"><span>ISBN: <%= volumeChapter.ISBN %></span></div><%
                }
                if (!String.IsNullOrEmpty(volumeChapter.PagesChapterFrom))
                {
                    %><div class="Pages">S. <%= volumeChapter.PagesChapterFrom %><%
                        if (!String.IsNullOrEmpty(volumeChapter.PagesChapterTo))
                        {%>–<%= volumeChapter.PagesChapterTo %><%}
                    %></div><%
                }
                if (!String.IsNullOrEmpty(Model.WikipediaURL))
                {
                    %><div class="WikiUrl"><a href="<%= Model.WikipediaURL %>"><span><%= Model.WikipediaURL %></span></a></div><%
                }
            %></div>
<%
            break;
            
        case CategoryType.WebsiteArticle:
            var websiteArticle = (CategoryTypeWebsiteArticle) type;
%>
           <div class="Reference WebsiteArticle">
                <div class="Icon show-tooltip" title="<%= CategoryType.WebsiteArticle.GetName() %>"><i class="fa fa-laptop"></i></div><%
                if (!String.IsNullOrEmpty(websiteArticle.Title))
                {%>
                    <div class="Name">
                        <a href="<%= Links.CategoryDetail(Model.Name, Model.Id) %>">
                            <span><%= websiteArticle.Title %><%= String.IsNullOrEmpty(websiteArticle.Subtitle) ? "" : " - " + websiteArticle.Subtitle %></span>
                        </a>
                    </div>
                <%}
                if (!String.IsNullOrEmpty(websiteArticle.Author))
                {
                    var htmlAuthors = websiteArticle.Author
                        .Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                        .Aggregate((a, b) => (a + ";&nbsp" + b)); 
                    %><div class="Author"><span>von <%= htmlAuthors %></span></div><%
                }
               
                if (DateTime.TryParse(websiteArticle.PublicationDateYear + "-" + websiteArticle.PublicationDateMonth + "-" + websiteArticle.PublicationDateDay, out date))
                {
                    %><div class="PublicationDate"><span>erschienen am <%= date.ToString("dd.MM.yyyy")%></span></div><%
                }
                if (!String.IsNullOrEmpty(Model.Url))
                {
                    %><div class="Url show-tooltip" title="<%= Model.Url %>"><a href="<%= Model.Url %>"><span><%= Model.Url %></span></a></div><%
                }
            %></div>
<%
            break;
    }
%> 

                  





