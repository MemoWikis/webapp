<%@ Page Title="Kontakt & Anfahrt" Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" 
	Inherits="ViewPage<BaseModel>"%>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="ContentHeadSEO" ContentPlaceHolderID="HeadSEO" runat="server">
    <meta name="description" content="Kontakt und Anfahrt zum Lerntool und Wissensassistenten memucho">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/Welcome/Contact.css" rel="stylesheet" />
    <%= Scripts.Render("~/bundles/mailto") %>
    
    <% Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem{Text = "Kontakt", Url = "/Kontakt", ImageClass = "fa-question-circle", ToolTipText = "Kontakt"});
       Model.TopNavMenu.IsCategoryBreadCrumb = false; %>
</asp:Content>


<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="PageHeader">
        <h1><span class="ColoredUnderline GeneralMemucho">Kontakt & Anfahrt</span></h1>
    </div>

    <p class="digitalAddresses">
        E-Mail: <span class="mailme">team at memucho dot de</span> <br />
        Mobil: +49 - 1577 - 682 5707
    </p>
    <p>
        Unsere Adressen sind: <br/>
    </p>    
    <p class="postalAddress">
        <i class="fa fa-map-marker">&nbsp;</i>&nbsp;memucho <br />
        Kirchstraße 1<br />  
        15745 Wildau<br />
    </p>

    <div id="approachSketch">
        <iframe src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d2439.0201433548277!2d13.626368615799853!3d52.31563697977604!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x47a83ee5f00ac0d5%3A0x994dedc548320f21!2sKirchstra%C3%9Fe+1%2C+15745+Wildau!5e0!3m2!1sde!2sde!4v1523209238243" width="100%" height="450" frameborder="0" style="border:0" allowfullscreen></iframe>
    </div>

    <p>
        Deinen Anfahrtsweg kannst du <a href="https://goo.gl/maps/eKVFRULcQpu" target="blank"><b>hier planen</b></a>.
    </p>
    <hr/>
    
    <p class="postalAddress1">
        <i class="fa fa-map-marker">&nbsp;</i>&nbsp;memucho <br />
        In der Thinkfarm <br />
        Moosdorfstraße 7-9 <br />
        1. OG, rechter Hand 1. Büro <br />
        12435 Berlin
    </p>
    
    <div id="approachSketch1">
        <iframe src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d2429.5093910670025!2d13.458313715806257!3d52.488018179808165!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x47a84f03feeb7749%3A0x88c78e7e85e9e280!2sMoosdorfstra%C3%9Fe+7-9%2C+12435+Berlin!5e0!3m2!1sde!2sde!4v1523188521796" width="100%" height="450" frameborder="0" style="border:0" allowfullscreen></iframe>
    </div>

    <p>
        Deinen Anfahrtsweg kannst du <a href="https://goo.gl/maps/vDuHrFGxqBT2" target="blank"><b>hier planen</b></a>.
    </p>
    

   
 
</asp:Content>