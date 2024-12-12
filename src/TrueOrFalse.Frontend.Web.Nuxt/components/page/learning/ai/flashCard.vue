<script lang="ts" setup>
import { useEditor, EditorContent, JSONContent } from '@tiptap/vue-3'
import StarterKit from '@tiptap/starter-kit'
import { GeneratedFlashCard, usePageStore } from '../../pageStore'
import { isEmpty } from 'underscore'

interface Props {
    flashCard: GeneratedFlashCard
}

const props = defineProps<Props>()

watch(() => props.flashCard, (flashCard) => {
    if (flashCard) {
        setFlashCardData()
    }
}, { deep: true })

onMounted(async () => {
    await nextTick()
    if (props.flashCard) {
        setFlashCardData()
    }
})

const setFlashCardData = () => {
    frontEditor.value?.commands.setContent(props.flashCard.front)
    backEditor.value?.commands.setContent(props.flashCard.back)

    questionHtml.value = frontEditor.value?.getHTML() || ''
    answerHtml.value = backEditor.value?.getHTML() || ''
}

const frontEditor = useEditor({
    extensions: [
        StarterKit.configure({
            codeBlock: false,
        }),
    ],
    editorProps: {
        handleClick: (view, pos, event) => {
        },
        handlePaste: (view, pos, event) => {
            const eventContent = event.content as any
            const content = eventContent.content
            if (content.length >= 1 && !isEmpty(content[0].attrs)) {
                const src = content[0].attrs.src
                if (src.startsWith('data:image')) {
                    return false
                }
            }
        },
    },
    onUpdate: ({ editor }) => {
        questionJson.value = editor.getJSON()
        questionHtml.value = editor.getHTML()
    },
})
const backEditor = useEditor({
    extensions: [
        StarterKit.configure({
            codeBlock: false,
        }),
    ],
    editorProps: {
        handleClick: (view, pos, event) => {
        },
        handlePaste: (view, pos, event) => {
            const eventContent = event.content as any
            const content = eventContent.content
            if (content.length >= 1 && !isEmpty(content[0].attrs)) {
                const src = content[0].attrs.src
                if (src.startsWith('data:image')) {
                    return false
                }
            }
        },
    },
    onUpdate: ({ editor }) => {
        answerJson.value = editor.getJSON()
        answerHtml.value = editor.getHTML()
    },
})


const questionJson = ref(null as null | JSONContent)
const questionHtml = ref('')


const answerJson = ref(null as null | JSONContent)
const answerHtml = ref('')

</script>

<template>
    <div class="flashcard-container" v-if="frontEditor && backEditor">
        <div class="flashcard-content">
            <EditorMenuBar :editor="frontEditor" />
            <editor-content :editor="frontEditor" />
        </div>
        <div class="flashcard-content">
            <EditorMenuBar :editor="backEditor" />
            <editor-content :editor="backEditor" />
        </div>
    </div>

</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.flashcard-container {
    display: flex;
    justify-content: center;
    align-items: center;

    .flashcard-content {
        padding: 0 12px;
        margin-bottom: 48px;
        margin-top: 24px;
        width: 50%;
        max-width: 600px;
    }
}
</style>