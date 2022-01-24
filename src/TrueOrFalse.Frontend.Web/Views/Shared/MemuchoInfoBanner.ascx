﻿<%@ Control Language="C#" AutoEventWireup="true" %>

<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<% var infoCookieValue = "";

    if (Request.Cookies["memuchoInfoBanner"] != null)
        infoCookieValue = Request.Cookies["memuchoInfoBanner"].Value;
    var notFirstTimeInfoClass = "";
    if (infoCookieValue == "notFirstTime")
        notFirstTimeInfoClass = "skip-animation";

    var betaCookieValue = "";
    if (Request.Cookies["memuchoBetaBanner"] != null)
        betaCookieValue = Request.Cookies["memuchoBetaBanner"].Value;
    var notFirstTimeBetaClass = "";
    var topBannerClass = "";
    if (betaCookieValue == "notFirstTime")
        notFirstTimeBetaClass = "skip-animation";
    if (betaCookieValue == "hide")
        topBannerClass = "topBannerClass";

%>

<div id="MemuchoBetaBanner" class="<%= notFirstTimeBetaClass %>">
    <div class="container">
        <div id="BannerContainer" class="row">
            <div id="BannerText" class="col-xs-12 col-sm-7 memucho-info-partial">
                <div class="sub-text">
                    <b>Hi early adopters!</b> memucho befindet sich in der zweiten <a href="/Entwicklungsstatus-Beta/9032">Beta-Phase</a>
                </div>
                <a href="#" onclick="hideBetaBanner()" class="visible-xs close-banner mobile-close" style="top: auto">
                    <img src="/img/close_black.svg" alt="close Button" />
                </a>
            </div>
            <div id="BannerRedirectBtn" class="hidden-xs col-sm-5 memucho-info-partial">
                <a href="#" style="visibility: hidden;">
                    <div class="memo-button btn btn-primary">Jetzt mehr erfahren</div>
                </a>
                <a href="#" onclick="hideBetaBanner()" class="close-banner">
                    <img src="/img/close_black.svg" alt="close Button" />
                </a>
            </div>
        </div>
    </div>
</div>
<div id="MemuchoInfoBanner" class="<%= notFirstTimeInfoClass %>">
    <div id="InfoBannerContainer" class="container <%= topBannerClass %>">
        <div id="BannerContainer" class="row">
            <div id="BannerText" class="col-xs-12 col-sm-7 memucho-info-partial">
                <div class="row fullWidth">
                    <div id="BannerImg" class="col-sm-1 hidden-xs">
                        <img src="/img/fire_heart.svg" alt="FeurigesHerz" />
                    </div>
                    <div class="col-sm-11 bannerTextContainer">
                        <div class="sub-text">
                            Alles an einem Ort
                        </div>
                        <div class="visible-xs sub-text">
                            <img src="/img/fire_heart.svg" alt="FeurigesHerz" />
                        </div>
                        <div class="main-text">Wiki und Lernwerkzeuge vereint!</div>
                        <a href="#" onclick="hideInfoBanner()" class="visible-xs close-banner mobile-close">
                            <img src="/img/close_black.svg" alt="X" />
                        </a>
                    </div>
                </div>
            </div>
            <div id="BannerRedirectBtn" class="col-xs-12 col-sm-5 memucho-info-partial">
                <a href="#" onclick="OpenInfo('<%= Links.CategoryDetail(EntityCache.GetCategoryCacheItem(RootCategory.IntroCategoryId)) %>')">
                    <div class="memo-button btn btn-primary">Zur Dokumentation</div>
                </a>
                <a href="#" onclick="hideInfoBanner()" class="hidden-xs close-banner">
                    <img src="/img/close_black.svg" alt="close Button" />
                </a>
            </div>
        </div>
    </div>
</div>
