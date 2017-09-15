
$(function () {
    $("span.mailme")
        .each(function () {
            var spt = this.innerHTML;
            var at = / at /;
            var dot = / dot /g;
            var addr = spt.replace(at, "@").replace(dot, ".");
            $(this).after('<a href="mailto:' + addr + '" title="Schreibe eine E-Mail">' + addr + '</a>');
            $(this).remove();
        });
});
