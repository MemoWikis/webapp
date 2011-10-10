<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<ExportQuestionsModel>" ContentType="text/xml" %>
<trueorfalse>    
    <version>0.1</version>
    <% foreach (var question in Model.Questions) { %>
    <question>
        <text><%=question.Text %></text>   
        <creatorId><%=question.Creator.Id %></creatorId>        
        <% foreach (var answer in question.Answers) { %>
        <answer>
            <text><%=answer.Text %></text>
            <creatorId><%=answer.Creator.Id%></creatorId>    
        </answer>            
        <% } %>
    </question>     
    <% } %>    
</trueorfalse>