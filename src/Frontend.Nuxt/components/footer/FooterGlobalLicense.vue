<script lang="ts" setup>
import { SiteType } from '../shared/siteEnum'
import { usePageStore } from '../page/pageStore'
import { Visibility } from '../shared/visibilityEnum'
import { useSideSheetStore } from '~/components/sideSheet/sideSheetStore'

const sideSheetStore = useSideSheetStore()

interface Props {
    isError?: boolean
    site: SiteType,
    questionPageIsPrivate?: boolean
}

const props = defineProps<Props>()
const pageStore = usePageStore()

function handleError() {
    if (props.isError)
        clearError()
}

const { t } = useI18n()
const { isMobile } = useDevice()

</script>

<template>
    <div id="GlobalLicense"
        :class="{
            'sidesheet-open': sideSheetStore.showSideSheet && !isMobile,
        }" v-if="(props.site === SiteType.Page && pageStore.visibility === Visibility.Public) || (props.site === SiteType.Question && !props.questionPageIsPrivate)">
        <div class="footer-area license-container">

            <NuxtLink @click="handleError()" class="CCLogo" rel="license"
                to="https://creativecommons.org/licenses/by/4.0/" :external="true">
                <Image src="/Images/Licenses/cc-by 88x31.png" alt="Creative Commons Lizenzvertrag" />
            </NuxtLink>
            <div class="Text cc-license-text">
                {{ t('globalLicense.text.partOne') }}
                <NuxtLink rel="license" to="https://creativecommons.org/licenses/by/4.0/" :external="true">
                    {{ t('globalLicense.creativeCommonsLabel') }}
                </NuxtLink>.
                <br />
                {{ t('globalLicense.text.partTwo') }}
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

#GlobalLicense {
    transition: all 0.3s ease-in-out;
    width: 100%;
    padding: 0 10px;
    background: @memo-grey-lighter;

    @media (min-width: 901px) {
        padding-left: 90px;
    }

    .license-container {
        padding: 0px;
        margin-top: 24px;
        margin-bottom: 24px;
    }

    &.sidesheet-open {
        padding-left: 420px;

        .footer-area {
            margin-right: 10px;
            width: 100%;
        }

        @media (max-width: 1500px) {
            width: calc(100vw - 40px);

            .footer-area {
                margin-right: 10px;
                width: 100%;
            }
        }
    }
}
</style>