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
const answerEditor = useEditor({
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

const answerJson = ref(null)
const answerHtml = ref(null)

onMounted(() => {
    if (props.solution.value) {
        answerEditor.value?.commands.setContent(props.solution.value)
        emit('setFlashCardContent', answerEditor.value)
    }
})

function clearFlashCard() {
    answerEditor.value?.commands.setContent('')
}
</script>

<template>
</template>