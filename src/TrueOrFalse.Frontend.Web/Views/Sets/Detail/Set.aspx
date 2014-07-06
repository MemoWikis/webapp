<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" 
    Inherits="ViewPage<SetModel>" %>

<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
    <title>Fragesatz - <%= Model.Set.Name %></title>
    <%= Styles.Render("~/Views/Sets/Detail/Set.css") %>
    <%= Scripts.Render("~/bundles/Set") %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="xxs-stack col-xs-9">
            <h2 style="margin-top:0px;" >
                <span style="margin-right: 15px;"><%= Model.Name %></span>
                <span style="display: inline-block; font-size: 20px; font-weight: normal;" class="Pin" data-set-id="<%= Model.Id %>">
                    <a href="#" class="noTextdecoration" style="font-size: 22px; height: 10px;">
                        <i class="fa fa-heart show-tooltip iAdded <%= Model.IsInWishknowledge ? "" : "hide2" %>" style="color:#b13a48;" title="Aus deinem Wunschwissen entfernen"></i>
                        <i class="fa fa-heart-o show-tooltip iAddedNot <%= Model.IsInWishknowledge ? "hide2" : "" %>" style="color:#b13a48;" title="Zu deinem Wunschwissen hinzuzufügen"></i>
                        <i class="fa fa-spinner fa-spin hide2 iAddSpinner" style="color:#b13a48;"></i>
                    </a>
                    
                    <span class="show-tooltip" id="totalPins" title="Ist bei <%= Model.TotalPins%> Personen im Wunschwissen"><%= Model.TotalPins %>x</span>
                    
                    
                    <span>
                        <i class="fa fa-tachometer" style="margin-left: 20px; color: green;"></i> 3/3    
                    </span>

                </span>
            </h2>
        </div>
        <div class="col-xs-3 xxs-stack">
            <div class="navLinks">
                <a href="<%= Links.Sets(Url) %>" style="font-size: 12px; margin: 0px;"><i class="fa fa-list"></i>&nbsp;zur Übersicht</a>
                <% if(Model.IsOwner){ %>
                    <a href="<%= Links.QuestionSetEdit(Url, Model.Id) %>" style="font-size: 12px; margin: 0px;"><i class="fa fa-pencil"></i>&nbsp;bearbeiten</a> 
                <% } %>
            </div>
        </div>
   
        <div class="col-lg-10 col-xs-9 xxs-stack" style="margin-top: 20px;">
            <%  foreach(var questionRow in Model.QuestionsInSet){ %>

                <div class="row question-row">
                    <div class="col-md-9 col-1">                    
                        <img src="<%= QuestionImageSettings.Create(questionRow.Question.Id).GetUrl_128px_square().Url %>" class="img-responsive" />

                        <a href="<%= Links.AnswerQuestion(Url, questionRow.Question, Model.Set) %>" style="font-weight:normal; font-size:17px;">
                            <%=questionRow.Question.Text %>
                        </a>    
                    </div>
                    <div class="col-md-3 col-2">
                        <% Html.RenderPartial("HistoryAndProbability", questionRow.HistoryAndProbability); %>
                    </div>
                </div>

            <% } %>

            <div class="row "style="margin-top: 30px; height: 40px;">
                <div class="col-md-12">
                    <% if (Model.QuestionsInSet.Any()){ %>
                        <div class="pull-right">
                            <a class="btn btn-info" href="<%= Links.AnswerQuestion(Url, Model.QuestionsInSet.First().Question, Model.Set) %>">
                                 Jetzt üben
                            </a>
                            <a class="btn btn-primary" href="<%= Links.AnswerQuestion(Url, Model.QuestionsInSet.First().Question, Model.Set) %>"><i class="fa fa-lightbulb-o"></i> 
                                Jetzt testen
                            </a>
                        </div>
                    <% } %>
                </div>
            </div>
        </div>
    
        <div class="col-lg-2 col-xs-3 xxs-stack" style="margin-top: 20px;">
        
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
    </div>

</asp:Content>