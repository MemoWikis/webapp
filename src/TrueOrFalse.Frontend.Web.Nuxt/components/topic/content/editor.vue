<script lang="ts" setup>
import { Editor, EditorContent, JSONContent } from '@tiptap/vue-3'
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
const doc = new Y.Doc()
const config = useRuntimeConfig()

const providerContentLoaded = ref(false)

const provider = shallowRef<TiptapCollabProvider>()
const editor = shallowRef<Editor>()
const loadCollab = ref(true)
const recreate = () => {
    provider.value?.destroy()
    editor.value?.destroy()

    if (userStore.isLoggedIn && loadCollab.value) {
        provider.value = new TiptapCollabProvider({
            baseUrl: config.public.hocuspocusWebsocketUrl,
            name: 'ydoc-' + topicStore.id,
            token: userStore.collaborationToken,
            preserveConnection: false,
            document: doc,
            onAuthenticated() {
                new IndexeddbPersistence(`${userStore.id}|document-${topicStore.id}`, doc)
            },
            onAuthenticationFailed: ({ reason }) => {
                loadCollab.value = false
                recreate()
            },
            onSynced() {
                if (!doc.getMap('config').get('initialContentLoaded') && editor.value) {
                    doc.getMap('config').set('initialContentLoaded', true)
                    editor.value.commands.setContent(topicStore.initialContent)
                    providerContentLoaded.value = true
                }

                if (editor.value) {
                    const contentArray: JSONContent[] | undefined = editor.value.getJSON().content
                    if (contentArray)
                        outlineStore.setHeadings(contentArray)
                }
            },
            onClose(c) {
                if (c.event.code === 1006) {
                    // alertStore.openAlert(AlertType.Error, { text: messages.error.collaboration.connectionLost })
                    if (!providerContentLoaded.value) {
                        loadCollab.value = false
                        recreate()
                    }

                }
            }
        })
    }
    editor.value = new Editor({
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
                    },
                    render: user => {
                        console.debug(user)
                        const cursor = document.createElement('span')
                        cursor.classList.add('collaboration-cursor__caret')
                        cursor.setAttribute('style', `border-color: ${user.color}`)

                        const labelContainer = document.createElement('div')
                        labelContainer.setAttribute('style', `background-color: ${user.color}`)
                        labelContainer.classList.add('collaboration-cursor__label-container')
                        labelContainer.insertBefore(document.createTextNode(user.name), null)

                        const label = document.createElement('div')
                        label.classList.add('collaboration-cursor__label')
                        label.insertBefore(document.createTextNode(user.name), null)

                        labelContainer.insertBefore(label, null)
                        cursor.insertBefore(labelContainer, null)
                        return cursor
                    },
                    selectionRender: user => {
                        return {
                            nodeName: 'span',
                            class: 'collaboration-cursor__selection',
                            style: `background-color: ${user.color}33`,
                            'data-user': user.name, 'padding': '1.4em'
                        }
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
}

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
    recreate()

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

onBeforeUnmount(() => {
    provider.value?.destroy()
    editor.value?.destroy()
})

watch(() => userStore.isLoggedIn, () => recreate())
</script>

<template>
    <template v-if="editor">
        <LazyEditorMenuBar :editor="editor" :heading="true" :is-topic-content="true"
            v-if="loadCollab && userStore.isLoggedIn" />
        <LazyEditorMenuBar :editor="editor" :heading="true" :is-topic-content="true"
            v-else />

        <editor-content :editor="editor" class="col-xs-12" />
    </template>
</template>

<style lang="less">
@import (reference) '~~/assets/includes/imports.less';

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

        .collaboration-cursor__label,
        .collaboration-cursor__label-container {
            border-radius: 4px 4px 4px 0;
            font-size: 14px;
            font-style: normal;
            font-weight: 600;
            left: -1px;
            padding: 0 12px;
            position: absolute;
            user-select: none;
            white-space: nowrap;
            height: 28px;
            display: flex;
            justify-content: center;
            align-items: center;
        }

        .collaboration-cursor__label-container {
            top: -1.4em;
            z-index: 2;
            color: white;
            box-shadow: 0 2px 6px rgb(0 0 0 / 16%);
        }

        .collaboration-cursor__label {
            top: 0px;
            z-index: 3;
            color: @memo-blue;
        }

        .collaboration-cursor__label-container::before {
            border-radius: 4px 4px 4px 0;
            content: '';
            position: absolute;
            top: 0px;
            left: 2px;
            width: calc(100% - 2px);
            height: 100%;
            background: white;
            opacity: 1;
            z-index: 3;
        }

        .collaboration-cursor__label-container::after {
            content: '';
            position: absolute;
            bottom: -4px;
            left: -2px;
            width: 0;
            height: 0;
            border-left: 4px solid transparent;
            border-right: 4px solid transparent;
            border-top: 4px solid transparent;
            border-bottom: 4px solid white;
            transform: rotate(315deg);
            z-index: 2;
        }
    }

    &.ProseMirror-focused {
        .collaboration-cursor__caret {
            opacity: 0.6;
        }
    }
}
</style>
