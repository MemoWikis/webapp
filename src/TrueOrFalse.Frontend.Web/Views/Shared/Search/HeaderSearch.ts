new Vue({
    el: '#HeaderSearch',
    data() {
        return {
            showSearch: false,
            searchType: SearchType.All,
            isLoggedIn: IsLoggedIn.Yes,
            lastWidth: 0,
        }
    },
    mounted() {
        this.init();
        this.lastWidth = this.$el.clientWidth;
        var self = this;
        window.addEventListener("resize", self.init);
    },
    destroyed() {
        var self = this;
        window.removeEventListener("resize", self.init);
    },
    watch: {
        showSearch(val) {
            let logoContainer = $('#LogoContainer');
            let headerBodyContainer = $('#HeaderBodyContainer');
            let loginAndHelp = $('#loginAndHelp');

            if (val && (window.innerWidth <= 750 || (window.innerWidth <= 992 && !this.isLoggedIn))) {
                logoContainer.addClass('hidden-xs');
                headerBodyContainer.removeClass('col-xs-10');
                headerBodyContainer.addClass('col-xs-12');
                loginAndHelp.addClass('hidden-xs');
            } else if (!val && (window.innerWidth <= 750 || (window.innerWidth <= 992 && !this.isLoggedIn))) {
                logoContainer.removeClass('hidden-xs');
                headerBodyContainer.addClass('col-xs-10');
                headerBodyContainer.removeClass('col-xs-12');
                loginAndHelp.removeClass('hidden-xs');
            }
        }
    },
    methods: {
        init() {
            if (this.lastWidth == this.$el.clientWidth)
                return;
            else this.lastWidth = this.$el.clientWidth;
            if (this.isLoggedIn && window.innerWidth > 750)
                this.showSearch = true;
            else if (!this.isLoggedIn && window.innerWidth > 992)
                this.showSearch = true;
            else this.showSearch = false;
        },
        openUrl(val) {
            location.href = val.Url;
        }
    }
})