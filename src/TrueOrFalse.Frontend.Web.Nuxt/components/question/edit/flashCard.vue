<script lang="ts" setup>
import { useEditor, EditorContent } from '@tiptap/vue-3'
import StarterKit from '@tiptap/starter-kit'
import Link from '@tiptap/extension-link'
import Placeholder from '@tiptap/extension-placeholder'
import Underline from '@tiptap/extension-underline'
import Image from '@tiptap/extension-image'
import CodeBlockLowlight from '@tiptap/extension-code-block-lowlight'
import { all, createLowlight } from 'lowlight'
import { isEmpty } from 'underscore'
import { AlertType, useAlertStore, AlertMsg, messages } from '../../alert/alertStore'
import ImageResize from 'tiptap-extension-resize-image'

interface Props {
    highlightEmptyFields: boolean
    solution?: string
}
const props = defineProps<Props>()
const emit = defineEmits(['setFlashCardContent'])
const alertStore = useAlertStore()
const content = ref(null)
const lowlight = createLowlight(all)
const editor = useEditor({
    editable: true,
    extensions: [
        StarterKit.configure({
            codeBlock: false,
        }),
        Link.configure({
            HTMLAttributes: {
                target: '_self',
                rel: 'noopener noreferrer nofollow'
            }
        }),
        CodeBlockLowlight.configure({
            lowlight,
        }),
        Underline,
        Placeholder.configure({
            emptyEditorClass: 'is-editor-empty',
            emptyNodeClass: 'is-empty',
            placeholder: 'Rückseite der Karteikarte',
            showOnlyCurrent: true,
        }),
        Image
    ],
    content: content.value,
    onUpdate: ({ editor }) => {
        setFlashCardContent()
    },
    editorProps: {
        handlePaste: (view, pos, event) => {
            let eventContent = event.content as any
            let content = eventContent.content
            if (content.length >= 1 && !isEmpty(content[0].attrs)) {
                let src = content[0].attrs.src
                if (src?.length > 1048576 && src.startsWith('data:image')) {
                    alertStore.openAlert(AlertType.Error, { text: messages.error.image.tooBig })
                    return true
                }
            }
        },
    }
})

function initSolution() {
    if (props.solution && props.solution.trim() != editor.value?.getHTML().trim()) {
        editor.value?.commands.setContent(props.solution)
        setFlashCardContent()
    }
}
onMounted(() => initSolution())
watch(() => props.solution, () => initSolution())

function setFlashCardContent() {
    if (editor.value) {
        const content = {
            solution: editor.value.getHTML(),
            solutionIsValid: editor.value.state.doc.textContent.length > 0
        }
        emit('setFlashCardContent', content)
    }

}

function clearFlashCard() {
    editor.value?.commands.setContent('')
}

defineExpose({ clearFlashCard })
</script>

<template>
    <div class="input-container">
        <div class="overline-s no-line">Antwort</div>
        <template v-if="editor">
            <EditorMenuBar :editor="editor" />
        </template>
        <template v-if="editor">
            <editor-content :editor="editor"
                :class="{ 'is-empty': highlightEmptyFields && editor.state.doc.textContent.length <= 0 }" />
        </template>
        <div v-if="highlightEmptyFields && editor && editor.state.doc.textContent.length <= 0" class="field-error">
            Bitte gib eine Antwort an.
        </div>
    </div>
</template>