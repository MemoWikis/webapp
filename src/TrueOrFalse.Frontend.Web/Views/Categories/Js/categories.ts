var initCategories = () => {
    new Pin(PinType.Category);
    new CategoryDelete();    
}


$(() => {
    initCategories();
    new SearchInTabs(initCategories);
});