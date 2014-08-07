<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TrueOrFalse.Category>" %>
<%@ Import Namespace="System.Activities.Statements" %>
<%@ Import Namespace="System.Web.Razor.Parser.SyntaxTree" %>
<%@ Import Namespace="Microsoft.Ajax.Utilities" %>
<%@ Import Namespace="TrueOrFalse" %>

<%
    object type = Model.GetTypeModel();
    DateTime date;
    switch (Model.Type)
    {
        case CategoryType.Book:
            var book = (CategoryTypeBook) type;
%>
            <div class="Reference Book">
                <div class="Icon"><i class="fa fa-book"></i></div>
                <% if (!String.IsNullOrEmpty(book.Author))
                   {
                       var htmlAuthors = book.Author
                           .Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                           .Aggregate((a, b) => (a + ";&nbsp" + b)); %>
                        <div class="Author"><span><%= htmlAuthors %></span></div>
                <% }
            if (!String.IsNullOrEmpty(book.Title))
            {
                %><div class="Title"><%
                if (!String.IsNullOrEmpty(book.Subtitle))
                {
                    %><span><%= book.Title %> – <%= book.Subtitle %></span><%
                }
                else
                {
                    %><span><%= book.Title %></span><% 
                }
                %></div><%
            }
            if (!String.IsNullOrEmpty(book.PublicationCity) ||
                !String.IsNullOrEmpty(book.Publisher) ||
                !String.IsNullOrEmpty(book.PublicationYear))
            { %>
                    <div class="PublicationInfo">
                        <%
                            if (!String.IsNullOrEmpty(book.PublicationCity))
                            {
                                if (!String.IsNullOrEmpty(book.Publisher))
                                { %>
                                <span class="PublicationCity"><%= book.PublicationCity %>: </span><%
                                }
                                else
                                { %> 
                                <span class="PublicationCity"><%= book.PublicationCity %></span><%
                                }
                            }
                            if (!String.IsNullOrEmpty(book.Publisher))
                            { %>
                            <span class="Publisher"><%= book.Publisher %></span><%
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
                            } %>
                    </div>
                <% } %>
                    
                <% if (!String.IsNullOrEmpty(book.ISBN))
                   {
                %><div class="Isbn"><span>ISBN: <%= book.ISBN %></span></div>       
                <% }
            if (!String.IsNullOrEmpty(Model.WikipediaURL))
            {
                %><div class="WikiUrl"><a href="<%= Model.WikipediaURL %>"><span><%= Model.WikipediaURL %></span></a></div><%
            }
            if (!String.IsNullOrEmpty(Model.Description))
            {
                %><div class="Description"><span><%= Model.Description %></span></div><%
            } %>
            </div>
<%
            break;

        case CategoryType.Daily:
            var daily = (CategoryTypeDaily) type;
%>
           <div class="Reference Daily">
                <div class="Icon"><i class="fa fa-file-text-o"></i></div>
                <% if (!String.IsNullOrEmpty(Model.Name)){
                    %><div class="Name"><span><%= Model.Name %></span></div><%
                } 
                if (!String.IsNullOrEmpty(daily.ISSN)){
                    %><div class="Issn"><span>ISSN: <%= daily.ISSN %></span></div>       
                <% }
                if (!String.IsNullOrEmpty(daily.Publisher)){
                    %><div class="Publisher"><span><%= daily.Publisher %></span></div>       
                <% }
                if (!String.IsNullOrEmpty(daily.Url)){
                    %><div class="Url"><a href="<%= daily.Url %>"><span><%= daily.Url %></span></a></div><%
                }
                if (!String.IsNullOrEmpty(Model.WikipediaURL)){
                    %><div class="WikiUrl"><a href="<%= Model.WikipediaURL %>"><span><%= Model.WikipediaURL %></span></a></div><%
                }
                if (!String.IsNullOrEmpty(Model.Description)){
                    %><div class="Description"><span><%= Model.Description %></span></div><%
                } %>
            </div>
<%

            break;

        case CategoryType.DailyArticle:
            var dailyArticle = (CategoryTypeDailyArticle) type;
%>
           <div class="Reference DailyArticle">
                <div class="Icon"><i class="fa fa-file-text-o"></i></div>
                <% if (!String.IsNullOrEmpty(dailyArticle.Author))
                   {
                       var htmlAuthors = dailyArticle.Author
                           .Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                           .Aggregate((a, b) => (a + ";&nbsp" + b)); %>
                        <div class="Author"><span><%= htmlAuthors %></span></div>
                <% }
                if (!String.IsNullOrEmpty(dailyArticle.Title))
                {
                    %><div class="Title"><%
                    if (!String.IsNullOrEmpty(dailyArticle.Subtitle))
                    {
                        %><span><%= dailyArticle.Title %> – <%= dailyArticle.Subtitle %></span><%
                    }
                    else
                    {
                        %><span><%= dailyArticle.Title %></span><% 
                    }
                    %></div><%
                }
                if (!String.IsNullOrEmpty(dailyArticle.DailyIssue.Name)){
                    %><div class="DailyIssue">
                        <span><%= dailyArticle.DailyIssue.Name %></span><%
                        if (!String.IsNullOrEmpty(dailyArticle.PagesArticleFrom))
                        {
                        %><span class="Pages">
                            (S. <%= dailyArticle.PagesArticleFrom %><%
                             if (!String.IsNullOrEmpty(dailyArticle.PagesArticleTo))
                            {%>–<%= dailyArticle.PagesArticleTo %><%}
                         %>)</span><%
                        }
                                              
                    %></div>       
                <% }
                
                if (!String.IsNullOrEmpty(dailyArticle.Url)){
                    %><div class="Url"><a href="<%= dailyArticle.Url %>"><span><%= dailyArticle.Url %></span></a></div><%
                }
                if (!String.IsNullOrEmpty(Model.Description)){
                    %><div class="Description"><span><%= Model.Description %></span></div><%
                } %>
            </div>
<%
        break;

        case CategoryType.DailyIssue:
            var dailyIssue = (CategoryTypeDailyIssue) type;
        %>
           <div class="Reference DailyIssue">
                <div class="Icon"><i class="fa fa-file-text-o"></i></div>
                <% if (!String.IsNullOrEmpty(Model.Name)){
                    %><div class="Name"><span><%= Model.Name %></span></div><%
                }
                if (!String.IsNullOrEmpty(dailyIssue.Volume) || !String.IsNullOrEmpty(dailyIssue.No)){
                    %><div class="VolumeNo"> <%
                        if (!String.IsNullOrEmpty(dailyIssue.Volume))
                        {
                            %><span>Jg. <%= dailyIssue.Volume %></span><%
                            if (!String.IsNullOrEmpty(dailyIssue.No))
                            {
                                %><span>, Nr. <%= dailyIssue.No %></span><%                                   
                            }
                                                                  
                        } else {%><span>Nr. <%= dailyIssue.No %></span> <%}
                    %></div>       
                <% }

                if (DateTime.TryParse(dailyIssue.PublicationDateYear + "-" + dailyIssue.PublicationDateMonth + "-" + dailyIssue.PublicationDateDay, out date))
                {%>
                    <div class="PublicationDate">
                        <span>erschienen am 
                        <%= date.ToString("dd.MM.yyyy")%>
                        </span>
                    </div>   
                <%}
                if (!String.IsNullOrEmpty(Model.Description)){
                    %><div class="Description"><span><%= Model.Description %></span></div><%
                } %>
            </div>
<%
        break;
        
        case CategoryType.Magazine:
            var magazine = (CategoryTypeMagazine) type;
%>
           <div class="Reference Magazine">
                <div class="Icon"><i class="fa fa-file-text-o"></i></div>
                <% if (!String.IsNullOrEmpty(Model.Name)){
                    %><div class="Name"><span><%= Model.Name %></span></div><%
                } 
                if (!String.IsNullOrEmpty(magazine.ISSN)){
                    %><div class="Issn"><span>ISSN: <%= magazine.ISSN %></span></div>       
                <% }
                if (!String.IsNullOrEmpty(magazine.Publisher)){
                    %><div class="Publisher"><span><%= magazine.Publisher %></span></div>       
                <% }
                if (!String.IsNullOrEmpty(magazine.Url)){
                    %><div class="Url"><a href="<%= magazine.Url %>"><span><%= magazine.Url %></span></a></div><%
                }
                if (!String.IsNullOrEmpty(Model.WikipediaURL)){
                    %><div class="WikiUrl"><a href="<%= Model.WikipediaURL %>"><span><%= Model.WikipediaURL %></span></a></div><%
                }
                if (!String.IsNullOrEmpty(Model.Description)){
                    %><div class="Description"><span><%= Model.Description %></span></div><%
                } %>
            </div>
<%

            break;
            
            case CategoryType.MagazineIssue:
            var magazineIssue = (CategoryTypeMagazineIssue) type;
        %>
           <div class="Reference MagazineIssue">
                <div class="Icon"><i class="fa fa-file-text-o"></i></div>
                <% if (!String.IsNullOrEmpty(Model.Name)){
                    %><div class="Name"><span><%= Model.Name %></span></div><%
                }
                if (!String.IsNullOrEmpty(magazineIssue.Title))
                {
                    %><div class="Title"><span>"<%= magazineIssue.Title %>"</span></div><%
                }
                if (DateTime.TryParse(magazineIssue.PublicationDateYear + "-" + magazineIssue.PublicationDateMonth + "-" + magazineIssue.PublicationDateDay, out date))
                {%>
                    <div class="PublicationDate">
                        <span>erschienen am 
                        <%= date.ToString("dd.MM.yyyy")%>
                        </span>
                    </div>   
                <%}
                if (!String.IsNullOrEmpty(magazineIssue.Volume))
                {
                    %><div class="Volume"><span>Jg. <%= magazineIssue.Volume %></span></div><%
                }
                if (!String.IsNullOrEmpty(Model.Description)){
                    %><div class="Description"><span><%= Model.Description %></span></div><%
                } %>
            </div><%

            break;

    }
    //Urls!
    
%> 

                  





