

///<reference path="../../../../Scripts/YoutubeApi.ts"/>


var youtube = {
    // Url object (1.)
    transformYoutubeUrl: url => {
        var regExp = /^.*(youtu.be\/|v\/|u\/\w\/|embed\/|watch\?v=|\&v=|\?v=)([^#\&\?]*).*/;
        var match = url.match(regExp);
        return match;

    },
    loadPlayer: urlObject => {
        //fehleranzeige vermeiden versucht es sonst auch zu laden wenn Objekt null ist 
        if (urlObject === null)
            return;
        player.loadVideoById({
            videoId: urlObject[2]

        });
    },
    timeTransform: (value = "") => {
        var timeTransformValue = Math.floor(player.getCurrentTime() / 60) + ":" + value + Math.floor(player.getCurrentTime()) % 60;
        return timeTransformValue;
    },
    videoAvailable: (videoId)=> {

        return $.ajax({
            url: "https://www.googleapis.com/youtube/v3/videos?part=id&key=AIzaSyCPbY50W-gD0-KLnsKQCiS0d1Y5SKK0bOg&id=" + videoId
        });
    },
    videoAvailableSetDataVideoAvailableTrue: function () {
        $('#VideoUrl').attr('data-video-available', "true");
    },
    videoAvailableSetDataVideoAvailableFalse: function () {
        $('#VideoUrl').attr('data-video-available', "false");
    }
    
}

var optionsYoutubeTypeWatch = {
    callback: (data) => {
        var videoAvailable;

        var urlObject = youtube.transformYoutubeUrl(data);


       try{
            videoAvailable = youtube.videoAvailable(urlObject[2]);
        

        videoAvailable.done((d)=> {
            if (d.items.length < 1) {
                youtube.videoAvailableSetDataVideoAvailableFalse();
                everythingElse.hideElements();

            } else {
                youtube.videoAvailableSetDataVideoAvailableTrue();
                youtube.loadPlayer(urlObject);
                everythingElse.fadeInElements();
                player.stopVideo();

            }

            $("#VideoUrl").valid();
            });
       } catch (e) {
           everythingElse.hideElements();
       }
    },
    wait: 750,
    highlight: true,
    allowSubmit: true,
    captureLength: 0,
    allowSameSearch: true
};


var everythingElse = {
    hideElements: ()=> {
        $("#player").hide();
        $('#ulQuestions').removeClass('showTimeInput');
        $('.questionItem').find('input').hide();
    },

    fadeInElements: ()=> {
        $('#ulQuestions').addClass('showTimeInput');
        $('#player').fadeIn();
        $('.questionItem').find('input').fadeIn();
    }
}


class YoutubeApiLoad {
    constructor() {
        var initPlayerSettings = (): void => {

            everythingElse.hideElements();
            var url = $('#VideoUrl').val();
            var urlObject = youtube.transformYoutubeUrl(url);

            // es  kann eine Url gespeichert sein ,diese muss sofort geprüft werden
            if (url !== "") {
                var videoAvailable = youtube.videoAvailable(urlObject[2]);

                videoAvailable.done(function(data) {

                    if (data.items.length > 0) {
                        youtube.videoAvailableSetDataVideoAvailableTrue();
                        everythingElse.fadeInElements();
                        youtube.loadPlayer(urlObject);
                        player.stopVideo();
                    } else if (url !== "") {
                        youtube.videoAvailableSetDataVideoAvailableFalse();
                    }
                });
            }
        }
        initPlayer = () => {
            player = new YT.Player('player', {
                playerVars: { rel: 0 },
                events: {
                    onReady: () => { initPlayerSettings() }
                }

            });
        }

        apiLoad();
        
    }


}



    $(function() {
        new YoutubeApiLoad();
        
        $.validator.addMethod("UrlCheck",
            (value, element) => {
                if ($('#VideoUrl').val() === "") {
                    return true;
                } else {
                    return $(element).attr('data-video-available') === "true";  
                }
            },
            'Das Video ist nicht oder nicht mehr vorhanden');

        everythingElse.hideElements();
        $("#ulQuestions").on("click",
            ".time-button",
            function () {// don't change into lambda expression, won't work 
                var timecode;
                if (player.getCurrentTime() % 60 < 10) {
                    timecode = youtube.timeTransform("0");
                } else {
                    timecode = youtube.timeTransform();
                }
                var input = $(this).parent().find(".form-control");
                input.val(timecode);
                var questionInSetId = input.attr("data-in-set-id");
                $.post("/SetVideo/SaveTimeCode/", { timeCode: timecode, questionInSetId: questionInSetId });
                player.pauseVideo();
            });

        $("#VideoUrl").typeWatch(optionsYoutubeTypeWatch);

    });
