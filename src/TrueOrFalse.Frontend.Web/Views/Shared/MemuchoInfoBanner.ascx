<%@ Control Language="C#" AutoEventWireup="true" %>

<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<% var cookieValue = "";

    if (Request.Cookies["memuchoInfoBanner"] != null)
        cookieValue = Request.Cookies["memuchoInfoBanner"].Value;
    var notFirstTimeClass = "";
    if (cookieValue == "notFirstTime")
        notFirstTimeClass = "skip-animation";
%>

<div id="MemuchoBetaBanner" class="<%= notFirstTimeClass %>">
    <div class="container">
        <div id="BannerContainer">
            <div id="BannerText" class="col-xs-12 col-sm-7 memucho-info-partial">
                <div class="sub-text">
                    <b>Hi early adopters!</b> memucho befindet sich in der zweiten <a href="/Entwicklungsstatus-Beta/9032">Beta-Phase</a>
                </div>
                <a href="#" onclick="hideInfoBanner()" class="visible-xs close-banner mobile-close" style="top: auto">
                    <img src="/img/close_black.svg" alt="close Button"/>
                </a>
            </div>
            <div id="BannerRedirectBtn" class="hidden-xs col-sm-5 memucho-info-partial">
                <a href="#" style="visibility: hidden;">
                    <div class="memo-button btn btn-primary">Jetzt mehr erfahren</div>
                </a>
                <a href="#" onclick="hideInfoBanner()" class="close-banner">
                    <img src="/img/close_black.svg" alt="close Button"/>
                </a>
            </div>
        </div>
    </div>
</div>
<div id="MemuchoInfoBanner" class="<%= notFirstTimeClass %>">
    <div class="container">
        <div id="BannerContainer">
            <div id="BannerText" class="col-xs-12 col-sm-7 memucho-info-partial">
                <div class="sub-text">
                    Alles an einem Ort
                    <br class="visible-xs" />
                    <i class="fas fa-heart"></i>
                </div>
                <div class="main-text">Wiki und Lernwerkzeuge vereint!</div>
                <a href="#" onclick="hideInfoBanner()" class="visible-xs close-banner mobile-close">
                    <img src="/img/close_black.svg" alt="close Button"/>
                </a>
            </div>
            <div id="BannerRedirectBtn" class="col-xs-12 col-sm-5 memucho-info-partial">
                <a href="#" onclick="OpenInfo('<%= Links.CategoryDetail(EntityCache.GetCategoryCacheItem(RootCategory.IntroCategoryId)) %>')">
                    <div class="memo-button btn btn-primary">Jetzt mehr erfahren</div>
                </a>
                <a href="#" onclick="hideInfoBanner()" class="hidden-xs close-banner">
                    <img src="/img/close_black.svg" alt="close Button"/>
                </a>
            </div>
        </div>
    </div>
</div>
