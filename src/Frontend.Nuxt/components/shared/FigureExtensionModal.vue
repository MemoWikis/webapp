<script setup lang="ts">
import { useFigureExtensionStore } from './figureExtensionStore'
import { useEditor, EditorContent } from '@tiptap/vue-3'
import StarterKit from '@tiptap/starter-kit'
import Link from '@tiptap/extension-link'
import Placeholder from '@tiptap/extension-placeholder'
import { usePageStore } from '~/components/page/pageStore'

const figureExtensionStore = useFigureExtensionStore()
const { t } = useI18n()

const pageStore = usePageStore()

// Caption editor with basic text and links
const captionEditor = useEditor({
    editable: pageStore.canEdit,
    extensions: [
        StarterKit.configure({
            // Disable block elements - only allow inline text formatting
            heading: false,
            codeBlock: false,
            blockquote: false,
            bulletList: false,
            orderedList: false,
            listItem: false,
            horizontalRule: false,
        }),
        Link.configure({
            HTMLAttributes: {
                target: '_blank',
                rel: 'noopener noreferrer nofollow'
            }
        }),
        Placeholder.configure({
            placeholder: t('image.licenseInfo.captionPlaceholder')
        })
    ],
    content: '',
    editorProps: {
        attributes: {
            class: 'tiptap-editor caption-editor'
        }
    }
})

// License editor with basic text and links
const licenseEditor = useEditor({
    editable: pageStore.canEdit,
    extensions: [
        StarterKit.configure({
            // Disable block elements - only allow inline text formatting
            heading: false,
            codeBlock: false,
            blockquote: false,
            bulletList: false,
            orderedList: false,
            listItem: false,
            horizontalRule: false,
        }),
        Link.configure({
            HTMLAttributes: {
                target: '_blank',
                rel: 'noopener noreferrer nofollow'
            }
        }),
        Placeholder.configure({
            placeholder: () => pageStore.canEdit ? t('image.licenseInfo.licensePlaceholder') : ''
        })
    ],
    content: '',
    editorProps: {
        attributes: {
            class: 'tiptap-editor license-editor'
        }
    }
})

watch(() => figureExtensionStore.showEdit, (show) => {
    if (show && captionEditor.value && licenseEditor.value) {
        // Set initial content when modal opens
        captionEditor.value.commands.setContent(figureExtensionStore.caption || '')
        licenseEditor.value.commands.setContent(figureExtensionStore.license || '')
    }
}, { immediate: true })

const handleSave = () => {
    // Check if editors are empty using TipTap's isEmpty method
    const captionIsEmpty = captionEditor.value?.isEmpty ?? true
    const licenseIsEmpty = licenseEditor.value?.isEmpty ?? true

    // Get HTML content only if not empty, otherwise use null
    const cleanCaption = captionIsEmpty ? null : captionEditor.value?.getHTML()?.trim() || null
    const cleanLicense = licenseIsEmpty ? null : licenseEditor.value?.getHTML()?.trim() || null

    figureExtensionStore.saveEdit({
        caption: cleanCaption,
        license: cleanLicense
    })
}

const handleCancel = () => {
    figureExtensionStore.closeEditModal()
}

// Cleanup editors on unmount
onBeforeUnmount(() => {
    captionEditor.value?.destroy()
    licenseEditor.value?.destroy()
})
</script>

<template>
    <LazyModal
        :show="figureExtensionStore.showEdit"
        @close="handleCancel"
        :show-close-button="true"
        :primary-btn-label="pageStore.canEdit ? t('label.save') : undefined"
        :show-cancel-btn="pageStore.canEdit"
        @primary-btn="handleSave"
        @secondary-btn="handleCancel"
        @keydown.esc="handleCancel">

        <template v-slot:header>
            <h2>
                {{ pageStore.canEdit ? t('image.licenseInfo.editTitle') : t('image.licenseInfo.viewTitle') }}
            </h2>
        </template>

        <template v-slot:body>
            <div class="edit-license-container">
                <!-- Image Preview -->
                <div class="image-preview" v-if="figureExtensionStore.imageUrl">
                    <img
                        :src="figureExtensionStore.imageUrl"
                        :alt="figureExtensionStore.imageAlt || ''"
                        class="license-modal-image" />
                </div>

                <div class="form-group" v-if="pageStore.canEdit || figureExtensionStore.caption">
                    <label class="form-label">
                        {{ t('image.licenseInfo.caption') }}:
                    </label>
                    <div class="editor-wrapper">
                        <EditorContent :editor="captionEditor" class="tiptap-field" />
                    </div>
                </div>

                <div class="form-group">
                    <label class="form-label">
                        {{ t('image.licenseInfo.license') }}:
                    </label>
                    <div class="editor-wrapper">
                        <EditorContent :editor="licenseEditor" class="tiptap-field" v-if="pageStore.canEdit || figureExtensionStore.license" />
                        <div v-else class="tiptap-editor license-editor" contenteditable="false">
                            {{ t('image.licenseInfo.noLicense') }}
                        </div>
                    </div>
                </div>
            </div>
        </template>
    </LazyModal>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.edit-license-container {
    .image-preview {
        margin-bottom: 20px;
        text-align: center;

        .license-modal-image {
            max-width: 100%;
            max-height: auto;
        }
    }

    .form-group {
        margin-bottom: 20px;

        .form-label {
            display: block;
            margin-bottom: 5px;
            font-weight: 600;
            color: @memo-grey-darker;
            font-size: 14px;
        }

        .editor-wrapper {
            border: 1px solid @memo-grey-light;
            transition: border-color 0.2s;

            &:focus-within {
                border-color: @memo-green;
            }

            // Red border when editor is not editable
            &:has([contenteditable="false"]) {
                border-color: transparent;
            }
        }
    }
}

// TipTap editor styles
:deep(.tiptap-editor) {
    padding: 8px 12px;
    font-family: inherit;
    font-size: 14px;
    line-height: 1.4;
    outline: none;
    border: none;
    background: transparent;

    &.caption-editor,
    &.license-editor {

        // Not editable state
        &[contenteditable="false"] {
            background-color: @memo-grey-lightest;
            color: @memo-grey-darker;
            cursor: not-allowed;
        }
    }

    // Placeholder styles
    .is-editor-empty:first-child::before {
        color: @memo-grey;
        content: attr(data-placeholder);
        float: left;
        height: 0;
        pointer-events: none;
    }

    // Link styles
    a {
        color: @memo-blue-link;
        text-decoration: underline;

        &:hover {
            text-decoration: none;
        }
    }

    // Basic text formatting styles
    strong {
        font-weight: bold;
    }

    em {
        font-style: italic;
    }

    code {
        background: @memo-grey-lightest;
        border-radius: 3px;
        padding: 2px 4px;
        font-family: 'Courier New', monospace;
        font-size: 0.9em;
    }

    // Ensure proper line breaks
    p {
        margin: 0;

        &:not(:last-child) {
            margin-bottom: 0.5em;
        }
    }
}
</style>
