$(function () {
    $("[popover-all-sets-for]").click(function () {
        var elem = $(this);

        $.post("/Api/Sets/ForQuestion", {
            "questionId": elem.attr("popover-all-sets-for")
        }, function (data) {
            console.log(data);

            var content = "";
            for (var i = 0; i < data.length; i++) {
                content += "<a href=''><span class='label label-set'>" + data[i].Name + "</span></a>&nbsp;";
            }

            content = "<div style='width:150px'>" + content + "</div>";

            elem.popover({
                title: 'Alle Fragesätze',
                html: true,
                content: content,
                trigger: 'hover'
            });

            elem.popover('show');
        });
    });
});
//# sourceMappingURL=MM.Site.js.map
