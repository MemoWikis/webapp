<script lang="ts" setup>
import { useEditor, EditorContent } from '@tiptap/vue-3'
import StarterKit from '@tiptap/starter-kit'
import Link from '@tiptap/extension-link'
import Placeholder from '@tiptap/extension-placeholder'
import Underline from '@tiptap/extension-underline'
import Image from '@tiptap/extension-image'
import CodeBlockLowlight from '@tiptap/extension-code-block-lowlight'
import Blockquote from '@tiptap/extension-blockquote'
import { lowlight } from 'lowlight/lib/core'
import _ from 'underscore'
import { AlertType, useAlertStore, AlertMsg, messages } from '../../alert/alertStore'

const props = defineProps(['solution', 'highlightEmptyFields'])
const emit = defineEmits(['setFlashCardContent'])
const alertStore = useAlertStore()
const content = ref(null)
const editor = useEditor({
    editable: true,
    extensions: [
        StarterKit,
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
        Image,
        Blockquote
    ],
    content: content.value,
    onUpdate: ({ editor }) => {
        emit('setFlashCardContent', editor)
    },
    editorProps: {
        handlePaste: (view, pos, event) => {
            let eventContent = event.content as any
            let content = eventContent.content
            if (content.length >= 1 && !_.isEmpty(content[0].attrs)) {
                let src = content[0].attrs.src;
                if (src.length > 1048576 && src.startsWith('data:image')) {
                    alertStore.openAlert(AlertType.Error, { text: messages.error.image.tooBig })
                    return true
                }
            }
        },
    }
})

onMounted(() => {
    if (props.solution.value) {
        editor.value?.commands.setContent(props.solution.value)
        emit('setFlashCardContent', editor.value)
    }
})

function clearFlashCard() {
    editor.value?.commands.setContent('')
}
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