function FillSparklineTotals() {
    $(".sparklineTotals").each(function () {
        $(this).sparkline([parseInt($(this).attr("data-answersTrue")), parseInt($(this).attr("data-answersFalse"))], {
            type: 'pie',
            sliceColors: ['#3e7700', '#B13A48']
        });
    });

    $(".sparklineTotalsUser").each(function () {
        $(this).sparkline([parseInt($(this).attr("data-answersTrue")), parseInt($(this).attr("data-answersFalse"))], {
            type: 'pie',
            sliceColors: ['#3e7700', '#B13A48']
        });
    });    
}

function InitLabelTooltips() {
    $('.label-category').each(function () {
        $(this).addClass('show-tooltip');
        if ($(this).attr("data-isSpolier") === "true"){
            $(this).attr('title', 'Die Kategorie entspricht der Antwort.').attr('data-placement', 'top');
        }else{
            $(this).attr('title', 'Gehe zu Kategorie').attr('data-placement', 'top');
        }
    });
    $('.label-set').each(function () {
        $(this).addClass('show-tooltip');
        $(this).attr('title', 'Gehe zu Fragesatz').attr('data-placement', 'top');
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

$(function () {
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
                content += "<a href='"+ data[i].Url +"'><span class='label label-set' style='display:block;'>" + data[i].Name +  "</span></a>&nbsp;";
            }

            content = "<div style=''>" + content + "</div>";

            elem.popover({
                title: 'weitere Frages&#228tze',
                html : true,
                content: content,
                trigger: 'click'
            });

            elem.popover('show');
        });
    });
   
    $("#logo").hover(
        function () { $(this).animate({ 'background-size': '100%' }, 250); },
        function () { $(this).animate({ 'background-size': '86%' }, 250); }
    );

    FillSparklineTotals();
    InitLabelTooltips();
    InitIconTooltips('fa-trash-o', 'Löschen');
    InitIconTooltips('fa-pencil', 'Bearbeiten');
    Images.Init();
});

