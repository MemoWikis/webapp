<script setup lang="ts">
import { useLicenseLinkModalStore } from './licenseLinkModalStore'

interface CreatorData {
    id: number
    name: string
}

interface Props {
    licenseId: number
    creator: CreatorData
}

const props = defineProps<Props>()
const licenseLinkModalStore = useLicenseLinkModalStore()
const { t } = useI18n()

// Inline license logic for now (until composable auto-import works properly)
const getLicenseData = (licenseId: number) => {
    switch (licenseId) {
        case 1: // CC BY 4.0 - Default license
            return {
                isDefault: true,
                shortText: 'CC BY 4.0',
                fullText: t('license.ccBy40.displayTextFull'),
                imageUrl: '/Images/Licenses/cc-by 88x31.png'
            }
        case 2: // BAMF
            return {
                isDefault: false,
                shortText: t('license.bamf.displayTextShort'),
                fullText: t('license.bamf.displayTextFull'),
                imageUrl: null
            }
        case 3: // ELWIS
            return {
                isDefault: false,
                shortText: t('license.elwis.displayTextShort'),
                fullText: t('license.elwis.displayTextFull'),
                imageUrl: null
            }
        case 4: // BLAC
            return {
                isDefault: false,
                shortText: t('license.blac.displayTextShort'),
                fullText: t('license.blac.displayTextFull'),
                imageUrl: null
            }
        default:
            return {
                isDefault: true,
                shortText: 'CC BY 4.0',
                fullText: t('license.ccBy40.displayTextFull'),
                imageUrl: '/Images/Licenses/cc-by 88x31.png'
            }
    }
}

const licenseData = computed(() => getLicenseData(props.licenseId))

const openLicenseModal = () => {
    licenseLinkModalStore.openModal({
        isDefault: licenseData.value.isDefault,
        shortText: licenseData.value.shortText,
        fullText: licenseData.value.fullText
    }, props.creator)
}
</script>

<template>
    <div id="license-question">
        <button class="text-link-with-icon" @click="openLicenseModal()" type="button">
            <Image v-if="licenseData.imageUrl" :src="licenseData.imageUrl" :width="60" />
            <div class="text-div">
                <span class="text-span license-link">
                    {{ licenseData.shortText }}
                </span>
                <font-awesome-icon v-if="!licenseData.isDefault" icon="fa-solid fa-circle-info" class="license-info" />
            </div>
        </button>
    </div>
</template>

<style lang="less" scoped>
.text-link-with-icon {
    // Reset button styles
    background: none;
    border: none;
    padding: 0;
    font: inherit;

    // Custom styles
    cursor: pointer;
    transition: opacity 0.2s ease;
    display: flex;
    align-items: center;
    gap: 5px;

    &:hover {
        opacity: 0.8;
    }

    &:focus {
        outline: 2px solid #007acc;
        outline-offset: 2px;
        border-radius: 2px;
    }

    .text-div {
        display: flex;
        align-items: center;
        gap: 5px;

        .license-link {
            color: #007acc;
            text-decoration: underline;

            &:hover {
                text-decoration: none;
            }
        }

        .license-info {
            color: #666;
        }
    }
}
</style>
