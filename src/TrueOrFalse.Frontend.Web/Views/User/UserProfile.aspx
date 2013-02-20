<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master"
    Inherits="System.Web.Mvc.ViewPage<UserProfileModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="span10">
        <h2><%= Model.Name %></h2>
    </div>

    <div class="row">
        <div class="span2">
            <img alt="" style="border: 2px solid #2E487B;" src="<%=Model.ImageUrl_128 %>" /><br/>
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
    
         <div class="span6" style="">
            <h3>Reputation: 7821</h3>
             
             <div class="row">
                 <div class="span2">
                    220 Fragen erstellt    
                 </div>
                 <div class="span2">
                    10 Tage    
                 </div>                 
                 <div class="span2">
                    3 Tage erstellt    
                 </div>                 

             </div>

             
        </div>
        
        <div class="span2" style="background-color: #499B33">asdf</div>
    </div>
</asp:Content>
