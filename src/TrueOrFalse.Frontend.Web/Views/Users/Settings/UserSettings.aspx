<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master"
    Inherits="System.Web.Mvc.ViewPage<UserSettingsModel>" %>

<asp:Content ContentPlaceHolderID="Head" runat="server">
    <style>
        .column{ width: 167px;float: left; padding-right: 4px;}
    </style>
</asp:Content>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="span7" style="">
        <div class="box box-main">

            <h2 class="pull-left" style="padding-top:3px; padding-bottom: 5px; padding-left: 0px; margin-left:0px;  font-size: 30px;">
                <%= Model.Name %> <i class="icon-wrench show-tooltip" title="Hier kannst Du Deine Einstellungen bearbeiten"></i>
            </h2>
            <span class="pull-right" style="display: inline-block; font-size: 20px; font-weight: normal; position: relative; top: 13px;">Reputation: 7821 (Platz 7)</span>

            <div class="box-content" style="min-height: 120px; clear: both; padding-top: 10px;">
   
            </div>
             
        </div>
    </div>

    <div class="span3" >
        <div class="box">
            <img alt="" style="width: 200px;" src="<%=Model.ImageUrl_200 %>" />
            
            <script type="text/javascript">
                $(function () {
                    $("#imageUploadLink").click(function () {
                        $("#imageUpload").show();
                    });
                })
            </script>
            <a id="imageUploadLink" href="#">aendern</a>
            <div id="imageUpload" style="display: none">
                <% using (Html.BeginForm("UploadPicture", "UserSettings", null, FormMethod.Post, new { enctype = "multipart/form-data" })){ %>
                    <input type="file" accept="image/*" name="file" id="file" />
                    <input class="cancel" type="submit" value="Hochladen" />
                <% } %>
            </div>
            
            <% if(Model.ImageIsCustom){ %>
                <a href="#">[x]</a>       
            <%} %>
        </div>
    </div>

</asp:Content>
