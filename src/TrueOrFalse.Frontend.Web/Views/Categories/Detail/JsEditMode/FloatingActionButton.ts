
var FAB = Vue.component('floating-action-button',
    {
        props: ['tab'],
        data() {
            return {
                editMode: false,
                isOpen: false,
                showMiniFAB: false,
                isTopicTab: true,
                footerIsVisible: false,
        }
        },
        created() {
            window.addEventListener('scroll', this.handleScroll);
            window.addEventListener('resize', this.footerCheck);
        },
        updated() {
            this.footerCheck();
        },
        destroyed() {
            window.removeEventListener('scroll', this.handleScroll);
        },
        methods: {
            toggleFAB() {
                this.showMiniFAB = true;
                this.isOpen = !this.isOpen;
                if (this.editMode)
                    this.editCategoryContent();

            },
            editCategoryContent() {
                this.editMode = !this.editMode;
                eventBus.$emit('set-edit-mode', this.editMode);
                this.isOpen = false;

            },
            handleScroll() {
                this.scroll = true;
                this.footerCheck();
            },
            footerCheck() {
                const elFooter = document.getElementById('CategoryFooter');

                if (elFooter) {
                    var rect = elFooter.getBoundingClientRect();
                    var viewHeight = Math.max(document.documentElement.clientHeight, window.innerHeight);
                    if (rect.top - viewHeight >= 0 || rect.bottom < 0)
                        this.footerIsVisible = false;
                    else
                        this.footerIsVisible = true;
                };
            },

            saveContent() {

            },
            cancelEditMode() {
                this.editMode = false;
                this.isOpen = true;
            }
        }
    });

var FABContainer = new Vue({
    el: '#FloatingActionButtonApp',

})