<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master"
    Inherits="System.Web.Mvc.ViewPage<UserModel>" %>

<asp:Content ContentPlaceHolderID="Head" runat="server">
    <style>
        .column{ width: 167px;float: left; padding-right: 4px;}
    </style>
</asp:Content>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="col-md-6">
        <div class="box box-main">

            <h2 class="pull-left" style="padding-top:3px; padding-bottom: 5px; padding-left: 0px; margin-left:0px;  font-size: 30px;"><%= Model.Name %></h2>
            <span class="pull-right" style="display: inline-block; font-size: 20px; font-weight: normal; position: relative; top: 13px;">Reputation: 7821 (Platz 7)</span>

            <div class="box-content" style="min-height: 120px; clear: both; padding-top: 10px;">
                <div class="column" >
                    <h4>Erstellte Inhalte</h4>
                    <div><%= Model.AmountCreatedQuestions %> Fragen erstellt</div>
                    <div><%= Model.AmountCreatedSets %> Fragesätze erstellt</div>
                    <div>0 Kategorien erstellt</div>
                </div>
                <div class="column">
                    <h4>Wunschwissen</h4>
                    <div>90 Fragen </div>
                    <div>21 Fragen erstellt</div>
                    <div>30 Kategorien erstellt</div>
                </div>                 
                <div class="column">
                    <h4>Reputation</h4>
                    <div>2321 x gemerkt</div>
                    <div>70 x dupliziert</div>
                    <div>500 x gefollowed</div>
                </div>
                    
                <div style="clear: both"></div>
                <h1 style="margin-top: 10px;">Wunschwissen/Fragen</h1>
                    
                <div style="clear: both"></div>
                     
            </div>
             
        </div>
    </div>

    <div class="col-md-3" >
        <div class="box">
            <img alt="" style="width: 200px;" src="<%=Model.ImageUrl_200 %>" />
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
    </div>

</asp:Content>
