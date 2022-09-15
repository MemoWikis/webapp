<script lang="ts" setup>
import { ref } from 'vue'
const props = defineProps(['question'])
const front = ref('')
const back = ref('')
const flipped = ref(false)
const emit = defineEmits(['flip'])
function flip() {

}
</script>

<template>
    <div class="flashcard" @click="flip()" :class="{ 'flipped': flipped }">
        <div class="flashcard-inner">
            <div class="flashcard-front">
                <p class="QuestionText"
                    style="text-align: center; font-family: Open Sans, Arial, sans-serif; margin: 0;">
                    {{ props.question.QuesitonText }}</p>
                <template v-html="props.question.QuestionTextMarkdown"></template>
            </div>
            <div class="flashcard-back">

            </div>
        </div>
    </div>
</template>
 
<style lang="less" scoped>
.flashcard {
    background-color: transparent;
    width: 300px;
    height: 300px;
    perspective: 1000px;

    &.flipped {
        .flip-card-inner {
            transform: rotateY(180deg);
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