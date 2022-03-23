Vue.component('pin-wuwi-component',
    {
        props: {
            isInWishknowledge: Boolean,
            questionId: [Number, String],
            categoryId: [Number, String],
            isCategory: Boolean,
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
            isInWishknowledge(val) {
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

//enum PinState {
//    NotInWuwi,
//    Loading,
//    InWuwi
//}

//Vue.component('pin-component',
//    {
//        props: {
//            isInWishknowledge: Boolean,
//            questionId: [Number, String],
//            categoryId: [Number, String],
//            size: {
//                default: 14,
//                type: Number
//            }
//        },
//        template: '#pin-template',

//        data() {
//            return {
//                currentState: PinState.NotInWuwi,
//            }
//        },
//        mounted() {
//            eventBus.$on('set-pin',
//                (e) => {
//                    if (this.questionId= e.questionId || this.categoryId == e.categoryId)
//                        e.currState.InWuwi.state;
//                });

//            if (this.isInWishknowledge)
//                this.currentState = PinState.InWuwi;
//            else
//                this.currentState = PinState.NotInWuwi;
//        },
//        methods: {
//            addWuwi() {

//            },
//            removeWuwi() {

//            },

//            animatePin() {
//            }
//        }
//    });