declare var eventBus: any;
if (eventBus == null)
    var eventBus = new Vue();

Vue.component('my-world-toggle-component',
    {
        data() {
            return {
                showMyWorld: false,
                editMode: false,
                disabled: false,
            }
        },
        created() {

        },
        mounted() {
            this.loadCookie();
            eventBus.$on('set-edit-mode', (state) => {
                this.editMode = state;
            });
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
                if (this.editMode)
                    return;
                Utils.ShowSpinner();
                this.showMyWorld = !this.showMyWorld;
                var s = this.showMyWorld;

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
