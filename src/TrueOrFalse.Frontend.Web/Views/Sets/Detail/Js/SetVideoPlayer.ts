﻿var player: YT.Player;

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

    IsVideoPausingEnabled = true;

    constructor() {

        $(() => {
            this.InitVideoStops();
            this.StartTimecodeCheck();
            new VideoPausingButtons(this);
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

            if (!this.VideoIsPlaying || this.VideoCheckIntervalPaused || !this.IsVideoPausingEnabled)
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

class VideoPausingButtons {
    _stopPausingBtn: JQuery;
    _startPausingBtn: JQuery;

    _setVideoPlayer: SetVideoPlayer;

    constructor(setVideoPlayer : SetVideoPlayer) {
        this._setVideoPlayer = setVideoPlayer;
        this.InitPauseStopActions();
    }

    InitPauseStopActions() {
        this._stopPausingBtn = $("#stopPausingVideo");
        this._startPausingBtn = $("#startPausingVideo");

        if (this._setVideoPlayer.IsVideoPausingEnabled) {
            this.ShowStopPausingBtn();
        } else {
            this.ShowStartPausingBtn();
        }

        this._stopPausingBtn.click((e) => {
            e.preventDefault();
            this._setVideoPlayer.IsVideoPausingEnabled = false;
            this.ShowStartPausingBtn();
        });

        this._startPausingBtn.click((e) => {
            e.preventDefault();
            this._setVideoPlayer.IsVideoPausingEnabled = true;
            this.ShowStopPausingBtn();
        });
    }

    ShowStartPausingBtn() {
        this._startPausingBtn.show();
        this._stopPausingBtn.hide();
    }

    ShowStopPausingBtn() {
        this._startPausingBtn.hide();
        this._stopPausingBtn.show(); 
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