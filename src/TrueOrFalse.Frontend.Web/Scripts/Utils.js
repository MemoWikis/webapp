var Utils = (function () {
    function Utils() {
    }
    Utils.prototype.HtmlEncode = function (str) {
        return String(str).replace(/&/g, '&amp;').replace(/"/g, '&quot;').replace(/'/g, '&#39;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
    };
    return Utils;
})();
