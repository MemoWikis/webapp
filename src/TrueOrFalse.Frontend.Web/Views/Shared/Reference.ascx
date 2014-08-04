<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TrueOrFalse.Category>" %>
<%@ Import Namespace="TrueOrFalse" %>

<%
    object type = Model.GetTypeModel();
    switch (Model.Type)
    {
       case CategoryType.Book:
           var book = (CategoryTypeBook)type;
%>
            <div class="Reference Book">
                <% if (!String.IsNullOrEmpty(book.Author)){
                    var htmlAuthors = book.Author
                        .Split(Environment.NewLine.ToCharArray(),StringSplitOptions.RemoveEmptyEntries)
                        .Aggregate((a, b) => (a +
                            "</span><span class='Separator'>;&nbsp</span>" +
                            "<span class='Text'>" + b)); %>
                    <div class="Author"><span class="Text"><%= htmlAuthors %></span><span class="Separator">: </span>
                    </div>
                <% }
                if (!String.IsNullOrEmpty(book.Title)){%>
                    <span class="Title"><span class="Text"><%= book.Title %></span></span><%
                }
                if (!String.IsNullOrEmpty(book.Subtitle)){
                    %><span class="Subtitle"><span class="Separator">&nbsp;– </span><span class="Text"><%= book.Subtitle %></span></span><%
                } %><span class="Separator">. </span><%
                if (!String.IsNullOrEmpty(book.PublicationCity)){
                    %><span class="PublicationCity"><span class="Text"><%= book.PublicationCity %></span></span><%
                }
                if (!String.IsNullOrEmpty(book.Publisher)){
                    if (!String.IsNullOrEmpty(book.PublicationCity)){
                        %><span class="Publisher"><span class="Separator">: </span><span class="Text"><%= book.Publisher %></span></span>       
                    <% } else { 
                        %><span class="Publisher"><span class="Text"><%= book.Publisher %></span></span><% 
                    }
                }
                if (!String.IsNullOrEmpty(book.PublicationYear)){
                    %><span class="PublicationYear"><span class="Separator"> – </span><span class="Text"><%= book.Subtitle %></span></span><%
                }
                if (!String.IsNullOrEmpty(book.Subtitle)){
                    %><span class="Subtitle"><span class="Separator"> – </span><span class="Text"><%= book.Subtitle %></span><span class="Separator">.</span></span><%
                } %>
            </div>
<%
           break; 
   
       case CategoryType.Daily:
           var daily = (CategoryTypeDaily)type;
            
%>
           <html></html>
<%
           break; 
%> 




<% } %>
