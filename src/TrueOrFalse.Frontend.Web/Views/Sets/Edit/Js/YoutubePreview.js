


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
            $('#player').fadeIn();


        }


    },
    wait: 750,
    highlight: true,
    allowSubmit: false,
    captureLength: 0,
    allowSameSearch: true
};


// 2. This code loads the IFrame Player API code asynchronously.
var tag = document.createElement('script');
var isLoaded = false;
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
        videoId: "",
        events: {
            // ist der Youtubeplayer fertig geladen wird isLoadet auf true gesetzt , da wir nicht wissen was zuerst fertig ist Seite oder player
            'onReady': function () {
                isLoaded = true;
                // wird ein Trigger abgesetzt der aufs Document geht 
                $(document).trigger('playerIsLoaded');
            }
        }
        
    });
}



   function simulatdedClickOfVideoUrlInput() {
       $("#VideoUrl").trigger(jQuery.Event("keydown", { keycode: 39 }));
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
    
        player.pauseVideo();   
    });

    $("#VideoUrl").typeWatch(optionsYoutube);

    if ($("#VideoUrl").val() === "") {
        $("#player").hide();
    }
    // der Trigger enthält den Wert der Variablen isLoadet ist der Wert true wird der Trigger auf das Input feld gesetzt und dieses neu geladen
    // Sinn der Aktion ist es ,das typewatch aktualisiert wird und ein Wert an den Player übergeben wird 
    if (isLoaded) {
       // $('#VideoUrl').trigger(jQuery.Event('keydown', { keycode: 39 }));
        simulatdedClickOfVideoUrlInput();
    } else {
        // ist der Status false ,on wird erst ausgeführt wenn playerIsLoadet getriggert wurde
        $(document).on('playerIsLoaded', function () {
            simulatdedClickOfVideoUrlInput();
            //$('#VideoUrl').trigger(jQuery.Event('keydown', { keycode: 39 }));
        });
    }
});