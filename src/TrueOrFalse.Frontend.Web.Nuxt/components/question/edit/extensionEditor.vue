<script lang="ts" setup>
import { useEditor, EditorContent } from '@tiptap/vue-3'
import StarterKit from '@tiptap/starter-kit'
import Link from '@tiptap/extension-link'
import Placeholder from '@tiptap/extension-placeholder'
import Underline from '@tiptap/extension-underline'
import CodeBlockLowlight from '@tiptap/extension-code-block-lowlight'
import { all, createLowlight } from 'lowlight'
import { isEmpty } from 'underscore'
import { AlertType, useAlertStore, AlertMsg } from '../../alert/alertStore'
import ImageResize from '~~/components/shared/imageResizeExtension'
import UploadImage from '~/components/shared/imageUploadExtension'
import { useEditQuestionStore } from './editQuestionStore'

interface Props {
    highlightEmptyFields: boolean
    content: string
    isInit: boolean
}
const props = defineProps<Props>()
const editQuestionStore = useEditQuestionStore()

const emit = defineEmits(['setQuestionExtensionData'])
const lowlight = createLowlight(all)
const { t } = useI18n()
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
            placeholder: t('editor.placeholderDescription'),
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
        checkContentImages()
    },
})

const checkContentImages = () => {
    if (editor.value == null)
        return

    const { state } = editor.value
    state.doc.descendants((node: any, pos: number) => {
        if (node.type.name === 'uploadImage') {
            const src = node.attrs.src
            if (src.startsWith('/Images/'))
                editQuestionStore.uploadedImagesInContent.push(src)

        }
    })

    editQuestionStore.refreshDeleteImageList()
}

const showExtension = ref(false)

watch(() => props.content, (o, n) => {
    if (n != null && n.length > 0)
        showExtension.value = true
    if (o != n && props.isInit)
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