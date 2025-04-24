<script lang="ts" setup>
import { VueElement } from 'vue'

const props = defineProps({
    showAnswer: Boolean
})

const answer = ref('')
const textArea = ref()
function resize() {
    let element = textArea.value as VueElement
    if (element) {
        element.style.height = "56px"
        element.style.height = element.scrollHeight + "px"
    }
}

async function getAnswerDataString(): Promise<string> {
    await nextTick()
    return getAnswerText()
}
function getAnswerText(): string {
    return answer.value
}
defineExpose({ getAnswerDataString, getAnswerText })
const { t } = useI18n()
</script>

<template>
    <textarea id="txtAnswer" class="form-control " rows="1" :placeholder="t('answerBody.textPlaceholderAddAnswer')" @input="resize"
        v-model="answer" ref="textArea" :disabled="props.showAnswer">
            </textarea>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

textarea {
    width: 100%;
    border: 1px solid @memo-grey-light;
    outline: none;
    min-height: 54px;
    resize: none;
    padding: 10px;
    height: 54px;
    overflow: hidden;
    border-radius: 0;

    &:focus {
        border: solid 1px @memo-green;
        box-shadow: none;
    }

    &:active {
        border: solid 1px @memo-green;
        box-shadow: none;

    }
}
</style>