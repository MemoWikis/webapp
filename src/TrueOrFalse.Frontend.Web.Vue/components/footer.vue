<script lang="ts" setup>
import { Topic } from './topic/topicStore';
const config = useRuntimeConfig()
const { data: rootTopic } = await useFetch<Topic>(`/Footer/GetRootWiki`, {
    baseURL: config.apiBase,
})
const { data: mainTopics } = await useFetch<Topic[]>(`/Footer/GetMainTopics`, {
    baseURL: config.apiBase,
})
const { data: memoWiki } = await useFetch<Topic>(`/Footer/GetMemoWiki`, {
    baseURL: config.apiBase,
})
const { data: memoTopics } = await useFetch<Topic[]>(`/Footer/GetMemoTopics`, {
    baseURL: config.apiBase,
})
const { data: helpTopics } = await useFetch<Topic[]>(`/Footer/GetHelpTopics`, {
    baseURL: config.apiBase,
})
const { data: popularTopics } = await useFetch<Topic[]>(`/Footer/GetPopularTopics`, {
    baseURL: config.apiBase,
})
</script>

<template>
    <div id="MasterFooter">
        <div class="row">
            <div class="container">
                <div class="row Promoter">
                    <div class="col-xs-12">
                    </div>
                </div>

                <div class="row footer-links-memucho">

                    <div class="FooterCol xxs-stack col-xs-6 col-md-3">
                        <div id="MasterFooterLogoContainer">
                            <a href="/" id="MasterFooterLogo">
                                <Image url="/Images/Logo/LogoIconText.svg" />
                            </a>

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

                    <div class="FooterCol xxs-stack col-xs-6 col-md-3">
                        <div class="footer-group">
                            <div class="overline-m no-line">
                                <LazyNuxtLink :to="`/${memoWiki.Name}/${memoWiki.Id}`">
                                    {{ memoWiki.Name }}
                                </LazyNuxtLink>

                            </div>
                            <template v-for="(t, i) in memoTopics">
                                <LazyNuxtLink :to="`/${t.Name}/${t.Id}`">
                                    {{ t.Name }}
                                </LazyNuxtLink>
                                <br v-if="i < memoTopics.length - 1" />
                            </template>

                        </div>
                        <div class="footer-group">
                            <div class="overline-m no-line">Software</div>
                            <a href="https://github.com/TrueOrFalse/TrueOrFalse" target="_blank">
                                <font-awesome-icon icon="fa-brands fa-github" />&nbsp;Github
                            </a>
                            <br />
                            <a href="http://teamcity.memucho.de:8080/project.html?projectId=TrueOrFalse&guest=1"
                                target="_blank">
                                <font-awesome-icon icon="fa fa-cogs"></font-awesome-icon> Teamcity
                            </a>
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

                    <div class="FooterCol xxs-stack col-xs-6 col-md-3">
                        <div class="footer-group">
                            <div class="overline-m no-line">Hilfe & Kontakt</div>

                            <template v-for="(t, i) in helpTopics">
                                <LazyNuxtLink :to="`/${t.Name}/${t.Id}`">
                                    {{ t.Name }}
                                </LazyNuxtLink>
                                <br v-if="i < helpTopics.length - 1" />
                            </template>
                            <br />

                            <a href="https://discord.com/invite/nXKwGrN" target="_blank">
                                <font-awesome-icon icon="fa-brands fa-discord" />&nbsp;Discord
                            </a><br />
                            <a href="https://twitter.com/memuchoWissen" target="_blank">
                                <font-awesome-icon icon="fa-brands fa-twitter" />&nbsp;auf Twitter
                            </a><br />
                        </div>
                    </div>

                    <div class="FooterCol xxs-stack col-xs-6 col-md-3">
                        <div class="footer-group">
                            <div class="overline-m no-line">
                                <LazyNuxtLink :to="`/${rootTopic.Name}/${rootTopic.Id}`">
                                    {{ rootTopic.Name }}
                                </LazyNuxtLink>

                            </div>
                            <template v-for="(t, i) in mainTopics">
                                <LazyNuxtLink :to="`/${t.Name}/${t.Id}`">
                                    {{ t.Name }}
                                </LazyNuxtLink>
                                <br v-if="i < mainTopics.length - 1" />
                            </template>
                        </div>
                        <div class="footer-group">
                            <div class="overline-m no-line">Beliebte Themen</div>
                            <template v-for="(t, i) in popularTopics">
                                <LazyNuxtLink :to="`/${t.Name}/${t.Id}`">
                                    {{ t.Name }}
                                </LazyNuxtLink>
                                <br v-if="i < popularTopics.length - 1" />
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

<style scoped>
</style>
