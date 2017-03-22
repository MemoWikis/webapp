var iframeId = "memuchoFrame";
var senderDomains = ['http://memucho', 'http://memucho.local', 'https://memucho.de'];

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

                var maxWidth = iframes[i].getAttribute("maxWidth");
                if (maxWidth && maxWidth.length > 0) {
                    iframes[i].style.maxWidth = maxWidth;
                }

                return;
            }
        }
    }
}

function writeIframe(iframeId, iframeSource) {
    var width = scriptTag.getAttribute("width");

    var attrMaxWidth = "";
    var maxWidth = scriptTag.getAttribute("maxWidth");
    if (maxWidth && maxWidth.length > 0) {
        attrMaxWidth = "maxWidth=\"" + maxWidth + "\"";
    }

    var iframeHtml = '<iframe ' +
        'id="' + iframeId + '" name="widget" ' +
        'src="#" height="1" ' + attrMaxWidth + ' ' +
        'marginheight="0" marginwidth="0" ' +
        'frameborder="no" scrolling="no"> ' +
        '</iframe>';

    document.write(iframeHtml);

    var loadedIframe = parent.document.getElementById(iframeId);
    loadedIframe.width = width;
    loadedIframe.src = filePath;    
}

function getScripts() {
    var scripts_ = document.getElementsByTagName('script');

    var scripts = [];
    for (var i = 0; i < scripts_.length; i++) {
        if (scripts_[i].getAttribute("t"))
            scripts.push(scripts_[i]);
    }

    return scripts;
}

if (window.addEventListener) {
    window.addEventListener('message', receiveMessage, false);
}
else if (window.attachEvent) {
    window.attachEvent('onmessage', receiveMessage);
}

var scripts = getScripts();
var scriptIndex;

if(scriptIndex == undefined)
    scriptIndex = -1;

scriptIndex++;

var scriptTag = scripts[scriptIndex];

var type_ = scriptTag.getAttribute("t");

var domain = "https://memucho.de";
var domainForDebug = scriptTag.getAttribute("domainForDebug");

if (domainForDebug && domainForDebug.length > 0)
    domain = domainForDebug;


var queryKnowledgeBtn = "";
var hideKnowledgeBtn = scriptTag.getAttribute("hideKnowledgeBtn");
if (hideKnowledgeBtn && hideKnowledgeBtn.length > 0 && hideKnowledgeBtn == "true") {
    queryKnowledgeBtn = "?hideAddToKnowledge=true";
}

if (type_ === "question")
{
    var questionId = scriptTag.getAttribute("id");

    var filePath = domain + '/widget/frage/' + questionId + queryKnowledgeBtn;
    var iframeId = "iframe-q" + questionId + Math.floor((Math.random() * 10000) + 1);

    writeIframe(iframeId, filePath);
}    
else if (type_ === "set")
{
    var setId = scriptTag.getAttribute("id");

    var filePath = domain + '/widget/fragesatz/' + setId + queryKnowledgeBtn;
    var iframeId = "iframe-s" + setId + Math.floor((Math.random() * 10000) + 1);

    writeIframe(iframeId, filePath);
}
else if (type_ === "setVideo") {
    var setId = scriptTag.getAttribute("id");

    var filePath = domain + '/widget/fragesatz-v/' + setId + queryKnowledgeBtn;
    var iframeId = "iframe-sv" + setId + Math.floor((Math.random() * 10000) + 1);

    writeIframe(iframeId, filePath);
}