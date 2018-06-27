class _Dashboard {
    constructor() {
        $(".third-cell").on("click",
            "fa-trash-o",
            () => {
                if (this.getCountDates(445) === 0) {

                }
            });
    }

    private getCountDates(userId): number {
        return 0;

    }
}