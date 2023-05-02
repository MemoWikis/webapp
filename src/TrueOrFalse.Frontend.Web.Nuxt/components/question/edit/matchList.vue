<script lang="ts" setup>
interface Props {
    highlightEmptyFields: boolean
    solution?: string
}
const props = defineProps<Props>()
const pairs = ref([{
    ElementRight: { Text: "" },
    ElementLeft: { Text: "" }
}])
const rightElements = ref<{ [key: string]: string }[]>([])
const solutionIsOrdered = ref(false)

const emit = defineEmits(['setMatchlistJson'])

function validateSolution() {
    const allDroppableAnswersAreValid = rightElements.value.every((e: { [key: string]: string }) => {
        return e.Text.trim() != ''
    })

    const allPairsAreValid = pairs.value.every((p) => {
        return p.ElementLeft.Text.trim() != '' && p.ElementRight.Text.trim() != ''
    })

    return allDroppableAnswersAreValid && allPairsAreValid
}

function solutionBuilder() {
    const solution = {
        Pairs: pairs.value,
        RightElements: rightElements.value,
        IsSolutionOrdered: solutionIsOrdered.value
    }
    emit('setMatchlistJson', { solution: JSON.stringify(solution), solutionIsValid: validateSolution() })
}

function initSolution() {
    if (props.solution) {
        var json = JSON.parse(props.solution)
        pairs.value = json.Pairs
        rightElements.value = json.RightElements
        solutionIsOrdered.value = json.IsSolutionOrdered
        solutionBuilder()
    }
}

watch(() => props.solution, () => initSolution())
onMounted(() => initSolution())

watch([pairs, rightElements], () => { solutionBuilder() }, { deep: true })

function deletePair(index: number) {
    pairs.value.splice(index, 1)
}

function addPair() {
    let placeHolder = {
        ElementRight: { Text: "" },
        ElementLeft: { Text: "" }
    }
    pairs.value.push(placeHolder)
}

function deleteRightElement(index: number) {
    rightElements.value.splice(index, 1)
}

function addRightElement() {
    let rightElement = { Text: "" }
    rightElements.value.push(rightElement)
}

</script>

<template>
    <div class="input-container matchlist-container">
        <div class="overline-s no-line">Antworten</div>

        <form class="form-inline matchlist-pairs" v-for="(pair, index) in pairs" :key="'pair' + index">
            <div class="matchlist-left form-group">
                <div v-if="pair.ElementLeft.Text.length <= 0 && props.highlightEmptyFields" class="field-error-container">
                    <div class="field-error">Bitte gib ein linkes Element an.</div>
                </div>
                <input type="text" class="form-control col-sm-10 matchlist-input" :id="'left-' + index"
                    v-model="pair.ElementLeft.Text" placeholder="Linkes Element" v-on:change="solutionBuilder()"
                    :class="{ 'is-empty': pair.ElementLeft.Text.length <= 0 && props.highlightEmptyFields }">
                <font-awesome-icon icon="fa-solid fa-arrow-right" class="col-sm-2 col-spacer fa-icon" />
            </div>
            <div class="matchlist-right form-group">
                <div v-if="pair.ElementLeft.Text.length <= 0 && props.highlightEmptyFields" class="field-error-container">
                    <div class="field-error">Bitte wähle ein rechtes Element aus.</div>
                </div>
                <select v-model="pair.ElementRight.Text" :id="'right-' + index" class="col-sm-10 matchlist-select"
                    v-on:change="solutionBuilder()"
                    :class="{ 'is-empty': pair.ElementRight.Text.length <= 0 && props.highlightEmptyFields }">
                    <option disabled selected value="" hidden>Rechtes Element</option>
                    <template v-for="el in rightElements">
                        <option v-if="el.Text != null && el.Text.length > 0" :value="el.Text">
                            {{ el.Text }}</option>
                    </template>

                </select>
                <div @click="deletePair(index)" class="btn grey-bg col-sm-2 col-spacer delete-btn" v-if="pairs.length > 1">
                    <font-awesome-icon icon="fa-solid fa-trash" />
                </div>
                <div class="col-sm-2 col-spacer" v-else></div>
            </div>
        </form>
        <div class="matchlist-options">
            <div class="matchlist-left d-flex">
                <div @click="addPair()" class="form-control btn col-sm-10 grey-bg">Paar hinzufügen</div>
                <div class="col-sm-2 col-spacer xs-hide"></div>
            </div>
            <div class="matchlist-right">
                <div v-for="(element, i) in rightElements" :key="i" class="form-group">
                    <div class="d-flex">
                        <input type="text" class="form-control col-sm-10 matchlist-input" :id="i.toString()"
                            v-model="element.Text" placeholder="" v-on:change="solutionBuilder()"
                            :class="{ 'is-empty': i == 0 && element.Text.length <= 0 && props.highlightEmptyFields }">
                        <div @click="deleteRightElement(i)" class="btn grey-bg col-sm-2 col-spacer delete-btn">
                            <font-awesome-icon icon="fa-solid fa-trash" />
                        </div>
                    </div>
                </div>
                <div class="d-flex">
                    <div @click="addRightElement()" class="btn col-sm-10 form-control grey-bg"
                        :class="{ 'is-empty': rightElements.length <= 0 && props.highlightEmptyFields }">Rechtes
                        Element
                        erstellen</div>
                    <div class="col-sm-2 col-spacer xs-hide"></div>
                </div>
                <div v-if="rightElements.length <= 0 && props.highlightEmptyFields" class="field-error">Bitte
                    erstelle
                    ein
                    rechtes Element.</div>
            </div>

        </div>
        <div class="checkbox-container">
            <div class="checkbox">
                <label>
                    <input type="checkbox" v-model="solutionIsOrdered" :true-value="false" :false-value="true">Paare
                    zufällig anordnen
                </label>
            </div>
        </div>


    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';


.matchlist-container {

    .matchlist-input,
    .matchlist-select {
        padding: 6px 12px;
    }

    select {
        height: 34px;
        width: 190px;

        option {
            &:disabled {
                font-style: italic;
            }
        }

        &:focus,
        &:focus-visible {
            border: solid 1px @memo-green;
        }
    }

    .form-group {
        display: flex;
        align-items: center;
    }

    .matchlist-options,
    .matchlist-pairs {
        display: flex;
        justify-content: space-between;
        margin-bottom: 8px;

        input,
        select,
        .input-group {
            width: 100%;
        }

        .matchlist-left,
        .matchlist-right {
            width: 50%;

            .fa-icon {
                box-sizing: border-box !important;
            }
        }

        .delete-btn {
            height: 34px;
        }
    }

    .matchlist-options {
        @media (max-width:576px) {
            flex-direction: column;

            .matchlist-left,
            .matchlist-right {
                width: 100%;
            }

            .matchlist-right {
                margin-top: 26px;
            }

            .xs-hide {
                &.col-spacer {
                    min-width: 0px;
                    width: 0px;
                    padding: 0;
                }
            }
        }
    }
}

.col-spacer {
    min-width: 38px;
}
</style>