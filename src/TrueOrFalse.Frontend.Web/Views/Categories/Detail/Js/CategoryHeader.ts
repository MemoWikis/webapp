declare var d3;

interface ViewsPerDay {
    Date: string;
    Views: number;
}

class CategoryHeader {

    private _page: CategoryPage;
    private _isLoaded: boolean;

    constructor(page: CategoryPage) {

        $("#SessionConfigReminderHeader>.fa-times-circle").on('click',
            () => {
                $.post("/Category/SetSettingsCookie?name=ShowSessionConfigurationMessageTab");
                $("#SessionConfigReminderHeader").hide(200);
            });
    }
}