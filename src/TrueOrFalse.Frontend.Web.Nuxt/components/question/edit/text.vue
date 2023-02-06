<script lang="ts" setup>
import { VueElement } from 'vue'
const props = defineProps(['solution', 'highlightEmptyFields'])
const text = ref('')
const isEmpty = ref('')

const emit = defineEmits(['setSolutionIsValid', 'setSolution'])
const textArea = ref()

function resize() {
    if (textArea.value != null) {
        let element = textArea.value as VueElement
        element.style.height = "43px"
        element.style.height = element.scrollHeight + "px"
    }
}
onMounted(() => window.addEventListener('resize', resize))

function initSolution() {
    if (props.solution?.value)
        text.value = props.solution
}
watch(() => props.solution, () => initSolution())
onMounted(() => initSolution())

watch(text, (e) => {
    if (e.length > 0)
        emit('setSolutionIsValid')

    let metadataJson = { IsText: true }
    emit('setSolution', { textSolution: text.value, solutionMetadataJson: metadataJson })

    isEmpty.value = e.length == 0 ? 'is-empty' : ''
})


</script>

<template>
    <div class="input-container">
        <div class="overline-s no-line">Antwort</div>

        <form class="form-horizontal">
            <div class="form-group">
                <div class="col-sm-12 textsolution">
                    <textarea placeholder="Tippe hier deine Antwort" @input="resize()" ref="textArea" v-model="text"
                        :class="{ 'is-empty': text.length === 0 && props.highlightEmptyFields }"></textarea>

                    <div v-if="text.length === 0 && props.highlightEmptyFields" class="field-error"
                        style="margin-top : -5px;">Bitte gib eine Antwort ein.</div>
                </div>
            </div>
        </form>

    </div>
</template>

<style lang="less" scoped>
.textsolution {
    textarea {
        width: 100%;
        outline: none;
        min-height: 43px;
        resize: none;
        padding-left: 12px;
        overflow: hidden;
        padding: 11px 15px 0;
    }
}
</style>