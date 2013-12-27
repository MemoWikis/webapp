<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master"
    Inherits="System.Web.Mvc.ViewPage<UserModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ContentPlaceHolderID="Head" runat="server">
    <style>
        .column{ width: 33%;float: left; padding-right: 4px;}
    </style>
</asp:Content>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="col-md-6">
        
        <h2 class="pull-left" style="margin-bottom: 5px; margin-top: 0px;  font-size: 30px;">
            <%= Model.Name %>
            <span style="display: inline-block; font-size: 20px; font-weight: normal;">
                &nbsp;(Reputation: <%=Model.ReputationTotal %> - Platz 7)
            </span>
        </h2>

        <div class="box-content" style="min-height: 120px; clear: both; padding-top: 10px;">
            
            <div class="column">
                <h4>Reputation</h4>
                <div>- <%= Model.Reputation.ForQuestionsCreated %> für erstelle Fragen</div>
                <div>- <%= Model.Reputation.ForQuestionsWishKnow + Model.Reputation.ForQuestionsWishCount %> für eigene Fragen im Wunschwissen anderer </div>
                <div>- <%= Model.Reputation.ForSetWishCount + Model.Reputation.ForSetWishKnow %> für eigene Fragesätze im Wunschwissen anderer</div>
            </div>
            <div class="column" >
                <h4>Erstellte Inhalte</h4>
                <div><%= Model.AmountCreatedQuestions %> Fragen erstellt</div>
                <div><%= Model.AmountCreatedSets %> Fragesätze erstellt</div>
                <div><%= Model.AmountCreatedCategories %>  Kategorien erstellt</div>
            </div>
            
            <div class="column">
                <h4>Wunschwissen</h4>
                <div><%= Model.AmountWishCountQuestions %> Fragen gemerkt</div>
                <div><%= Model.AmountWishCountSets %> Fragesätze gemerkt</div>
                <div></div>
            </div>

            <div style="clear: both"></div>
            <h3 style="margin-top: 20px; margin-bottom: 4px;">Wunschwissen</h3>
            <div style="clear: both; padding-top: 14px; margin-bottom: 3px; border-bottom: 1px solid #ffd700;">Fragesätze (<%= Model.WishSets.Count %>):</div>
            <% foreach(var set in Model.WishSets){ %>
                <div><a href="<%: Links.SetDetail(Url, set) %>"><%: set.Text %></a></div>
            <% } %>
            
            <div style="clear: both; padding-top: 14px; margin-bottom: 3px; border-bottom: 1px solid #afd534;">Fragen (<%= Model.WishQuestions.Count %>):</div>
            <% foreach(var question in Model.WishQuestions){ %>
                <div><a href="<%: Links.AnswerQuestion(Url, question) %>"><%: question.Text %></a></div>
            <% } %>

        </div>     
    </div>

    <div class="col-md-3" >
        <img style="width:100%;" src="<%=Model.ImageUrl_250 %>" />
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
            <a href="<%= Links.CategoryDetail(Url, category) %>"><span class="label label-category" style="margin-top: 7px;"><%= category.Name %></span></a>        
        <% } %>

    </div>

</asp:Content>
