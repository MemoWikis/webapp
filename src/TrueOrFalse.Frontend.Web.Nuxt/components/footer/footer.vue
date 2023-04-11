<script lang="ts" setup>
import { FooterTopics } from '../topic/topicStore'
import { useUserStore } from '../user/userStore'

interface Props {
    footerTopics: FooterTopics,
    isError?: boolean
}
const props = defineProps<Props>()
const userStore = useUserStore()
const config = useRuntimeConfig()

function handleError() {
    if (props.isError)
        clearError()
}
</script>

<template>
    <section id="GlobalLicense">
        <div class="license-container">
            <div class="license-text-container">
                <NuxtLink @click="handleError()" class="CCLogo" rel="license"
                    to="https://creativecommons.org/licenses/by/4.0/" :external="true">
                    <Image src="/Images/Licenses/cc-by 88x31.png" alt="Creative Commons Lizenzvertrag" />
                </NuxtLink>
                <div class="Text">
                    Alle Inhalte auf dieser Seite stehen, soweit nicht anders angegeben, unter der Lizenz <NuxtLink
                        rel="license" to="https://creativecommons.org/licenses/by/4.0/" :external="true">Creative
                        Commons Namensnennung
                        4.0 (CC-BY-4.0)</NuxtLink>. Einzelne Elemente (aus anderen Quellen übernommene Fragen, Bilder,
                    Videos,
                    Textabschnitte etc.) können anderen Lizenzen unterliegen und sind entsprechend gekennzeichnet.
                </div>
            </div>
        </div>
    </section>
    <div id="MasterFooter">
        <div class="row">
            <div class="container">
                <div class="row Promoter">
                    <div class="col-xs-12">
                    </div>
                </div>

                <div class="row footer-links-memucho">

                    <div class="FooterCol xxs-stack col-xs-12 col-sm-6 col-md-3">
                        <div id="MasterFooterLogoContainer">
                            <NuxtLink
                                :to="userStore.isLoggedIn ? `/${userStore.personalWiki?.EncodedName}/${userStore.personalWiki?.Id}` : '/Globales-Wiki/1'"
                                id="MasterFooterLogo">
                                <Image src="/Images/Logo/LogoIconText.svg" class="master-footer-logo-img" />
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
                                    :to="`/${props.footerTopics.MemoWiki.Name}/${props.footerTopics.MemoWiki.Id}`"
                                    v-if="props.footerTopics?.MemoWiki">
                                    {{ props.footerTopics.MemoWiki.Name }}
                                </NuxtLink>
                            </div>
                            <template v-for="(t, i) in props.footerTopics.MemoTopics" v-if="props.footerTopics?.MemoTopics">
                                <NuxtLink @click="handleError()" :to="`/${t.Name}/${t.Id}`">
                                    {{ t.Name }}
                                </NuxtLink>
                                <br v-if="i < props.footerTopics?.MemoTopics.length - 1" />
                            </template>

                        </div>
                        <div class="footer-group">
                            <div class="overline-m no-line">Software</div>
                            <NuxtLink @click="handleError()" to="https://github.com/TrueOrFalse/TrueOrFalse" target="_blank"
                                :external="true">
                                <font-awesome-icon :icon="['fa-brands', 'github']" />&nbsp;Github
                            </NuxtLink>
                            <br />
                            <NuxtLink @click="handleError()"
                                to="http://teamcity.memucho.de:8080/project.html?projectId=TrueOrFalse&guest=1"
                                target="_blank" :external="true">
                                <font-awesome-icon :icon="['fa-solid', 'gears']" /> Teamcity
                            </NuxtLink>
                            <br />
                        </div>
                    </div>
                    <div class="visible-xs visible-sm" style="clear: both"></div>

                    <div class="FooterCol xxs-stack col-xs-12 col-sm-6 col-md-3">
                        <div class="footer-group">
                            <div class="overline-m no-line">Hilfe & Kontakt</div>

                            <template v-for="(t, i) in props.footerTopics.HelpTopics" v-if="props.footerTopics?.HelpTopics">
                                <NuxtLink @click="handleError()" :to="`/${t.Name.replaceAll(' ', '-')}/${t.Id}`">
                                    {{ t.Name }}
                                </NuxtLink>
                                <br v-if="i < props.footerTopics.HelpTopics.length - 1" />
                            </template>
                            <br />

                            <NuxtLink @click="handleError()" :to="config.public.discord" target="_blank" :external="true">
                                <font-awesome-icon :icon="['fa-brands', 'discord']" />&nbsp;Discord
                            </NuxtLink><br />
                            <NuxtLink @click="handleError()" to="https://twitter.com/memuchoWissen" target="_blank"
                                :external="true">
                                <font-awesome-icon :icon="['fa-brands', 'twitter']" />&nbsp;auf Twitter
                            </NuxtLink><br />
                        </div>
                        <div class="footer-group">
                            <div class="overline-m no-line">Mitgliedschaft</div>
                            <LazyNuxtLink :to="'/Preise'">Preise</LazyNuxtLink>
                            <br />
                        </div>
                    </div>

                    <div class="FooterCol xxs-stack col-xs-12 col-sm-6 col-md-3">
                        <div class="footer-group">
                            <div class="overline-m no-line">
                                <NuxtLink
                                    :to="`/${props.footerTopics.RootWiki.Name.replaceAll(' ', '-')}/${props.footerTopics.RootWiki.Id}`"
                                    v-if="props.footerTopics?.RootWiki">
                                    {{ props.footerTopics.RootWiki.Name }}
                                </NuxtLink>

                            </div>
                            <template v-for="(t, i) in props.footerTopics.MainTopics" v-if="props.footerTopics?.MainTopics">
                                <NuxtLink @click="handleError()" :to="`/${t.Name.replaceAll(' ', '-')}/${t.Id}`">
                                    {{ t.Name }}
                                </NuxtLink>
                                <br v-if="i < props.footerTopics.MainTopics.length - 1" />
                            </template>
                        </div>

                        <div class="footer-group">
                            <div class="overline-m no-line">Beliebte Themen</div>
                            <template v-for="(t, i) in props.footerTopics.PopularTopics"
                                v-if="props.footerTopics?.PopularTopics">
                                <NuxtLink @click="handleError()" :to="`/${t.Name.replaceAll(' ', '-')}/${t.Id}`">
                                    {{ t.Name }}
                                </NuxtLink>
                                <br v-if="i < props.footerTopics.PopularTopics.length - 1" />
                            </template>
                        </div>
                    </div>


                    <div id="FooterEndContainer" class="col-xs-12 col-lg-12">
                        <div id="FooterEnd">
                            <div>
                                Entwickelt von:
                            </div>
                            <NuxtLink @click="handleError()" to="https://bitwerke.de/" :external="true">
                                <Image src="/Images/Logo/BitwerkeLogo.svg" class="bitwerke-logo" />
                            </NuxtLink>
                            <NuxtLink @click="handleError()" to="https://bitwerke.de/" :external="true">
                                <div>
                                    Individualsoftware, UX/UI, Entwicklung und Beratung
                                </div>
                            </NuxtLink>
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
}

.bitwerke-logo {
    padding: 0 10px;
}
</style>