﻿declare var eventBus: any;
if (eventBus == null)
    var eventBus = new Vue();

var defaultModalComponent = Vue.component('default-modal-component',
    {
        template: '#default-modal-component',
        props: [
            'id', 'showCloseButton', 'adminContent', 'modalType', 'iconClasses', 'button1Text', 'button2Text',
            'action1Emit', 'action2Emit', 'modalWidth', 'isFullSizeButtons'
        ],
        data: function() {
            return {
                isError: false,
                isSuccess: false,
                isAdminContent: this.adminContent == "true",
                modalWidthData: this.modalWidth + 'px',
            }
        },
        created() {
            var self = this;
            if (self.modalWidth == null) {
                self.modalWidthData = '50%';
            }
            document.body.classList.add('no-scroll');
            if (self.modalType == 'error') {
                self.isError = true;
            }
            if (self.modalType == 'success') {
                self.isSuccess = true;
            }
        },
        mounted() {
            this.resize();

            $(window).resize(() => {
                this.resize();
            });
        },
        methods: {
            resize() {
                var windowWidth = $(window).width();
                this.modalWidthData = windowWidth < this.modalWidth ? windowWidth + 'px' : this.modalWidth + 'px';
            },
            closeModal() {
                document.body.classList.remove('no-scroll');
                eventBus.$emit('close-modal');
            },

            action1() {
                var self = this;
                eventBus.$emit(self.action1Emit);
            },
            action2() {
                var self = this;
                if (self.action2Emit == "" || self.action2Emit == null) {
                    self.closeModal();
                    return;
                }
                eventBus.$emit(self.action2Emit);
            },
        }
    })