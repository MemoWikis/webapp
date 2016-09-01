<%@ Page Title="Ausgeloggt" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuNo.Master" Inherits="ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row" style="margin-bottom: 100px;">
        <div class="BackToHome col-md-3">
            <i class="fa fa-chevron-left"></i>&nbsp;<a href="/">zur Startseite</a>
        </div>
    
        <div class="form-horizontal col-md-9">
            <h2>Ausgeloggt</h2>
            <p>Du wurdest erfolgreich ausgeloggt.</p>    
        </div>        
    </div>

</asp:Content>


