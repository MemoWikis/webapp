<script lang="ts" setup>
import { CurrentUser, useUserStore } from '~/components/user/userStore'
import { Topic, useTopicStore } from '~/components/topic/topicStore'
import { Page } from './components/shared/pageEnum'

const userStore = useUserStore()
const config = useRuntimeConfig()

const headers = useRequestHeaders(['cookie']) as HeadersInit

const { data: currentUser } = await useFetch<CurrentUser>('/apiVue/VueSessionUser/GetCurrentUser', {
  method: 'Get',
  credentials: 'include',
  mode: 'no-cors',
  onRequest({ options }) {
    if (process.server) {
      options.headers = headers
      options.baseURL = config.public.serverBase
    }
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
  mode: 'no-cors',
  onRequest({ options }) {
    if (process.server) {
      options.baseURL = config.public.serverBase
    }
  }
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
  <Footer :footer-topics="footerTopics" />
</template>
