var arrayStops;
var setVideo: SetVideo;                                                //setvideo global

declare var initPlayer: () => void;                                    // initPlayer wird declariert Rückgabe = void

class StopVideoAt {                                                     // wird ein Object erzeugt welches die QuestionId und die geplante Stopzeit enthält

    QuestionId: number;
    Seconds : number;

    constructor(questionId: number, seconds: number) {
        this.QuestionId = questionId;
        this.Seconds = seconds;
    }
}

class SetVideoPlayer                                                              
{
    VideoStops: StopVideoAt[] = [];                  // StopArray
    VideoIsPlaying = false;                          // Videostatus standard = stoppt
    VideoCheckIntervalPaused = false;                // Video wird gecheckt = standard

    IsVideoPausingEnabled = true;                    // ?????

    Player : YT.Player;

    constructor() {                                               // YoutubePlayer wird erstellt 
        initPlayer = () => {                                      // wird nach download der YoutubeApi aufgerufen 
            player = new YT.Player('player', {               
                playerVars: { rel: 0 },
                events: {
                    'onReady':
                        setVideoPlayer.OnPlayerReady,             // wenn PLayer Rdy wird die OnplayerRdy aufgerufen        
                    'onStateChange': function (e) { setVideoPlayer.OnStateChange(e, setVideoPlayer) }        // bei Änderungen am Player wird eine eine nonyme Function aufgerufen das Event übergeben 
                }                                                                                            // diese ruft setVideoPlayer.OnStateChange(e, setVideoPlayer) auf und erstellt ein neuen setVideoPlayer
            });
        }
        apiLoad();

        $(() => {
            this.InitVideoStops();
            this.StartTimecodeCheck();
            new VideoPausingButtons(this);
        });
    }

    public OnPlayerReady() {                                        // Wenn dokument fertig wird player an Player übergeben
        $(document).ready(() => {
            this.Player = player;
      
        });
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

        $("#video-pager a[data-video-question-id]").each(function () {                  // suche alle lInks die data-video-question-id in #video-pager enthalten 

            var pauseAt = +$(this).attr("data-video-pause-at");                       //1        // hier wird die geplante Pause abgeholt ergo muss sie schon vorher eingetragen werden

            if (pauseAt == 0)                                                           // ist die pause 0 verlasse funktion
                return;

            self.VideoStops.push(                                                  // Videostops erzeugen und in Html setzen
                new StopVideoAt(                                                                                                                         
                    +$(this).attr("data-video-question-id"),                         // neuer Stop erste wird die ID gesetzt 
                    pauseAt                                                           // wann Pause siehe 1 
                ));
        });
      
    }

   

    public evaluationArray = (stops) => {
        var temp = 0;
        
        $(document).on('click', '.test', (e) => {
            
            e.preventDefault();
            temp += 1;
                if (temp < stops.length) {
                    SetVideo.ClickItem(stops[temp].QuestionId);
                    console.log(temp);
                    return stops;
                    
                } else {
                    player.playVideo();
                    setVideo.HideYoutubeOverlay();
                    return[];
                }
            
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
           
            if (stops.length > 1) {
                player.pauseVideo();
                setVideo.ShowYoutubeOverlay();
                   arrayStops = this.evaluationArray(stops);
            } else if (stops.length === 1) {   
                player.pauseVideo();
                setVideo.ShowYoutubeOverlay();
                    SetVideo.ClickItem(stops[0].QuestionId);
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
