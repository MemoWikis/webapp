<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionRowModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<div class="row question-row">
    <div class="column-1" >
        <div>Gemerkt: <img src="/Images/star.png"/> </div>
        <div>Relevanz: 4 (&#216;   4,2)</div>
        <div>Qualtität: 3 (&#216;   4,6)</div>        
    </div>

    <div class="column-2">
        <div style="height: 20px;">
            <img src="/Images/delete.png"/> 

            <a href="<%= Url.Action(Links.EditQuestion, Links.EditQuestionController, new {id = Model.QuestionId}, null) %>">
                <img src="/Images/edit.png"/> 
            </a>
            
            von <a href="<%= Model.AnswerQuestionLink(Url)  %>"><%=Model.CreatorName %></a>
        </div>
        <div style="height: 100%; font-weight:normal; font-size:large;">
            <a href="<%= Model.AnswerQuestionLink(Url) %>"><%=Model.QuestionShort%></a>
        </div>   
    </div>

    <div class="column-3">
       <div class="row header" >
           <div class="column answersTotal ">Antwort</div>
           <div class="column truePercentage" style="color:green">richtig</div>
           <div class="column last falsePercentage" style="color:red">falsch</div>
       </div>

       <div class="row">
           <div class="column answersTotal ">Alle: 4237x</div>
           <div class="column truePercentage">64%</div>
           <div class="column last falsePercentage">36%</div>       
       </div>

       <div class="row">
           <div class="column answersTotal">Ich: 4237x</div>
           <div class="column truePercentage">48%</div>
           <div class="column last falsePercentage">52%</div>       
       </div>

    </div>
</div>