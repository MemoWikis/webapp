
var initSets = () => {
    new SetDelete();
    new Pin(PinType.Set);    
}

$(() => {
    initSets();
    new SearchInTabs(initSets);
});