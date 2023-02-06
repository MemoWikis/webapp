<script lang="ts" setup>
interface Props {
    text: string
    solution: string
    markedAsCorrect: boolean
}
const props = defineProps<Props>()
const flipped = ref(false)

const solutionHtml = ref('')
function init() {
    solutionHtml.value = JSON.parse(props.solution).Text
}
onBeforeMount(() => {
    init()
})
watch(() => props.solution, () => init())


function flip() {
    flipped.value = !flipped.value
}

function getAnswerDataString(): string {
    return props.markedAsCorrect ? "(Antwort gewusst)" : "(Antwort nicht gewusst)"
}
function getAnswerText(): string {
    return ''
}
defineExpose({ flip, getAnswerDataString, getAnswerText })

const front = ref()
const back = ref()

function getMinHeight() {
    let minHeight = 200
    if (front.value != null && back.value != null) {
        minHeight = Math.max(front.value.clientHeight, back.value.clientHeight, minHeight) + 68
    }
    return `min-height: ${minHeight}px`
}
</script>

<template>
    <div class="flashcard" @click="flip()" :class="{ 'flipped': flipped }">
        <div class="flashcard-inner">
            <div class="flashcard-front" :style="getMinHeight()">
                <div class="question-text" ref="front">
                    {{ props.text }}
                </div>
                <div class="flip-label">
                    <font-awesome-icon icon="fa-solid fa-rotate" />
                    Zum Umdrehen klicken
                </div>
            </div>
            <div class="flashcard-back" :style="getMinHeight()">
                <div v-if="solutionHtml.length > 0" v-html="solutionHtml" ref="back"></div>
                <div class="flip-label">
                    <font-awesome-icon icon="fa-solid fa-rotate" />
                    Zum Umdrehen klicken
                </div>
            </div>
        </div>
    </div>
</template>
 
<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.question-text {
    text-align: center;
    font-family: Open Sans, Arial, sans-serif;
    margin: 0;
}

.flashcard {
    background-color: transparent;
    width: 100%;
    height: 100%;
    perspective: 1000px;
    cursor: pointer;

    &.flipped {
        .flashcard-inner {
            transform: rotateY(180deg) !important;
        }
    }
}

.flashcard-inner {
    position: relative;
    width: 100%;
    height: 100%;
    text-align: center;
    transition: transform 0.6s;
    transform-style: preserve-3d;
    box-shadow: 0 2px 6px rgb(0 0 0 / 16%);
    border: 1px @memo-grey-light solid;
}

.flashcard-front,
.flashcard-back {
    padding: 12px 12px 44px 12px;
    display: flex;
    justify-content: center;
    align-items: center;
    width: 100%;
    -webkit-backface-visibility: hidden;
    backface-visibility: hidden;
    min-height: 268px;
}

.flashcard-front {
    background-color: white;
    color: @memo-grey-darker;
}

.flashcard-back {
    background-color: white;
    color: @memo-grey-darker;
    transform: rotateY(180deg);
    position: absolute;
    top: 0;

}

.flip-label {
    color: @memo-grey-light;
    position: absolute;
    bottom: 0;
    right: 0;
    padding: 12px;
}
</style>