<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" 
    Inherits="System.Web.Mvc.ViewPage<QuestionSetModel>" Title="Fragesatz" %>

<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
    <%--<%= Scripts.Render("~/Views/QuestionSets/QuestionSets.js") %>--%>
    <%= Styles.Render("~/Views/QuestionSets/QuestionSet.css") %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="row-fluid">
        
        <div class="span8">
        
            <div class="box box-main" >
                <h2><%= Model.Name %></h2>
        
                <div class="box-content" style="min-height: 200px;">
                    <% var index = 0; foreach(var question in Model.Questions){ index++; %>
                        <div>
                            <%= index %> <a href="<%= Links.AnswerQuestion(Url, question, 0) %>"><%=question.Text %></a>
                        </div>
                    <% } %>            
                </div>
            </div>
        
        </div>
    
        <div class="span4">
            <div class="box">
                <div class="box-content">
                    Ersteller: Robert Mischke <br/>
                    Am: 22.04.2012
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