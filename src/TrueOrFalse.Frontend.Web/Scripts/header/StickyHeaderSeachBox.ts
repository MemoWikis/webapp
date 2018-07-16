var isOpen = false;
var isSmallHeaderSearchBoxOpen = false;

function SearchButtonClick() {

    var SearchButton = document.getElementById('StickySearchButton');
    var inputBox = document.getElementById('StickyHeaderSearchBox');
    var searchBox = document.getElementById('StickyHeaderSearchBoxDiv');

    var SmallHeaderSearchButton = document.getElementById('SmallHeaderSearchButton');
    var SmallHeaderInputBox = document.getElementById('SmallHeaderSearchBox');
    var SmallHeaderSearchBox = document.getElementById('SmallHeaderSearchBoxDiv');


    if (isOpen == false) {
        searchBox.style.width = '262.41px';
        inputBox.style.padding = '6px 12px';
        SearchButton.style.border = '#979797 1px solid';
        SearchButton.style.background = '#ebebeb';
        inputBox.style.border = '1px #979797 solid';
        isOpen = true;
    } else {
        searchBox.style.width = '47px';
        inputBox.style.padding = '0px';
        SearchButton.style.border = 'none';
        SearchButton.style.background = 'none';
        inputBox.style.border = 'none';
        isOpen = false;
    }

    if (isSmallHeaderSearchBoxOpen == false) {
         SmallHeaderSearchBox.style.width = '262.41px';
         SmallHeaderInputBox.style.padding = '6px 12px';
         SmallHeaderSearchButton.style.border = '#979797 1px solid';
         SmallHeaderSearchButton.style.background = '#ebebeb';
         SmallHeaderInputBox.style.border = '1px #979797 solid';
         isSmallHeaderSearchBoxOpen = true;

    } else {
         SmallHeaderSearchBox.style.width = '47px';
         SmallHeaderInputBox.style.padding = '0px';
         isSmallHeaderSearchBoxOpen = false;
    }

    $(document).mouseup((e) => {
        if ($("#StickyHeaderSearchBox, #StickyHeaderSearchBoxDiv").has(e.target).length === 0 &&
            $("#StickySearchButton").has(e.target).length === 0) {
            if (isOpen == true) {
                searchBox.style.width = '47px';
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
                    SmallHeaderSearchBox.style.width = '47px';
                    SmallHeaderInputBox.style.padding = '0px';
                    isSmallHeaderSearchBoxOpen = false;
                }
        }
    });

}

