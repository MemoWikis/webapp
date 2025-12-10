<script lang="ts" setup>
import { SolutionType } from '~/components/question/solutionTypeEnum'
import { useUserStore } from '~/components/user/userStore'
import { usePublishQuestionStore } from '~/components/question/edit/publish/publishQuestionStore'

interface Props {
    answerBodyModel: any
}

const props = defineProps<Props>()
const userStore = useUserStore()
const publishQuestionStore = usePublishQuestionStore()
</script>

<template>
    <div class="answerbody-header">
        <div class="answerbody-text">
            <h3 v-if="answerBodyModel.solutionType != SolutionType.Flashcard" class="QuestionText">
                {{ answerBodyModel.text }}
            </h3>
        </div>

        <div class="AnswerQuestionBodyMenu">
            <div class="answerbody-btn visibility" v-if="answerBodyModel.isPrivate">
                <div class="answerbody-btn-inner" @click="publishQuestionStore.openModal(answerBodyModel.id)">
                    <font-awesome-icon :icon="['fas', 'lock']" class="no-hover" />
                    <font-awesome-icon :icon="['fas', 'unlock']" class="hover" />
                </div>
            </div>
            <div class="Pin answerbody-btn" :data-question-id="answerBodyModel.id">
                <div class="answerbody-btn-inner">
                    <QuestionPin
                        :question-id="answerBodyModel.id"
                        :key="answerBodyModel.id"
                        :is-in-wish-knowledge="answerBodyModel.isInWishKnowledge" />
                </div>
            </div>
            <QuestionAnswerBodyOptions
                v-if="answerBodyModel"
                :id="answerBodyModel.id"
                :title="answerBodyModel.title"
                :can-edit="answerBodyModel.isCreator || userStore.isAdmin" />
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.answerbody-header {
    display: flex;
    flex-wrap: nowrap;
    justify-content: space-between;

    .answerbody-text {
        margin-right: 8px;

        h3 {
            margin-top: 0;
            margin-bottom: 34px;
        }
    }

    .answerbody-btn {
        font-size: 18px;

        .answerbody-btn-inner {
            cursor: pointer;
            background: white;
            height: 32px;
            width: 32px;
            display: flex;
            justify-content: center;
            align-items: center;
            border-radius: 15px;

            .hover {
                display: none;
            }

            .no-hover {
                display: block;
            }

            &:hover {
                filter: brightness(0.95);

                .hover {
                    display: block;
                }

                .no-hover {
                    display: none;
                }
            }

            &:active {
                filter: brightness(0.85);
            }
        }
    }
}
</style>
