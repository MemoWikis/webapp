<script lang=ts setup>

const props = defineProps([])
interface Choice {
    Text: string,
    IsCorrect: boolean
}
const localChoices = ref<Choice[]>([])
</script>

<template>

    <div class="checkbox" v-for="choice in localChoices">
        <label>
            <input type="checkbox" name="answer" :value="choice.Text" />
            {{ choice.Text }} <br />
        </label>
    </div>
    <br />
    <h6 class="ItemInfo">
        Es können keine oder mehrere Antworten richtig sein!
    </h6>

</template>