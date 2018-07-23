﻿var isOpen = false;
var isSmallHeaderSearchBoxOpen = false;

function SearchButtonClick() {

    var SearchButton = document.getElementById('StickySearchButton');
    var inputBox = document.getElementById('StickyHeaderSearchBox');
    var searchBox = document.getElementById('StickyHeaderSearchBoxDiv');

    var SmallHeaderSearchButton = document.getElementById('SmallHeaderSearchButton');
    var SmallHeaderInputBox = document.getElementById('SmallHeaderSearchBox');
    var SmallHeaderSearchBox = document.getElementById('SmallHeaderSearchBoxDiv');


    if (isOpen == false) {
        searchBox.classList.add("SearchBoxDivMaxWidth");
        inputBox.style.padding = '6px 12px';
        SearchButton.style.border = '#979797 1px solid';
        SearchButton.style.background = '#ebebeb';
        inputBox.style.border = '1px #979797 solid';
        isOpen = true;
    } else {
        searchBox.classList.remove("SearchBoxDivMaxWidth");
        inputBox.style.padding = '0px';
        SearchButton.style.border = 'none';
        SearchButton.style.background = 'none';
        inputBox.style.border = 'none';
        isOpen = false;
    }

    if (isSmallHeaderSearchBoxOpen == false) {
        SmallHeaderSearchBox.classList.add("SearchBoxDivMaxWidth");
        SmallHeaderInputBox.style.padding = '6px 12px';
        SmallHeaderInputBox.style.border = '1px #979797 solid';
        isSmallHeaderSearchBoxOpen = true;
        SmallHeaderSearchButton.style.background = '#ebebeb';     
        SmallHeaderSearchButton.style.border = '#979797 1px solid';

    } else {
        SmallHeaderSearchBox.classList.remove("SearchBoxDivMaxWidth");
        SmallHeaderInputBox.style.padding = '0px';
        isSmallHeaderSearchBoxOpen = false;
    }

    $(document).mouseup((e) => {
        if ($("#StickyHeaderSearchBox, #StickyHeaderSearchBoxDiv").has(e.target).length === 0 &&
            $("#StickySearchButton").has(e.target).length === 0) {
            if (isOpen == true) {
                searchBox.classList.remove("SearchBoxDivMaxWidth");
                inputBox.style.padding = '0px';
                SearchButton.style.border = 'none';
                SearchButton.style.background = 'none';
                inputBox.style.border = 'none';
                isOpen = false;
            }
        }
        if ($("#SmallHeaderSearchBox, #SmallHeaderSearchBoxDiv").has(e.target).length === 0 &&
            $("#SmallHeaderSearchButton").has(e.target).length === 0) {
                if (isSmallHeaderSearchBoxOpen == true) {
                    SmallHeaderSearchBox.classList.remove("SearchBoxDivMaxWidth");
                    SmallHeaderInputBox.style.padding = '0px';
                    isSmallHeaderSearchBoxOpen = false;
                }
        }
    });

}

