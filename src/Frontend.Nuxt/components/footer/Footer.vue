<script lang="ts" setup>
import { FooterPages } from '../page/pageStore'
import { SiteType } from '../shared/siteEnum'
import { useUserStore } from '../user/userStore'
import { useSideSheetStore } from '~/components/sideSheet/sideSheetStore'

const sideSheetStore = useSideSheetStore()
interface Props {
    footerPages: FooterPages,
    isError?: boolean
    site: SiteType
    questionPageIsPrivate?: boolean
}
const props = defineProps<Props>()
const userStore = useUserStore()
const config = useRuntimeConfig()

function handleError() {
    if (props.isError)
        clearError()
}

const { $urlHelper } = useNuxtApp()

const windowLoaded = ref(false)
onMounted(() => {
    if (window)
        windowLoaded.value = true
})

const { t, setLocale, locale, locales } = useI18n()
const { isMobile } = useDevice()
</script>

<template>
    <div
        id="MasterFooter"
        :class="{
            'window-loading': !windowLoaded,
            'sidesheet-open': sideSheetStore.showSideSheet && !isMobile,
        }">
        <div class="footer-area">

            <div class="row footer-links-memoWikis col-xs-12">

                <div class="FooterCol xxs-stack col-xs-12 col-sm-6 col-md-3">
                    <div id="MasterFooterLogoContainer">
                        <NuxtLink
                            :to="userStore.isLoggedIn ? $urlHelper.getPageUrl(userStore.personalWiki?.name!, userStore.personalWiki?.id!) : $urlHelper.getPageUrl(footerPages.rootWiki.name, footerPages.rootWiki.id)"
                            id="MasterFooterLogo">

                            <Image src="/Images/Logo/LogoGreyDarker.svg" class="master-footer-logo-img" alt="memoWikis logo" />

                        </NuxtLink>

                        <div class="overline-s no-line">
                            {{ t('footer.subLabelOne') }}
                            <br />
                            {{ t('footer.subLabelTwo') }}
                        </div>
                    </div>
                    <div class="footer-group">
                        <NuxtLink @click="handleError()" :to="`/${t('url.termsOfUse')}`">{{ t('label.termsOfUse') }}</NuxtLink>
                        <br />
                        <NuxtLink @click="handleError()" :to="`/${t('url.legalNotice')}`">{{ t('label.legalNotice') }}</NuxtLink>
                        <br />
                        <NuxtLink @click="handleError()" :to="`/${t('url.users')}`">{{ t('footer.allUsers') }}</NuxtLink>
                        <br />
                    </div>

                    <div class="footer-group language-selector-container">
                        <div class="overline-m no-line">
                            {{ t('label.language') }}
                        </div>
                        <div v-for="l in locales" :key="l.code" @click.prevent.stop="setLocale(l.code)" class="language-selector" :class="{ 'active': l.code === locale }">
                            <CircleFlags :country="l.flag" class="country-flag" />
                            {{ l.name }}
                        </div>
                    </div>
                </div>

                <div class="FooterCol xxs-stack col-xs-12 col-sm-6 col-md-3">
                    <div class="footer-group">
                        <div class="overline-m no-line">
                            <NuxtLink @click="handleError()" :to="$urlHelper.getPageUrl(props.footerPages.memoWiki.name, props.footerPages.memoWiki.id)" v-if="props.footerPages?.memoWiki">
                                {{ props.footerPages.memoWiki.name }}
                            </NuxtLink>
                        </div>
                        <template v-for="(t, i) in props.footerPages.memoPages" v-if="props.footerPages?.memoPages" :key="i">
                            <NuxtLink @click="handleError()" :to="$urlHelper.getPageUrl(t.name, t.id)">
                                {{ t.name }}
                            </NuxtLink>
                            <br v-if="i < props.footerPages?.memoPages.length - 1" />
                        </template>

                    </div>
                    <div class="footer-group">
                        <div class="overline-m no-line">{{ t('footer.software') }}</div>
                        <NuxtLink @click="handleError()" to="https://github.com/MemoWikis/webapp" target="_blank" :external="true">
                            <font-awesome-icon :icon="['fa-brands', 'github']" /> {{ t('footer.github') }}
                        </NuxtLink>
                    </div>
                </div>
                <div class="visible-xs visible-sm" style="clear: both"></div>

                <div class="FooterCol xxs-stack col-xs-12 col-sm-6 col-md-3">
                    <div class="footer-group">
                        <div class="overline-m no-line">{{ t('footer.helpAndContact') }}</div>

                        <template v-for="(t, i) in props.footerPages.helpPages" v-if="props.footerPages?.helpPages">
                            <NuxtLink @click="handleError()" :to="$urlHelper.getPageUrl(t.name, t.id)">
                                {{ t.name }}
                            </NuxtLink>
                            <br v-if="i < props.footerPages.helpPages.length - 1" />
                        </template>
                        <br />

                        <NuxtLink @click="handleError()" :to="config.public.discord" target="_blank" :external="true">
                            <font-awesome-icon :icon="['fa-brands', 'discord']" /> {{ t('label.discord') }}
                        </NuxtLink><br />
                        <NuxtLink @click="handleError()" to="https://twitter.com/memoWikisWissen" target="_blank" :external="true">
                            <font-awesome-icon :icon="['fa-brands', 'twitter']" /> {{ t('label.twitter') }}
                        </NuxtLink>
                        <br />
                    </div>
                    <div class="footer-group">
                        <div class="overline-m no-line">{{ t('footer.membership') }}</div>
                        <LazyNuxtLink :to="`/${t('url.prices')}`">{{ t('url.prices') }}</LazyNuxtLink>
                        <br />
                    </div>
                </div>

                <div class="FooterCol xxs-stack col-xs-12 col-sm-6 col-md-3">
                    <div class="footer-group">
                        <div class="overline-m no-line">
                            <NuxtLink :to="$urlHelper.getPageUrl(footerPages.rootWiki.name, footerPages.rootWiki.id)" v-if="props.footerPages?.rootWiki">
                                {{ props.footerPages.rootWiki.name }}
                            </NuxtLink>

                        </div>
                        <template v-for="(t, i) in props.footerPages.mainPages" v-if="props.footerPages?.mainPages">
                            <NuxtLink @click="handleError()" :to="$urlHelper.getPageUrl(t.name, t.id)">
                                {{ t.name }}
                            </NuxtLink>
                            <br v-if="i < props.footerPages.mainPages.length - 1" />
                        </template>
                    </div>

                    <div class="footer-group">
                        <div class="overline-m no-line">{{ t('footer.popularPages') }}</div>
                        <template v-for="(t, i) in props.footerPages.popularPages" v-if="props.footerPages?.popularPages">
                            <NuxtLink @click="handleError()" :to="$urlHelper.getPageUrl(t.name, t.id)">
                                {{ t.name }}
                            </NuxtLink>
                            <br v-if="i < props.footerPages.popularPages.length - 1" />
                        </template>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

#FooterBack {
    background: @memo-grey-light;
    z-index: 3000;
    position: relative;
    bottom: 0;
    display: flex;
    flex-grow: 1;
}

.master-footer-logo-img {
    margin-bottom: 20px;
    padding-right: 20px;
    fill: @memo-grey-dark;
    opacity: 1;
}

.bitwerke-logo {
    padding: 0 10px;
    filter: brightness(0.36);
}

.cc-license-text {
    color: @memo-grey-darker;
}

#MasterFooter {
    transition: all 0.3s ease-in-out;
    width: 100%;
    padding: 0 10px;

    @media (min-width: 901px) {
        padding-left: 90px;
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

.footer-group {
    &.language-selector-container {
        .language-selector {
            display: flex;
            align-items: center;
            cursor: pointer;
            padding: 4px;
            border-radius: 4rem;
            background: @memo-grey-lightest;

            .country-flag {
                height: 2rem;
                width: 2rem;
                margin-right: 8px;
            }

            &:hover {
                filter: brightness(0.95)
            }

            &:active {
                filter: brightness(0.85)
            }

            &.active {
                filter: brightness(0.95);
                font-weight: 600;
            }
        }
    }
}
</style>
