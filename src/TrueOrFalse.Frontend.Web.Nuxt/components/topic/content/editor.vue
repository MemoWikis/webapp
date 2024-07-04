<script lang="ts" setup>
import { useEditor, EditorContent } from '@tiptap/vue-3'
import StarterKit from '@tiptap/starter-kit'
import Link from '@tiptap/extension-link'
import Placeholder from '@tiptap/extension-placeholder'
import Underline from '@tiptap/extension-underline'
import CodeBlockLowlight from '@tiptap/extension-code-block-lowlight'
import TaskList from '@tiptap/extension-task-list'
import TaskItem from '@tiptap/extension-task-item'
import { all, createLowlight } from 'lowlight'
import { useTopicStore } from '~~/components/topic/topicStore'
import { useSpinnerStore } from '~~/components/spinner/spinnerStore'
import { useAlertStore, AlertType } from '~~/components/alert/alertStore'
import { isEmpty } from 'underscore'
import { messages } from '~~/components/alert/alertStore'
import { Indent } from '../../editor/indent'
import ImageResize from '~~/components/shared/imageResizeExtension'
import { slugify } from '~~/components/shared/utils'

import { CustomHeading } from '~/components/shared/headingExtension'

const alertStore = useAlertStore()
const topicStore = useTopicStore()
const lowlight = createLowlight(all)

const editor = useEditor({
    content: topicStore.initialContent,
    extensions: [
        StarterKit.configure({
            heading: false,
            codeBlock: false,
        }),
        CustomHeading.configure({
            levels: [1, 2, 3, 4, 5, 6],
            HTMLAttributes: {
                class: 'heading',
            },
        }),
        Link.configure({
            HTMLAttributes: {
                rel: 'noopener noreferrer nofollow'
            },
            openOnClick: true,
        }),
        Placeholder.configure({
            emptyEditorClass: 'is-editor-empty',
            emptyNodeClass: 'is-empty',
            placeholder: 'Klicke hier um zu tippen ...',
            showOnlyWhenEditable: true,
            showOnlyCurrent: true,
        }),
        Underline,
        ImageResize.configure({
            inline: true,
            allowBase64: true,
        }),
        CodeBlockLowlight.configure({
            lowlight,
        }),
        TaskList,
        TaskItem.configure({
            nested: true,
        }),
        Indent,

    ],
    onUpdate({ editor }) {
        topicStore.contentHasChanged = true
        if (editor.isEmpty)
            topicStore.content = ''
        else
            topicStore.content = editor.getHTML()
    },
    editorProps: {
        handlePaste: (view, pos, event) => {
            const firstNode = event.content.firstChild
            if (firstNode != null && firstNode.type.name == 'image') {
                if (!isEmpty(firstNode.attrs)) {
                    let src = firstNode.attrs.src;
                    if (src.length > 1048576 && src.startsWith('data:image')) {
                        alertStore.openAlert(AlertType.Error, { text: messages.error.image.tooBig })
                        return true
                    }
                }
            }

        },
        attributes: {
            id: 'InlineEdit',
        }
    },
})

editor.value?.on('transaction', ({ transactions }: any) => {
    const shouldUpdate = transactions.some((tr: { docChanged: any }) => tr.docChanged)
    if (shouldUpdate) {
        updateHeadingIds()
    }
})

const updateHeadingIds = () => {
    console.log('--uuupdate')
    if (editor.value) {
        const { state, commands } = editor.value
        state.doc.descendants((node, pos) => {
            if (node.type.name === 'heading') {
                const textContent = node.textContent
                const newId = slugify(textContent)
                if (node.attrs.id !== newId) {
                    commands.updateAttributes('heading', { id: newId })
                }
            }
        })
    }
}

topicStore.$onAction(({ name, after }) => {
    after(() => {
        if (name == 'resetContent')
            editor.value?.commands.setContent(topicStore.content)
    })
})

const spinnerStore = useSpinnerStore()
onMounted(() => {
    spinnerStore.hideSpinner()
    updateHeadingIds()

})
</script>

<template>
    <template v-if="editor">
        <EditorMenuBar :editor="editor" :heading="true" :is-topic-content="true" />
        <editor-content :editor="editor" class="col-xs-12" />
    </template>
</template>

<style lang="less">
.ProseMirror {
    .content-placeholder {
        :deep(p:empty) {
            min-height: 20px;
        }
    }

    ul[data-type="taskList"] {
        list-style: none;
        padding: 0;

        p {
            margin: 0;
        }

        li {
            display: flex;

            >label {
                flex: 0 0 auto;
                margin-right: 0.5rem;
                user-select: none;
            }

            >div {
                flex: 1 1 auto;
            }

            ul li,
            ol li {
                display: list-item;
            }

            ul[data-type="taskList"]>li {
                display: flex;
            }
        }
    }
}
</style>
