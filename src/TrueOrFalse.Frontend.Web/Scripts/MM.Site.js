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
            for (var i = 0; i < data.length; i++) {
                content += "<a href='" + data[i].Url + "'><span class='label label-set'>" + data[i].Name + "</span></a>&nbsp;";
            }

            content = "<div style='width:150px;'>" + content + "</div>";

            elem.popover({
                title: 'Alle Frages&#196;tze',
                html: true,
                content: content,
                trigger: 'click'
            });

            elem.popover('show');
        });
    });
});
//# sourceMappingURL=MM.Site.js.map
