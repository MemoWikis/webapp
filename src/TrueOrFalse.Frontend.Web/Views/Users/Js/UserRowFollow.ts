 class UserRowFollow
 {
    constructor() {
        $("[data-type=btn-follow]").click(function() {
            var $this = $(this);

            var userId = UserRow.GetUserId($this);

            $this.parent().find("[data-type=btnFollowSpinner]");
            $this.hide();

            $.post("/Users/Follow/", { "userId" : userId }, () => {
                $this.parent().find("[data-type=btn-unfollow]").show();
                $("#btnFollowSpinner").hide();
            });
        });

        $("[data-type=btn-unfollow]").click(function () {
            var $this = $(this);

            var userId = UserRow.GetUserId($this);

            $this.parent().find("[data-type=btnFollowSpinner]");
            $this.hide();

            $.post("/Users/UnFollow/", { "userId": userId }, () => {
                $this.parent().find("[data-type=btn-follow]").show();
                $("#btnFollowSpinner").hide();
            });
        });
    }
 }