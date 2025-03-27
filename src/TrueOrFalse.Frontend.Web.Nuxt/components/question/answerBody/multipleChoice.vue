<script lang=ts setup>

interface Props {
    solution: string,
    showAnswer: boolean
}

const props = defineProps<Props>()
interface Choice {
    Text: string,
    IsCorrect: boolean
}

function shuffleChoices(choices: Choice[]) {
    let copy = choices.slice()  // Copy the array
    let currentIndex = copy.length, temporaryValue, randomIndex

    // While there remain elements to shuffle...
    while (0 !== currentIndex) {

        // Pick a remaining element...
        randomIndex = Math.floor(Math.random() * currentIndex)
        currentIndex -= 1

        // And swap it with the current element.
        temporaryValue = copy[currentIndex]
        copy[currentIndex] = copy[randomIndex]
        copy[randomIndex] = temporaryValue
    }

    return copy
}

function init() {
    selected.value = []
    const json: { Choices: Choice[], isSolutionOrdered: boolean } = JSON.parse(props.solution)
    localChoices.value = json.isSolutionOrdered === true ? json.Choices : shuffleChoices(json.Choices)
}

watch(() => props.solution, () => init())

const localChoices = ref<Choice[]>([])
const selected = ref<string[]>([])

async function getAnswerDataString(): Promise<string> {
    await nextTick()
    return selected.value.join("%seperate&xyz%")
}

function getAnswerText(): string {
    return selected.value.join("</br>")
}

defineExpose({ getAnswerDataString, getAnswerText })

function getClass(c: Choice) {
    if (props.showAnswer && c.IsCorrect)
        return 'is-correct show-solution'
    else if ((selected.value.indexOf(c.Text) >= 0 && !c.IsCorrect) && props.showAnswer)
        return 'is-wrong show-solution'
    return ''
}

init()
const { t } = useI18n()
</script>

<template>
    <div>
        <div v-for="choice in localChoices" :class="getClass(choice)">
            <label>
                <div class="checkbox-container">
                    <input type="checkbox" name="answer" :value="choice.Text" v-model="selected" class="hidden"
                        :disabled="props.showAnswer" />
                    <font-awesome-icon icon="fa-solid fa-square-check" v-if="selected.indexOf(choice.Text) >= 0"
                        class="checkbox-icon" :class="{ 'disabled': props.showAnswer }" />
                    <font-awesome-icon icon="fa-regular fa-square" v-else class="checkbox-icon"
                        :class="{ 'disabled': props.showAnswer }" />
                    <span class="checkbox-label">
                        {{ choice.Text }}
                    </span>
                </div>
            </label>
        </div>
        <br />
        <h6 class="ItemInfo">
            {{ t('answerbody.multipleChoiceInfo') }}
        </h6>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.checkbox-container {
    display: flex;
    justify-items: space-between;
    align-items: center;
    width: 100%;
    padding-left: 0px;
    cursor: pointer;

    .checkbox-icon {
        font-size: 18px;
        margin-right: 12px;

        &.disabled {
            color: @memo-grey-light;
        }
    }

    .label-icon {
        margin-left: 12px;
        font-size: 18px;



        .inner {
            color: @memo-blue;
            transform: scale(.6);
        }
    }
}

.is-correct {
    color: @memo-green-correct;

    .checkbox-label {
        font-weight: 700;
    }
}

.is-wrong {
    color: @memo-grey-dark;
}
</style>