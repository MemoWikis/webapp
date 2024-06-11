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
import { AlertType, useAlertStore, messages } from '../../alert/alertStore'
import ImageResize from 'tiptap-extension-resize-image'

interface Props {
    highlightEmptyFields: boolean
    content: string
}
const props = defineProps<Props>()
const alertStore = useAlertStore()

const emit = defineEmits(['setAnswer'])

const lowlight = createLowlight(all)
const editor = useEditor({
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
            placeholder: 'Beschreibe hier dein Anliegen. Bitte höflich, freundlich und sachlich schreiben...',
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
            if (content.length >= 1 && !isEmpty(content[0].attrs)) {
                let src = content[0].attrs.src;
                if (src.length > 1048576 && src.startsWith('data:image')) {
                    alertStore.openAlert(AlertType.Error, { text: messages.error.image.tooBig })
                    return true
                }
            }
        },
    },
    onUpdate: ({ editor }) => {
        emit('setAnswer', editor)
    },
})

watch(() => props.content, (c) => {
    if (c != editor.value?.getHTML())
        editor.value?.commands.setContent(c)
})
</script>

<template>
    <div>
        <div class="commentAnswerAddContainer" id="CommentAnswerAddComponent">
            <div class="row">
                <div class="col-sm-2 hidden-xs"></div>
                <div class="col-sm-10 col-xs-12">
                    <div id="AddAnswerTextFormContainer" class="inline-question-editor">
                        <div class="input-container">
                            <div class="overline-s no-line">Deine Antwort</div>
                            <div v-if="editor">
                                <EditorMenuBar :editor="editor" />
                                <editor-content :editor="editor"
                                    :class="{ 'is-empty': props.highlightEmptyFields && editor.state.doc.textContent.length <= 0 }" />
                                <div v-if="props.highlightEmptyFields && editor.state.doc.textContent.length <= 0"
                                    class="field-error">
                                    Bitte formuliere einen Kommentar mit min. 10 Zeichen.
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>