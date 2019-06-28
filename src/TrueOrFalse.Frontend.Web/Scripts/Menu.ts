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
    private _isOpenUser: boolean = false;

    private _masterHeaderContainermarginRight: string;
    private _breadcrumbUserDropdownMarginRight: number; 



    constructor() {
        this._masterHeaderContainermarginRight = (parseInt($("#MasterHeaderContainer").css("margin-right"), 10) + parseInt($("#MasterHeaderContainer").css("padding-right"), 10) + 5).toString();

        $(window).resize(() => {
            this._masterHeaderContainermarginRight = (parseInt($("#MasterHeaderContainer").css("margin-right"), 10) + parseInt($("#MasterHeaderContainer").css("padding-right"), 10) + 5).toString();
            this._breadcrumbUserDropdownMarginRight = parseInt($("#BreadCrumbContainer").css("margin-right"), 10) + $("#StickyMenuButton").outerHeight() + parseInt($("#BreadcrumbUserDropdownImage").css("margin-right"));
            this.calculateMarginRightToElement(this._masterHeaderContainermarginRight, $("#RightMainMenu"));
            this.calculateMarginRightToElement(this._breadcrumbUserDropdownMarginRight.toString(), $("#BreadcrumbUserDropdown"));
            $("#BreadcrumbUserDropdown").css("margin-right", this._breadcrumbUserDropdownMarginRight + "px");
        });

        $("#BreadCrumbTrail").on("mouseover",".path",  (event) => {
            $("#Path").css("display", "block");
            $(".path").on("mouseleave", () => {
                $("#Path").fadeOut(200);
            });
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

        $("#BreadcrumbUserDropdownImage").on("click",
            () => {
                this._breadcrumbUserDropdownMarginRight = parseInt($("#BreadCrumbContainer").css("margin-right"), 10) + $("#StickyMenuButton").outerHeight() + parseInt($("#BreadcrumbUserDropdownImage").css("margin-right")); // doesnt work in  constructor
                this.calculateMarginRightToElement(this._breadcrumbUserDropdownMarginRight.toString(), $("#BreadcrumbUserDropdown") );
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
    private test(element: boolean) {
        return element;
    }
    private calculateMarginRightToElement(marginRight: string, elementThatGetsTheMargin:JQuery ) {
        elementThatGetsTheMargin.css("margin-right", marginRight + "px" );
    }
}

$(() => {   
    new MenuMobile();
    new Menu();
    
});
