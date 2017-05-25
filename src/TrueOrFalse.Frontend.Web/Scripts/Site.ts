function FillSparklineTotals() {
    $(".sparklineTotals").each(function () {
        $(this).sparkline([parseInt($(this).attr("data-answersTrue")), parseInt($(this).attr("data-answersFalse"))], {
            type: 'pie',
            sliceColors: ['#90EE90', '#FFA07A']
        });
    });

    $(".sparklineTotalsUser").each(function () {
        $(this).sparkline([parseInt($(this).attr("data-answersTrue")), parseInt($(this).attr("data-answersFalse"))], {
            type: 'pie',
            sliceColors: ['#90EE90', '#FFA07A']
        });
    });    
}

function InitLabelTooltips() {
    $('.label-category').each(function () {

        if (!$(this).attr('data-original-title') && !$(this).attr('title')) {//Proceed only for those labels that don't have a tooltip text specified yet
            $(this).addClass('show-tooltip');
            if ($(this).attr("data-isSpolier") === "true") {
                $(this).attr('title', 'Das Thema entspricht der Antwort.').attr('data-placement', 'top');
            } else {
                if ($(this).innerWidth() == (parseInt($(this).css('max-width')) - 2 * (parseInt($(this).css('border-left-width')))))
                    $(this).attr('title', 'Zum Thema "' + $(this).html() + '"').attr('data-placement', 'top');
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

            //console.log("clientWidth: " + $(this)[0].clientWidth + " -scrollWidth: " + $(this)[0].scrollWidth + " -offsetWidth: " + $(this)[0].offsetWidth + " -innerWidth: " + $(this).innerWidth() + " -maxWidth:" + $(this).css('maxWidth'));
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
                title: 'weitere Frages&#228tze',
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

    static RedirectToDashboard() { location.href = "/Wissenszentrale"; }
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

$(() => {

    new Site();

    $("#logo").hover(
        function () { $(this).animate({ 'background-size': '100%' }, 250); },
        function () { $(this).animate({ 'background-size': '86%' }, 250); }
    );

    InitPopoverForAllSets();
    FillSparklineTotals();
    InitTooltips();
    Images.Init();
    Allowed_only_for_active_users();
    InitClickLog();
    PreventDropdonwnsFromBeingHorizontallyOffscreen();
});