<script lang="ts" setup>
import { useEditor, EditorContent, JSONContent } from '@tiptap/vue-3'
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

import { CustomHeading } from '~/components/shared/headingExtension'
import { useOutlineStore } from '~/components/sidebar/outlineStore'
import { slugify } from '~/components/shared/utils'
import { nanoid } from 'nanoid'

const alertStore = useAlertStore()
const topicStore = useTopicStore()
const outlineStore = useOutlineStore()
const lowlight = createLowlight(all)

const currentNodeIndex = ref()

const editor = useEditor({
    content: topicStore.initialContent,
    extensions: [
        StarterKit.configure({
            heading: false,
            codeBlock: false,
        }),
        CustomHeading.configure({
            levels: [2, 3, 4],
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

        const contentArray: JSONContent[] | undefined = editor.getJSON().content
        if (contentArray)
            outlineStore.setHeadings(contentArray)

        if (editor.isActive('heading'))
            updateHeadingIds()
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

topicStore.$onAction(({ name, after }) => {
    after(() => {
        if (name == 'resetContent')
            editor.value?.commands.setContent(topicStore.content)
    })
})

function updateCursorIndex() {
    console.log('updatecursorIndex')
    if (editor.value == null)
        return

    const cursorIndex = editor.value.state.selection.from // Get cursor position

    // Get the node at the cursor position
    const resolvedPos = editor.value.state.doc.resolve(editor.value.state.selection.from);
    const nIndex = resolvedPos.index(0) || 0; // Path gives us the node indexes

    console.log('N index:', nIndex, 'Cursor index:', cursorIndex)
}

function updateHeadingIds() {
    if (editor.value == null)
        return

    const { state, commands } = editor.value
    state.doc.descendants((node: any, pos: number) => {
        if (node.type.name === 'heading') {
            const textContent = node.textContent
            const newId = slugify(textContent) + `-${nanoid(5)}`
            if (node.attrs.id == null) {
                commands.updateAttributes(node.type.name, { id: newId })
            }
        }
    })
}

const spinnerStore = useSpinnerStore()

onMounted(() => {
    spinnerStore.hideSpinner()

    const contentArray: JSONContent[] | undefined = editor.value?.getJSON().content
    if (contentArray)
        outlineStore.setHeadings(contentArray)

    if (editor.value) {
        editor.value.on('focus', () => {
            outlineStore.editorIsFocused = true
        })

        editor.value.on('blur', () => {
            outlineStore.editorIsFocused = false
        })
    }
})

</script>

<template>
    <template v-if="editor">
        <EditorMenuBar :editor="editor" :heading="true" :is-topic-content="true" />
        <editor-content :editor="editor" class="col-xs-12" @click="updateCursorIndex" />
    </template>
</template>

<style lang="less">
.ProseMirror {
    .content-placeholder {
        :deep(p:empty) {
            min-height: 20px;
        }
    }

    h2,
    h3,
    h4 {
        scroll-margin-top: 10rem;
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
