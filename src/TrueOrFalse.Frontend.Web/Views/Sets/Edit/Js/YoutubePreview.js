


var optionsYoutube = {
    callback: function (data) {
        // No Else ,validierung Is carried out in validation.ts

        var regExp = /^.*(youtu.be\/|v\/|u\/\w\/|embed\/|watch\?v=|\&v=|\?v=)([^#\&\?]*).*/;
        var match = data.match(regExp);
        if (match && match[2].length == 11) {

            player.loadVideoById({
                'videoId': match[2],
                'suggestedQuality': 'large'
            });

            console.log('https://www.youtube.com/embed/' + match[2]);
            $('#player').fadeIn();


        }


    },
    wait: 750,
    highlight: true,
    allowSubmit: false,
    captureLength: 0,
    allowSameSearch: true
}







// 2. This code loads the IFrame Player API code asynchronously.
var tag = document.createElement('script');

    tag.src = "https://www.youtube.com/iframe_api";
var firstScriptTag = document.getElementsByTagName('script')[0];
firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);

// 3. This function creates an <iframe> (and YouTube player)
//    after the API code downloads.
var player;
function onYouTubeIframeAPIReady() {
    player = new YT.Player("player", {
        height: "360",
        width: "640",
        videoId: ""
        
    });
}



   

   
   


$(function () {
    $("#ulQuestions").on("click", ".time-button", function () {
        var temp=0;
        if (player.getCurrentTime() % 60 < 10) {
             temp = Math.floor(player.getCurrentTime() / 60) + ":" + "0" + (player.getCurrentTime() % 60).toFixed();            
        }else{
            temp = Math.floor(player.getCurrentTime() / 60) + ":" + (player.getCurrentTime() % 60).toFixed(); 
        }
        $(this).parent().find(".form-control").val(temp);
        var timeCode = temp;       
        var questionInSetId = $(this).attr("data-in-set-id");
        $.post("/SetVideo/SaveTimeCode/", { timeCode: timeCode, questionInSetId: questionInSetId });
    
        player.stopVideo();

       
    });

    $("#VideoUrl").typeWatch(optionsYoutube);

    if ($("#VideoUrl").val() === "") {
        $("#player").hide();
    }

});