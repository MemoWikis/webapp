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

            $.post("/Users/Follow/", { "userId" : userId }, () => {
                $this.parent().find("[data-type=btn-unfollow]").show();
                parentSpinner.hide();
            });
        });

        $("[data-type=btn-unfollow]").click(function () {
            var $this = $(this);

            var userId = UserRow.GetUserId($this);
            var parentSpinner = $this.parent().find("[data-type=btnFollowSpinner]");

            $this.hide();
            parentSpinner.show();

            $.post("/Users/UnFollow/", { "userId": userId }, () => {
                $this.parent().find("[data-type=btn-follow]").show();
                parentSpinner.hide();
            });
        });
    }
 }