class Menu {

    constructor() {
        $(".list-group-item").hover(
                function () {
                    $(this).find(".show-on-hover").show(150);
                },
                function () {
                    $(this).find(".show-on-hover").hide(150);
                }
            );
    }
}

class MenuMobile {

    private _isOpen: boolean = false;
    private _masterHeaderContainermarginRight: string; 



    constructor() {
        this._masterHeaderContainermarginRight = (parseInt($("#MasterHeaderContainer").css("margin-right"), 10) + parseInt($("#MasterHeaderContainer").css("padding-right"), 10) + 5).toString();

        $(window).resize(() => {
            this._masterHeaderContainermarginRight = (parseInt($("#MasterHeaderContainer").css("margin-right"), 10) + parseInt($("#MasterHeaderContainer").css("padding-right"), 10) + 5).toString();
            this.calculateMarginRightToElement(this._masterHeaderContainermarginRight, $("#RightMainMenu"));
        });


        $("#MenuButton").click(() => {
            if (this._isOpen) {
                this.closeMenu(); 
               
            } else {
                this.openMenu();
                this.calculateMarginRightToElement(this._masterHeaderContainermarginRight, $("#RightMainMenu"));


            }
        });

        $("#StickyMenuButton").click(() => {
            if (this._isOpen) {
                this.closeMenu();

            } else {
                this.openMenu();
                this.calculateMarginRightToElement(this._masterHeaderContainermarginRight, $("#RightMainMenu"));
            }
        });

        //close on click outside the menu
        $(document).mouseup((e) => {
            if (!this._isOpen) {
                return;
            }

            if ($("#mainMenu, #RightMainMenu").has(e.target).length === 0 &&
                $("#MenuButton, #StickyMenuButton").has(e.target).length === 0) {
                this.closeMenu();
            }           
            
        });


        //close on ESC
        $(document).keyup((e: any) => {
            if (!this._isOpen) {
                return;
            }

            if (e.keyCode == 27) {
                this.closeMenu();
            }
        });
    }

    openMenu() {

        $("#mainMenu, #RightMainMenu").show();
        this._isOpen = true;  
    }

    closeMenu() {

        $("#mainMenu, #RightMainMenu").hide();
        this._isOpen = false;

    }

    private calculateMarginRightToElement(marginRight: string, elementThatGetsTheMargin:JQuery ) {
        elementThatGetsTheMargin.css("margin-right", marginRight + "px" );
    }
}

$(function() {
    new MenuMobile();
    new Menu();
});
