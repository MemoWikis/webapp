<script lang=ts setup>

interface Props {
    solution: string
}

const props = defineProps<Props>()
interface Choice {
    Text: string,
    IsCorrect: boolean
}

function init() {
    const json: { Choices: Choice[], isSolutionOrdered: boolean } = JSON.parse(props.solution)
    localChoices.value = json.Choices
}
onBeforeMount(() => {
    init()
})

watch(() => props.solution, () => init())

const localChoices = ref<Choice[]>([])
const selected = ref([])

function getAnswerDataString(): string {
    return selected.value.join("%seperate&xyz%")
}
function getAnswerText(): string {
    return selected.value.join("</br>")
}
defineExpose({ getAnswerDataString, getAnswerText })
</script>

<template>

    <div class="checkbox" v-for="choice in localChoices">
        <label>
            <input type="checkbox" name="answer" :value="choice.Text" v-model="selected" />
            {{ choice.Text }} <br />
        </label>
    </div>
    <br />
    <h6 class="ItemInfo">
        Es können keine oder mehrere Antworten richtig sein!
    </h6>

</template>