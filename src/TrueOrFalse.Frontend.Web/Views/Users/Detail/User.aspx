<%@ Page Title="Nutzer" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master"
    Inherits="System.Web.Mvc.ViewPage<UserModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ContentPlaceHolderID="Head" runat="server">
    <title>Benutzer <%=Model.Name %> </title>
    <style>
        .column{ width: 33%;float: left; padding-right: 4px;}
    </style>
</asp:Content>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="xxs-stack col-xs-12">
            <div class="row">
                <div class="col-xs-9 xxs-stack" style="margin-bottom: 10px;">
                    <h2 class="pull-left ColoredUnderline User" style="margin-bottom: 10px; margin-top: 0px;  font-size: 30px;">
                        <%= Model.Name %>
                        <span style="display: inline-block; font-size: 20px; font-weight: normal;">
                            &nbsp;(Reputation: <%=Model.ReputationTotal %> - Rang <%= Model.ReputationRank %>)
                        </span>
                    </h2>
                </div>
                <div class="col-xs-3 xxs-stack">
                    <div class="navLinks">
                        <a href="<%= Url.Action("Users", "Users")%>" style="font-size: 12px; margin: 0px;"><i class="fa fa-list"></i>&nbsp;zur Übersicht</a>
                        <% if (Model.IsCurrentUser) { %>
                            <a href="<%= Url.Action(Links.UserSettings, Links.UserSettingsController) %>" style="font-size: 12px; margin: 0px;"><i class="fa fa-pencil"></i>&nbsp;bearbeiten</a> 
                        <% } %>
                    </div>
                </div>
            </div>
        </div>
    
        <div class="col-lg-10 col-xs-9 xxs-stack">
            <div class="box-content" style="min-height: 120px; clear: both; ">
            
                <div class="column">
                    <h4 style="margin-top: 0px;">Reputation</h4>
                    <div>- <%= Model.Reputation.ForQuestionsCreated %> für erstelle Fragen</div>
                    <div>- <%= Model.Reputation.ForQuestionsWishKnow + Model.Reputation.ForQuestionsWishCount %> für eigene Fragen im Wunschwissen anderer </div>
                    <div>- <%= Model.Reputation.ForSetWishCount + Model.Reputation.ForSetWishKnow %> für eigene Fragesätze im Wunschwissen anderer</div>
                </div>
                <div class="column" >
                    <h4 style="margin-top: 0px;">Erstellte Inhalte</h4>
                    <div><a href="<%= Links.QuestionWithCreatorFilter(Url, Model.User) %>"><%= Model.AmountCreatedQuestions %> Fragen erstellt</a></div>
                    <div><%= Model.AmountCreatedSets %> Fragesätze erstellt</div>
                    <div><%= Model.AmountCreatedCategories %>  Kategorien erstellt</div>
                </div>
            
                <div class="column">
                    <h4 style="margin-top: 0px;">Wunschwissen</h4>
                    <div><%= Model.AmountWishCountQuestions %> Fragen gemerkt</div>
                    <div><%= Model.AmountWishCountSets %> Fragesätze gemerkt</div>
                    <div></div>
                </div>

                <div style="clear: both"></div>
                <h3 style="margin-top: 20px; margin-bottom: 4px;">Wunschwissen</h3>

                <% if(!Model.User.ShowWishKnowledge && !Model.IsCurrentUser){ %>

                    <div class="bs-callout bs-callout-info" style="margin-top: 15px;">
                        <h4>Nicht öffentlich</h4>
                        <p>
                            <%= Model.User.Name %> hat sein Wunschwissen nicht veröffentlicht.
                        </p>
                        
                        <p>
                            <a href="#" class="btn btn-default btn-sm" onclick="alert('Diese Funktion ist noch nicht umgesetzt.')">Bitte zeige mir dein Wunschwissen</a>    
                        </p>
                    </div>                    

                <% }else{ %>
                
                    <div style="clear: both; padding-top: 14px; margin-bottom: 3px; border-bottom: 1px solid #ffd700;">Fragesätze (<%= Model.WishSets.Count %>):</div>
                    <% if (Model.WishSets.Count > 0){ %>
                        <% foreach(var set in Model.WishSets){ %>
                            <div><a href="<%: Links.SetDetail(Url, set) %>"><%: set.Text %></a></div>
                        <% } %>
                    <% } else { %>
                        <div style="padding-top: 10px; padding-bottom: 10px;">--
                            <%= Model.IsCurrentUser ?  
                                "Du hast keine Fragesätze zu deinem Wunschwissen hinzugefügt" : 
                                 Model.Name + " hat keine Fragesätze zum Wunschwissen hinzugefügt." %> --
                        </div>
                    <% } %>

                    <div style="clear: both; padding-top: 14px; margin-bottom: 3px; border-bottom: 1px solid #afd534;">Fragen (<%= Model.WishQuestions.Count %>):</div>
                    <% if (Model.WishQuestions.Count > 0){ %>
                        <% foreach(var question in Model.WishQuestions){ %>
                            <div>
                                <% if(question.IsPrivate()){ %> <i class="fa fa-lock show-tooltip" title="Private Frage"></i><% } %>
                                <a href="<%: Links.AnswerQuestion(Url, question) %>"><%: question.Text %></a>
                            </div>
                        <% } %>
                    <% } else { %>
                        <div style="padding-top: 10px; padding-bottom: 10px;">--
                            <%= Model.IsCurrentUser ?  
                                "Du hast keine Fragen zu deinem Wunschwissen hinzugefügt" :
                                Model.Name + " hat keine Fragen zum Wunschwissen hinzugefügt."  %> --
                        </div>
                    <% } %>

                <% } %>
            </div>
        </div>

        <div class="col-lg-2 col-xs-3 xxs-stack">
            <img style="width:100%; border-radius:5px;" src="<%=Model.ImageUrl_250 %>" />
            <% if (Model.IsCurrentUser){ %>  
                <script type="text/javascript">
                    $(function () {
                        $("#imageUploadLink").click(function () {
                            $("#imageUpload").show();
                        });
                    })
                </script>
                <a id="imageUploadLink" href="#">aendern</a>
                <div id="imageUpload" style="display: none">
                    <% using (Html.BeginForm("UploadPicture", "User", null, FormMethod.Post, new { enctype = "multipart/form-data" })){ %>
                        <input type="file" accept="image/*" name="file" id="file" />
                        <input class="cancel" type="submit" value="Hochladen" />
                    <% } %>
                </div>
            
                <% if(Model.ImageIsCustom){ %>
                    <a href="#">[x]</a>       
                <%} %>
            <% } %>
        
            <h4 style="margin-top: 20px;">Wunschwissen-Kategorienfilter</h4>
            <% foreach (var category in Model.WishQuestionsCategories){ %>
                <a href="<%= Links.CategoryDetail(category) %>"><span class="label label-category" style="margin-top: 7px;"><%= category.Name %></span></a>        
            <% } %>

        </div>
    </div>
</asp:Content>
