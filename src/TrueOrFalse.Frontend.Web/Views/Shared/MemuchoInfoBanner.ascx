<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div id="MemuchoInfoBanner">
    <div class="container">
        <div id="BannerContainer">
            <div id="BannerText" class="col-xs-12 col-sm-7 memucho-info-partial">
                <div class="sub-text">
                    Alles an einem Ort 
                    <br class="visible-xs"/>
                    <i class="fas fa-heart"></i>
                </div>
                <div class="main-text">Wiki und Lernwerkzeuge vereint!</div>
                <a href="#" onclick="HideInfoBanner()" class="visible-xs close-banner mobile-close">
                    <i class="fas fa-times"></i>
                </a>
            </div>
            <div id="BannerRedirectBtn" class="col-xs-12 col-sm-5 memucho-info-partial">
                <a href="#" onclick="OpenInfo('<%= Links.CategoryDetail(EntityCache.GetCategoryCacheItem(RootCategory.IntroCategoryId)) %>')">
                    <div class="memo-button btn btn-primary">Jetzt mehr erfahren</div>
                </a>
                <a href="#" onclick="HideInfoBanner()" class="hidden-xs close-banner">
                    <i class="fas fa-times"></i>
                </a>
            </div>
        </div>

    </div>

</div>