<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl"  %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<header id="MasterHeader">
    <div class="container">
        <div class="row">
            <div class="col-md-12">
                <div class="row HeaderMainRow">
                    <div class="col-xs-6 col-Logo">
                        <a id="MenuButton"><i class="fa fa-bars"></i><span class="caret"></span></a>
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
                        <a id="BetaLogo" href="/">
                            <img src="/Images/Logo/LogoSmall.png">
                            [Beta]
                        </a>
                    </div>
                    <div class="col-xs-6 col-LoginAndHelp">
            	        <div id="loginAndHelp" >
                            <% Html.RenderPartial(UserControls.Logon); %>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</header>