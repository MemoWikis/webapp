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

function init() {
    selected.value = []
    const json: { Choices: Choice[], isSolutionOrdered: boolean } = JSON.parse(props.solution)
    localChoices.value = json.Choices
}
onBeforeMount(() => {
    init()
})

watch(() => props.solution, () => init())

const localChoices = ref<Choice[]>([])
const selected = ref<string[]>([])

function getAnswerDataString(): string {
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
</script>

<template>

    <div class="checkbox" v-for="choice in localChoices" :class="getClass(choice)">
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
                <!-- 
                <template v-if="props.showAnswer">

                    <font-awesome-layers v-if="validate(choice)" class="label-icon is-correct">
                        <font-awesome-icon icon="circle" />
                        <font-awesome-icon icon="check" class="inner" />
                    </font-awesome-layers>

                    <font-awesome-layers v-else class="label-icon is-wrong">
                        <font-awesome-icon icon="circle" />
                        <font-awesome-icon icon="fa-xmark" class="inner" />
                    </font-awesome-layers>

                </template> -->

            </div>
        </label>
    </div>
    <br />
    <h6 class="ItemInfo">
        Es können keine oder mehrere Antworten richtig sein!
    </h6>

</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.checkbox-container {
    display: flex;
    justify-items: space-between;
    align-items: center;
    width: 100%;

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