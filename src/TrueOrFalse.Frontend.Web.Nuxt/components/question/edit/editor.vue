<script lang="ts" setup>
import { useEditor, EditorContent } from '@tiptap/vue-3'
import StarterKit from '@tiptap/starter-kit'
import Link from '@tiptap/extension-link'
import Placeholder from '@tiptap/extension-placeholder'
import Underline from '@tiptap/extension-underline'
import CodeBlockLowlight from '@tiptap/extension-code-block-lowlight'
import { Indent } from '../../editor/indent'
import { all, createLowlight } from 'lowlight'
import { useEditQuestionStore } from './editQuestionStore'
import { ReplaceStep, ReplaceAroundStep } from 'prosemirror-transform'

interface Props {
    highlightEmptyFields: boolean
    content: string
    isInit: boolean
}

const editQuestionStore = useEditQuestionStore()

const props = defineProps<Props>()

const emit = defineEmits(['setQuestionData'])
const lowlight = createLowlight(all)
const deleteImageSrc = ref<string | null>(null)
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
            placeholder: t('editor.placeholderQuestion'),
            showOnlyCurrent: true,
        }),
        Indent,
    ],
    editorProps: {
        attributes: {
            id: 'QuestionEditor',
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

onMounted(async () => {
    editor.value?.commands.setContent(props.content)
    await nextTick()
    emit('setQuestionData', editor.value)
})

watch(() => props.content, (c) => {
    if (c != editor.value?.getHTML() && props.isInit)
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
        <EditorMenuBar :editor="editor" :allow-images="false" @handle-undo-redo="checkContentImages" />
        <editor-content :editor="editor"
            :class="{ 'is-empty': props.highlightEmptyFields && editor.state.doc.textContent.length <= 0 }" />
        <div v-if="props.highlightEmptyFields && editor.state.doc.textContent.length <= 0" class="field-error">
            Bitte formuliere eine Frage.
        </div>
    </div>
</template>