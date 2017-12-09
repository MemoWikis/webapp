<%@ Page Title="Kontakt & Anfahrt" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master" 
	Inherits="ViewPage<BaseModel>"%>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="ContentHeadSEO" ContentPlaceHolderID="HeadSEO" runat="server">
    <meta name="description" content="Kontakt und Anfahrt zum Lerntool und Wissensassistenten memucho">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/Welcome/Contact.css" rel="stylesheet" />
    <%= Scripts.Render("~/bundles/mailto") %>
</asp:Content>


<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="PageHeader">
        <h1><span class="ColoredUnderline GeneralMemucho">Kontakt & Anfahrt</span></h1>
    </div>

    <p class="digitalAddresses">
        E-Mail: <span class="mailme">team at memucho dot de</span> <br />
        Telefon: +49 - 30 - 616 566 26 <br />
        Mobil: +49 - 1577 - 682 5707
    </p>
        
    <p class="postalAddress">
        <i class="fa fa-map-marker">&nbsp;</i>&nbsp;memucho <br />
        <span style="color: #aaaaaa; padding-left: 15px;">c/o Raecke/Schreiber</span> <br />
        Erkelenzdamm 59/61 <br />
        Aufgang 3a, 4. OG <br />
        10999 Berlin
    </p>

    <div id="approachSketch">
        <iframe src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d2429.026168991007!2d13.4107269157264!3d52.49676587980987!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x47a84fcd1092c5b9%3A0xd784ecc27397c2b6!2sErkelenzdamm+59%2C+10999+Berlin!5e0!3m2!1sde!2sde!4v1499695893799" width="100%" height="450" frameborder="0" style="border:0" allowfullscreen></iframe>
    </div>
    <p>
        Du findest unser Büro im 4. OG des Aufgangs ("Portal") 3A. Dieser Aufgang befindet sich im 2. (linken) Hinterhof gegenüber den Fahrrädern. <br />
    </p>
    <p>
        Deinen Anfahrtsweg kannst du <a href="https://maps.google.com/maps?ll=52.496126,13.411542&z=15&t=m&hl=de-DE&gl=DE&mapclient=embed&daddr=Erkelenzdamm%2059%2010999%20Berlin@52.4967659,13.4129156" target="blank"><b>hier planen</b></a>.

    </p>
</asp:Content>