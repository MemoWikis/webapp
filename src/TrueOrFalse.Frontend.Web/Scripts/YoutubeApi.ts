var player;
var initPlayer = (): void => {}

function apiLoad() {
    // 2. This code loads the IFrame Player API code asynchronously.
    var tag = document.createElement('script');
    tag.src = "https://www.youtube.com/iframe_api";
    var firstScriptTag = document.getElementsByTagName('script')[0];
    firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);   
}

function onYouTubeIframeAPIReady() {
    initPlayer();
}


// onYouTubeIframeAPIReady() muss ausgelagert werden da Typescript gleiche Funktionsnamen global überprüft,
// da es keine Alternative zu onYouTubeIframeAPIReady() gibt, da diese nach dem Apiload automatisch aufgerufen wird 
// muss sie sichergestellt werden das sie jederzeit und in jedem benötigten Programmteil verfügbar ist ohne sie ein 2. mal zu deklarieren 

// die leere funktion myfunction(){} stellt die Basis der Funktionen zur Verfügung die jedesmal überschrieben werden müssem 