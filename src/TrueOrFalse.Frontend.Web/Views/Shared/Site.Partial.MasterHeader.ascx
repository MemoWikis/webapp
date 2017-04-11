<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl"  %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<header id="MasterHeader">
    <div class="container">
        <div class="row">
            <div class="col-md-12">
                <div class="row HeaderMainRow">
                    <div class="col-xs-7 col-Logo">
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
                        <%--<input type="text" id="headerSearchBox" placeholder="Suche" class="form-control"  />--%>

                        <div class="input-group" style="margin-left: 4px; margin-top: 2px; width: 190px;">
                            <input type="text" class="form-control" placeholder="Suche" id="headerSearchBox">
                            <div class="input-group-btn">
                                <button class="btn btn-default" type="submit"><i class="fa fa-search" aria-hidden="true"></i></button>
                            </div>
                        </div>

                    </div>
                    <div class="col-xs-5 col-LoginAndHelp">
            	        <div id="loginAndHelp" >
                            <% Html.RenderPartial(UserControls.Logon); %>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</header>