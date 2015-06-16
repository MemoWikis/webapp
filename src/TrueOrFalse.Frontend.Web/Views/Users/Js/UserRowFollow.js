var UserRowFollow = (function () {
    function UserRowFollow() {
        $("[data-type=btn-follow]").click(function () {
            if (NotLoggedIn.Yes()) {
                NotLoggedIn.ShowErrorMsg();
                return;
            }

            var $this = $(this);

            var userId = UserRow.GetUserId($this);
            var parentSpinner = $this.parent().find("[data-type=btnFollowSpinner]");

            $this.hide();
            parentSpinner.show();

            $.post("/Users/Follow/", { "userId": userId }, function () {
                parentSpinner.hide();
                $this.parent().find("[data-type=btn-unfollow]").show();

                UserRowFollow.UiIncreaseFollowerCount();
            });
        });

        $("[data-type=btn-unfollow]").click(function () {
            var $this = $(this);

            var userId = UserRow.GetUserId($this);
            var parentSpinner = $this.parent().find("[data-type=btnFollowSpinner]");

            $this.hide();
            parentSpinner.show();

            $.post("/Users/UnFollow/", { "userId": userId }, function () {
                parentSpinner.hide();
                $this.parent().find("[data-type=btn-follow]").show();

                UserRowFollow.UiDecreaseFollowerCount();
            });
        });
    }
    UserRowFollow.UiIncreaseFollowerCount = function () {
        Utils.SetElementValue(".JS-AmountFollowers", (UserRowFollow.CurrentAmountFollowers() + 1).toString());
    };

    UserRowFollow.UiDecreaseFollowerCount = function () {
        Utils.SetElementValue(".JS-AmountFollowers", (UserRowFollow.CurrentAmountFollowers() - 1).toString());
    };

    UserRowFollow.CurrentAmountFollowers = function () {
        return parseInt($($(".JS-AmountFollowers")[0]).html());
    };
    return UserRowFollow;
})();
//# sourceMappingURL=UserRowFollow.js.map
