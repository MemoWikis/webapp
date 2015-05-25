/// <reference path="../../../../scripts/typescript.defs/lib.d.ts" />
$(function () {
    var solutionEntry = new SolutionEntry();
    solutionEntry.Init();

    function loadFacebook(d, s, id) {
        var js, fjs = d.getElementsByTagName(s)[0];
        if (d.getElementById(id))
            return;
        js = d.createElement(s);
        js.id = id;
        js.src = "//connect.facebook.net/de_DE/all.js#xfbml=1&appId=128827270569993";
        fjs.parentNode.insertBefore(js, fjs);
    }
    loadFacebook(document, 'script', 'facebook-jssdk');
});
//# sourceMappingURL=Page.js.map
