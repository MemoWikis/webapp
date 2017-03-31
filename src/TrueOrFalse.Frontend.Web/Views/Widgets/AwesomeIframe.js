(function (win, doc) {

    var awesomeIframe = {};

    // get requestAnimationFrameObject - see: https://developer.mozilla.org/en-US/docs/Web/API/window/requestAnimationFrame

    awesomeIframe.requestAnimFrame = (function () {
        var lastTime = 0;

        return win.requestAnimationFrame ||

                // polyfill with setTimeout fallback for IE8/9
                // heavily inspired from @darius gist mod: https://gist.github.com/paulirish/1579671#comment-837945

                function (callback) {

                    var now = +new Date(), nextTime = Math.max(lastTime + 16, now);
                    return setTimeout(function () {
                        callback(lastTime = nextTime);
                    }, nextTime - now);
                };
    })();

    awesomeIframe.windowHeight = 0;
    awesomeIframe.htmlElement = doc.getElementsByTagName('html')[0];

    // Domains to send post messages to - '*' for wildcard domains
    awesomeIframe.targetDomain = '*';

    awesomeIframe.resizeFrame = function () {

        var windowHeight = doc.body ?
            Math.max(doc.body.offsetHeight, awesomeIframe.htmlElement.offsetHeight) :
            awesomeIframe.htmlElement.offsetHeight;

        if (awesomeIframe.windowHeight === windowHeight) {

            awesomeIframe.requestAnimFrame.call(win, awesomeIframe.resizeFrame);

            return false;
        }

        awesomeIframe.windowHeight = windowHeight;

        try {

            // Same Origin iFrame
            // manipulate style of the iframe-element the page is embedded in - see: https://developer.mozilla.org/en-US/docs/Web/API/Window/frameElement

            win.frameElement.style.height = windowHeight + 'px';
        }
        catch (e) {

            // Cross Origin iFrame
            // post message to parent iframe - see https://developer.mozilla.org/en-US/docs/Web/API/Window/postMessage

            win.parent.postMessage('resize:' + windowHeight, awesomeIframe.targetDomain);
        }

        awesomeIframe.requestAnimFrame.call(win, awesomeIframe.resizeFrame);
    };

    awesomeIframe.requestAnimFrame.call(win, awesomeIframe.resizeFrame);

})(window, document, undefined);
