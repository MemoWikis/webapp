<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BaseModel>"  %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<%
    var showEnvironment = false;
    var backgroundColor = "";
    var text = "";

    if (Request.Url.Host == "memucho.local" || Request.Url.Host == "memucho"){
        showEnvironment = true; backgroundColor = "#afd534"; text = "L O C A L";
    }

    if (Request.Url.Host == "future.memucho.de"){
        showEnvironment = true; backgroundColor = "lightpink"; text = "F U T U R E";
    }

    if (Request.Url.Host == "stage.memucho.de"){
        showEnvironment = true; backgroundColor = "orange"; text = "S T A G E";
    }
%>

<% if(showEnvironment){ %>
    <div class="" style="background-color: <%= backgroundColor%>; z-index: 10000; position: fixed; top: 11px; right: -57px; width: 127px; text-align: center; font-size: 11px; padding: 2px; padding-left: 40px; transform: rotate(90deg); color:white">
        <%= text %>
    </div>
<% } %>

<header id="MasterHeader">
    <div id="MasterHeaderContainer"class="container">
        <div class="row" style="background-color:#003264">
            <div class=" HeaderMainRow" style="background-color:#003264">
                <div class="col-xs-6 col-Logo">
                    <a id="LogoLink" href="/">
                        <div id="Pictogram">
                            <img src="/Images/Logo/LogoPictogram.png">
                        </div>
                        <div id="Wordmark">
                            <img src="/Images/Logo/LogoWordmarkWithTagline.png">
                        </div>
                        <div id="LogoSmall">
                            <img src="/Images/Logo/LogoSmall.png">
                        </div>
                    </a>

                </div>
                <div id="StickySearch">
                    <search-component v-on:select-item="openUrl" :search-type="searchType" id="StickySearchComponent"/>
                </div>
                <div class="col-xs-6 col-LoginAndHelp">
            	    <div id="loginAndHelp" >
                        <% Html.RenderPartial(UserControls.Logon); %>
                    </div>
                </div>                    
            </div>
        </div>
    </div>
</header>