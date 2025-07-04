<script setup lang="ts">
import { useTiptapImageLicenseStore } from './tiptapImageLicenseStore'
import { useUserStore } from '~/components/user/userStore'

const tiptapImageLicenseStore = useTiptapImageLicenseStore()
const userStore = useUserStore()
const { t } = useI18n()

const handleEdit = () => {
    // Close the view modal first
    tiptapImageLicenseStore.closeModal()

    // Open the edit modal with the same data
    tiptapImageLicenseStore.openEditModal(
        tiptapImageLicenseStore.caption,
        tiptapImageLicenseStore.license,
        tiptapImageLicenseStore.imageUrl,
        tiptapImageLicenseStore.imageAlt,
        () => {
            // Callback function - could be used to refresh data if needed
            // For now, we don't need to do anything as the edit modal handles updates
        }
    )
}
</script>

<template>
    <LazyModal
        :show="tiptapImageLicenseStore.show"
        @close="tiptapImageLicenseStore.closeModal()"
        :show-close-button="true"
        :secondary-btn-label="userStore.isLoggedIn ? t('label.edit') : undefined"
        @secondary-btn="handleEdit"
        @keydown.esc="tiptapImageLicenseStore.closeModal()">

        <template v-slot:header>
            <h2>
                {{ t('image.licenseInfo.title') }}
            </h2>
        </template>

        <template v-slot:body>
            <div class="image-license-container">
                <div class="image-preview" v-if="tiptapImageLicenseStore.imageUrl">
                    <img
                        :src="tiptapImageLicenseStore.imageUrl"
                        :alt="tiptapImageLicenseStore.imageAlt || ''"
                        class="license-modal-image" />
                </div>

                <div class="license-info">
                    <div class="caption-section" v-if="tiptapImageLicenseStore.caption">
                        <h4>{{ t('image.licenseInfo.caption') }}</h4>
                        <div class="caption-text" v-html="tiptapImageLicenseStore.caption"></div>
                    </div>

                    <div class="license-section">
                        <h4>{{ t('image.licenseInfo.license') }}</h4>
                        <div class="license-text" v-if="tiptapImageLicenseStore.license" v-html="tiptapImageLicenseStore.license">
                        </div>
                        <p class="license-text no-license" v-else>
                            {{ t('image.licenseInfo.noLicense') }}
                        </p>
                    </div>
                </div>
            </div>
        </template>
    </LazyModal>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.image-license-container {
    .image-preview {
        text-align: center;
        margin-bottom: 20px;

        .license-modal-image {
            max-width: 100%;
            max-height: 300px;
            border-radius: 8px;
            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
        }
    }

    .license-info {

        .caption-section,
        .license-section {
            margin-bottom: 15px;

            h4 {
                margin: 0 0 8px 0;
                font-size: 14px;
                font-weight: 600;
                color: @memo-grey-darker;
            }

            .caption-text,
            .license-text {
                margin: 0;
                padding: 8px 12px;
                background: @memo-grey-lightest;
                border-radius: 4px;

                &.no-license {
                    color: darken(@memo-red-wrong, 20%);
                    background: lighten(@memo-red-wrong, 45%);
                }

                // Style HTML content
                :deep(p) {
                    margin: 0;

                    &:not(:last-child) {
                        margin-bottom: 0.5em;
                    }
                }

                :deep(a) {
                    color: @memo-blue-link;
                    text-decoration: underline;

                    &:hover {
                        text-decoration: none;
                    }
                }

                :deep(strong) {
                    font-weight: bold;
                }

                :deep(em) {
                    font-style: italic;
                }

                :deep(code) {
                    background: rgba(0, 0, 0, 0.1);
                    border-radius: 3px;
                    padding: 2px 4px;
                    font-family: 'Courier New', monospace;
                    font-size: 0.9em;
                }
            }
        }
    }
}
</style>
