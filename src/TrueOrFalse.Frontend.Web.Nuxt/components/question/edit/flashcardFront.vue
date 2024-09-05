<script lang="ts" setup>
import { useEditor, EditorContent } from '@tiptap/vue-3'
import StarterKit from '@tiptap/starter-kit'
import Link from '@tiptap/extension-link'
import Placeholder from '@tiptap/extension-placeholder'
import Underline from '@tiptap/extension-underline'
import CodeBlockLowlight from '@tiptap/extension-code-block-lowlight'
import { Indent } from '../../editor/indent'
import { all, createLowlight } from 'lowlight'
import { isEmpty } from 'underscore'
import { useAlertStore } from '../../alert/alertStore'
import ImageResize from '~~/components/shared/imageResizeExtension'
import { ReplaceStep, ReplaceAroundStep } from 'prosemirror-transform'
import UploadImage from '~/components/shared/imageUploadExtension'
import { useEditQuestionStore } from './editQuestionStore'

interface Props {
    highlightEmptyFields: boolean
    content: string
}

const props = defineProps<Props>()
const alertStore = useAlertStore()
const editQuestionStore = useEditQuestionStore()

const emit = defineEmits(['setQuestionData'])
const lowlight = createLowlight(all)
const deleteImageSrc = ref<string | null>(null)

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
            placeholder: 'Gib den Fragetext ein',
            showOnlyCurrent: true,
        }),
        Indent,
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
            id: 'FlashcardEditor',
        }
    },
    onTransaction({ transaction }) {
        let clearDeleteImageSrcRef = true
        const { selection, doc } = transaction

        const node = doc.nodeAt(selection.from)
        if (node && node.type.name === 'uploadImage') {
            deleteImageSrc.value = node.attrs.src
            clearDeleteImageSrcRef = false
        }

        const hasDeleted = transaction.steps.some(step =>
            step instanceof ReplaceStep || step instanceof ReplaceAroundStep
        )

        if (hasDeleted && deleteImageSrc.value)
            editQuestionStore.addImageUrlToDeleteList(deleteImageSrc.value)

        if (clearDeleteImageSrcRef)
            deleteImageSrc.value = null
    },
    onUpdate: ({ editor }) => {
        emit('setQuestionData', editor)
        checkContentImages()
    },
})

onMounted(() => {
    editor.value?.commands.setContent(props.content)
})

watch(() => props.content, (c) => {
    if (c != editor.value?.getHTML())
        editor.value?.commands.setContent(c)
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
    <div v-if="editor">
        <EditorMenuBar :editor="editor" :allow-images="true" @handle-undo-redo="checkContentImages" />
        <editor-content :editor="editor"
            :class="{ 'is-empty': props.highlightEmptyFields && editor.state.doc.textContent.length <= 0 }" />
        <div v-if="props.highlightEmptyFields && editor.state.doc.textContent.length <= 0" class="field-error">
            Bitte formuliere eine Frage.
        </div>
    </div>
</template>