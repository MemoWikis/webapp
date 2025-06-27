<script setup lang="ts">
import { useLicenseLinkModalStore } from './licenseLinkModalStore'

const licenseLinkModalStore = useLicenseLinkModalStore()
const { t } = useI18n()
</script>

<template>
    <LazyModal :show="licenseLinkModalStore.show" @close="licenseLinkModalStore.closeModal()" :show-close-button="true"
        @keydown.esc="licenseLinkModalStore.closeModal()">
        <template v-slot:header>
            <h2>
                {{ t('answerbody.details.licenseInfoHeader') }}
                <br />

                {{ licenseLinkModalStore.license.shortText }}
            </h2>
        </template>
        <template v-slot:body>
            <div class="license-container">
                <div class="license-info">
                    <template v-if="licenseLinkModalStore.license.isDefault">
                        <div class="author-info" v-if="licenseLinkModalStore.creator.id > 0">
                            <span class="info-label">{{ t('answerbody.details.author') }}:</span>
                            <NuxtLink :to="$urlHelper.getUserUrl(licenseLinkModalStore.creator.name, licenseLinkModalStore.creator.id)">
                                {{ licenseLinkModalStore.creator.name }}
                            </NuxtLink>
                        </div>
                        <div class="license-image-container">
                            <Image src="/Images/Licenses/cc-by 88x31.png" :width="88" />
                        </div>
                    </template>
                    <div class="license-text" v-if="licenseLinkModalStore.license.fullText.length > 0" v-html="licenseLinkModalStore.license.fullText">
                    </div>
                </div>
            </div>
        </template>
        <template v-slot:footer>
        </template>
    </LazyModal>
</template>

<style lang="less" scoped>
.license-container {
    display: flex;
    flex-direction: column;
    gap: 15px;

    .license-info {
        .author-info {
            font-size: 1.4rem;
            margin-bottom: 15px;

            .info-label {
                font-weight: bold;
                margin-right: 5px;
            }
        }

        .license-image-container {
            display: flex;
            justify-content: center;
            margin: 15px 0;
        }

        .license-text {
            font-size: 1.4rem;
        }
    }
}
</style>
