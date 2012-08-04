<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master"
    Inherits="System.Web.Mvc.ViewPage<UserProfileModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%= Model.Name %></h2>
    
    <div class="row">
        <div style="background-color: crimson" class="span2">
        <img alt="" src="<%=string.Format(Model.ImageUrl, 128) %>" /><br/>
        <% if (Model.IsCurrentUserProfile){ %>  
            <script type="text/javascript">
            $(function () {
                $("#imageUploadLink").click(function () {
                    $("#imageUpload").show();
                });
            })
            </script>
            <a id="imageUploadLink" href="#">aendern</a>
            <div id="imageUpload" style="display: none">
                <% using (Html.BeginForm("UploadProfilePicture", "UserProfile", null, FormMethod.Post, new { enctype = "multipart/form-data" })){ %>
                    <input type="file" accept="image/*" name="file" id="file" />
                    <input class="cancel" type="submit" value="Hochladen" />
                <% } %>
            </div>
            
            <% if(Model.ImageIsCustom){ %>
                <a href="#">[x]</a>       
            <%} %>
        <% } %> 
    </div>
        <div class="span6" style="background-color: cyan">asdf</div>
        <div class="span2" style="background-color: coral">asdf</div>
    </div>
</asp:Content>
