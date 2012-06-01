<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" Inherits="System.Web.Mvc.ViewPage<UserProfileModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%= Model.Name %></h2>
    <img alt="" src="/Images/no-profile-picture-128.png"/>
    
    <% if (Model.IsCurrentUserProfile ) { %>
        <script type="text/javascript">
            $(function () {
                $("#imageUploadLink").click(function () {
                    $("#imageUpload").show();
                });
            })
        </script>
        <a id="imageUploadLink" href="#">Profilbild hochladen</a>
        <div id="imageUpload" style="display:none">
            <%using (Html.BeginForm("UploadProfilePicture")) { %>
                <input type="file" accept="image/*"/>
                <input class="cancel" type="submit" value="Hochladen"/>
            <% } %>
        </div>
    <% } %>

</asp:Content>


