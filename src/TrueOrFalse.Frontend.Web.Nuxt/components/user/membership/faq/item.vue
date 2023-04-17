<script lang="ts" setup>

interface Props {
    question: string
    answer: string
}

const props = defineProps<Props>()
const show = ref<boolean>(false)
</script>

<template>
    <div class="question">
        <div class="question-inner" @click="show = !show">
            <div class="text">{{ props.question }}</div>
            <div class="icon">
                <font-awesome-icon :icon="['fa-solid', 'fa-chevron-down']" :class="{ 'open': show }" />
            </div>
        </div>
        <div v-show="show" class="answer-block">
            {{ props.answer }}
        </div>
    </div>
</template>

<style lang="less" scoped>
.question {
    background-color: white;
    margin-top: 19px;
    padding: 20px;
    margin-right: 30px;
    margin-left: 30px;

    .question-inner {
        display: flex;
        cursor: pointer;

        .icon {
            margin-left: auto;
        }
    }

    .answer-block {
        margin-top: 20px;
    }
}

.open {
    transform: rotate(180deg)
}
</style>