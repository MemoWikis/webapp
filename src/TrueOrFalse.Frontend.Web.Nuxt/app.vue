<script lang="ts" setup>
import { CurrentUser, useUserStore } from '~/components/user/userStore'
import { Topic, useTopicStore } from '~/components/topic/topicStore'
import { Page } from './components/shared/pageEnum'

const userStore = useUserStore()
const config = useRuntimeConfig()

const headers = useRequestHeaders(['cookie']) as HeadersInit

const { data: currentUser } = await useFetch<CurrentUser>('/apiVue/VueSessionUser/GetCurrentUser', {
  baseURL: process.server ? config.public.serverBase : config.public.clientBase,
  method: 'Get',
  credentials: 'include',
  mode: 'no-cors',
  onResponse({ options }) {
    if (process.server)
      options.headers = headers
  }
})
if (currentUser.value)
  userStore.initUser(currentUser.value)

interface FooterTopics {
  RootTopic: Topic
  MainTopics: Topic[]
  MemoWiki: Topic
  MemoTopics: Topic[]
  HelpTopics: Topic[]
  PopularTopics: Topic[]
}
const { data: footerTopics } = await useFetch<FooterTopics>(`/apiVue/Footer/GetFooterTopics`, {
  method: 'Get',
  baseURL: process.server ? config.public.serverBase : config.public.clientBase,
  mode: 'no-cors',
})

const page = ref(Page.Default)

const topicStore = useTopicStore()

function setPage(type: Page | null = null) {
  if (type != null) {
    page.value = type
    if (type != Page.Topic) {
      topicStore.setTopic(new Topic())
    }
  }
}
</script>

<template>
  <HeaderGuest v-if="!userStore.isLoggedIn" />
  <HeaderMain :page="page" />
  <NuxtPage @set-page="setPage" />
  <LazyClientOnly>
    <LazyUserLogin v-if="!userStore.isLoggedIn" />
    <LazySpinner />
    <!-- <LazyAlert /> -->
  </LazyClientOnly>
  <div>
    <section id="GlobalLicense">
      <div class="license-container">
        <div class="license-text-container">
          <NuxtLink class="CCLogo" rel="license" to="https://creativecommons.org/licenses/by/4.0/" :external="true">
            <img alt="Creative Commons Lizenzvertrag" style="border-width:0" src="" />
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

            <div class="FooterCol xxs-stack col-xs-6 col-md-3">
              <div id="MasterFooterLogoContainer">
                <NuxtLink to="/" id="MasterFooterLogo">
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

            <div class="FooterCol xxs-stack col-xs-6 col-md-3">
              <div class="footer-group">
                <div class="overline-m no-line">
                  <LazyNuxtLink :to="`/${footerTopics.MemoWiki.Name}/${footerTopics.MemoWiki.Id}`"
                    v-if="footerTopics?.MemoWiki">
                    {{ footerTopics.MemoWiki.Name }}
                  </LazyNuxtLink>

                </div>
                <template v-for="(t, i) in footerTopics.MemoTopics" v-if="footerTopics?.MemoTopics">
                  <LazyNuxtLink :to="`/${t.Name}/${t.Id}`">
                    {{ t.Name }}
                  </LazyNuxtLink>
                  <br v-if="i < footerTopics?.MemoTopics.length - 1" />
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

            <div class="FooterCol xxs-stack col-xs-6 col-md-3">
              <div class="footer-group">
                <div class="overline-m no-line">Hilfe & Kontakt</div>

                <template v-for="(t, i) in footerTopics.HelpTopics" v-if="footerTopics?.HelpTopics">
                  <LazyNuxtLink :to="`/${t.Name.replaceAll(' ', '-')}/${t.Id}`">
                    {{ t.Name }}
                  </LazyNuxtLink>
                  <br v-if="i < footerTopics.HelpTopics.length - 1" />
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

            <div class="FooterCol xxs-stack col-xs-6 col-md-3">
              <div class="footer-group">
                <div class="overline-m no-line">
                  <LazyNuxtLink
                    :to="`/${footerTopics.RootTopic.Name.replaceAll(' ', '-')}/${footerTopics.RootTopic.Id}`"
                    v-if="footerTopics?.RootTopic">
                    {{ footerTopics.RootTopic.Name }}
                  </LazyNuxtLink>

                </div>
                <template v-for="(t, i) in footerTopics.MainTopics" v-if="footerTopics?.MainTopics">
                  <LazyNuxtLink :to="`/${t.Name.replaceAll(' ', '-')}/${t.Id}`">
                    {{ t.Name }}
                  </LazyNuxtLink>
                  <br v-if="i < footerTopics.MainTopics.length - 1" />
                </template>
              </div>
              <div class="footer-group">
                <div class="overline-m no-line">Beliebte Themen</div>
                <template v-for="(t, i) in footerTopics.PopularTopics" v-if="footerTopics?.PopularTopics">
                  <LazyNuxtLink :to="`/${t.Name.replaceAll(' ', '-')}/${t.Id}`">
                    {{ t.Name }}
                  </LazyNuxtLink>
                  <br v-if="i < footerTopics.PopularTopics.length - 1" />
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
  </div>
</template>

<style scoped>
#FooterBack {
  background: grey;
  z-index: 3000;
  position: relative;
  bottom: 0;
  display: flex;
  flex-grow: 1;
}
</style>