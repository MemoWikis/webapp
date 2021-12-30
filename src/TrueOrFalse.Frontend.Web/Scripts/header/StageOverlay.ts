function hideStageOverlay() {
    document.getElementById('StageOverlay').classList.add('closedContainer');
    sessionStorage.setItem('showStageOverlay', 'false');
}
function checkStageOverlay() {
    var showStageOverlay = sessionStorage.getItem('showStageOverlay');
    var overlay = document.getElementById('StageOverlay');
    if (showStageOverlay == 'false' && overlay != null) {
        overlay.classList.add('closedContainer');
    }
}