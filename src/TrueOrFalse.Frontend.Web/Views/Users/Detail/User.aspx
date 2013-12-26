<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master"
    Inherits="System.Web.Mvc.ViewPage<UserModel>" %>

<asp:Content ContentPlaceHolderID="Head" runat="server">
    <style>
        .column{ width: 167px;float: left; padding-right: 4px;}
    </style>
</asp:Content>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="col-md-6">
        
        <h2 class="pull-left" style="margin-bottom: 5px; margin-top: 0px;  font-size: 30px;">
            <%= Model.Name %>
            <span style="display: inline-block; font-size: 20px; font-weight: normal;">
                (Reputation: <%=Model.ReputationTotal %> (Platz 7))
            </span>
        </h2>

        <div class="box-content" style="min-height: 120px; clear: both; padding-top: 10px;">
            
            <div class="column">
                <h4>Wunschwissen</h4>
                <div><%= Model.AmountWishCountQuestions %> Fragen gemerkt</div>
                <div><%= Model.AmountWishCountSets %> Fragesätze gemerkt</div>
                <div></div>
            </div>
            <div class="column" >
                <h4>Erstellte Inhalte</h4>
                <div><%= Model.AmountCreatedQuestions %> Fragen erstellt</div>
                <div><%= Model.AmountCreatedSets %> Fragesätze erstellt</div>
                <div><%= Model.AmountCreatedCategories %>  Kategorien erstellt</div>
            </div>
            <div class="column">
                <h4>Reputation</h4>
                <div>- <%=Model.Reputation.ForQuestionsCreated %> für erstelle Fragen</div>
                <div>- <%=Model.Reputation.ForQuestionsWishKnow + Model.Reputation.ForQuestionsWishCount %> für eigenen Fragen im Wunschwissen anderer </div>
            </div>

            <div style="clear: both"></div>
            <h3 style="margin-top: 30px;">Wunschwissen/Fragen</h3>
                    
            <div style="clear: both"></div>
                     
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
    </div>

</asp:Content>
