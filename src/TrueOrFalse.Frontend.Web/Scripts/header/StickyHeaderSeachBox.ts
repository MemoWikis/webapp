var isOpen = false;

function SearchButtonClick() {

    var SearchButton = document.getElementById('SearchButton');
    var inputBox = document.getElementById('StickyHeaderSearchBox');
    var searchBox = document.getElementById('StickyHeaderSearchBoxDiv');


    if (isOpen == false) {
        inputBox.style.padding = '6px 12px';
        searchBox.style.width = '262.41px';
        isOpen = true;
    } else {
        inputBox.style.padding = '0px';
        searchBox.style.width = '48px';
        isOpen = false;
    }
}

