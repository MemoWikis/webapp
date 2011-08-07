<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<ExportCategoriesModel>" ContentType="text/xml" %>
<trueorfalse>    
    <version>0.1</version>
    <% foreach (var question in Model.Categories) { %>
    <category>
        <name><%=question.Name %></name>        
        <% foreach (var classification in question.Classifications) { %>
        <classification>
            <name><%=classification.Name %></name>
            <type><%=classification.Type %></type>
           <% foreach (var item in classification.Items) { %>
            <item>
                <name><%=item.Name %></name>
            </item>            
        <% } %>
        </classification>            
        <% } %>
    </category>     
    <% } %>    
</trueorfalse>