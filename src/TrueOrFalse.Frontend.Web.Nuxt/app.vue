<script lang="ts" setup>
import { CurrentUser, useUserStore } from '~/components/user/userStore'
import { Topic, useTopicStore, FooterTopics } from '~/components/topic/topicStore'
import { Page } from './components/shared/pageEnum'
import { BreadcrumbItem } from './components/header/breadcrumbItems'

const userStore = useUserStore()
const config = useRuntimeConfig()

const headers = useRequestHeaders(['cookie']) as HeadersInit

const { data: currentUser } = await useFetch<CurrentUser>('/apiVue/App/GetCurrentUser', {
	method: 'GET',
	credentials: 'include',
	mode: 'no-cors',
	onRequest({ options }) {
		if (process.server) {
			options.headers = headers
			options.baseURL = config.public.serverBase
		}
	}
})
if (currentUser.value != null) {
	userStore.initUser(currentUser.value)
	useState('currentuser', () => currentUser.value)
}

const { data: footerTopics } = await useFetch<FooterTopics>(`/apiVue/App/GetFooterTopics`, {
	method: 'GET',
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
}>()
function setQuestionpageBreadcrumb(e: {
	primaryTopicName: string
	primaryTopicUrl: string
	title: string
}) {
	questionPageData.value = e
}

const breadcrumbItems = ref<BreadcrumbItem[]>()
function setBreadcrumb(e: BreadcrumbItem[]) {
	breadcrumbItems.value = e
}

</script>

<template>
	<Html lang="de">

	</Html>
	<HeaderGuest v-if="!userStore.isLoggedIn" />
	<HeaderMain :page="page" :question-page-data="questionPageData" :breadcrumb-items="breadcrumbItems" />
	<BannerInfo :documentation="footerTopics?.Documentation!" />
	<NuxtPage @set-page="setPage" @set-question-page-data="setQuestionpageBreadcrumb" @set-breadcrumb="setBreadcrumb"
		:documentation="footerTopics?.Documentation!" />
	<ClientOnly>
		<LazyUserLogin v-if="!userStore.isLoggedIn" />
		<LazySpinner />
		<LazyAlert />
		<LazyActivityPointsLevelPopUp />
		<LazyImageLicenseDetailModal />
	</ClientOnly>

	<Footer :footer-topics="footerTopics" v-if="footerTopics" />
</template>
