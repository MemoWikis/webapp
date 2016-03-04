<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" 
    Inherits="ViewPage<SetModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
    <title>Fragesatz - <%= Model.Set.Name %></title>
    <%= Styles.Render("~/Views/Sets/Detail/Set.css") %>
    <%= Scripts.Render("~/bundles/Set") %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="xxs-stack col-xs-9">
            <h2 style="margin-top:0px;">
                <span style="margin-right: 15px;" class="ColoredUnderline Set"><%= Model.Name %></span>
                <span style="display: inline-block; font-size: 20px; font-weight: normal;" class="Pin" data-set-id="<%= Model.Id %>">
                    <a href="#" class="noTextdecoration" style="font-size: 22px; height: 10px;">
                        <i class="fa fa-heart show-tooltip iAdded <%= Model.IsInWishknowledge ? "" : "hide2" %>" style="color:#b13a48;" title="Aus deinem Wunschwissen entfernen"></i>
                        <i class="fa fa-heart-o show-tooltip iAddedNot <%= Model.IsInWishknowledge ? "hide2" : "" %>" style="color:#b13a48;" title="Zu deinem Wunschwissen hinzuzufügen"></i>
                        <i class="fa fa-spinner fa-spin hide2 iAddSpinner" style="color:#b13a48;"></i>
                    </a>
                    
                    <span class="show-tooltip" id="totalPins" title="Ist bei <%= Model.TotalPins%> Personen im Wunschwissen"><%= Model.TotalPins %>x</span>

                    <span class="show-tooltip" title="<%= Model.ActiveMemory.TotalInActiveMemory %> von <%= Model.ActiveMemory.TotalQuestions%> Fragen 
                        aus diesem Fragesatz <br> sind in deinem aktiven Wissen. <br><br> Im 'aktiven Wissen' ist eine Frage, wenn die<br> Antwortwahrscheinlichkeit über 90% liegt." 
                        data-html="true" data-placement="bottom">
                        <i class="fa fa-tachometer" style="margin-left: 20px; color: #69D069;"></i> 
                        <%= Model.ActiveMemory.TotalInActiveMemory %>/<%= Model.ActiveMemory.TotalQuestions %>
                    </span>

                </span>
                
                <span style="font-size: 16px;">
                    Kategorien:
                    <div style="display: inline-block; position: relative; top:-2px;">
                        <% foreach (var category in Model.Set.Categories){ %>
                            <a href="<%= Links.CategoryDetail(category) %>"><span class="label label-category"><%= category.Name %></span></a>    
                        <% } %>
                    </div>
                </span>
            </h2>
        </div>
        <div class="col-xs-3 xxs-stack">
            <div class="navLinks">
                <a href="<%= Links.Sets() %>" style="font-size: 12px; margin: 0px;"><i class="fa fa-list"></i>&nbsp;zur Übersicht</a>
                <% if(Model.IsOwner){ %>
                    <a href="<%= Links.QuestionSetEdit(Url, Model.Id) %>" style="font-size: 12px; margin: 0px;"><i class="fa fa-pencil"></i>&nbsp;bearbeiten</a> 
                <% } %>
                
                <a style="font-size: 12px;" data-allowed="logged-in" href="<%= Links.StartSetLearningSession(Model.Id) %>"><i class="fa fa-line-chart"></i> 
                    Jetzt üben
                </a>
                
                <a style="font-size: 12px;" href="<%= Links.GameCreateFromSet(Model.Id) %>" class="show-tooltip" data-original-title="Spiel mit Fragen aus diesem Termin starten." >
                    <i class="fa fa-gamepad" style="font-size: 18px;"></i>
                    Spiel starten
                </a>

            </div>
        </div>
   
        <div class="col-lg-10 col-xs-9 xxs-stack" style="margin-top: 20px;">
            <%  foreach(var questionRow in Model.QuestionsInSet){ %>
                <% Html.RenderPartial("/Views/Sets/Detail/SetQuestionRowResult.ascx", questionRow); %>
            <% } %>

            <div class="row "style="margin-top: 20px; height: 40px;">
                <div class="col-md-12">
                    <% if (Model.QuestionsInSet.Any()){ %>
                        <div class="pull-right">
                            <a href="<%= Links.GameCreateFromSet(Model.Id) %>" class="show-tooltip" data-original-title="Spiel mit Fragen aus diesem Termin starten." style="display: inline-block; padding-right: 15px; margin-top: 29px;">
                                <i class="fa fa-gamepad" style="font-size: 18px;"></i>
                                Spiel starten
                            </a>
                            <a class="btn btn-primary" data-btn="startLearningSession" href="<%= Links.StartSetLearningSession(Model.Id) %>"><i class="fa fa-line-chart"></i> 
                                Jetzt üben
                            </a>
                        </div>
                    <% } %>
                </div>
            </div>
        </div>
    
        <div class="col-lg-2 col-xs-3 xxs-stack" style="margin-top: 20px;">
        
            <div>
                <div class="ImageContainer">
                    <%= Model.ImageFrontendData.RenderHtmlImageBasis(350, false, ImageType.QuestionSet, "ImageContainer") %>
                </div>
            </div>

            <div style="margin-top: 10px;">
                von: <a href="<%= Links.UserDetail(Model.Creator) %>"> <%= Model.CreatorName %> </a> <br/>
                vor <a href="#" class="show-tooltip" title="erstellt am <%= Model.CreationDate %>" ><%= Model.CreationDateNiceText%></a> <br />
            </div>
            
        </div>
    </div>

</asp:Content>