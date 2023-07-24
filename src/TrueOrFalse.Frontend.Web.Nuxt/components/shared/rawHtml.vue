<script lang="ts" setup>
import { getHighlightedCode, handleNewLine } from './utils'

interface Props {
    html: string,
    id?: string
}

const props = defineProps<Props>()
const safeHtml = ref<string>()
onBeforeMount(() => {
    const handledHtml = handleNewLine(props.html)
    if (handledHtml.length > 0)
        safeHtml.value = handledHtml
})

function highlightCode() {
    if (!props.id)
        return

    var el = document.getElementById(props.id)
    if (el != null)
        el.querySelectorAll('code').forEach(block => {
            if (block.textContent != null)
                block.innerHTML = getHighlightedCode(block.textContent)
        })
}
const mounted = ref(false)
onMounted(async () => {
    await nextTick()
    mounted.value = true
    await nextTick()
    highlightCode()
})
</script>

<template>
    <div :id="props.id" v-if="safeHtml && mounted" v-html="safeHtml"></div>
</template>