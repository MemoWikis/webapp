<script lang="ts" setup>
import { FooterPages, usePageStore } from '../page/pageStore'
import { Site } from '../shared/siteEnum'
import { Visibility } from '../shared/visibilityEnum'
import { useUserStore } from '../user/userStore'

interface Props {
    footerPages: FooterPages,
    isError?: boolean
    site: Site
    questionPageIsPrivate?: boolean
}
const props = defineProps<Props>()
const userStore = useUserStore()
const pageStore = usePageStore()
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

</script>

<template>
    <section id="GlobalLicense" v-if="(props.site === Site.Page && pageStore.visibility === Visibility.All) || (props.site === Site.Question && !props.questionPageIsPrivate)">
        <div class="license-container row">
            <div class="license-text-container container">
                <div class="row">
                    <div class="col-xs-12">
                        <NuxtLink @click="handleError()" class="CCLogo" rel="license"
                            to="https://creativecommons.org/licenses/by/4.0/" :external="true">
                            <Image src="/Images/Licenses/cc-by 88x31.png" alt="Creative Commons Lizenzvertrag" />
                        </NuxtLink>
                        <div class="Text cc-license-text">
                            Alle Inhalte auf dieser Seite stehen, soweit nicht anders angegeben, unter der Lizenz <NuxtLink
                                rel="license" to="https://creativecommons.org/licenses/by/4.0/" :external="true">Creative
                                Commons Namensnennung
                                4.0 (CC-BY-4.0)</NuxtLink>. Einzelne Elemente (aus anderen Quellen übernommene Fragen, Bilder,
                            Videos,
                            Textabschnitte etc.) können anderen Lizenzen unterliegen und sind entsprechend gekennzeichnet.
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
    <div id="MasterFooter" :class="{ 'window-loading': !windowLoaded }">
        <div class="row">
            <div class="container footer-container">
                <div class="row Promoter">
                    <div class="col-xs-12">
                    </div>
                </div>

                <div class="row footer-links-memucho col-xs-12">

                    <div class="FooterCol xxs-stack col-xs-12 col-sm-6 col-md-3">
                        <div id="MasterFooterLogoContainer">
                            <NuxtLink
                                :to="userStore.isLoggedIn ? $urlHelper.getPageUrl(userStore.personalWiki?.name!, userStore.personalWiki?.id!) : $urlHelper.getPageUrl(footerPages.rootWiki.name, footerPages.rootWiki.id)"
                                id="MasterFooterLogo">
                                <Image src="/Images/Logo/LogoIconText.svg" class="master-footer-logo-img"
                                    alt="memucho logo" />
                            </NuxtLink>

                            <div class="overline-s no-line">
                                Alles an einem Ort –
                                <br />
                                Wiki und Lernwerkzeug vereint!
                            </div>
                        </div>
                        <div class="footer-group">
                            <NuxtLink @click="handleError()" to="/AGB">Nutzungsbedingungen (AGBs)</NuxtLink>
                            <br />
                            <NuxtLink @click="handleError()" to="/Impressum">Impressum & Datenschutz</NuxtLink>
                            <br />
                            <NuxtLink @click="handleError()" to="/Nutzer">Alle Nutzer</NuxtLink>
                            <br />
                        </div>
                    </div>

                    <div class="FooterCol xxs-stack col-xs-12 col-sm-6 col-md-3">
                        <div class="footer-group">
                            <div class="overline-m no-line">
                                <NuxtLink @click="handleError()"
                                    :to="$urlHelper.getPageUrl(props.footerPages.memoWiki.name, props.footerPages.memoWiki.id)"
                                    v-if="props.footerPages?.memoWiki">
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
                            <div class="overline-m no-line">Software</div>
                            <NuxtLink @click="handleError()" to="https://github.com/TrueOrFalse/TrueOrFalse" target="_blank" :external="true">
                                <font-awesome-icon :icon="['fa-brands', 'github']" />&nbsp;Github
                            </NuxtLink>
                            <br />
                            <NuxtLink @click="handleError()" to="http://teamcity.memucho.de:8080/project.html?projectId=TrueOrFalse&guest=1" target="_blank" :external="true">
                                <font-awesome-icon :icon="['fa-solid', 'gears']" /> Teamcity
                            </NuxtLink>
                            <br />
                        </div>
                    </div>
                    <div class="visible-xs visible-sm" style="clear: both"></div>

                    <div class="FooterCol xxs-stack col-xs-12 col-sm-6 col-md-3">
                        <div class="footer-group">
                            <div class="overline-m no-line">Hilfe & Kontakt</div>

                            <template v-for="(t, i) in props.footerPages.helpPages" v-if="props.footerPages?.helpPages">
                                <NuxtLink @click="handleError()" :to="$urlHelper.getPageUrl(t.name, t.id)">
                                    {{ t.name }}
                                </NuxtLink>
                                <br v-if="i < props.footerPages.helpPages.length - 1" />
                            </template>
                            <br />

                            <NuxtLink @click="handleError()" :to="config.public.discord" target="_blank" :external="true">
                                <font-awesome-icon :icon="['fa-brands', 'discord']" />&nbsp;Discord
                            </NuxtLink><br />
                            <NuxtLink @click="handleError()" to="https://twitter.com/memuchoWissen" target="_blank" :external="true">
                                <font-awesome-icon :icon="['fa-brands', 'twitter']" />&nbsp;auf Twitter
                            </NuxtLink>
                            <br />
                        </div>
                        <div class="footer-group">
                            <div class="overline-m no-line">Mitgliedschaft</div>
                            <LazyNuxtLink to="/Preise">Preise</LazyNuxtLink>
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
                            <div class="overline-m no-line">Beliebte Seiten</div>
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
    filter: brightness(0.36);
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
    padding-left: 0px;

    @media (min-width: 900px) and (max-width: 1650px) {
        padding-left: clamp(100px, 10vw, 320px);
    }

    @media (min-width: 1651px) {
        padding-left: clamp(100px, 20vw, 320px);
    }

    .footer-container {


        &.window-loading {
            padding-left: 0px;
        }
    }
}

#GlobalLicense {
    background: @memo-grey-lighter;
    transition: all 0.3s ease-in-out;
    padding: 0 10px;

    @media (min-width: 900px) and (max-width: 1650px) {
        padding-left: clamp(100px, 10vw, 320px);
    }

    @media (min-width: 1651px) {
        padding-left: clamp(100px, 20vw, 260px);
    }

    .license-container {
        padding: 0px;
        margin-top: 24px;
        margin-bottom: 24px;
    }
}
</style>
