var fnInitPopover = function(jObject) {
    jObject.each(function() {
        $(this).find($('.PopoverFocus'))
            .click(function (e) {
                e.preventDefault();
            })
            .popover(
            {
                trigger: "focus",
                placement: "right",
                html: "true",
            }
            );
        $(this).find($('.PopoverHover'))
            .click(function (e) {
                e.preventDefault();
            })
            .popover(
                {
                    trigger: "hover",
                    placement: "right",
                    html: "true",
                }
            );
    });
}