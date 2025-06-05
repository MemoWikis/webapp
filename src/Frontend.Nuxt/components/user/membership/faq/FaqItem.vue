<script lang="ts" setup>

interface Props {
    question: string
    answer: string
    answerParams?: Record<string, string>
}

const props = withDefaults(defineProps<Props>(), {
    answerParams: () => ({})
})

const show = ref<boolean>(false)
const { t } = useI18n()
</script>

<template>
    <div class="question" @click="show = !show">
        <div class="question-inner">
            <div class="text">{{ t(props.question) }}</div>
            <div class="icon">
                <font-awesome-icon :icon="['fa-solid', 'fa-chevron-down']" :class="{ 'open': show }" />
            </div>
        </div>
    </div>
    <div v-show="show" class="answer" v-html="t(props.answer, props.answerParams)"></div>
</template>

<style lang="less" scoped>
.question {
    background-color: white;
    margin-top: 20px;
    padding: 20px;
    margin-right: 30px;
    margin-left: 30px;
    user-select: none;
    cursor: pointer;
    border-radius: 8px;

    .question-inner {
        display: flex;

        .icon {
            margin-left: auto;
        }
    }

    &:active {
        filter: brightness(0.975);
    }

}

.answer {
    background-color: white;
    padding: 20px;
    margin-right: 30px;
    margin-left: 30px;
    transition: all ease-in 0.1s;

}

.open {
    transform: rotate(180deg)
}
</style>