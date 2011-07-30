<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<ExportQuestionsModel>" ContentType="text/xml" %>
<trueorfalse>    
    <% foreach (var question in Model.Questions) { %>
    <question>
        <text><%=question.Text %></text>        
        <% foreach (var answer in question.Answers) { %>
        <answer>
            <text><%=answer.Text %></text>
        </answer>            
        <% } %>
    </question>     
    <% } %>    
</trueorfalse>