function FillSparklineTotals() {
    if (isAnswered(".sparklineTotalsUser"))
        FillSparksElements(".sparklineTotalsUser");
    else
        FillSparksElements(".sparklineTotalsUser", "#949494", '#FFA07A', "pie", false);

    if (isAnswered(".sparklineTotals"))
        FillSparksElements(".sparklineTotals");
    else
        FillSparksElements(".sparklineTotals", "#949494",'#FFA07A', "pie",  false);
}

function isAnswered(elementName: string) {
    if (parseInt($(elementName).attr("data-answersTrue")) === 0 &&
        parseInt($(elementName).attr("data-answersFalse")) === 0)
        return false;

    return true; 
}

function FillSparksElements(elementName: string, color1 = '#AFD534', color2 = '#FFA07A', sparkType = "pie", isAnswered = true) {
    
    $(elementName).each(function () {
        if (isAnswered) {
            $(this).sparkline([parseInt($(this).attr("data-answersTrue")), parseInt($(this).attr("data-answersFalse"))],
                {
                    type: sparkType,
                    sliceColors: [color1, color2]
                });
        } else {
            $(this).sparkline([100, 0],
                {
                    type: sparkType,
                    sliceColors: [color1, color2]
                });
        }
    });
}

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

    $('.label-set').each(function () {
        if (!$(this).attr('data-original-title') && !$(this).attr('title')) {//Proceed only for those labels that don't have a tooltip text specified yet

            $(this).addClass('show-tooltip');
            if ($(this)[0]
                .scrollWidth >
                $(this).innerWidth())
                //this is simpler and more to the point, but in cases when content is just truncated does not work in firefox; reason: scrollWidth gives different values in FF and Chrome
                //if ($(this).innerWidth() == (parseInt($(this).css('max-width')) - 2*(parseInt($(this).css('border-left-width')))))
                $(this).attr('title', 'Zum Lernset "' + $(this).html() + '"').attr('data-placement', 'top');
            else
                $(this).attr('title', 'Zum Lernset').attr('data-placement', 'top');
            $(this).tooltip();
        }
    });
}

function InitIconTooltips(awesomeClass : string, tooltipText : string) {
    $('i.' + awesomeClass).each(function () {

        var hasTitleAttribute = jQuery.inArray(this, $.makeArray($('.' + awesomeClass + '[title]'))) !== -1 //Does title attribute exist altogether
            && $(this).attr('title') !== "";//Is title attribute not empty
        if (!hasTitleAttribute){
            $(this).addClass('show-tooltip');
            $(this).attr('title', tooltipText).attr('data-placement', 'top');
        }
    });
}

function Allowed_only_for_active_users() {
    $("[data-allowed=logged-in]")
        .click(function(e) {
            var elem = $(this);
            if (NotLoggedIn.Yes()) {
                e.preventDefault();
                NotLoggedIn.ShowErrorMsg(elem.attr("data-allowed-type"));
            }
        });
}

function InitClickLog(limitingSlector: string = null){
    $(limitingSlector + "[data-click-log]")
        .click(function () {
            var data = $(this).attr("data-click-log");
            var datas = data.split(",");

            var category = <string>datas[0];
            var action = <string>datas[1];

            var label = "";
            if (data.length > 2)
                label = <string>datas[2];

            Utils.SendGaEvent(category, action, label);
        });
}

function InitTooltips() {
    InitLabelTooltips();
    InitIconTooltips('fa-trash-o', 'Löschen');
    InitIconTooltips('fa-pencil', 'Bearbeiten');
    $('.show-tooltip').tooltip();
}

function InitPopoverForAllSets() {
    $("[popover-all-sets-for]").click(function (e) {

        e.preventDefault();

        var elem = $(this);

        if (elem.attr("loaded") == "true")
            return;

        $.post("/Api/Sets/ForQuestion", {
            "questionId": elem.attr("popover-all-sets-for")
        }, function (data) {

            elem.attr("loaded", "true");

            var content = "";
            for (var i = 5; i < data.length; i++) {
                content += "<a href='" + data[i].Url + "'><span class='label label-set' style='display:block;'>" + data[i].Name + "</span></a>&nbsp;";
            }

            content = "<div style=''>" + content + "</div>";

            elem.popover({
                title: 'Weitere Lernsets:',
                html: true,
                content: content,
                trigger: 'click'
            });

            elem.popover('show');
        });
    });    
}

function PreventDropdonwnsFromBeingHorizontallyOffscreen(limitingSlector: string = null) {
    $(limitingSlector + '.dropdown')
        .on('shown.bs.dropdown',
            function(e) {
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

    private LogoutClick(e)
    {
        e.preventDefault();

        var redirect = () => { location.href = $(this).attr("data-url"); }

        if ($(this).attr("data-is-facebook") == "true") {
            FacebookMemuchoUser.Logout(redirect);
        } else {
            redirect();
        }
    }
    static RedirectToDashboard() { location.href = "/Wissenszentrale/Ueberblick"; }
    static RedirectToRegistrationSuccess() { location.href = "/Register/RegisterSuccess"; }
    static RedirectToRegistration() { location.href = "/Registrieren"; }

    static ReloadPage() { window.location.reload(true) };
    static ReloadPage_butNotTo_Logout() {
        if (location.href.indexOf("Ausloggen") !== -1)
            Site.RedirectToDashboard();
        else
            Site.ReloadPage();
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

function SetBrowserClass() {
    BrowserDetect.init();
    $('html').addClass(BrowserDetect.browser);
}

$(() => {

    new Site();

    $("#logo").hover(
        function () { $(this).animate({ 'background-size': '100%' }, 250); },
        function () { $(this).animate({ 'background-size': '86%' }, 250); }
    );

    SetBrowserClass();
    InitPopoverForAllSets();
    FillSparklineTotals();
    InitTooltips();
    Images.Init();
    Allowed_only_for_active_users();
    InitClickLog();
    PreventDropdonwnsFromBeingHorizontallyOffscreen();
});