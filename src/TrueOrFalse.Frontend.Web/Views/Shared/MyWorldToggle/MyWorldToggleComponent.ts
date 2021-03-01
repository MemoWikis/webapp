declare var eventBus: any;
if (eventBus == null)
    var eventBus = new Vue();

Vue.component('my-world-toggle-component',
    {
        data() {
            return {
                showMyWorld: false,
                disabled: false,
            }
        },
        created() {

        },
        mounted() {
            this.loadCookie();
        },
        watch: {
            showMyWorld(val) {
                this.$root.showMyWorld = val;
            }
        },
        methods: {
            loadCookie() {
                $.get("/Category/GetMyWorldCookie/", (showMyWorld) => {
                    this.showMyWorld = showMyWorld == "True";
                    this.$root.showMyWorld = this.showMyWorld;
                });
            },
            toggleMyWorld() {
                console.log('test');
                Utils.ShowSpinner();
                this.showMyWorld = !this.showMyWorld;
                var s = this.showMyWorld;

                if(IsLoggedIn.Yes)
                    $.post(`/Category/SetMyWorldCookie/?showMyWorld=${s}`).done(() => {
                        location.reload();
                    });
            },
            sendShowMyWorld() {
                $.post("/User/SetUserWorldInUserCache",
                    { showMyWorld: this.showMyWorld });
            }
        }
    });
