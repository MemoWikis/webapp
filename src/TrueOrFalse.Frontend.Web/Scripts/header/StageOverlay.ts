function hideStageOverlay() {
    document.getElementById('StageOverlay').classList.add('closedContainer');
    sessionStorage.setItem('showStageOverlay', 'false');
}
function checkStageOverlay() {
    var showStageOverlay = sessionStorage.getItem('showStageOverlay');

    if (showStageOverlay == 'false') {
        document.getElementById('StageOverlay').classList.add('closedContainer');
    }
}

checkStageOverlay();