<script lang="ts" setup>
import { useEditor, EditorContent } from '@tiptap/vue-3'
import StarterKit from '@tiptap/starter-kit'
import Link from '@tiptap/extension-link'
import Placeholder from '@tiptap/extension-placeholder'
import Underline from '@tiptap/extension-underline'
import CodeBlockLowlight from '@tiptap/extension-code-block-lowlight'
import { all, createLowlight } from 'lowlight'
import { isEmpty } from 'underscore'
import { AlertType, useAlertStore, AlertMsg, messages } from '../../alert/alertStore'
import ImageResize from '~~/components/shared/imageResizeExtension'
import UploadImage from '~/components/shared/imageUploadExtension'
import { useEditQuestionStore } from './editQuestionStore'

interface Props {
    highlightEmptyFields: boolean
    content: string
}
const props = defineProps<Props>()
const alertStore = useAlertStore()
const editQuestionStore = useEditQuestionStore()

const emit = defineEmits(['setQuestionExtensionData'])
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
            placeholder: 'Ergänzungen zur Frage zB. Bilder, Code usw.',
            showOnlyCurrent: true,
        }),
        ImageResize.configure({
            inline: true,
            allowBase64: true,
        }),
        UploadImage.configure({
            uploadFn: editQuestionStore.uploadContentImage
        })
    ],
    editorProps: {
        handlePaste: (view, pos, event) => {
            const eventContent = event.content as any
            const content = eventContent.content
            if (content.length >= 1 && !isEmpty(content[0].attrs)) {
                const src = content[0].attrs.src
                if (src.startsWith('data:image')) {
                    editor.value?.commands.addBase64Image(src)
                    return true
                }
            }
        },
        attributes: {
            id: 'ExtensionEditor',
        }
    },
    onUpdate: ({ editor }) => {
        emit('setQuestionExtensionData', editor)
    },
})

const showExtension = ref(false)

watch(() => props.content, (o, n) => {
    if (n != null && n.length > 0)
        showExtension.value = true
    if (o != n)
        editor.value?.commands.setContent(n)
})
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