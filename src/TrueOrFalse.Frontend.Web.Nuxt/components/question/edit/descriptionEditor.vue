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

interface Props {
    highlightEmptyFields: boolean
    content: string
}
const props = defineProps<Props>()
const alertStore = useAlertStore()

const emit = defineEmits(['setDescriptionData'])
const showDescription = ref(false)
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
        attributes: {
            id: 'QuestionInputField',
        }
    },
    onUpdate: ({ editor }) => {
        emit('setDescriptionData', editor)
    },
})
watch(() => props.content, (o, n) => {
    if (n != null && n.length > 0)
        showDescription.value = true
    if (o != n)
        editor.value?.commands.setContent(n)
})

</script>

<template>
    <div v-if="showDescription && editor">
        <EditorMenuBar :editor="editor" />
        <editor-content :editor="editor" />
    </div>
    <template v-else>
        <div class="d-flex">
            <div class="btn grey-bg form-control col-md-6" @click="showDescription = true">
                Ergänzungen hinzufügen</div>
            <div class="col-sm-12 hidden-xs"></div>
        </div>
    </template>
</template>