


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
    captureLength: 15,
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
    player = new YT.Player('player', {
        height: '360',
        width: '640',
        videoId: '',
        
    });
}



   

   
   


$(function () {
    $("#ulQuestions").on('click', '.form-control', function () {
       
        if (player.getCurrentTime() % 60 < 10) {
            console.log
            var temp = Math.floor(player.getCurrentTime() / 60) + ":" + "0" + (player.getCurrentTime() % 60).toFixed();
            console.log(temp);
        }else{
        var temp = Math.floor(player.getCurrentTime() / 60) + ":" + (player.getCurrentTime() % 60).toFixed(); 
        }
        $(this).val(temp);
        player.stopVideo();
       
    });

    $("#VideoUrl").typeWatch(optionsYoutube);
    $('#player').hide();


});