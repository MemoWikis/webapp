<script lang="ts" setup>

const props = defineProps({ showAnswer: Boolean })

interface Pair {
    ElementLeft: ElementLeft;
    ElementRight: ElementRight;
}

interface ElementLeft {
    Text: string;
}

interface ElementRight {
    Text: string;
}

const pairs = ref([])
function getAnswerDataString(): string {
    // return selected.value.join("%seperate&xyz%")
    return ''
}
function getAnswerText(): string {
    // return selected.value.join("</br>")
    return ''

}
defineExpose({ getAnswerDataString, getAnswerText })

</script>

<template>

    <div class="row">
        <div class="col-sm-12">
            <div class="row">
                <div class="col-sm-12">
                    <div id="matchlist-pairs" class="row"></div>
                </div>
                <div class="col-sm-12">
                    <div class="row">
                        <div id="matchlist-rightelements"></div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</template>