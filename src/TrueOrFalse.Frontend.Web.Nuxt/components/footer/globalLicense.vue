<script lang="ts" setup>
import { SiteType } from '../shared/siteEnum'
import { usePageStore } from '../page/pageStore'
import { Visibility } from '../shared/visibilityEnum'

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

</script>

<template>
    <section id="GlobalLicense" v-if="(props.site === SiteType.Page && pageStore.visibility === Visibility.All) || (props.site === SiteType.Question && !props.questionPageIsPrivate)">
        <div class="license-container row">
            <div class="license-text-container container">
                <div class="row">
                    <div class="col-xs-12">
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
            </div>
        </div>
    </section>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

#GlobalLicense {
    background: @memo-grey-lighter;
    transition: all 0.3s ease-in-out;
    padding: 0 10px;

    @media (min-width: 900px) and (max-width: 1650px) {
        padding-left: clamp(100px, 10vw, 320px);
    }

    @media (min-width: 1651px) {
        padding-left: clamp(100px, 20vw, 320px);
    }

    .license-container {
        padding: 0px;
        margin-top: 24px;
        margin-bottom: 24px;
    }
}
</style>