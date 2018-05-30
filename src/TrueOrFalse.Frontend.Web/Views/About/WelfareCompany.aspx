<%@ Page Title="memucho Gemeinwohlökonomie" Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="System.Web.Mvc.ViewPage<BaseModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <% Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem{Text = "Gemeinwohlökonomie", Url = "/Gemeinwohlökonomie", ImageUrl = "fa-question-circle", ToolTipText = "Gemainwohlökonomie"});
       Model.TopNavMenu.IsCategoryBreadCrumb = false; %>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="row" style="padding-top:30px;">
    <div class="BackToHome col-md-3">
            <a href="/"><i class="fa fa-chevron-left">&nbsp;</i>Zur Startseite</a>
    </div>
    <div class="form-horizontal col-md-9">
        
        <h1 style="margin-bottom: 15px; margin-top: 0px;">Wir unterstützen die Gemeinwohlökonomie</h1>

        <p>
            Unser Unternehmen soll dem Gemeinwohl dienen und deshalb auf gemeinwohlfördernden Werten aufbauen. 
        </p>
        <p>
            Als Teil der <a href="http://www.gemeinwohl-oekonomie.org/de"><i class="fa fa-external-link">&nbsp;</i>Gemeinwohlökonomie</a> 
            sind wir davon überzeugt, dass Unternehmen der Gemeinschaft dienen müssen und deshalb 
            eine ethische, soziale und ökologische Verantwortung haben. Daher werden wir in Zukunft 
            eine Gemeinwohlbilanz veröffentlichen.
        </p>        

    </div>
</div>

</asp:Content>

