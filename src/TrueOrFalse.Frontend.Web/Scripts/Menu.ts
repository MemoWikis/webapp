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
    private _isInProgress: boolean = false;


    constructor() {
        
        $("#MenuButton").click(() => {
            if (this._isOpen) {
                this.closeMenu(); 
               
            } else {
                this.openMenu();
                
            }
        });

        $("#MobileMenuButton").click(() => {
            if (this._isOpen) {
                this.closeMenu();

            } else {
                this.openMenu();

            }
        });
      

        //close on click outside the menu
        $(document).mouseup((e) => {
            if (!this._isOpen && !this._isInProgress) {
                return;
            }

            if ($("#mainMenu, #LongMenu").has(e.target).length === 0 &&
                $("#MenuButton").has(e.target).length === 0) {
                this.closeMenu();
            }

            if ($("#mainMenu, #LongMenu").has(e.target).length === 0 &&
                $("#MobileMenuButton").has(e.target).length === 0) {
                this.closeMenu();
            }
           
            
        });


        //close on ESC
        $(document).keyup((e: any) => {
            if (!this._isOpen && !this._isInProgress) {
                return;
            }

            if (e.keyCode == 27) {
                this.closeMenu();
            }
        });
    }

    openMenu() {

        if (this._isInProgress) {
            return;
        }
            
        this._isInProgress = true;
        $("#mainMenu, #LongMenu").slideDown(400, () => {
            this._isOpen = true;
            this._isInProgress = false;
        });
        
    }

    closeMenu() {

        if (this._isInProgress) {
            return;
        }

        this._isInProgress = true;
        $("#mainMenu, #LongMenu").slideUp(400, () => {
            this._isOpen = false;
            this._isInProgress = false;    
        });
        
    }

    
}

$(function() {
    new MenuMobile();
    new Menu();
});