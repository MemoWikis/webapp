<script lang="ts" setup>

const props = defineProps(['solution', 'highlightEmptyFields'])
const pairs = reactive({
    value: [{
        ElementRight: { Text: "" },
        ElementLeft: { Text: "" }
    }]
})
const rightElements = reactive({ value: [] })
const solutionIsOrdered = ref(false)

const emit = defineEmits(['setSolutionIsValid', 'setMatchListJson'])

function validateSolution() {
    var hasEmptyAnswer = rightElements.value.some((e) => {
        return e.Text.trim() == ''
    })
    var leftElementHasNoAnswer = pairs.value.some((p) => {
        return p.ElementLeft.Text.trim() == ''
    })
    var rightElementHasNoAnswer = pairs.value.some((p) => {
        return p.ElementRight.Text.trim() == ''
    })
    emit('setSolutionIsValid', !hasEmptyAnswer && !leftElementHasNoAnswer && !rightElementHasNoAnswer)
}
function solutionBuilder() {
    validateSolution()
    let solution = {
        Pairs: pairs.value,
        RightElements: rightElements.value,
        IsSolutionOrdered: solutionIsOrdered.value
    }
    emit('setMatchListJson', solution)
}


function initiateSolution() {
    var json = JSON.parse(props.solution)
    pairs.value = json.Pairs
    rightElements.value = json.RightElements
    solutionIsOrdered.value = json.IsSolutionOrdered
    solutionBuilder()
    validateSolution()
}

onMounted(() => {
    if (props.solution.value)
        initiateSolution()
})

function deletePair(index) {
    pairs.value.splice(index, 1)
    solutionBuilder()
}

</script>

<template>
    <div class="input-container matchlist-container">
        <div class="overline-s no-line">Antworten</div>

        <form class="form-inline matchlist-pairs" v-for="(pair, index) in pairs" :key="'pair' + index">
            <div class="matchlist-left form-group">
                <div v-if="pair.ElementLeft.Text.length <= 0 && props.highlightEmptyFields"
                    class="field-error-container">
                    <div class="field-error">Bitte gib ein linkes Element an.</div>
                </div>
                <input type="text" class="form-control col-sm-10" :id="'left-'+index" v-model="pair.ElementLeft.Text"
                    placeholder="Linkes Element" v-on:change="solutionBuilder()"
                    :class="{ 'is-empty': pair.ElementLeft.Text.length <= 0 && props.highlightEmptyFields }">
                <i class="fas fa-arrow-right col-sm-2 col-spacer"></i>
            </div>
            <div class="matchlist-right form-group">
                <div v-if="pair.ElementLeft.Text.length <= 0 && props.highlightEmptyFields"
                    class="field-error-container">
                    <div class="field-error">Bitte wähle ein rechtes Element aus.</div>
                </div>
                <select v-model="pair.ElementRight.Text" :id="'right-'+index" class="col-sm-10"
                    v-on:change="solutionBuilder()"
                    :class="{ 'is-empty': pair.ElementRight.Text.length <= 0 && props.highlightEmptyFields }">
                    <option disabled selected value="" hidden>Rechtes Element</option>
                    <option v-for="el in rightElements" v-if="el.Text != null && el.Text.length > 0" :value="el.Text">
                        {{el.Text}}</option>
                </select>
                <div @click="deletePair(index)" class="btn grey-bg col-sm-2 col-spacer" v-if="pairs.length > 1">
                    <i class="fas fa-trash"></i>
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
                        <input type="text" class="form-control col-sm-10" :id="i" v-model="element.Text" placeholder=""
                            v-on:change="solutionBuilder()"
                            :class="{ 'is-empty': i == 0 && element.Text.length <= 0 && props.highlightEmptyFields }">
                        <div @click="deleteRightElement(i)" class="btn grey-bg col-sm-2 col-spacer">
                            <i class="fas fa-trash"></i>
                        </div>
                    </div>
                </div>
                <div class="d-flex">
                    <div @click="addRightElement()" class="btn col-sm-10 form-control grey-bg"
                        :class="{ 'is-empty': rightElements.length <= 0 && props.highlightEmptyFields }">Rechtes Element
                        erstellen</div>
                    <div class="col-sm-2 col-spacer xs-hide"></div>
                </div>
                <div v-if="rightElements.length <= 0 && props.highlightEmptyFields" class="field-error">Bitte erstelle
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