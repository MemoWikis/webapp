<script lang="ts" setup>
import { useEditor, EditorContent } from '@tiptap/vue-3'
import StarterKit from '@tiptap/starter-kit'
import Link from '@tiptap/extension-link'
import Placeholder from '@tiptap/extension-placeholder'
import Underline from '@tiptap/extension-underline'
import CodeBlockLowlight from '@tiptap/extension-code-block-lowlight'
import { all, createLowlight } from 'lowlight'
import { isEmpty } from 'underscore'
import { useEditQuestionStore } from './editQuestionStore'
import UploadImage from '~/components/shared/imageUploadExtension'
import FigureExtension from '~~/components/shared/figureExtension'

interface Props {
    highlightEmptyFields: boolean
    content: string
    isInit: boolean
}
const props = defineProps<Props>()
const editQuestionStore = useEditQuestionStore()

const emit = defineEmits(['setDescriptionData'])
const showDescription = ref(false)
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
        FigureExtension.configure({
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
            id: 'QuestionDescription',
        }
    },
    onUpdate: ({ editor }) => {
        emit('setDescriptionData', editor)
        checkContentImages()
    },
})
watch(() => props.content, (o, n) => {
    if (n != null && n.length > 0)
        showDescription.value = true
    if (o != n && props.isInit)
        editor.value?.commands.setContent(n)
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
</script>

<template>
    <div v-if="showDescription && editor">
        <EditorMenuBar :editor="editor" @handle-undo-redo="checkContentImages" />
        <editor-content :editor="editor" />
    </div>
    <template v-else>
        <div class="d-flex">
            <div class="btn grey-bg form-control col-md-6" @click="showDescription = true">
                {{ t('editor.placeholderAdditions') }}
            </div>
            <div class="col-sm-12 hidden-xs"></div>
        </div>
    </template>
</template>