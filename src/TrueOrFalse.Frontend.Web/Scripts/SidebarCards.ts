$(function() {
    $("#AllAutorsContainer").on('click', function () {  
        $("#ExtendAngle").toggleClass("rotate");
        if ($("#ExtendAngle").hasClass("rotate")) {
            $("#AllAutorsContainer").css('color', '#979797');
            $("#AllAutorsList").slideDown(400);

        } else {
            $("#AllAutorsContainer").css('color', '#0560AB');
            $("#AllAutorsList").slideUp(400);
        }
    });
    
        new Follower();
});


class Follower {
    private _follower: JQuery;
    private _isFollow: JQuery; 
    private _authorId: number; 

    constructor() {

        this._follower = $(".follower");
        this._isFollow = $("#isFollow");

        if (typeof $("#isFollow").val() !== "undefined") {
            this.loadCorrektClass();
            this._follower.css('cursor', 'pointer');
        }

        if (IsLoggedIn.Yes) {
            $(".follower").on("click",
                () => {
                    this._authorId = parseInt($("#author").val());
                    this.toggleClasses();
                });
        }
    }

    private toggleClasses(): void {
        if (this._isFollow.val().toLowerCase() === "true") {

            if (this._follower.hasClass("fa-user-minus"))
                this._follower.addClass("fa-user-plus").removeClass("fa-user-times");
            else
                this._follower.addClass("fa-user-plus");

            $.post("/Users/UnFollow/", { "userId": this._authorId }, () => {
                this._isFollow.val("False");
            });
        } else {
            if (this._follower.hasClass("fa-user-plus"))
                this._follower.addClass("fa-user-minus").removeClass("fa-user-plus");
            else
                this._follower.addClass("fa-user-minus");

            $.post("/Users/Follow/", { "userId": this._authorId }, () => {
                this._isFollow.val("True");
            });  
        }
    }

    private loadCorrektClass() {
        if (this._isFollow.val().toLowerCase() === "true") 
            this._follower.addClass("fa-user-minus");
        else
            this._follower.addClass("fa-user-plus");        
    }
    
}