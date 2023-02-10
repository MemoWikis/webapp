<script lang="ts" setup>
import { CurrentUser, useUserStore } from '~/components/user/userStore'
import { Topic, useTopicStore, FooterTopics } from '~/components/topic/topicStore'
import { Page } from './components/shared/pageEnum'

const userStore = useUserStore()
const config = useRuntimeConfig()

const headers = useRequestHeaders(['cookie']) as HeadersInit

const { data: currentUser } = await useFetch<CurrentUser>('/apiVue/App/GetCurrentUser', {
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

const { data: footerTopics } = await useFetch<FooterTopics>(`/apiVue/App/GetFooterTopics`, {
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
const questionPageData = ref<{
  primaryTopicName: string
  primaryTopicUrl: string
  title: string
} | undefined>(undefined)
function setBreadcrumb(e: {
  primaryTopicName: string
  primaryTopicUrl: string
  title: string
}) {
  questionPageData.value = e
}
</script>

<template>
  <HeaderGuest v-if="!userStore.isLoggedIn" />
  <HeaderMain :page="page" :question-page-data="questionPageData" />
  <NuxtPage @set-page="setPage" @set-question-page-data="setBreadcrumb" />
  <LazyUserLogin v-if="!userStore.isLoggedIn" />
  <LazySpinner />
  <LazyAlert />
  <LazyActivityPointsLevelPopUp />
  <Footer :footer-topics="footerTopics" v-if="footerTopics" />
</template>
