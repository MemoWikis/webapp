<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<ExportModel>" ContentType="text/xml" %>

<trueorfalse>    
    <version>0.1</version>
        <% foreach (var category in Model.Categories)
           { %>
    <category>
        <id><%=category.Id %></id>
        <name><%=category.Name %></name>
        <relatedCategories>
        <% foreach (var related in category.ParentCategories())
           { %>
            <id><%=related.Id %></id>
        <% } %>
        </relatedCategories>
    </category>     
    <% } %>    
    <% foreach (var question in Model.Questions)
       { %>
    <question>
        <id><%=question.Id%></id>
        <text><%=question.Text %></text>
        <description><%= question.Description %></description>
        <visibility><%= question.Visibility %></visibility>
        <creatorId><%=question.Creator.Id %></creatorId>
        <solution><%=question.Solution %></solution>
        <categories>
            <% foreach (var category in question.Categories)
               { %>
                <id><%=category.Id %></id>
            <% } %>
        </categories>
    </question>    
    <% } %>    
</trueorfalse>
