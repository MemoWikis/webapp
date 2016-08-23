class UserRowFollow
{
    constructor() {

        $("[data-type=btn-follow]").click(function() {
            if (NotLoggedIn.Yes()) {
                NotLoggedIn.ShowErrorMsg(); return;
            }

            var $this = $(this);

            var userId = UserRow.GetUserId($this);
            var parentSpinner = $this.parent().find("[data-type=btnFollowSpinner]");

            $this.hide();
            parentSpinner.show();

            $.post("/Users/Follow/", { "userId": userId }, () => {
                parentSpinner.hide();
                $this.parent().find("[data-type=btn-unfollow]").show();

                UserRowFollow.UiIncreaseFollowerCount(); //only necessary when called from UserRow.ascx (and not from User.aspx)
            });
        });

        $("[data-type=btn-unfollow]").click(function () {
            var $this = $(this);

            var userId = UserRow.GetUserId($this);
            var parentSpinner = $this.parent().find("[data-type=btnFollowSpinner]");

            $this.hide();
            parentSpinner.show();

            $.post("/Users/UnFollow/", { "userId": userId }, () => {
                parentSpinner.hide();
                $this.parent().find("[data-type=btn-follow]").show();

                UserRowFollow.UiDecreaseFollowerCount(); //only necessary when called from UserRow.ascx (and not from User.aspx)
            });
        });
    }

     static UiIncreaseFollowerCount() {
        Utils.SetElementValue(".JS-AmountFollowers", (UserRowFollow.CurrentAmountFollowers() + 1).toString());
    }

    static UiDecreaseFollowerCount() {
        Utils.SetElementValue(".JS-AmountFollowers", (UserRowFollow.CurrentAmountFollowers() - 1).toString());
    }

    static CurrentAmountFollowers() : number {
        return parseInt($($(".JS-AmountFollowers")[0]).html());
    }

 }