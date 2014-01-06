<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" 
    Inherits="ViewPage<SetModel>" Title="Fragesatz" %>

<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/Views/Sets/Detail/Set.css") %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="col-md-6" style="margin-bottom: 20px;">
        <h2 style="margin-top:0px;" ><%= Model.Name %></h2>
    </div>
    <div class="col-md-3" style="margin-bottom: 20px;">
        <div class="pull-right">
            <a href="<%= Links.Sets(Url) %>" style="font-size: 12px; margin: 0px;"><i class="fa fa-list"></i>&nbsp;zur Übersicht</a><br/>
            <% if(Model.IsOwner){ %>
                <a href="<%= Links.QuestionSetEdit(Url, Model.Id) %>" style="font-size: 12px; margin: 0px;"><i class="fa fa-pencil"></i>&nbsp;bearbeiten</a> 
            <% } %>
        </div>
    </div>
   
    <div class="col-md-6">
        <% var index = 0; foreach(var questionInSet in Model.QuestionsInSet){ index++; %>
        
            <div class="row" style="margin-bottom: 15px;">
                <div class="col-md-1">
                    <img src="/Images/no-question-50.png" width="32" style="vertical-align: top;" />
                </div>
                <div class="col-md-9" style="line-height: 15px; height: 32px; overflow:hidden;">
                    <%= index %> <a href="<%= Links.AnswerQuestion(Url, questionInSet.Question, Model.Set) %>"><%=questionInSet.Question.Text %></a>    
                </div>
                <div class="col-md-2">
                    <div class="show-tooltip active pull-right" data-placement="right" 
                        data-original-title="72% Wahrscheinlichkeit, dass Du die Frage richtig beantwortest. Schnitt: 71% "
                        style="display: inline-block; width: 60px; background-color:beige; height: 32px;vertical-align: top;">
                        <span style="font-size: 25px; color: green; position: relative; top: 7px; left: 14px;">72%</span>
                    </div>
                </div>
            </div>
        <% } %>

        <div class="row "style="margin-top: 10px; height: 40px;">
            <div class="col-md-12">
                <div class="pull-right">
                    <a class="btn btn-info" href="#"> Jetzt üben</a>
                    <a class="btn btn-primary" href="#"><i class="fa fa-lightbulb-o"></i> Jetzt testen</a>
                </div>
            </div>
        </div>
    </div>
    
    <div class="col-md-3">
        
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