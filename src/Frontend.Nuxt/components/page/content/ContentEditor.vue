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
import FigureExtension from '~~/components/shared/figureExtension'
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
            FigureExtension.configure({
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
            // NEW APPROACH: Precisely detect when image nodes are actually deleted
            try {
                if (transaction.steps.length > 0) {
                    transaction.steps.forEach((step, stepIndex) => {
                        if (step instanceof ReplaceStep) {
                            console.log(`ContentEditor.onTransaction: ReplaceStep ${stepIndex} - from: ${step.from}, to: ${step.to}`)

                            // Get the content that was replaced/removed
                            const oldDoc = transaction.before
                            if (step.from < oldDoc.content.size && step.to <= oldDoc.content.size) {
                                const removedSlice = oldDoc.slice(step.from, step.to)

                                // Check if the removed content contains any uploadImage nodes
                                removedSlice.content.descendants((node, pos) => {
                                    if (node.type.name === 'uploadImage') {
                                        const imageSrc = node.attrs.src
                                        if (imageSrc && imageSrc.startsWith('/Images/')) {
                                            console.log('ContentEditor.onTransaction: Detected actual deletion of image:', imageSrc)
                                            pageStore.addImageUrlToDeleteList(imageSrc)
                                        }
                                    }
                                })
                            }
                        }
                    })
                }
            } catch (error) {
                console.warn('ContentEditor.onTransaction: Error in precise image deletion detection:', error)

                // FALLBACK: Use the old logic but make it more restrictive
                let clearDeleteImageSrcRef = true
                const { selection, doc } = transaction

                const node = doc.nodeAt(selection.from)
                if (node && node.type.name === 'uploadImage') {
                    deleteImageSrc.value = node.attrs.src
                    clearDeleteImageSrcRef = false
                    console.log('ContentEditor.onTransaction: FALLBACK - Found uploadImage node at cursor:', deleteImageSrc.value)
                }

                const hasDeleted = transaction.steps.some(step =>
                    step instanceof ReplaceStep || step instanceof ReplaceAroundStep
                )

                // IMPROVED: Only mark for deletion if the transaction was user-initiated (not from collaboration or auto-formatting)
                const isUserInitiated = transaction.getMeta('addToHistory') !== false

                console.log('ContentEditor.onTransaction: FALLBACK - hasDeleted:', hasDeleted, 'deleteImageSrc:', deleteImageSrc.value, 'isUserInitiated:', isUserInitiated)

                if (hasDeleted && deleteImageSrc.value && isUserInitiated) {
                    console.log('ContentEditor.onTransaction: FALLBACK - Adding image to delete list due to user deletion:', deleteImageSrc.value)
                    pageStore.addImageUrlToDeleteList(deleteImageSrc.value)
                }

                if (clearDeleteImageSrcRef)
                    deleteImageSrc.value = null
            }
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
                if (firstNode != null && (firstNode.type.name === 'image' || firstNode.type.name === 'figure')) {
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

const checkContentImages = async () => {
    if (editor.value == null) {
        console.log('ContentEditor.checkContentImages: Editor is null, skipping')
        return
    }

    console.log('ContentEditor.checkContentImages: Scanning document for images')
    const { state } = editor.value
    pageStore.uploadedImagesInContent = []

    let imageCount = 0
    const imagesToVerify: { src: string, pos: number }[] = []

    // First pass: collect all image URLs from the document
    state.doc.descendants((node: any, pos: number) => {
        if (node.type.name === 'uploadImage') {
            imageCount++
            const src = node.attrs.src
            console.log('ContentEditor.checkContentImages: Found image node:', src, 'at position', pos)
            if (src.startsWith('/Images/')) {
                imagesToVerify.push({ src, pos })
            } else {
                console.log('ContentEditor.checkContentImages: Skipped image (not /Images/ path):', src)
            }
        }
    })

    console.log('ContentEditor.checkContentImages: Found', imageCount, 'total image nodes,', imagesToVerify.length, 'images to verify')

    // Second pass: verify each image actually exists on the server
    const brokenImages = []

    if (imagesToVerify.length > 0) {
        try {
            console.log('ContentEditor.checkContentImages: Verifying', imagesToVerify.length, 'images via backend API')
            const { $api } = useNuxtApp()
            const verificationResults = await ($api as any)('/apiVue/PageStore/VerifyImages', {
                method: 'POST',
                body: {
                    pageId: pageStore.id,
                    imageUrls: imagesToVerify.map(img => img.src)
                }
            })

            for (let i = 0; i < imagesToVerify.length; i++) {
                const { src, pos } = imagesToVerify[i]
                const result = verificationResults[i]

                if (result && result.exists) {
                    pageStore.uploadedImagesInContent.push(src)
                    console.log('ContentEditor.checkContentImages: ✓ Image exists:', src)
                } else {
                    brokenImages.push({ src, pos })
                    console.warn('ContentEditor.checkContentImages: ✗ Image missing:', src, result?.reason || 'Unknown reason')
                }
            }
        } catch (error) {
            console.warn('ContentEditor.checkContentImages: Backend verification failed, falling back to HEAD requests:', error)

            // Fallback to HEAD requests if backend API fails
            for (const { src, pos } of imagesToVerify) {
                try {
                    const response = await fetch(src, { method: 'HEAD' })
                    if (response.ok) {
                        pageStore.uploadedImagesInContent.push(src)
                        console.log('ContentEditor.checkContentImages: ✓ Image exists (HEAD):', src)
                    } else {
                        brokenImages.push({ src, pos })
                        console.warn('ContentEditor.checkContentImages: ✗ Image missing (HEAD, HTTP', response.status, '):', src)
                    }
                } catch (headError) {
                    brokenImages.push({ src, pos })
                    console.warn('ContentEditor.checkContentImages: ✗ Image verification failed (HEAD):', src, headError)
                }
            }
        }
    }

    // Handle broken images
    if (brokenImages.length > 0) {
        console.warn('ContentEditor.checkContentImages: Found', brokenImages.length, 'broken images:', brokenImages.map(img => img.src))

        await handleBrokenImages(brokenImages)
    }

    console.log('ContentEditor.checkContentImages: Final uploadedImagesInContent:', pageStore.uploadedImagesInContent)
    console.log('ContentEditor.checkContentImages: Broken images found:', brokenImages.length)

    pageStore.refreshDeleteImageList()
}

const handleBrokenImages = async (brokenImages: { src: string, pos: number }[]) => {
    if (!editor.value) return

    const { t } = useI18n()
    console.log('ContentEditor.handleBrokenImages: Processing', brokenImages.length, 'broken images')

    // Group broken images by handling strategy
    const imagesToRemove = []
    const imagesToReplace = []

    for (const brokenImage of brokenImages) {
        // Check if this is a recently uploaded image (might be recoverable)
        const isRecentUpload = brokenImage.src.includes(`${pageStore.id}_`) &&
            pageStore.contentHasChanged // Content was modified in this session

        if (isRecentUpload) {
            console.log('ContentEditor.handleBrokenImages: Recent upload, attempting recovery:', brokenImage.src)
            // Try to recover recent uploads
            imagesToReplace.push(brokenImage)
        } else {
            console.log('ContentEditor.handleBrokenImages: Old broken image, will remove:', brokenImage.src)
            // Remove old broken images
            imagesToRemove.push(brokenImage)
        }
    }

    // Remove broken images from document
    if (imagesToRemove.length > 0) {
        console.log('ContentEditor.handleBrokenImages: Removing', imagesToRemove.length, 'broken images from document')

        const { state, view } = editor.value
        const tr = state.tr

        // Sort by position in reverse order to avoid position shifts
        const sortedByPosition = imagesToRemove.sort((a, b) => b.pos - a.pos)

        for (const { pos } of sortedByPosition) {
            const node = state.doc.nodeAt(pos)
            if (node && node.type.name === 'uploadImage') {
                const nodeEnd = pos + node.nodeSize
                tr.delete(pos, nodeEnd)
                console.log('ContentEditor.handleBrokenImages: Deleted node at position', pos)
            }
        }

        if (tr.docChanged) {
            view.dispatch(tr)
            console.log('ContentEditor.handleBrokenImages: Applied deletions to document')
        }
    }

    // Handle recent uploads - try to recover or replace with user-friendly message
    if (imagesToReplace.length > 0) {
        console.log('ContentEditor.handleBrokenImages: Processing', imagesToReplace.length, 'recent uploads')

        // Attempt recovery for recent uploads
        const recoveryAttempts = await attemptImageRecovery(imagesToReplace)

        // Replace any remaining broken images with user-friendly message
        const stillBroken = imagesToReplace.filter(img =>
            !recoveryAttempts.some(recovery => recovery.originalSrc === img.src && recovery.success)
        )

        if (stillBroken.length > 0) {
            console.log('ContentEditor.handleBrokenImages: Replacing', stillBroken.length, 'unrecoverable images with placeholders')

            const { state, view } = editor.value
            const tr = state.tr

            for (const { src, pos } of stillBroken) {
                const node = state.doc.nodeAt(pos)
                if (node && node.type.name === 'uploadImage') {
                    // Create a more user-friendly error message
                    const fileName = src.split('/').pop() || 'unknown'
                    const errorText = `[📷 Missing Image: ${fileName}]\n\nThis image file is missing from the server. Please insert a new image to replace it.`

                    // Create a paragraph with the error message
                    const paragraphNode = state.schema.nodes.paragraph.create({}, state.schema.text(errorText))
                    tr.replaceWith(pos, pos + node.nodeSize, paragraphNode)
                    console.log('ContentEditor.handleBrokenImages: Replaced broken image with error message at position', pos)
                }
            }

            if (tr.docChanged) {
                view.dispatch(tr)
                console.log('ContentEditor.handleBrokenImages: Applied replacements to document')
            }
        }
    }

    // Show user notification if any images were handled
    if (brokenImages.length > 0) {
        let message = ''
        let notificationType: 'warning' | 'error' | 'success' = 'warning'

        if (imagesToRemove.length > 0 && imagesToReplace.length > 0) {
            message = `Cleaned up ${imagesToRemove.length} broken image(s) and found ${imagesToReplace.length} recoverable image(s)`
        } else if (imagesToRemove.length > 0) {
            message = `Removed ${imagesToRemove.length} broken image(s) from document`
            notificationType = 'success'
        } else {
            message = `Found ${imagesToReplace.length} missing image(s) - attempting recovery...`
        }

        console.warn('ContentEditor.handleBrokenImages: User notification:', message)

        const snackbarData: SnackbarData = {
            type: notificationType,
            text: { message },
            duration: 8000
        }
        snackbarStore.showSnackbar(snackbarData)
    }
}

const attemptImageRecovery = async (brokenImages: { src: string, pos: number }[]) => {
    console.log('ContentEditor.attemptImageRecovery: Attempting to recover', brokenImages.length, 'images')

    const results = []

    for (const { src, pos } of brokenImages) {
        try {
            // Strategy 1: Check if image exists in browser cache and can be re-uploaded
            const cacheResult = await checkImageInCache(src)
            if (cacheResult.exists && cacheResult.blob) {
                console.log('ContentEditor.attemptImageRecovery: Found image in cache, attempting re-upload:', src)
                const reuploadResult = await reuploadFromCache(src, cacheResult.blob)

                if (reuploadResult.success && reuploadResult.newSrc) {
                    // Update the image src in the document
                    await updateImageSrcInDocument(pos, reuploadResult.newSrc)
                    results.push({ originalSrc: src, success: true, newSrc: reuploadResult.newSrc, method: 'cache' })
                    continue
                }
            }

            // Strategy 2: Check if this was a recently uploaded temp image that can be recovered
            // (This would require tracking temp images differently)

            // Strategy 3: Failed to recover
            console.warn('ContentEditor.attemptImageRecovery: Could not recover image:', src)
            results.push({ originalSrc: src, success: false, error: 'No recovery method available' })

        } catch (error) {
            console.error('ContentEditor.attemptImageRecovery: Error recovering image:', src, error)
            results.push({ originalSrc: src, success: false, error: error instanceof Error ? error.message : 'Unknown error' })
        }
    }

    // Show recovery results to user
    const successful = results.filter(r => r.success).length
    const failed = results.filter(r => !r.success).length

    if (successful > 0 || failed > 0) {
        let message = ''
        let type: 'success' | 'warning' | 'error' = 'success'

        if (successful > 0 && failed === 0) {
            message = `✅ Successfully recovered ${successful} image(s)`
            type = 'success'
        } else if (successful > 0 && failed > 0) {
            message = `⚠️ Recovered ${successful} image(s), ${failed} could not be recovered`
            type = 'warning'
        } else {
            message = `❌ Could not recover ${failed} image(s) - please re-upload manually`
            type = 'error'
        }

        const snackbarData: SnackbarData = {
            type,
            text: { message },
            duration: 6000
        }
        snackbarStore.showSnackbar(snackbarData)
    }

    return results
}

const checkImageInCache = async (src: string) => {
    try {
        // Try to fetch from cache (this will work if image is in browser cache)
        const response = await fetch(src, { cache: 'force-cache' })
        if (response.ok) {
            const blob = await response.blob()
            return { exists: true, blob }
        }
    } catch (error) {
        console.log('ContentEditor.checkImageInCache: Image not in cache:', src)
    }
    return { exists: false, blob: null }
}

const reuploadFromCache = async (originalSrc: string, blob: Blob): Promise<{ success: boolean, newSrc?: string, error?: string }> => {
    try {
        // Convert blob to file
        const fileName = originalSrc.split('/').pop() || 'recovered-image.jpg'
        const file = new File([blob], fileName, { type: blob.type })

        // Use the existing upload mechanism
        const newSrc = await pageStore.uploadContentImage(file)

        console.log('ContentEditor.reuploadFromCache: Successfully re-uploaded:', originalSrc, '->', newSrc)
        return { success: true, newSrc }

    } catch (error) {
        console.error('ContentEditor.reuploadFromCache: Failed to re-upload:', originalSrc, error)
        return { success: false, error: error instanceof Error ? error.message : 'Unknown error' }
    }
}

const updateImageSrcInDocument = async (pos: number, newSrc: string) => {
    if (!editor.value) return

    const { state, view } = editor.value
    const node = state.doc.nodeAt(pos)

    if (node && node.type.name === 'uploadImage') {
        const tr = state.tr
        tr.setNodeMarkup(pos, null, { ...node.attrs, src: newSrc })
        view.dispatch(tr)
        console.log('ContentEditor.updateImageSrcInDocument: Updated image src at position', pos, 'to', newSrc)
    }
}

// Wrapper for sync calls to checkContentImages
const checkContentImagesSync = () => {
    checkContentImages().catch(error => {
        console.error('ContentEditor.checkContentImagesSync: Error checking content images:', error)
    })
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

    console.log('ContentEditor.autoSave: Starting autosave timers for page', pageStore.id)

    if (autoSaveTimer.value)
        clearTimeout(autoSaveTimer.value)

    if (deletePageContentImageTimer.value) {
        console.log('ContentEditor.autoSave: Clearing existing deletePageContentImageTimer')
        clearTimeout(deletePageContentImageTimer.value)
    }

    autoSaveTimer.value = setTimeout(() => {
        if (editor.value) {
            console.log('ContentEditor.autoSave: Executing saveContent after 3s idle')
            pageStore.saveContent()
        }
    }, 3000)

    // TEMPORARILY DISABLED: Automatic deletion timer to prevent false deletions
    // This was causing images to be deleted incorrectly after idle periods
    deletePageContentImageTimer.value = setTimeout(async () => {
        console.log('ContentEditor.autoSave: Automatic deletion timer triggered after 60s idle')
        console.log('ContentEditor.autoSave: Current uploadedImagesMarkedForDeletion:', pageStore.uploadedImagesMarkedForDeletion)
        console.log('ContentEditor.autoSave: Current uploadedImagesInContent:', pageStore.uploadedImagesInContent)

        // Only proceed if there are actually images marked for deletion
        if (pageStore.uploadedImagesMarkedForDeletion.length === 0) {
            console.log('ContentEditor.autoSave: No images marked for deletion, skipping automatic cleanup')
            return
        }

        // Check content images right before deletion to ensure accuracy
        await checkContentImages()

        console.log('ContentEditor.autoSave: After checkContentImages - uploadedImagesMarkedForDeletion:', pageStore.uploadedImagesMarkedForDeletion)

        // Final safety check
        if (pageStore.uploadedImagesMarkedForDeletion.length === 0) {
            console.log('ContentEditor.autoSave: No images marked for deletion after content check, skipping')
            return
        }

        console.log('ContentEditor.autoSave: WARNING - About to delete images after automatic cleanup. This should be rare.')
        pageStore.deletePageContentImages()
    }, 60000)  // Increased to 60s and added more safety checks
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
        <LazyEditorMenuBar v-if="loadCollab && userStore.isLoggedIn && editor" :editor="editor" :heading="true" :is-page-content="true" @handle-undo-redo="checkContentImagesSync" class="page-content-menubar">
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

    &.ProseMirror-focused {

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


<style lang="less">
@import (reference) '~~/assets/includes/imports.less';

.ProseMirror {
    .collaboration-cursor__caret {
        border-left: 1px solid @memo-grey-darkest;
        border-right: 1px solid @memo-grey-darkest;
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
