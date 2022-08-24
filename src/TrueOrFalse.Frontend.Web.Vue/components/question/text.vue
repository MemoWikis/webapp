<script lang="ts" setup>
import { ref } from 'vue'
const props = defineProps(['solution', 'highlightEmptyFields'])
const text = ref('')
const isEmpty = ref('')

const emit = defineEmits(['setSolutionIsValid', 'setSolution'])
const textArea = ref(null)

function resize() {
    if (textArea.value != null) {
        let element = textArea.value
        element.style.height = "43px"
        element.style.height = element.scrollHeight + "px"
    }

}
onMounted(() => {
    if (props.solution)
        text.value = props.solution;
    if (text.value.length > 0)
        emit('setSolutionIsValid')

    window.addEventListener('resize', resize)

})

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
                    <textarea placeholder="Gib deinem Thema einen Namen" @input="resize()" ref="textArea" v-model="text"
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
        border: none;
        outline: none;
        min-height: 43px;
        resize: none;
        margin-top: -8px;
        padding: 0;
        padding-left: 12px;
        overflow: hidden;
    }
}
</style>