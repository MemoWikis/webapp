<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" 
    Inherits="System.Web.Mvc.ViewPage<QuestionSetModel>" Title="Fragesatz" %>

<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/Views/QuestionSets/QuestionSet.css") %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="span7">
        <div class="box box-main" >
            <h2 class="pull-left"><%= Model.Name %></h2>
            <div class="pull-right">
                <div>
                    <a href="/QuestionSets" style="font-size: 12px; margin: 0px;"><i class="icon-list"></i>&nbsp;zur Übersicht</a><br/>
                    <% if(Model.IsOwner){ %>
                        <a href="<%= Links.QuestionSetEdit(Url, Model.Id) %>" style="font-size: 12px; margin: 0px;"><i class="icon-pencil"></i>&nbsp;bearbeiten</a> 
                    <% } %>
                </div>
            </div>
        
            <div class="box-content" style="min-height: 120px; clear: both; padding-top: 10px;">
                <% var index = 0; foreach(var questionInSet in Model.QuestionsInSet){ index++; %>
                    <div style="margin-bottom: 5px;">
                        <img src="/Images/no-question-50.png" width="32" style="vertical-align: top;" />
                        <div style="display: inline-block; width: 385px; line-height: 15px; height: 32px; overflow:hidden;">
                            <%= index %> <a href="<%= Links.AnswerQuestion(Url, questionInSet.Question, 0) %>"><%=questionInSet.Question.Text %></a>    
                        </div>
                        <div class="show-tooltip active" data-placement="right" 
                                data-original-title="72% Wahrscheinlichkeit, dass Du die Frage richtig beantwortest. Schnitt: 71% "
                            style="display: inline-block; width: 80px; background-color:beige; height: 32px;vertical-align: top;">
                            <span style="font-size: 25px; color: green; position: relative; top: 7px; left: 14px;">72%</span>
                        </div>
                        
                    </div>
                <% } %>

                <div style="margin-top: 10px; margin-right: 10px; height: 40px;" class="pull-right">
                    <a class="btn btn-info" href="#"> Jetzt üben</a>
                    <a class="btn btn-primary" href="#"><i class="icon-lightbulb"></i> Jetzt testen</a>
                </div>
                <div style="clear: both"></div>
            </div>
        </div>
    </div>
    
    <div class="span3">
        <div>
            <div class="box">
                <img src="<%= Model.ImageUrl_206px %>"/>
            </div>

            <div class="box">
                <div class="box-content">
                    von: <a href="<%= Links.UserDetail(Url, Model.Creator) %>"> <%= Model.CreatorName %> </a> <br/>
                    vor <a href="#" class="show-tooltip" title="erstellt am <%= Model.CreationDate %>" ><%= Model.CreationDateNiceText%></a> <br />
                </div>
            </div>
            
            <div class="box">
                <div class="box-content">
                    <b style="color: darkgray">Alle</b><br/>
                    gemerkt: 837x  (Rang: 7)<br/>          
                    gesehen: 20x (Rang: 71)<br/>
                    
                    <b style="color: darkgray; margin-top:7px; display: block">Ich</b>
                    gesehen: 2x<br/>
                    merken: 
                    
                </div>
            </div>
        </div>
    </div>

</asp:Content>