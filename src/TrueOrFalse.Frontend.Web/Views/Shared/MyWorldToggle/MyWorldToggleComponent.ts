Vue.component('my-world-toggle-component',
    {
        data() {
            return {
                showMyWorld: false,
            }
        },
        watch: {
            showMyWorld() {
                this.toggleMyWorld();
                this.sendShowMyWorld();
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
                var s = this.showMyWorld;
                $.post(`/Category/SetMyWorldCookie/?showMyWorld=${s}`, () => this.loadCookie());
            },
            sendShowMyWorld() {
                $.post("/User/SetUserWorldInUserCache",
                    { showMyWorld: this.showMyWorld });
                location.reload();
            }
        }
    });
