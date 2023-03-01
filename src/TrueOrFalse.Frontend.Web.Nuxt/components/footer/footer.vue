<script lang="ts" setup>
import { FooterTopics } from '../topic/topicStore'
import { useUserStore } from '../user/userStore'

interface Props {
    footerTopics: FooterTopics
}
const props = defineProps<Props>()
const userStore = useUserStore()
</script>

<template>
    <section id="GlobalLicense">
        <div class="license-container">
            <div class="license-text-container">
                <NuxtLink class="CCLogo" rel="license" to="https://creativecommons.org/licenses/by/4.0/" :external="true">
                    <Image url="/Images/Licenses/cc-by 88x31.png" alt="Creative Commons Lizenzvertrag" />
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
                                <Image url="/Images/Logo/LogoIconText.svg" />
                            </NuxtLink>

                            <div class="overline-s no-line">
                                Alles an einem Ort –
                                <br />
                                Wiki und Lernwerkzeug vereint!
                            </div>
                        </div>
                        <div class="footer-group">
                            <LazyNuxtLink to="/terms">Nutzungsbedingungen (AGBs)</LazyNuxtLink>
                            <br />
                            <LazyNuxtLink to="/imprint">Impressum & Datenschutz</LazyNuxtLink>
                        </div>
                    </div>

                    <div class="FooterCol xxs-stack col-xs-12 col-sm-6 col-md-3">
                        <div class="footer-group">
                            <div class="overline-m no-line">
                                <LazyNuxtLink :to="`/${props.footerTopics.MemoWiki.Name}/${props.footerTopics.MemoWiki.Id}`"
                                    v-if="props.footerTopics?.MemoWiki">
                                    {{ props.footerTopics.MemoWiki.Name }}
                                </LazyNuxtLink>

                            </div>
                            <template v-for="(t, i) in props.footerTopics.MemoTopics" v-if="props.footerTopics?.MemoTopics">
                                <LazyNuxtLink :to="`/${t.Name}/${t.Id}`">
                                    {{ t.Name }}
                                </LazyNuxtLink>
                                <br v-if="i < props.footerTopics?.MemoTopics.length - 1" />
                            </template>

                        </div>
                        <div class="footer-group">
                        <div class="overline-m no-line">Software</div>
                        <NuxtLink to="https://github.com/TrueOrFalse/TrueOrFalse" target="_blank" :external="true">
                            <font-awesome-icon :icon="['fa-brands', 'github']" />&nbsp;Github
                        </NuxtLink>
                        <br />
                        <NuxtLink to="http://teamcity.memucho.de:8080/project.html?projectId=TrueOrFalse&guest=1"
                            target="_blank" :external="true">
                            <font-awesome-icon :icon="['fa-solid', 'gears']" /> Teamcity
                            </NuxtLink>
                            <br />
                            <!-- <% if (Request.IsLocal)
                   { %>
                    <%= Html.ActionLink("Algorithmus-Einblick", "Forecast", "AlgoInsight") %><br/>
                <% } %>
                <% var assembly = Assembly.Load("TrueOrFalse"); %>
                <span style="color: darkgray">
                    (Build: <%= assembly.GetName().Version.Major %> am
                    <%= Html.Raw(AssemblyLinkerTimestamp.Get(assembly).ToString("dd.MM.yyyy 'um' HH:mm")) %>)
                </span> -->
                        </div>

                    </div>
                    <div class="visible-xs visible-sm" style="clear: both"></div>

                    <div class="FooterCol xxs-stack col-xs-12 col-sm-6 col-md-3">
                        <div class="footer-group">
                            <div class="overline-m no-line">Hilfe & Kontakt</div>

                            <template v-for="(t, i) in props.footerTopics.HelpTopics" v-if="props.footerTopics?.HelpTopics">
                                <LazyNuxtLink :to="`/${t.Name.replaceAll(' ', '-')}/${t.Id}`">
                                    {{ t.Name }}
                                </LazyNuxtLink>
                                <br v-if="i < props.footerTopics.HelpTopics.length - 1" />
                            </template>
                            <br />

                            <a href="https://discord.com/invite/nXKwGrN" target="_blank">
                                <font-awesome-icon :icon="['fa-brands', 'discord']" />&nbsp;Discord
                            </a><br />
                            <a href="https://twitter.com/memuchoWissen" target="_blank">
                                <font-awesome-icon :icon="['fa-brands', 'twitter']" />&nbsp;auf Twitter
                            </a><br />
                        </div>
                    </div>

                    <div class="FooterCol xxs-stack col-xs-12 col-sm-6 col-md-3">
                        <div class="footer-group">
                            <div class="overline-m no-line">
                                <LazyNuxtLink
                                    :to="`/${props.footerTopics.RootWiki.Name.replaceAll(' ', '-')}/${props.footerTopics.RootWiki.Id}`"
                                    v-if="props.footerTopics?.RootWiki">
                                    {{ props.footerTopics.RootWiki.Name }}
                                </LazyNuxtLink>

                            </div>
                            <template v-for="(t, i) in props.footerTopics.MainTopics" v-if="props.footerTopics?.MainTopics">
                                <LazyNuxtLink :to="`/${t.Name.replaceAll(' ', '-')}/${t.Id}`">
                                    {{ t.Name }}
                                </LazyNuxtLink>
                                <br v-if="i < props.footerTopics.MainTopics.length - 1" />
                            </template>
                        </div>
                        <div class="footer-group">
                            <div class="overline-m no-line">Beliebte Themen</div>
                            <template v-for="(t, i) in props.footerTopics.PopularTopics"
                                v-if="props.footerTopics?.PopularTopics">
                                <LazyNuxtLink :to="`/${t.Name.replaceAll(' ', '-')}/${t.Id}`">
                                    {{ t.Name }}
                                </LazyNuxtLink>
                                <br v-if="i < props.footerTopics.PopularTopics.length - 1" />
                            </template>
                        </div>
                    </div>

                    <div id="FooterEndContainer" class="col-xs-12 col-lg-12">
                        <div id="FooterEnd">
                            <div>
                                Entwickelt von:
                            </div>
                            <a href="https://bitwerke.de/">
                                <Image url="/Images/Logo/BitwerkeLogo.svg" />
                            </a>
                            <a href="https://bitwerke.de/">
                                <div>
                                    Individualsoftware, UX/UI, Entwicklung und Beratung
                                </div>
                            </a>
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
</style>