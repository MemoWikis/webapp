var isStickyHeaderSearchBoxOpen = false;
var isSmallHeaderSearchBoxOpen = false;

function SearchButtonClick() {

    var searchButton = document.getElementById('StickyHeaderSearchButton');
    var inputBox = document.getElementById('StickyHeaderSearchBox');
    var searchBox = document.getElementById('StickyHeaderSearchBoxDiv');

    var smallHeaderInputBox = document.getElementById('SmallHeaderSearchBox');
    var smallHeaderSearchBox = document.getElementById('SmallHeaderSearchBoxDiv');


    if (!isSmallHeaderSearchBoxOpen && !isStickyHeaderSearchBoxOpen) {
        OpenSmallHeaderSearchBox();
        OpenStickyHeaderSearchBox();
    } else {
        CloseSmallHeaderSearchBox();
        CloseStickyHeaderSearchBox();
    }

    function OpenStickyHeaderSearchBox() {
            searchBox.classList.add("SearchBoxDivMaxWidth");
            inputBox.style.padding = '6px 12px';
            searchButton.style.border = '#979797 1px solid';
            searchButton.style.background = '#ebebeb';
            inputBox.style.border = '1px #979797 solid';
            document.getElementById('KnowledgeImage').style.display = "none";
            document.getElementById('BreadcrumbUserDropdownImage').style.display = "none";          
            isStickyHeaderSearchBoxOpen = true;

           
    } 

    function CloseStickyHeaderSearchBox() {
            searchBox.classList.remove("SearchBoxDivMaxWidth");
            inputBox.style.padding = '0px';
            searchButton.style.border = "none";
            searchButton.style.background = "none";
            inputBox.style.border = "none";
            document.getElementById('KnowledgeImage').style.display = 'block';
            document.getElementById('BreadcrumbUserDropdownImage').style.display = 'block';
            isStickyHeaderSearchBoxOpen = false;
    }

    function OpenSmallHeaderSearchBox() {
        smallHeaderInputBox.style.borderColor = 'rgb(151, 151, 151)';
        smallHeaderInputBox.style.backgroundColor = '#fff';
        smallHeaderSearchBox.classList.add("SearchBoxDivMaxWidth");
        smallHeaderInputBox.style.padding = '6px 12px';
        smallHeaderInputBox.style.border = '1px #979797 solid';
        smallHeaderInputBox.style.display = 'block';
        isSmallHeaderSearchBoxOpen = true;
    }

    function CloseSmallHeaderSearchBox() {
            smallHeaderSearchBox.classList.remove("SearchBoxDivMaxWidth");
            smallHeaderInputBox.style.padding = '0px';
            isSmallHeaderSearchBoxOpen = false;
            smallHeaderInputBox.style.borderColor = '#003264';
            smallHeaderInputBox.style.backgroundColor = 'transparent';       
    }


    $(document).mouseup((e) => {
        if ($("#SmallHeaderSearchBox, #SmallHeaderSearchBoxDiv").has(e.target).length === 0 &&
            $("#StickyHeaderSearchBox, #StickyHeaderSearchBoxDiv").has(e.target).length === 0 &&
            $("#SmallHeaderSearchButton").has(e.target).length === 0 &&
            $("#StickyHeaderSearchButton").has(e.target).length === 0) {

            if (isSmallHeaderSearchBoxOpen && isStickyHeaderSearchBoxOpen) {
                CloseSmallHeaderSearchBox();
                CloseStickyHeaderSearchBox();
            }
        }
    });
}