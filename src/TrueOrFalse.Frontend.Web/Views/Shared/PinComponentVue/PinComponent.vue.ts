Vue.component('pin-wuwi-component',
    {
        props: {
            isInWishknowledge: Boolean,
            questionId: Number
        },
        template: '#pin-wuwi-template',

        data() {
            return {
                stateLoad: 'loading',
                showAddTxt: false,
                type: 'default'
            }
        },
        watch: {
            isInWishknowledge: function(val) {
                if (val)
                    this.stateLoad = 'added';
                else
                    this.stateLoad = "";
            }
        },
        mounted() {
            if (this.isInWishknowledge)
                this.stateLoad = 'added';
            else
                this.stateLoad = "";
        },
        methods: {
            PinUnpin: function () {

                class data {
                    questionId; 
                    isInWishknowledge;
                }

                var helper = new data();
                helper.questionId = this.questionId;
                helper.isInWishknowledge = !this.isInWishknowledge;

                if ($("#LearningTabWithOptions").hasClass("active")) {
                            this._pinRowType = PinType.Question;

                            if (this.questionId == $("#questionId").val()) {

                                if (this.isInWishknowledge) {
                                    $(".AnswerQuestionBodyMenu .iAdded").click();
                                } else {
                                    $(".AnswerQuestionBodyMenu .iAddedNot").click();
                                }
                            } else {

                                if (this.isInWishknowledge) {
                                    $.post("/Api/Questions/UnPin/", { questionId: this.questionId });
                                    eventBus.$emit("reload-wishknowledge-state-per-question",helper);
                                } else {
                                    $.post("/Api/Questions/Pin/", { questionId: this.questionId });
                                    eventBus.$emit("reload-wishknowledge-state-per-question", helper);
                                }
                            }
                        }
            },

            animatePin() {
            }
        }
    });