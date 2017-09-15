var initCategories = () => {
    new Pin(PinType.Category);
    new CategoryDelete();    
}


$(() => {
    initCategories();
    new SearchInTabs(initCategories);

    new CategoryNetworkNavigation(); //only needed for InstallationAdmins
});