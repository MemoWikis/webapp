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
    private _authorName: string;

    constructor() {
        this._follower = $(".follower");
        this._isFollow = $("#isFollow");


        if (typeof this._isFollow.val() !== "undefined") {
            this.loadCorrektClassAndTooltip();
            this._follower.css('cursor', 'pointer');
            this._authorName = $("#author").attr("name");
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
            this.InDecrementDisplayFollower(false); 

            this._isFollow.val("False");
            $("#follow-tooltip").attr("data-original-title",
                "Folge " + this._authorName + ", um an ihren/seinen Aktivitäten teilzuhaben.");

            if (this._follower.hasClass("fa-user-minus"))
                this._follower.addClass("fa-user-plus").removeClass("fa-user-times");
            else
                this._follower.addClass("fa-user-plus");

            $.post("/Users/UnFollow/", { "userId": this._authorId }, () => {});
        } else {
            this._isFollow.val("True");
            $("#follow-tooltip").attr("data-original-title",
                "Du folgst " + this._authorName + " und nimmst an ihren/seinen Aktivitäten teil.");

              this.InDecrementDisplayFollower(true);
           
            if (this._follower.hasClass("fa-user-plus"))
                this._follower.addClass("fa-user-minus").removeClass("fa-user-plus");
            else
                this._follower.addClass("fa-user-minus");

            $.post("/Users/Follow/", { "userId": this._authorId }, () => {});  
        }
    }

    private loadCorrektClassAndTooltip() {
        if (this._isFollow.val().toLowerCase() === "true") {
            this._follower.addClass("fa-user-minus");
        } else
            this._follower.addClass("fa-user-plus");
    }

    private InDecrementDisplayFollower(increment:boolean ):void {
        var counter: number = increment ? + 1 : - 1; 
        var followercount = parseInt($("#FollowerCount").text()) + counter;
        $("#FollowerCount").text(followercount.toString()); 
    }
    
}