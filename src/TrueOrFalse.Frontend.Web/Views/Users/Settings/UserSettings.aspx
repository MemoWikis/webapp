<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master"
    Inherits="System.Web.Mvc.ViewPage<UserSettingsModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ContentPlaceHolderID="Head" runat="server">
    <style>
        .column{ width: 167px;float: left; padding-right: 4px;}
    </style>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="row">
        <div class="xxs-stack col-xs-12">
            <div class="row">
                <div class="col-xs-9 xxs-stack" style="margin-bottom: 10px;">
                    <h2 class="pull-left" style="margin-bottom: 10px; margin-top: 0px;  font-size: 30px;">
                        <%= Model.Name %> 
                        
                        <span style="display: inline-block; font-size: 20px; font-weight: normal;">
                            &nbsp;(Reputation: <%=Model.ReputationTotal %> - Rang <%= Model.ReputationRank %>)
                        </span>

                        <i class="fa fa-wrench show-tooltip" title="Hier kannst Du Deine Einstellungen bearbeiten"></i>
                    </h2>
                </div>
                <div class="col-xs-3 xxs-stack">
                    <div class="navLinks">
                        <a href="<%= Url.Action("Users", "Users")%>" style="font-size: 12px; margin: 0px;"><i class="fa fa-list"></i>&nbsp;zur Übersicht</a>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-md-9">
            <form class="form-horizontal" method="POST">

                <% Html.Message(Model.Message); %>

                <div class="form-group">
                    <%= Html.LabelFor(m => m.Name, new { @class = "col-sm-3 control-label" })%>
                    <div class="col-xs-4">
                        <%= Html.TextBoxFor(m => m.Name, new {@class="form-control"} )%>
                    </div>
                </div>                    
                <div class="form-group">
                    <%= Html.LabelFor(m => m.Email, new { @class = "col-sm-3 control-label" })%>
                    <div class="col-xs-4">
                        <%= Html.TextBoxFor(m => m.Email, new {@class="form-control"} )%>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-offset-3 col-sm-9">
                        <label class="checkbox">
                            <input type="checkbox">
                            <%= Html.CheckBoxFor(m => m.AllowsSupportiveLogin)%>
                            <label for="AllowsSupportiveLogin">
                                Erlaube Mitarbeiten von RIOFA zur Fehlerbehebung oder zu deiner Unterstützung, 
                                sich in deinem Nutzerkonto anzumelden. Das ist nur nach Rücksprache nötig. 
                                (<a href="<%= Url.Action("DatenSicherheit","Help") %>">Mehr zur Datensicherheit</a>)
                            </label>
                        </label>
                    </div>
                </div>
                
                <div class="form-group" style="margin-top: 30px;">
                    <div class="col-sm-offset-3 col-sm-9">
                        <button type="submit" class="btn btn-primary" name="btnSave" value="ssdfasdfave">Speichern</button>&nbsp;&nbsp;&nbsp;
                    </div>
                </div>

            </form>
        </div>

        <div class="col-md-3" >
            <img alt="" src="<%=Model.ImageUrl_200 %>" class="img-responsive" style="border-radius:5px;" />
            
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
