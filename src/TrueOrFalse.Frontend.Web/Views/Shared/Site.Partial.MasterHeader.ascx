<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl"  %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<script>
    window.fbAsyncInit = function() {
        FB.init({
            appId      : '1789061994647406',
            xfbml      : true,
            version    : 'v2.8'
        });
    };

    (function(d, s, id){
        var js, fjs = d.getElementsByTagName(s)[0];
        if (d.getElementById(id)) {return;}
        js = d.createElement(s); js.id = id;
        js.src = "//connect.facebook.net/de_DE/sdk.js";
        fjs.parentNode.insertBefore(js, fjs);
    }(document, 'script', 'facebook-jssdk'));
</script>

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
                        <a id="BetaLogo" href="<%= Links.BetaInfo() %>">
                            <%--<img src="/Images/Logo/beta-logo.png" width="22" height="56">--%>
                            [beta]
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