<script lang="ts" setup>
import type { JSONContent } from '@tiptap/vue-3';
import { useEditor, EditorContent } from '@tiptap/vue-3'
import StarterKit from '@tiptap/starter-kit'
import type { GeneratedFlashcard } from '../../pageStore';
import { isEmpty } from 'underscore'

interface Props {
    flashcard: GeneratedFlashcard,
    index: number
}

const props = defineProps<Props>()

watch(() => props.flashcard, (flashcard) => {
    if (flashcard) {
        setFlashcardData()
    }
}, { deep: true })

onMounted(async () => {
    await nextTick()
    if (props.flashcard) {
        setFlashcardData()
    }
})

const setFlashcardData = () => {
    frontEditor.value?.commands.setContent(props.flashcard.front)
    backEditor.value?.commands.setContent(props.flashcard.back)

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
        handleClick: (_view, _pos, _event) => {
        },
        handlePaste: (_view, _pos, event) => {
            const eventContent = event.content as unknown as { content: { attrs?: { src?: string } }[] }
            const content = eventContent.content
            if (content.length >= 1 && !isEmpty(content[0].attrs)) {
                const src = content[0].attrs?.src
                if (src?.startsWith('data:image')) {
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
        handleClick: (_view, _pos, _event) => {
        },
        handlePaste: (_view, _pos, event) => {
            const eventContent = event.content as unknown as { content: { attrs?: { src?: string } }[] }
            const content = eventContent.content
            if (content.length >= 1 && !isEmpty(content[0].attrs)) {
                const src = content[0].attrs?.src
                if (src?.startsWith('data:image')) {
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

const emit = defineEmits(['delete-flashcard'])
const hover = ref(false)
const deleteFlashcard = () => {
    emit('delete-flashcard', props.index)
}
</script>

<template>
    <div class="flashcard-section" :class="{ 'hover': hover }" @mouseenter="hover = true" @mouseleave="hover = false">
        <div class="delete-flashcard-container" @click="deleteFlashcard">
            <div class="delete-flashcard">
                <font-awesome-icon :icon="['fas', 'trash']" />
            </div>
        </div>
        <div v-if="frontEditor && backEditor" class="flashcard-container">
            <div class="flashcard-content">
                <EditorMenuBar :editor="frontEditor" />
                <editor-content :editor="frontEditor" />
            </div>
            <div class="flashcard-content back">
                <EditorMenuBar :editor="backEditor" />
                <editor-content :editor="backEditor" />
            </div>
        </div>
    </div>

</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.flashcard-section {
    padding-top: 24px;
    padding-bottom: 24px;
    margin-bottom: 24px;

    .flashcard-container {
        display: flex;
        justify-content: center;
        align-items: center;

        @media (max-width: 700px) {
            flex-direction: column;
            margin-left: -64px;
            margin-right: -64px;
            padding-left: 64px;
            padding-right: 64px;
        }

        .flashcard-content {
            padding: 0 12px;
            width: 50%;
            // max-width: 430px;
            min-width: 300px;

            @media (max-width: 700px) {
                margin-bottom: 16px;
                width: 100%;
                max-width: 100%;

                &.back {
                    margin-bottom: 0;
                }
            }
        }
    }

    .delete-flashcard-container {
        width: 100%;
        position: relative;

        .delete-flashcard {
            position: absolute;
            top: 0;
            right: -48px;
            padding: 8px;
            cursor: pointer;
            color: @memo-grey-light;
            transition: color 0.3s ease-in-out;
            font-size: 18px;
            z-index: 2000;

            &:hover {
                color: @memo-grey;
            }
        }
    }


    &.hover {
        background-color: @memo-grey-lightest;
        margin-left: -64px;
        margin-right: -64px;
        padding-left: 64px;
        padding-right: 64px;
    }
}
</style>

<style lang="less">
#AiFlashcard {
    .flashcard-content {
        .ProseMirror {
            background-color: white;
        }
    }
}
</style>