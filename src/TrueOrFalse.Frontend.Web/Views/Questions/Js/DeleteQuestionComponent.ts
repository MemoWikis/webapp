Vue.component('delete-question-component',
    {
        data() {
            return {
                id: 0,
                name: '',
                errorMsg: '',
                showErrorMsg: false,
                showDeleteBtn: false,
                showDeleteInfo: true,
                deletionInProgress: false,
            }
        },
        template: '#delete-question-modal',
        mounted() {
            eventBus.$on('delete-question', (id) => {
                this.id = id;
                this.getDeleteDetails();
            });
            $('#modalDeleteQuestion').on('hidden.bs.modal',
                () => this.resetData());
        },
        beforeDestroy() {
            eventBus.$off('delete-question');
        },
        methods: {
            resetData() {
                Object.assign(this.$data, this.$options.data.apply(this));
            },
            getDeleteDetails() {
                var self = this;
                $.ajax({
                    type: 'POST',
                    url: "/Question/DeleteDetails/" + self.id,
                    success(result) {
                        self.name = result.questionTitle;
                        if (result.canNotBeDeleted) {
                            if (result.wuwiCount > 0) {
                                self.errorMsg = messages.error.question.isInWuwi(result.wuwiCount);
                            } else
                                self.errorMsg = messages.error.question.rights,

                            self.showDeleteInfo = false;
                            self.showErrorMsg = true;
                        } else 
                            self.showDeleteBtn = true;

                        $('#modalDeleteQuestion').modal('show');
                    },
                    error() {
                        Alerts.showError({
                            text: self.errorMsg
                        });
                    }
                });
            },

            deleteQuestion() {
                this.deletionInProgress = true;
                this.showDeleteBtn = false;
                Utils.ShowSpinner();

                var self = this;
                var data = {
                    questionId: self.id,
                    sessionIndex: $('#hddIsLearningSession').attr('data-current-step-idx')
                }
                $.ajax({
                    type: 'POST',
                    url: "/Question/Delete",
                    contentType: "application/json",
                    data: JSON.stringify(data),
                    success(result) {
                        Utils.HideSpinner();
                        self.deletionInProgress = false;
                        eventBus.$emit('question-deleted', { id: result.questionId, index: result.sessionIndex - 1});
                        Alerts.showSuccess({
                            text: messages.success.question.delete
                        });
                        $('#modalDeleteQuestion').modal('hide');
                    },
                    error() {
                        Utils.HideSpinner();
                        self.deletionInProgress = false;
                        self.showDeleteBtn = false;
                        self.showErrorMsg = messages.error.question.errorOnDelete;
                    }
                });
            }
        }
    })