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
            }
        },
        created() {
            $.get("/Category/GetMyWorldCookie/", (showMyWorld) => {
                this.showMyWorld = showMyWorld == "True";
            });
        },
        methods: {
            toggleMyWorld() {
                var s = this.showMyWorld;
                $.post(`/Category/SetMyWorldCookie/?showMyWorld=${s}`);
            }
        }
    });
