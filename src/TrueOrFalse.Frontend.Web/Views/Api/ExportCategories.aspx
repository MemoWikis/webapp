<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<ExportCategoriesModel>" ContentType="text/xml" %>
<trueorfalse>    
    <version>0.1</version>
    <% foreach (var question in Model.Categories) { %>
    <category>
        <name><%=question.Name %></name>        
        <% foreach (var related in question.RelatedCategories) { %>
        <related><%=related.Name %></related>           
        <% } %>
    </category>     
    <% } %>    
</trueorfalse>