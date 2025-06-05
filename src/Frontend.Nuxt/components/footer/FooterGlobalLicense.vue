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
        class="layout-wrapper"
        :class="{
            'sidesheet-open': sideSheetStore.showSideSheet && !isMobile,
        }" v-if="(props.site === SiteType.Page && pageStore.visibility === Visibility.Public) || (props.site === SiteType.Question && !props.questionPageIsPrivate)">
        <div class="footer-area">

            <div class="license-content" :class="{ 'mobile': isMobile }">
                <NuxtLink @click="handleError()" class="CCLogo" rel="license"
                    to="https://creativecommons.org/licenses/by/4.0/" :external="true">
                    <Image src="/Images/Licenses/cc-by 88x31.png" alt="Creative Commons Lizenzvertrag" />
                </NuxtLink>
                <div class="cc-license-text">
                    {{ t('globalLicense.text.partOne') }}
                    <NuxtLink rel="license" to="https://creativecommons.org/licenses/by/4.0/" :external="true">
                        {{ t('globalLicense.creativeCommonsLabel') }}
                    </NuxtLink>.
                    <br />
                    {{ t('globalLicense.text.partTwo') }}
                </div>
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

    .license-content {
        display: flex;
        flex-direction: row;
        align-items: center;
        width: 100%;
        gap: 1rem 2rem;

        padding: 24px 10px;

        &.mobile {
            flex-direction: column;
            align-items: center;
        }

        &.cc-license-text {
            flex-direction: column;
        }

        .CCLogo {
            flex: 0 40px 90px;
        }

        .cc-license-text {}
    }
}
</style>