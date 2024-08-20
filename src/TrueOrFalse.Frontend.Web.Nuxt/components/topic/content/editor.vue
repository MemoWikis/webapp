<script lang="ts" setup>
import { useEditor, EditorContent, JSONContent } from '@tiptap/vue-3'
import StarterKit from '@tiptap/starter-kit'
import Link from '@tiptap/extension-link'
import Placeholder from '@tiptap/extension-placeholder'
import Underline from '@tiptap/extension-underline'
import CodeBlockLowlight from '@tiptap/extension-code-block-lowlight'
import TaskList from '@tiptap/extension-task-list'
import TaskItem from '@tiptap/extension-task-item'
import History from '@tiptap/extension-history'
import { all, createLowlight } from 'lowlight'
import ImageResize from '~~/components/shared/imageResizeExtension'
import { Indent } from '../../editor/indent'

import { useTopicStore } from '~~/components/topic/topicStore'
import { useSpinnerStore } from '~~/components/spinner/spinnerStore'
import { useAlertStore, AlertType } from '~~/components/alert/alertStore'
import { isEmpty } from 'underscore'
import { messages } from '~~/components/alert/alertStore'

import { getRandomColor } from '~/components/shared/utils'

import { CustomHeading } from '~/components/shared/headingExtension'
import { useOutlineStore } from '~/components/sidebar/outlineStore'
import { slugify } from '~/components/shared/utils'
import { nanoid } from 'nanoid'

import Collaboration from '@tiptap/extension-collaboration'
import CollaborationCursor from '@tiptap/extension-collaboration-cursor'

import * as Y from 'yjs'
import { TiptapCollabProvider } from '@hocuspocus/provider'
import { useUserStore } from '~/components/user/userStore'
import { IndexeddbPersistence } from 'y-indexeddb'

const alertStore = useAlertStore()
const topicStore = useTopicStore()
const outlineStore = useOutlineStore()
const lowlight = createLowlight(all)
const userStore = useUserStore()
const doc = new Y.Doc() // Initialize Y.Doc for shared editing

const providerContentLoaded = ref(false)
const provider = ref<TiptapCollabProvider | null>(null)

if (userStore.isLoggedIn) {
    provider.value = new TiptapCollabProvider({
        baseUrl: "ws://localhost:3010/collaboration",
        name: 'ydoc-' + topicStore.id,
        token: userStore.collaborationToken,
        preserveConnection: false,
        document: doc,
        onSynced() {
            if (!doc.getMap('config').get('initialContentLoaded') && editor.value) {
                doc.getMap('config').set('initialContentLoaded', true)
                editor.value.commands.setContent(topicStore.initialContent)
                providerContentLoaded.value = true
            }
        },
        onAuthenticationFailed: ({ reason }) => {
            loadCollab.value = false
        },
    })

    // new IndexeddbPersistence(`document-${topicStore.id}`, doc)
}

const loadCollab = ref(userStore.isLoggedIn)

const editor = useEditor({
    content: topicStore.initialContent,
    extensions: [
        StarterKit.configure({
            heading: false,
            codeBlock: false,
            history: false,
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
        ...(userStore.isLoggedIn && loadCollab.value) ? [
            Collaboration.configure({
                document: doc
            }),
            CollaborationCursor.configure({
                provider: provider.value,
                user: {
                    name: userStore.name,
                    color: getRandomColor(),
                    avatar: userStore.imgUrl
                },
            })
        ] : [History]
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

        updateCursorIndex()
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

function setHeadings() {
    const contentArray: JSONContent[] | undefined = editor.value?.getJSON().content
    if (contentArray)
        outlineStore.setHeadings(contentArray)
}

topicStore.$onAction(({ name, after }) => {
    after(async () => {
        if (name == 'resetContent') {
            editor.value?.commands.setContent(topicStore.content)
            setHeadings()
        }
    })
})

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
    setHeadings()

    if (editor.value) {
        editor.value.on('focus', () => {
            outlineStore.editorIsFocused = true
        })

        editor.value.on('selectionUpdate', updateCursorIndex);

        editor.value.on('blur', () => {
            outlineStore.editorIsFocused = false
        })
    }
})

function updateCursorIndex() {
    if (editor.value == null)
        return

    const cursorIndex = editor.value.state.selection.from
    const resolvedPos = editor.value.state.doc.resolve(cursorIndex)
    const nodeIndex = resolvedPos.index(0) || 0
    outlineStore.nodeIndex = nodeIndex
}

</script>

<template>
    <template v-if="editor">
        <EditorMenuBar :editor="editor" :heading="true" :is-topic-content="true" />
        <editor-content :editor="editor" class="col-xs-12" ref="editorRef" />
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

    .collaboration-cursor__caret {
        border-left: 1px solid #0d0d0d;
        border-right: 1px solid #0d0d0d;
        margin-left: -1px;
        margin-right: -1px;
        pointer-events: none;
        position: relative;
        word-break: normal;

        .collaboration-cursor__label {
            border-radius: 3px 3px 3px 0;
            color: #0d0d0d;
            font-size: 12px;
            font-style: normal;
            font-weight: 600;
            left: -1px;
            line-height: normal;
            padding: .1rem .3rem;
            position: absolute;
            top: -1.4em;
            -webkit-user-select: none;
            -moz-user-select: none;
            user-select: none;
            white-space: nowrap;
        }
    }
}
</style>
