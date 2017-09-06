

var youtube = {

    transformYoutubeUrl: function (url) {
        var regExp = /^.*(youtu.be\/|v\/|u\/\w\/|embed\/|watch\?v=|\&v=|\?v=)([^#\&\?]*).*/;
        var match = url.match(regExp);
        return match;

    },

    loadPlayer: function (urlObject) {
        //fehleranzeige vermeiden versucht es sonst auch zu laden wenn Objekt null ist 
        if (urlObject === null)
            return;
        player.loadVideoById({
            videoId: urlObject[2]

        });
    },
    timeTransform: function (value = "") {
        var timeTransformValue = Math.floor(player.getCurrentTime() / 60) + ":" + value + (player.getCurrentTime() % 60).toFixed();
        return timeTransformValue;
    },
    videoAvailable: function (videoId) {

        return $.ajax({
            url: "https://www.googleapis.com/youtube/v3/videos?part=id&key=AIzaSyCPbY50W-gD0-KLnsKQCiS0d1Y5SKK0bOg&id=" + videoId
        });
    },
    videoAvailableSetDataVideoAvailableTrue: function () {
        $('#VideoUrl').attr('data-video-available', true);
    },
    videoAvailableSetDataVideoAvailableFalse: function () {
        $('#VideoUrl').attr('data-video-available', false);
    }
}


var optionsYoutubeTypeWatch = {
    callback: function (data) {

        var urlObject = youtube.transformYoutubeUrl(data);

        var videoAvailable = youtube.videoAvailable(urlObject[2]);

        videoAvailable.success(function (d) {
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
    },
    wait: 750,
    highlight: true,
    allowSubmit: true,
    captureLength: 0,
    allowSameSearch: true
};


// 2. This code loads the IFrame Player API code asynchronously.
var tag = document.createElement("script");
var isLoaded = false;
tag.src = "https://www.youtube.com/iframe_api";
var firstScriptTag = document.getElementsByTagName('script')[0];
firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);

// 3. This function creates an <iframe> (and YouTube player)
// after the API code downloads.
var player;

function onYouTubeIframeAPIReady() {
    player = new YT.Player("player", {
        height: "360",
        width: "640",
        events: {
            'onReady': function () {
                $('document').ready(function () {
                    //Standard ausblenden

                    everythingElse.hideElements();
                    var url = $('#VideoUrl').val();
                    var urlObject = youtube.transformYoutubeUrl(url);

                    // es  kann eine Url gespeichert sein ,diese muss sofort geprüft werden
                    var videoAvailable = youtube.videoAvailable(urlObject[2]);
                    videoAvailable.success(function (data) {


                        if (data.items.length > 0) {
                            everythingElse.fadeInElements();
                            youtube.loadPlayer(urlObject);
                            player.stopVideo();
                        } else if (url !== "") {
                            youtube.videoAvailableSetDataVideoAvailableFalse();


                        }
                    });

                });
            }
        }
    }
    )
};
var everythingElse = {

    hideElements: function () {
        $("#player").hide();
        $('#ulQuestions').removeClass('showTimeInput');
    },

    fadeInElements: function () {
        $('#ulQuestions').addClass('showTimeInput');
        $('#player').fadeIn();


    }



}





$(function () {

    $.validator.addMethod("UrlCheck", function (value, element) {
        return $(element).attr('data-video-available') === "true";

    }, 'Das Video ist nicht oder nicht mehr vorhanden');


    everythingElse.hideElements();
    $("#ulQuestions").on("click", ".time-button", function () {
        var temp;
        if (player.getCurrentTime() % 60 < 10) {
            temp = youtube.timeTransform("0");
        } else {
            temp = youtube.timeTransform();
        }

        $(this).parent().find(".form-control").val(temp);

        var questionInSetId = $(this).parent().find(".form-control").attr("data-in-set-id");
        $.post("/SetVideo/SaveTimeCode/", { timeCode: temp, questionInSetId: questionInSetId });

        player.pauseVideo();
    });

    $("#VideoUrl").typeWatch(optionsYoutubeTypeWatch);





});