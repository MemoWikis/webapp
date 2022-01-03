function InitLabelTooltips() {
    $('.label-category').each(function () {

        if (!$(this).attr('data-original-title') && !$(this).attr('title')) {//Proceed only for those labels that don't have a tooltip text specified yet
            $(this).addClass('show-tooltip');
            if ($(this).attr("data-isSpolier") === "true") {
                $(this).attr('title', 'Das Thema entspricht der Antwort.').attr('data-placement', 'top');
            } else {
                let boarderLeftRight = 2;
                if ($(this).innerWidth() == (parseInt($(this).css('max-width')) - boarderLeftRight * (parseInt($(this).css('border-left-width')))))
                    $(this).attr('title', 'Zum Thema "' + $("<p>" + $(this).html() + "</p>").text() + '"').attr('data-placement', 'top'); // p-tags added for .text() to work normal when stripping of tags
                else
                    $(this).attr('title', 'Zum Thema').attr('data-placement', 'top');
            }
            $(this).tooltip();
        }
    });
}

function InitIconTooltips(awesomeClass: string, tooltipText: string) {
    $('i.' + awesomeClass).each(function () {

        var hasTitleAttribute = jQuery.inArray(this, $.makeArray($('.' + awesomeClass + '[title]'))) !== -1 //Does title attribute exist altogether
            && $(this).attr('title') !== "";//Is title attribute not empty
        if (!hasTitleAttribute) {
            $(this).addClass('show-tooltip');
            $(this).attr('title', tooltipText).attr('data-placement', 'top');
        }
    });
}

function allowedOnlyForActiveUsers() {
    $("[data-allowed=logged-in]")
        .click(function (e) {
            var elem = $(this);
            if (NotLoggedIn.Yes()) {
                e.preventDefault();
                NotLoggedIn.ShowErrorMsg(elem.attr("data-allowed-type"));
            }
        });
}

function initClickLog(limitingSlector: string = null) {
    $(limitingSlector + "[data-click-log]")
        .click(function () {
            var data = $(this).attr("data-click-log");
            var datas = data.split(",");

            var category = <string>datas[0];
            var action = <string>datas[1];

            var label = "";
            if (data.length > 2)
                label = <string>datas[2];
        });
}

function initTooltips() {
    InitLabelTooltips();
    InitIconTooltips('fa-trash-o', 'Löschen');
    InitIconTooltips('fa-pencil', 'Bearbeiten');
    $('.show-tooltip').tooltip();
}

function preventDropdonwnsFromBeingHorizontallyOffscreen(limitingSlector: string = null) {
    $(limitingSlector + '.dropdown')
        .on('shown.bs.dropdown',
            function (e) {
                var dropdown = $(e.delegateTarget).find('ul');
                if (dropdown.offset().left + dropdown.outerWidth() > document.body.clientWidth) {
                    dropdown.addClass('AlignDdRight');
                }
            });
}

var developOffline;

class Site {

    constructor() {
        $("#btn-logout").click(this.LogoutClick);
    }

    private LogoutClick(e) {
        e.preventDefault();

        var redirect = () => { location.href = $(this).attr("data-url"); }

        if ($(this).attr("data-is-facebook") == "true") {
            $.post("/Category/DeleteCookie").done(
                (data) => {
                    console.log(data);
                    FacebookMemuchoUser.Logout(redirect);
                }
            );
            
        } else {
            $.post("/Category/DeleteCookie").done(
                () => {
                    redirect();
                });
        }
    }

    static RedirectToPersonalHomepage( link: string) { location.href = link; }
    static RedirectToRegistrationSuccess() { location.href = "/"; }
    static RedirectToRegistration() { location.href = "/Registrieren"; }

    static ReloadPage() { window.location.reload(true) };

    static ReloadPage_butNotTo_Logout(link: string = window.location.pathname) {
        Site.RedirectToPersonalHomepage(link);
    }

    static CloseAllModals() {
        $('.modal').modal('hide');
    }

    static DevelopOffline() {
        return developOffline;
    }
}

var BrowserDetect = {//https://stackoverflow.com/a/13480430
    browser: "",
    //version: "",
    init: function () {
        this.browser = this.searchString(this.dataBrowser) || "Other";
        //this.version = this.searchVersion(navigator.userAgent) || this.searchVersion(navigator.appVersion) || "Unknown";
    },
    searchString: function (data) {
        for (var i = 0; i < data.length; i++) {
            var dataString = data[i].string;
            this.versionSearchString = data[i].subString;

            if (dataString.indexOf(data[i].subString) !== -1) {
                return data[i].identity;
            }
        }
    },
    //searchVersion: function (dataString) {
    //    var index = dataString.indexOf(this.versionSearchString);
    //    if (index === -1) {
    //        return;
    //    }

    //    var rv = dataString.indexOf("rv:");
    //    if (this.versionSearchString === "Trident" && rv !== -1) {
    //        return parseFloat(dataString.substring(rv + 3));
    //    } else {
    //        return parseFloat(dataString.substring(index + this.versionSearchString.length + 1));
    //    }
    //},

    dataBrowser: [
        { string: navigator.userAgent, subString: "Edge", identity: "Edge" },
        { string: navigator.userAgent, subString: "MSIE", identity: "Explorer" },
        { string: navigator.userAgent, subString: "Trident", identity: "Explorer" },
        { string: navigator.userAgent, subString: "Firefox", identity: "Firefox" },
        { string: navigator.userAgent, subString: "Opera", identity: "Opera" },
        { string: navigator.userAgent, subString: "OPR", identity: "Opera" },

        { string: navigator.userAgent, subString: "Chrome", identity: "Chrome" },
        { string: navigator.userAgent, subString: "Safari", identity: "Safari" }
    ]

};

function setBrowserClass() {
    BrowserDetect.init();
    $('html').addClass(BrowserDetect.browser);
}

function loadInfoBanner() {
    var cookie = document.cookie.match('(^|;)\\s*' + "memuchoInfoBanner" + '\\s*=\\s*([^;]+)')?.pop() || '';
    if (cookie != 'hide') {
        $('#MemuchoInfoBanner').addClass('show-banner');
        document.cookie = "memuchoInfoBanner=notFirstTime; expires=Fri, 31 Dec 9999 23:59:59 GMT;path=/";
    }
}

function hideInfoBanner() {
    $('#MemuchoInfoBanner').removeClass('show-banner');
    document.cookie = "memuchoInfoBanner=hide; expires=Fri, 31 Dec 9999 23:59:59 GMT;path=/";
}

function OpenInfo(url) {
    window.location.href = url;
}

function updateBreadCrumb() {
    var sessionStorage = window.sessionStorage;
    var currentCategoryId = $('#hhdCategoryId').val();

    if (currentCategoryId == undefined) {
        new StickyHeaderClass();
        return;
    }
    if ($('#hddIsWiki').val() == 'True')
        sessionStorage.setItem('currentWikiId', currentCategoryId);
    var sessionWikiId = parseInt(sessionStorage.getItem('currentWikiId'));

    var currentWikiId = 0;
    if (!isNaN(sessionWikiId))
        currentWikiId = sessionWikiId;

    var json = {
        wikiId: currentWikiId,
        currentCategoryId: currentCategoryId,
    }
    $.ajax({
        type: 'Post',
        contentType: "application/json",
        url: '/BreadcrumbApi/SetWiki',
        data: JSON.stringify(json),
        success: function (result) {
            $('#FirstChevron').replaceWith(result.firstChevron);
            $('#BreadCrumbTrail').html(result.breadcrumbTrail);

            sessionStorage.setItem('currentWikiId', result.newWikiId);
            new StickyHeaderClass();
        },
    });
}

$(() => {
    new Site();
    setBrowserClass();
    updateBreadCrumb();
    initTooltips();
    Images.Init();
    allowedOnlyForActiveUsers();
    initClickLog();
    preventDropdonwnsFromBeingHorizontallyOffscreen();
    loadInfoBanner();
    if (window.location.host.startsWith("stage.memucho.de"))
        checkStageOverlay();
});