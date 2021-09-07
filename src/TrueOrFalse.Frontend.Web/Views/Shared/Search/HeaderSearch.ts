new Vue({
    el: '#HeaderSearch',
    data() {
        return {
            showSearch: false,
            searchType: SearchType.All,
            windowWidth: 0,
            isLoggedIn: IsLoggedIn.Yes
        }
    },
    created() {
        this.windowWidth = window.innerWidth;
        if (this.isLoggedIn && this.windowWidth > 750)
            this.showSearch = true;
        else if (!this.isLoggedIn && this.windowWidth > 992)
            this.showSearch = true;
    },
    watch: {
        showSearch(val) {
            let logoContainer = $('#LogoContainer');
            let headerBodyContainer = $('#HeaderBodyContainer');
            let loginAndHelp = $('#loginAndHelp');

            if (val && (this.windowWidth <= 750 || (this.windowWidth <= 992 && !this.isLoggedIn))) {
                logoContainer.addClass('hidden-xs');
                headerBodyContainer.removeClass('col-xs-10');
                headerBodyContainer.addClass('col-xs-12');
                loginAndHelp.addClass('hidden-xs');
            } else if (!val && (this.windowWidth <= 750 || (this.windowWidth <= 992 && !this.isLoggedIn))) {
                logoContainer.removeClass('hidden-xs');
                headerBodyContainer.addClass('col-xs-10');
                headerBodyContainer.removeClass('col-xs-12');
                loginAndHelp.removeClass('hidden-xs');
            }
        }
    },
    methods: {
        openUrl(val) {
            location.href = val.Url;
        }
    }
})