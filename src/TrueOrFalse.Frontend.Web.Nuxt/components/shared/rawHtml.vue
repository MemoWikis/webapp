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

async function highlightCode() {
    await nextTick()

    if (!props.id) {
        mounted.value = true
        return
    }
    mounted.value = false
    const el = document.getElementById(props.id)
    if (el != null)
        el.querySelectorAll('code').forEach(block => {
            if (block.textContent != null)
                block.innerHTML = getHighlightedCode(block.textContent)
        })
    await nextTick()
    mounted.value = true
}
const mounted = ref(false)
onMounted(() => {
    highlightCode()

    watch(() => props.html, async (val) => {
        if (val) {
            const handledHtml = handleNewLine(props.html)
            if (handledHtml.length > 0)
                safeHtml.value = handledHtml
            highlightCode()
        }
    })

})

</script>

<template>
    <div :id="props.id" v-if="props.html && mounted" v-html="safeHtml"></div>
</template>