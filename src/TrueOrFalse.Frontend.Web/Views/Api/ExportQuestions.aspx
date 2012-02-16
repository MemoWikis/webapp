<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<ExportQuestionsModel>" ContentType="text/xml" %>
<trueorfalse>    
    <version>0.1</version>
    <% foreach (var question in Model.Questions) { %>
    <question>
        <text><%=question.Text %></text>
        <description><%= question.Description %></description>
        <visibility><%= question.Visibility %></visibility>
        <creatorId><%=question.Creator.Id %></creatorId>
        <solution><%=question.Solution %></solution>
        <categories>
            <% foreach (var category in question.Categories){ %>
                <id><%=category.Id %></id>
            <% } %>
        </categories>
    </question>    
    <% } %>    
</trueorfalse>