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
        
            <div class="box-content" style="min-height: 200px; clear: both; padding-top: 10px;">
                <% var index = 0; foreach(var question in Model.Questions){ index++; %>
                    <div>
                        <%= index %> <a href="<%= Links.AnswerQuestion(Url, question, 0) %>"><%=question.Text %></a>
                    </div>
                <% } %>            
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
                    von: <a href="<%= Links.Profile(Url, Model.Creator) %>"> <%= Model.CreatorName %> </a> <br/>
                    vor <a href="#" class="show-tooltip" title="erstellt am <%= Model.CreationDate %>" ><%= Model.CreationDateNiceText%></a> <br />
                </div>
            </div>
            
            <div class="box">
                <div class="box-content">
                    Followers: 837  (Rang: 7)<br/>          
                </div>
            </div>
        </div>
    </div>

</asp:Content>