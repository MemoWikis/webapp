var player: YT.Player;

class StopVideoAt {

    QuestionId: number;
    Seconds : number;

    constructor(questionId: number, seconds: number) {
        this.QuestionId = questionId;
        this.Seconds = seconds;
    }
}

class SetVideoPlayer
{
    VideoStops: StopVideoAt[] = [];
    VideoIsPlaying = false;
    VideoCheckIntervalPaused = false;

    constructor() {

        $(() => {
            this.InitVideoStops();
            this.StartTimecodeCheck();
        });
    }

    public OnPlayerReady() {
        //console.log("player ready");
    }

    public OnStateChange(event : YT.EventArgs, setVideoPlayer : SetVideoPlayer) {

        if (event.data == YT.PlayerState.PLAYING) {
            setVideoPlayer.VideoIsPlaying = true;
            this.VideoCheckIntervalPaused = false;
        } else {
            setVideoPlayer.VideoIsPlaying = false;    
        }
    }

    public InitVideoStops() {
        var self = this;

        $("#video-pager a[data-video-question-id]").each(function () {

            self.VideoStops.push(
                new StopVideoAt(
                    +$(this).attr("data-video-question-id"),
                    +$(this).attr("data-video-pause-at")
                ));
        });    
    }

    public StartTimecodeCheck() {

        var lastVideoCheck = 0;
        var interval = setInterval(() => {

            if (!this.VideoIsPlaying || this.VideoCheckIntervalPaused)
                return;

            this.VideoCheckIntervalPaused = true;

            var currentTime = Math.round(player.getCurrentTime()) - 1;

            if (lastVideoCheck == currentTime) {
                this.VideoCheckIntervalPaused = false;
                return;
            }

            lastVideoCheck = currentTime;

            var stops: StopVideoAt[] = this.VideoStops.filter(item => item.Seconds == currentTime);

            if (stops.length > 0) {

                player.pauseVideo();
                SetVideo.ClickItem(stops[0].QuestionId);

                console.log("Video pausiert, beantworte die Frage");

            } else {
                this.VideoCheckIntervalPaused = false;
            }

        }, 500);
    }

}


var setVideoPlayer = new SetVideoPlayer();

// 2. This code loads the IFrame Player API code asynchronously.
var tag = document.createElement('script');
tag.src = "https://www.youtube.com/iframe_api";
var firstScriptTag = document.getElementsByTagName('script')[0];
firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);


function onYouTubeIframeAPIReady() {

    player = new YT.Player('player', {
        events: {
            'onReady': setVideoPlayer.OnPlayerReady,
            'onStateChange': function (e) { setVideoPlayer.OnStateChange(e, setVideoPlayer) }
        }
    });

}