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


    constructor() {
        
        $("#MenuButton").click(() => {
            if (this._isOpen) {
                this.closeMenu(); 
               
            } else {
                this.openMenu();
                
            }
        });

        $("#StickyMenuButton").click(() => {
            if (this._isOpen) {
                this.closeMenu();

            } else {
                this.openMenu();

            }
        });

        //close on click outside the menu
        $(document).mouseup((e) => {
            if (!this._isOpen) {
                return;
            }

            if ($("#mainMenu, #RightMainMenu").has(e.target).length === 0 &&
                $("#MenuButton").has(e.target).length === 0) {
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

    
}

$(function() {
    new MenuMobile();
    new Menu();
});