class YoutubePreview {
    constructor() {
    //    $("#VideoUrl").on("click", () => {
    //        event.preventDefault();
    //        $.post("/EditSet/GetYoutubeUrl",
    //            { url: "https://www.youtube.com/watch?v=R6odDPA5XfU" },
    //            function(data) {
    //                console.log(data);

    //            });
    //    });
        
        var options = {
            callback: function () {
               
                $.post("/EditSet/GetYoutubeUrl",
                    { url: $("#VideoUrl").val()  },
                    function (data) {
                        if (data != undefined || data != '') {
                            var regExp = /^.*(youtu.be\/|v\/|u\/\w\/|embed\/|watch\?v=|\&v=|\?v=)([^#\&\?]*).*/;
                            var match = data.match(regExp);
                            if (match && match[2].length == 11) {
                                // Do anything for being valid
                                // if need to change the url to embed url then use below line
                                $('#ytplayerSide').attr('src', 'https://www.youtube.com/embed/' + match[2] + '?autoplay=0');
                                
                            }

                        } else {
                            console.log("fuck");

                        }

                    });
            },
            wait: 750,
            highlight: true,
            allowSubmit: false,
            captureLength: 2,
            allowSameSearch: true
        }
        $("input").typeWatch(options);
    }
}