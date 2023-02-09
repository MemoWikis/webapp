<script lang="ts" setup>

const props = defineProps(['solution', 'highlightEmptyFields'])
const choices = ref([{
    Text: '',
    IsCorrect: true
}]
)
const solutionIsOrdered = ref(false)

const emit = defineEmits(['solutionIsValid', 'setMultipleChoiceSolution'])

function validateSolution() {
    var hasEmptyAnswer = choices.value.some((c) => {
        return c.Text.trim() == ''
    })
    emit('solutionIsValid', !hasEmptyAnswer)
}

function initSolution() {
    if (props.solution?.value) {
        var json = JSON.parse(props.solution.value)
        choices.value = json.Choices
        solutionIsOrdered.value = json.IsSolutionOrdered
        validateSolution()
    }
}

watch(() => props.solution, () => initSolution())
onMounted(() => initSolution())

function updateElement(index: number, newVal: {
    Text: string,
    IsCorrect: boolean
}) {
    choices.value[index] = newVal
}

function addChoice() {
    let placeHolder = {
        Text: '',
        IsCorrect: false
    }
    choices.value.push(placeHolder)
}

function deleteChoice(index: number) {
    choices.value.splice(index, 1)
}

function solutionBuilder() {
    validateSolution()

    let solution = {
        Choices: choices.value,
        IsSolutionOrdered: solutionIsOrdered.value
    }

    emit('setMultipleChoiceSolution', solution)
}

function toggleCorrectness(index: number) {
    choices.value[index].IsCorrect = !choices.value[index].IsCorrect
    solutionBuilder()
}
</script>

<template>
    <div class="input-container">
        <div class="overline-s no-line">Antworten</div>

        <div class="form-group" v-for="(choice, index) in choices" :key="index">
            <div class="input-group">
                <div @click="toggleCorrectness(index)"
                    class="input-group-addon toggle-correctness btn is-correct grey-bg"
                    :class="{ active: choice.IsCorrect }">
                    <font-awesome-icon icon="fa-solid fa-check" />
                </div>
                <div @click="toggleCorrectness(index)" class="input-group-addon toggle-correctness btn is-wrong grey-bg"
                    :class="{ active: choice.IsCorrect == false }">
                    <font-awesome-icon icon="fa-solid fa-xmark" />
                </div>
                <input type="text" class="form-control multiplechoice-input" :id="'SolutionInput-' + index"
                    placeholder="" v-model="choice.Text" v-on:change="solutionBuilder()"
                    :class="{ 'is-empty': choice.Text.length <= 0 && highlightEmptyFields }">
                <div v-if="choices.length > 1" @click="deleteChoice(index)" class="input-group-addon btn grey-bg">
                    <font-awesome-icon icon="fa-solid fa-trash" />
                </div>
            </div>
            <div v-if="choice.Text.length <= 0 && highlightEmptyFields" class="field-error">Bitte gib eine Antwort ein.
            </div>
        </div>
        <div class="d-flex">
            <div @click="addChoice()" class="btn grey-bg form-control col-md-6">Antwort hinzufügen</div>
            <div class="col-sm-12 hidden-xs"></div>
        </div>
        <div class="checkbox-container">
            <div class="checkbox">
                <label>
                    <input type="checkbox" v-model="solutionIsOrdered" :true-value="false" :false-value="true">
                    Antworten zufällig anordnen
                </label>
            </div>
        </div>
    </div>


</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.multiplechoice-input {
    padding: 0 8px;
    min-height: 43px;
}

.toggle-correctness {
    min-width: 43px;
    box-shadow: none;
    border-top: 1px solid @memo-grey-light;
    border-bottom: 1px solid @memo-grey-light;
    border-left: 1px solid @memo-grey-light;
    border-right: none;

    &.active {
        color: white;

        &.is-correct {
            background-color: @memo-green;
        }

        &.is-wrong {
            background-color: @memo-salmon;
        }
    }
}

.btn {
    min-width: 43px;
}
</style>