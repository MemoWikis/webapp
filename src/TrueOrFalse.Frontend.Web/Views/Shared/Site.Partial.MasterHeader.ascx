<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BaseModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<%
    var showEnvironment = false;
    var backgroundColor = "";
    var text = "";
    var showStageOverlay = false;
    if (Request.Url.Host == "memucho.local" || Request.Url.Host == "memucho" || Request.Url.Host == "localhost")
    {
        showEnvironment = true;
        backgroundColor = "#afd534";
        text = "L O C A L";
    }

    if (Request.Url.Host == "future.memucho.de")
    {
        showEnvironment = true;
        backgroundColor = "lightpink";
        text = "F U T U R E";
    }

    if (Request.Url.Host == "stage.memucho.de")
    {
        showEnvironment = true;
        backgroundColor = "orange";
        text = "S T A G E";
        showStageOverlay = true;
    }

    var userSession = new SessionUser();
    var searchClasses = userSession.IsLoggedIn ? "col-sm-9 col-md-8 col-lg-7" : "col-sm-3 col-md-5 col-lg-6";
    var loginClasses = userSession.IsLoggedIn ? "col-sm-3 col-md-4 col-lg-5" : "col-sm-9 col-md-7 col-lg-6";
%>


<% if (showEnvironment)
    { %>
    <div style="background-color: <%= backgroundColor %>;" class="stageBanner">
        <%= text %>
    </div>
    <% if (showStageOverlay)
       { %>
    <div class="stageOverlayContainer" id="StageOverlay">
        <h3 class="stageOverlayText"><img src="/Images/Logo/LogoPictogram.png"/> <br/> <br/>Du bist auf stage.memucho.de. Hier werden Entwicklungen getestet, bevor sie Live-gehen. 
           <br/> <b>Achtung: Alle Änderungen, die du hier machst, werden verworfen.</b> <br/>
            Die live Seite findest du hier: <a href="https://memucho.de/">memucho</a>.</h3>
        <img class="stageOverlayCloseButton" src="/img/close_black.svg" onclick="hideStageOverlay()"/>
    </div>
    <% } %>
<% } %>

<% if (!userSession.IsLoggedIn)
    {%>
    <header id="MasterHeader">
        <div id="MasterHeaderContainer"class="container">
            <div class="HeaderMainRow row">
                <div id="LogoContainer" class="col-sm-3 col-Logo col-xs-2">
                    <a id="LogoLink" href="/">
                        <div id="FullLogo" class="hidden-xs">
                            <img src="/Images/Logo/Logo.svg">
                        </div>
                        <div id="MobileLogo" class="hidden-sm hidden-md hidden-lg">
                            <img src="/Images/Logo/LogoSmall.png">
                        </div>
                    </a>
                </div>
                <div id="HeaderBodyContainer" class="col-sm-9 col-LoginAndHelp col-xs-10 row">
                    <div id="HeaderSearch" class="<%= searchClasses %>">
                        <div class="searchButton" :class="{ 'showSearch' : showSearch }" @click="showSearch = !showSearch">
                            <i :class="[ showSearch ? 'fas fa-times' : 'fa fa-search']" aria-hidden="true"></i>
                        </div>
                        <div class="SearchContainer" :class="{ 'showSearch' : showSearch }">
                            <search-component v-on:select-item="openUrl" :search-type="searchType" id="SmallHeaderSearchComponent" :show-search="showSearch"/>
                        </div>
                    </div>
                    <div id="loginAndHelp" class="<%= loginClasses %>">
                        <% Html.RenderPartial(UserControls.Logon); %>
                    </div>
                </div>
            </div>
        </div>
    </header>
<%} %>

<%: Html.Partial("~/Views/Shared/Search/SearchTemplateLoader.ascx") %>
