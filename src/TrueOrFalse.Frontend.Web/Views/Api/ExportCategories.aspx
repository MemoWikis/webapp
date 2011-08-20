<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<ExportCategoriesModel>" ContentType="text/xml" %>
<trueorfalse>    
    <version>0.1</version>
    <% foreach (var question in Model.Categories) { %>
    <category>
        <name><%=question.Name %></name>        
        <% foreach (var subCategory in question.SubCategories) { %>
        <subCategory>
            <name><%=subCategory.Name %></name>
            <type><%=subCategory.Type %></type>
           <% foreach (var item in subCategory.Items) { %>
            <item>
                <name><%=item.Name %></name>
            </item>            
        <% } %>
        </subCategory>            
        <% } %>
    </category>     
    <% } %>    
</trueorfalse>