<script lang="ts" setup>
interface Props {
    text: string
    solution: string
    markedAsCorrect: boolean
}
const props = defineProps<Props>()
const front = ref('')
const back = ref('')
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


</script>

<template>
    <div class="flashcard" @click="flip()" :class="{ 'flipped': flipped }">
        <div class="flashcard-inner">
            <div class="flashcard-front">
                <p class="QuestionText"
                    style="text-align: center; font-family: Open Sans, Arial, sans-serif; margin: 0;">
                    {{ props.text }}</p>
            </div>
            <div class="flashcard-back">
                <template v-if="solutionHtml.length > 0" v-html="solutionHtml"></template>
            </div>
        </div>
    </div>
</template>
 
<style lang="less" scoped>
.flashcard {
    background-color: transparent;
    width: 300px;
    height: 100%;
    perspective: 1000px;

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
    box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2);
}

.flashcard-front,
.flashcard-back {
    position: absolute;
    width: 100%;
    height: 100%;
    -webkit-backface-visibility: hidden;
    backface-visibility: hidden;
}

.flashcard-front {
    background-color: #bbb;
    color: black;
}

.flashcard-back {
    background-color: #2980b9;
    color: white;
    transform: rotateY(180deg);
}
</style>