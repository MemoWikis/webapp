/// <reference path="../../../../scripts/typescript.defs/lib.d.ts" />
$(function () {
    function foo(d, s, id) {
        var js, fjs = d.getElementsByTagName(s)[0];
        if (d.getElementById(id))
            return;
        js = d.createElement(s);
        js.id = id;
        js.src = "//connect.facebook.net/de_DE/all.js#xfbml=1&appId=128827270569993";
        fjs.parentNode.insertBefore(js, fjs);
    }
    foo(document, 'script', 'facebook-jssdk');

    $("#iAdd").click(function (e) {
        if ($(this).hasClass("fa-heart-o"))
            $(this).switchClass("fa-heart-o", "fa-heart");
        else
            $(this).switchClass("fa-heart", "fa-heart-o");

        e.preventDefault();
    });
});

function Pin(value) {
}

function UnPin() {
}

function Call(url) {
    //$.ajax({
    //    type: 'POST',
    //    url: "/Api/Questions/Pin/" + window.questionId + "/" + value,
    //    cache: false,
    //});
}
//# sourceMappingURL=Page.js.map
