<script lang="ts" setup>
import { useEditor, EditorContent } from '@tiptap/vue-3'
import StarterKit from '@tiptap/starter-kit'
import Link from '@tiptap/extension-link'
import Placeholder from '@tiptap/extension-placeholder'
import Underline from '@tiptap/extension-underline'
import CodeBlockLowlight from '@tiptap/extension-code-block-lowlight'
import { all, createLowlight } from 'lowlight'
import { isEmpty } from 'underscore'
import FigureExtension from '~~/components/shared/figureExtension'
import { ReplaceStep, ReplaceAroundStep } from 'prosemirror-transform'
import UploadImage from '~/components/shared/imageUploadExtension'
import { useEditQuestionStore } from './editQuestionStore'

interface Props {
    highlightEmptyFields: boolean
    solution?: string
    isInit: boolean
}

const props = defineProps<Props>()
const emit = defineEmits(['setFlashcardContent'])
const editQuestionStore = useEditQuestionStore()

const content = ref(null)
const lowlight = createLowlight(all)
const deleteImageSrc = ref<string | null>(null)
const { t } = useI18n()
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
            placeholder: t('editor.placeholderFlashcardBack'),
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
    content: content.value,
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
        setFlashcardContent()
        checkContentImages()
    },
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
    }
})

function initSolution() {
    if (props.solution && props.solution.trim() != editor.value?.getHTML().trim() && props.isInit) {
        editor.value?.commands.setContent(props.solution)
        setFlashcardContent()
    }
}
onMounted(() => initSolution())
watch(() => props.solution, () => initSolution())

function setFlashcardContent() {
    if (editor.value) {
        const content = {
            solution: editor.value.getHTML(),
            solutionIsValid: editor.value.state.doc.textContent.length > 0
        }
        emit('setFlashcardContent', content)
    }
}

function clearFlashcard() {
    editor.value?.commands.setContent('')
}

defineExpose({ clearFlashcard })

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
    <div class="input-container">
        <div class="overline-s no-line">Antwort</div>
        <template v-if="editor">
            <EditorMenuBar :editor="editor" @handle-undo-redo="checkContentImages" />
            <editor-content :editor="editor"
                :class="{ 'is-empty': highlightEmptyFields && editor.state.doc.textContent.length <= 0 }" />
        </template>
        <div v-if="highlightEmptyFields && editor && editor.state.doc.textContent.length <= 0" class="field-error">
            {{ t('editor.answerInputError') }}
        </div>
    </div>
</template>