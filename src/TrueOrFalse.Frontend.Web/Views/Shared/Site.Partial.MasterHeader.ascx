<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BaseModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<%
    var showEnvironment = false;
    var backgroundColor = "";
    var text = "";

    if (Request.Url.Host == "memucho.local" || Request.Url.Host == "memucho")
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
    }

    var userSession = new SessionUser();
    var user = userSession.User;
    var searchClasses = userSession.IsLoggedIn ? "col-sm-9 col-md-8 col-lg-7" : "col-sm-4 col-md-5 col-lg-6";
    var loginClasses = userSession.IsLoggedIn ? "col-sm-3 col-md-4 col-lg-5" : "col-sm-8 col-md-7 col-lg-6";
%>

<% if (showEnvironment)
   { %>
    <div class="" style="background-color: <%= backgroundColor %>; z-index: 10000; position: fixed; top: 11px; right: -57px; width: 127px; text-align: center; font-size: 11px; padding: 2px; padding-left: 40px; transform: rotate(90deg); color: white">
        <%= text %>
    </div>
<% } %>

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
                        <i class="fa fa-search" aria-hidden="true"></i>
                    </div>
                    <div class="SearchContainer" :class="{ 'showSearch' : showSearch }">
                        <search-component v-on:select-item="openUrl" :search-type="searchType" id="SmallHeaderSearchComponent"/>
                    </div>
                </div>
                <div id="loginAndHelp" class="<%= loginClasses %>">
                    <% Html.RenderPartial(UserControls.Logon); %>
                </div>
            </div>
        </div>
    </div>
</header>

<%: Html.Partial("~/Views/Shared/Search/SearchTemplateLoader.ascx") %>
