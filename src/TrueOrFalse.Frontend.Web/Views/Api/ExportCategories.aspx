<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<ExportCategoriesModel>" ContentType="text/xml" %>
<trueorfalse>    
    <version>0.1</version>
    <% foreach (var question in Model.Categories) { %>
    <category>
        <name><%=question.Name %></name>
        <relatedCategories>
        <% foreach (var related in question.RelatedCategories) { %>
            <id><%=related.Name %></id>
        <% } %>
        </relatedCategories>
    </category>     
    <% } %>    
</trueorfalse>