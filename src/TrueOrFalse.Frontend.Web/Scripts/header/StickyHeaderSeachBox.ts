var isOpen = false;
var isSmallHeaderSearchBoxOpen = false;

function SearchButtonClick() {

    var SearchButton = document.getElementById('StickyHeaderSearchButton');
    var inputBox = document.getElementById('StickyHeaderSearchBox');
    var searchBox = document.getElementById('StickyHeaderSearchBoxDiv');

    var SmallHeaderSearchButton = document.getElementById('SmallHeaderSearchButton');
    var SmallHeaderInputBox = document.getElementById('SmallHeaderSearchBox');
    var SmallHeaderSearchBox = document.getElementById('SmallHeaderSearchBoxDiv');


    if (isOpen == false) {
        inputBox.style.padding = '6px 12px';
        searchBox.style.width = '262.41px';
        isOpen = true;
    } else {
        inputBox.style.padding = '0px';
        searchBox.style.width = '48px';
        isOpen = false;
    }

    if (isSmallHeaderSearchBoxOpen == false) {
         SmallHeaderInputBox.style.padding = '6px 12px';
         SmallHeaderSearchBox.style.width = '262.41px';
        isSmallHeaderSearchBoxOpen = true;
    } else {
         SmallHeaderInputBox.style.padding = '0px';
         SmallHeaderSearchBox.style.width = '48px';
        isSmallHeaderSearchBoxOpen = false;
    }

$(document).mouseup(function () {
    if (isOpen == true) {
        SearchButton.click();
    }
    if (isSmallHeaderSearchBoxOpen == true) {
        SmallHeaderSearchButton.click();
    }
});

}

