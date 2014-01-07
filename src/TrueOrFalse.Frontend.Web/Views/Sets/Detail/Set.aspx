<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" 
    Inherits="ViewPage<SetModel>" Title="Fragesatz" %>

<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/Views/Sets/Detail/Set.css") %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="col-md-7" style="margin-bottom: 20px;">
        <h2 style="margin-top:0px;" >
            <%= Model.Name %>
            <span style="display: inline-block; font-size: 20px; font-weight: normal;">
                &nbsp;(gemerkt: 22x)
            </span>
        </h2>
    </div>
    <div class="col-md-2" style="margin-bottom: 20px;">
        <div class="pull-right">
            <a href="<%= Links.Sets(Url) %>" style="font-size: 12px; margin: 0px;"><i class="fa fa-list"></i>&nbsp;zur Übersicht</a><br/>
            <% if(Model.IsOwner){ %>
                <a href="<%= Links.QuestionSetEdit(Url, Model.Id) %>" style="font-size: 12px; margin: 0px;"><i class="fa fa-pencil"></i>&nbsp;bearbeiten</a> 
            <% } %>
        </div>
    </div>
   
    <div class="col-md-7">
        <% var index = 0; foreach(var questionInSet in Model.QuestionsInSet){ index++; %>
            <div class="row question-row">
                <div class="col-md-11 col-1">                    
                    <img src="<%= QuestionImageSettings.Create(questionInSet.Id).GetUrl_128px_square().Url %>" class="img-responsive" />

                    <a href="<%= Links.AnswerQuestion(Url, questionInSet.Question, Model.Set) %>" style="font-weight:normal; font-size:large;">
                        <%=questionInSet.Question.Text %>
                    </a>    
                </div>
                <div class="col-md-1 col-2">
                    <div class="show-tooltip active pull-right" data-placement="right" 
                        data-original-title="72% Wahrscheinlichkeit, dass Du die Frage richtig beantwortest. Schnitt: 71% ">
                        <span style="font-size: 25px; color: green;">72%</span>
                    </div>
                </div>
            </div>
        <% } %>

        <div class="row "style="margin-top: 30px; height: 40px;">
            <div class="col-md-12">
                <div class="pull-right">
                    <a class="btn btn-info" href="<%= Links.AnswerQuestion(Url, Model.QuestionsInSet.First().Question, Model.Set) %>">
                         Jetzt üben
                    </a>
                    <a class="btn btn-primary" href="<%= Links.AnswerQuestion(Url, Model.QuestionsInSet.First().Question, Model.Set) %>"><i class="fa fa-lightbulb-o"></i> 
                        Jetzt testen
                    </a>
                </div>
            </div>
        </div>
    </div>
    
    <div class="col-md-2">
        
        <div>
            <img src="<%= Model.ImageUrl %>" class="img-responsive" style="border-radius:5px;" />
        </div>

        <div style="margin-top: 10px;">
            von: <a href="<%= Links.UserDetail(Url, Model.Creator) %>"> <%= Model.CreatorName %> </a> <br/>
            vor <a href="#" class="show-tooltip" title="erstellt am <%= Model.CreationDate %>" ><%= Model.CreationDateNiceText%></a> <br />                
        </div>
            
        <div style="margin-top: 10px;">
            <b style="color: darkgray">Alle</b><br/>
            gemerkt: 837x  (Rang: 7)<br/>          
            gesehen: 20x (Rang: 71)<br/>
                    
            <b style="color: darkgray; margin-top:7px; display: block">Ich</b>
            gesehen: 2x<br/>
            merken: 
        </div>
    </div>

</asp:Content>