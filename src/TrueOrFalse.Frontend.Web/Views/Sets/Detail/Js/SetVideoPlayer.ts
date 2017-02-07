class SetVideoPlayer
{
    public OnPlayerReady() {
        console.log("player ready");
    }

    public OnStateChange() {
        console.log("player state change");
    }
}


var setVideoPlayer = new SetVideoPlayer();

// 2. This code loads the IFrame Player API code asynchronously.
var tag = document.createElement('script');
tag.src = "https://www.youtube.com/iframe_api";
var firstScriptTag = document.getElementsByTagName('script')[0];
firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);

var player;
function onYouTubeIframeAPIReady() {
    player = new YT.Player('player', {
        events: {
            'onReady': setVideoPlayer.OnPlayerReady,
            'onStateChange': setVideoPlayer.OnStateChange
        }
    });
}