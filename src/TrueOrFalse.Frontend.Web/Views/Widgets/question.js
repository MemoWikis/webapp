var iframeId = "memuchoFrame";
var senderDomains = ['http://memucho', 'http://memucho.local', 'https://memucho.de'];

//http://stackoverflow.com/questions/9162933/make-iframe-height-dynamic-based-on-content-inside-jquery-javascript
function iframeLoaded() {
    //var iframeElement = document.getElementById(iframeId);
    //if (iframeElement) {
    //    // here you can make the height, I delete it first, then I make it again
    //    iframeElement.height = "";
    //    iframeElement.height = iframeElement.contentWindow.document.body.scrollHeight + "px";

    //    console.log(iframeElement.contentWindow.document.body.scrollHeight);
    //}
}

//https://github.com/closingtag/super-awesome-responsive-iframe-solution/blob/master/index.html
function receiveMessage(event) {
    var message = event.data.split(':');
    var eventName = message[0];
    var iframes, len, i = 0;
    // Domains to accept post messages from:
    // Fix .indexOf in IE8
    if (!Array.prototype.indexOf) {
        Array.prototype.indexOf = function (obj, start) {
            for (var i = (start || 0), j = this.length; i < j; i++) {
                if (this[i] === obj) { return i; }
            }
            return -1;
        }
    }

    if (senderDomains.indexOf(event.origin) !== -1 && eventName === 'resize') {
        iframes = document.getElementsByTagName('iframe');
        len = iframes.length;

        for (; i < len; i++) {
            if ((iframes[i].contentWindow || iframes[i].documentWindow) == event.source) {
                iframes[i].style.height = message[1] + "px";
                return;
            }
        }
    }
}

if (window.addEventListener) {
    window.addEventListener('message', receiveMessage, false);
}
else if (window.attachEvent) {
    window.attachEvent('onmessage', receiveMessage);
}

var scripts = document.getElementsByTagName('script');
var scriptTag = scripts[scripts.length - 1];
var questionId = scriptTag.getAttribute("questionId");
var domainForDebug = scriptTag.getAttribute("domainForDebug");
var width = scriptTag.getAttribute("width");

var domain = "https://memucho.de";

if (domainForDebug.length > 0)
    domain = domainForDebug;
        
var filePath = domain + '/widget/frage/' + questionId;

var iframeHtml = '<iframe ' +
    'id="' + iframeId + '" name="widget" ' +
    'src="#" height="1" ' +
    'marginheight="0" marginwidth="0" ' +
    'frameborder="no" scrolling="no" ' +
    'onload="iframeLoaded()"></iframe>';

document.write(iframeHtml);

var loadedIframe = parent.document.getElementById(iframeId);
loadedIframe.width = width;

loadedIframe.src = filePath;
//loadedIframe.style.border = "1px solid #999";
//loadedIframe.style.padding = "8px";