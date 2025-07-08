<script lang="ts" setup>
import { handleNewLine } from '~/utils/utils'
import { FlashcardAnswerTypeEnum } from '../flashCardAnswerTypeEnum'

interface Props {
    frontContent: string
    solution: string
    markedAsCorrect: boolean
}
const props = defineProps<Props>()
const flipped = ref(false)

const solutionHtml = ref('')
const init = () => {
    solutionHtml.value = JSON.parse(props.solution).Text
}

watch(() => props.solution, () => init())

const flip = () => {
    if (flipped.value && solutionHtml.value.charAt(0) === ' ')
        solutionHtml.value = solutionHtml.value.substring(1)
    else solutionHtml.value = ' ' + solutionHtml.value

    flipped.value = !flipped.value
}

const onFlashcardClick = (event: Event) => {
    // Prevent flip if clicking on figcaption or its children
    const target = event.target as HTMLElement
    if (target.closest('.tiptap-figcaption, figcaption')) {
        return // Don't flip when clicking on captions
    }
    flip()
}

watch(flipped, () => emit('flipped'))

const getAnswerDataString = async (): Promise<string> => {
    await nextTick()
    const result = props.markedAsCorrect ? FlashcardAnswerTypeEnum.Known : FlashcardAnswerTypeEnum.Unknown
    return result.toString()
}

const getAnswerText = (): string => {
    return ''
}
defineExpose({ flip, getAnswerDataString, getAnswerText })

const front = ref()
const back = ref()

const getMinHeight = () => {
    let minHeight = 200
    if (front.value != null && back.value != null) {
        minHeight = Math.max(front.value.clientHeight, back.value.clientHeight, minHeight) + 68
    }
    return `min-height: ${minHeight}px`
}

const emit = defineEmits((['flipped']))

init()

const { t } = useI18n()
</script>

<template>
    <div class="flashcard" @click="onFlashcardClick" :class="{ 'flipped': flipped }">
        <div class="flashcard-inner">
            <div class="flashcard-front" :style="getMinHeight()">
                <div class="question-text" ref="front" v-html="props.frontContent">
                </div>
                <div class="flip-label">
                    <font-awesome-icon icon="fa-solid fa-rotate" />
                    {{ t('answerbody.flipFlashcard') }}
                </div>
            </div>
            <div class="flashcard-back" :style="getMinHeight()">
                <div v-show="solutionHtml.length > 0" v-html="handleNewLine(solutionHtml)" ref="back"></div>
                <div class="flip-label">
                    <font-awesome-icon icon="fa-solid fa-rotate" />
                    {{ t('answerbody.flipFlashcard') }}

                </div>
            </div>
        </div>
    </div>
</template>

<style lang="less">
@import (reference) '~~/assets/includes/imports.less';

.question-text {
    p {
        .tiptapImgMixin(true);
    }
}

// Make figcaptions look clickable in flashcard content
.flashcard {

    .tiptap-figcaption,
    figcaption {
        cursor: pointer;

        &:hover {
            opacity: 0.8;
        }
    }
}
</style>

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


<style lang="less">
.question-text {
    pre {
        text-align: left;
    }
}
</style>