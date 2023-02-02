<script lang="ts" setup>
import { VueElement } from 'vue'

const answer = ref('')
const textArea = ref()
function resize() {
    let element = textArea.value as VueElement
    if (element) {
        element.style.height = "56px"
        element.style.height = element.scrollHeight + "px"
    }
}

function getAnswerDataString(): string {
    return getAnswerText()
}
function getAnswerText(): string {
    return answer.value
}
defineExpose({ getAnswerDataString, getAnswerText })

</script>

<template>

    <textarea id="txtAnswer" class="form-control " rows="1" placeholder="Gib hier bitte deine Antwort ein."
        @input="resize" v-model="answer" ref="textArea">
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
        @border-color: @memo-green;
        box-shadow: none;
    }

    &:active {
        @border-color: @memo-green;
        box-shadow: none;

    }
}
</style>