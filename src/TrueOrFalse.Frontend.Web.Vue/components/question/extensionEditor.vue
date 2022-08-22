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
import { AlertType, useAlertStore, AlertMsg, messages } from '../alert/alertStore'

const props = defineProps(['highlightEmptyFields'])
const alertStore = useAlertStore()

const emit = defineEmits(['setQuestionExtensionData'])

const editor = useEditor({
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
            placeholder: 'Ergänzungen zur Frage zB. Bilder, Code usw.',
            showOnlyCurrent: true,
        }),
        Image.configure({
            inline: true,
            allowBase64: true,
        })
    ],
    editorProps: {
        handleClick: (view, pos, event) => {
        },
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
        attributes: {
            id: 'QuestionInputField',
        }
    },
    onUpdate: ({ editor }) => {
        emit('setQuestionExtensionData', editor)
    },
})

const showExtension = ref(false)
</script>

<template>
    <div class="overline-s no-line">Ergänzungen zur Frage</div>
    <div v-if="showExtension && editor">
        <EditorMenuBar :editor="editor" />
        <editor-content :editor="editor" />
    </div>
    <template v-else>
        <div class="d-flex">
            <div class="btn grey-bg form-control col-md-6" @click="showExtension = true">
                Ergänzungen hinzufügen</div>
            <div class="col-sm-12 hidden-xs"></div>
        </div>
    </template>
</template>