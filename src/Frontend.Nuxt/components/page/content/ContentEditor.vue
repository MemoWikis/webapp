<script lang="ts" setup>
import { Editor, EditorContent, JSONContent } from '@tiptap/vue-3'
import { ReplaceStep, ReplaceAroundStep } from 'prosemirror-transform'
import StarterKit from '@tiptap/starter-kit'
import Placeholder from '@tiptap/extension-placeholder'
import Underline from '@tiptap/extension-underline'
import CodeBlockLowlight from '@tiptap/extension-code-block-lowlight'
import TaskList from '@tiptap/extension-task-list'
import TaskItem from '@tiptap/extension-task-item'
import History from '@tiptap/extension-history'
import { all, createLowlight } from 'lowlight'
import ImageResize from '~~/components/shared/imageResizeExtension'
import { Indent } from '../../editor/indent'
import UploadImage from '~/components/shared/imageUploadExtension'

import { usePageStore } from '~/components/page/pageStore'
import { useLoadingStore } from '~/components/loading/loadingStore'
import { isEmpty } from 'underscore'

import { getRandomColor } from '~/utils/utils'

import { CustomHeading } from '~/components/shared/headingExtension'
import { CustomLink } from '~/components/shared/linkExtension'

import { useOutlineStore } from '~/components/sidebar/outlineStore'
import { slugify } from '~/utils/utils'
import { nanoid } from 'nanoid'

import Collaboration from '@tiptap/extension-collaboration'
import CollaborationCursor from '@tiptap/extension-collaboration-cursor'

import * as Y from 'yjs'
import { HocuspocusProvider } from '@hocuspocus/provider'
import { FontSize, useUserStore } from '~/components/user/userStore'
import { IndexeddbPersistence } from 'y-indexeddb'
import { Visibility } from '~/components/shared/visibilityEnum'
import { SnackbarData, useSnackbarStore } from '~/components/snackBar/snackBarStore'

const pageStore = usePageStore()
const outlineStore = useOutlineStore()
const snackbarStore = useSnackbarStore()

const lowlight = createLowlight(all)
const userStore = useUserStore()
const doc = new Y.Doc()
const config = useRuntimeConfig()

const providerContentLoaded = ref(false)

const provider = shallowRef<HocuspocusProvider>()
const editor = shallowRef<Editor>()
const loadCollab = ref(true)
const providerLoaded = ref(false)

const connectionLostHandled = ref(false)

const handleConnectionLost = () => {
    if (connectionLostHandled.value)
        return

    const data: SnackbarData = {
        type: 'error',
        text: { message: t('error.collaboration.connectionLost') },
        duration: 8000
    }
    snackbarStore.showSnackbar(data)

    reconnectTimer.value = setTimeout(() => {
        tryReconnect()
    }, 10000)
}

const reconnectTimer = ref()
const isReconnecting = ref(false)
const isSynced = ref(false)
const tryReconnect = () => {
    if (reconnectTimer.value)
        clearTimeout(reconnectTimer.value)
    if (isReconnecting.value || isSynced.value)
        return

    isReconnecting.value = true

    if (provider.value)
        provider.value?.destroy()

    if (userStore.isLoggedIn && !isSynced.value)
        initProvider()

    isReconnecting.value = false
}

const initProvider = () => {
    // shareToken is the token from the pageSharing feature, 
    // collaborationToken is the token used as an identifier for the hocuspocus server, 
    // a collaboration token will always exist, but the shareToken is optional
    const token = pageStore.shareToken ? `${userStore.collaborationToken}|accessToken=${pageStore.shareToken}` : userStore.collaborationToken
    provider.value = new HocuspocusProvider({
        url: config.public.hocuspocusWebsocketUrl,
        name: `ydoc-${pageStore.id}`,
        token: token,
        document: doc,
        onAuthenticated() {
            new IndexeddbPersistence(`${userStore.id}|document-${pageStore.id}`, doc)
        },
        onAuthenticationFailed: ({ reason }) => {
            isSynced.value = false

            providerLoaded.value = true
            loadCollab.value = false
            recreate()
        },
        onSynced(e) {
            if (!doc.getMap('config').get('initialContentLoaded') && editor.value) {
                doc.getMap('config').set('initialContentLoaded', true)
                editor.value.commands.setContent(pageStore.initialContent)
            }
            providerContentLoaded.value = true

            if (editor.value) {
                const contentArray: JSONContent[] | undefined = editor.value.getJSON().content
                if (contentArray)
                    outlineStore.setHeadings(contentArray)
            }
            providerLoaded.value = true
            connectionLostHandled.value = false
            isSynced.value = true
        },
        onClose(c) {
            isSynced.value = false
            if (c.event.code === 1006 || c.event.code === 1005 || !providerLoaded.value) {
                providerLoaded.value = true

                handleConnectionLost()

                if (!providerContentLoaded.value) {
                    loadCollab.value = false
                    recreate()
                }
            }
        }
    })
}

const deleteImageSrc = ref()

const { t, locale } = useI18n()

const initEditor = () => {
    editor.value = new Editor({
        content: provider.value?.isSynced ? null : pageStore.initialContent,
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
            CustomLink.configure({
                HTMLAttributes: {
                    rel: 'noreferrer nofollow',
                    target: "_self",
                },
                openOnClick: true
            }),
            Placeholder.configure({
                emptyEditorClass: 'is-editor-empty',
                emptyNodeClass: 'is-empty',
                placeholder: t('editor.placeholder'),
                showOnlyWhenEditable: true,
                showOnlyCurrent: true,
            }),
            Underline,
            ImageResize.configure({
                inline: true,
                allowBase64: true
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
                            'data-user': user.name,
                            'padding': '1.4em'
                        }
                    },
                }),
                UploadImage.configure({
                    uploadFn: pageStore.uploadContentImage
                }),
            ] : [History]
        ],
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
                pageStore.addImageUrlToDeleteList(deleteImageSrc.value)

            if (clearDeleteImageSrcRef)
                deleteImageSrc.value = null
        },
        onUpdate({ editor, transaction }) {
            pageStore.contentHasChanged = providerContentLoaded.value
            if (editor.isEmpty)
                pageStore.content = ''
            else
                pageStore.content = editor.getHTML()

            const contentArray: JSONContent[] | undefined = editor.getJSON().content
            if (contentArray)
                outlineStore.setHeadings(contentArray)

            if (editor.isActive('heading'))
                updateHeadingIds()

            updateCursorIndex()

            if (pageStore.contentHasChanged)
                autoSave()

            pageStore.text = editor.getText()
        },
        editorProps: {
            handlePaste: (view, pos, event) => {
                const firstNode = event.content.firstChild
                if (firstNode != null && firstNode.type.name === 'image') {
                    if (!isEmpty(firstNode.attrs)) {
                        const src = firstNode.attrs.src
                        if (src.startsWith('data:image')) {
                            editor.value?.commands.addBase64Image(src)
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
    editor.value.setEditable(pageStore.canEdit)
}

watch(locale, () => {
    if (editor.value && editor.value.isEmpty)
        recreate()
})

const recreate = (login: boolean = false) => {
    provider.value?.destroy()
    editor.value?.destroy()

    if (login)
        loadCollab.value = true

    if (userStore.isLoggedIn && loadCollab.value)
        initProvider()
    else if (!userStore.isLoggedIn)
        providerLoaded.value = true

    initEditor()
}

function setHeadings() {
    const contentArray: JSONContent[] | undefined = editor.value?.getJSON().content
    if (contentArray)
        outlineStore.setHeadings(contentArray)
}

pageStore.$onAction(({ name, after }) => {
    after(async () => {
        if (name === 'reset') {
            editor.value?.commands.setContent(pageStore.content)
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
                commands.updateAttributes('heading', { id: newId })
            }
        }
    })
}

const checkContentImages = () => {
    if (editor.value == null)
        return

    const { state } = editor.value
    pageStore.uploadedImagesInContent = []
    state.doc.descendants((node: any, pos: number) => {
        if (node.type.name === 'uploadImage') {
            const src = node.attrs.src
            if (src.startsWith('/Images/'))
                pageStore.uploadedImagesInContent.push(src)
        }
    })

    pageStore.refreshDeleteImageList()
}

const loadingStore = useLoadingStore()

onMounted(() => {
    recreate()

    loadingStore.stopLoading()
    setHeadings()

    if (editor.value) {
        editor.value.on('focus', () => {
            outlineStore.editorIsFocused = true
        })

        editor.value.on('selectionUpdate', updateCursorIndex)

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

watch(() => userStore.isLoggedIn, (val) => recreate(val))

const autoSaveTimer = ref()
const deletePageContentImageTimer = ref()
const autoSave = () => {
    if (pageStore.visibility != Visibility.Private)
        return

    if (autoSaveTimer.value)
        clearTimeout(autoSaveTimer.value)

    if (deletePageContentImageTimer.value)
        clearTimeout(deletePageContentImageTimer.value)

    autoSaveTimer.value = setTimeout(() => {
        if (editor.value) {
            pageStore.saveContent()
        }
    }, 3000)

    deletePageContentImageTimer.value = setTimeout(() => {
        pageStore.deletePageContentImages()
    }, 10000)
}

const { isMobile } = useDevice()
const createFlashcard = () => {
    if (editor.value == null)
        return

    const { state } = editor.value
    const { selection } = state
    if (selection.empty)
        pageStore.generateFlashcard()
    else {
        const { from, to } = selection
        const text = state.doc.textBetween(from, to)
        pageStore.generateFlashcard(text)
    }
}



</script>

<template>
    <template v-if="editor && providerLoaded">
        <LazyEditorMenuBar v-if="loadCollab && userStore.isLoggedIn && editor" :editor="editor" :heading="true" :is-page-content="true" @handle-undo-redo="checkContentImages" class="page-content-menubar">
            <template v-slot:start v-if="userStore.isAdmin">
                <button class="menubar__button ai-create" @mousedown="createFlashcard">
                    <font-awesome-icon :icon="['fas', 'wand-magic-sparkles']" />
                </button>

                <div class="menubar__divider__container">
                    <div class="menubar__divider"></div>
                </div>
            </template>
        </LazyEditorMenuBar>
        <LazyEditorMenuBar v-else-if="editor" :editor="editor" :heading="true" :is-page-content="true" />
        <editor-content :editor="editor" class="" :class="{ 'small-font': userStore.fontSize === FontSize.Small, 'large-font': userStore.fontSize === FontSize.Large }" />
    </template>
    <div v-else class="" :class="{ 'private-page': pageStore.visibility === Visibility.Private, 'small-font': userStore.fontSize === FontSize.Small, 'large-font': userStore.fontSize === FontSize.Large }">
        <div class="ProseMirror content-placeholder" v-html="pageStore.content" id="PageContentPlaceholder" :class="{ 'is-mobile': isMobile }">
        </div>
    </div>

</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.page-content-menubar {
    padding: 0;
}

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

        &:focus,
        &:focus-visible {
            outline: none;
            border: none;
            box-shadow: none;
        }
    }
}

@font-size-h2-mem: 2.5rem;
@font-size-h3-mem: 2.1rem;
@font-size-h4-mem: 1.8rem;

#PageContent {
    .small-font {

        p {
            font-size: 16px;
        }

        .media-below-sm({
            font-size: 12px;

        });
}

.large-font {
    h2 {
        font-size: 2.6rem;
    }

    h3 {
        font-size: 2.3rem;
    }

    h4 {
        font-size: 2.1rem;
    }

    p {
        font-size: 20px;
    }

    .media-below-sm({
        font-size: 16px;
    })
}

&.is-mobile {
    h3 {
        font-size: 2.15rem;
    }

    h2 {
        font-size: 2.4rem;
    }

    .small-font {
        p {
            font-size: 14px;
        }

        h3 {
            font-size: 2rem;
        }

        h2 {
            font-size: 2.2rem;
        }
    }

    .large-font {
        h3 {
            font-size: 2.3rem;
        }

        h2 {
            font-size: 2.6rem;
        }
    }

}
}

.private-page {
    margin-bottom: -30px;
}

.menubar__button {
    color: @memo-blue-link;

    .fa-wand-magic-sparkles {
        color: @memo-blue-link;
    }
}
</style>
