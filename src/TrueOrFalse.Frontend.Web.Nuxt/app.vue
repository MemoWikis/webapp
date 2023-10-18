<script lang="ts" setup>
import { CurrentUser, useUserStore } from '~/components/user/userStore'
import { Topic, useTopicStore, FooterTopics } from '~/components/topic/topicStore'
import { Page } from './components/shared/pageEnum'
import { BreadcrumbItem } from './components/header/breadcrumbItems'
import { Visibility } from './components/shared/visibilityEnum'
import { useSpinnerStore } from './components/spinner/spinnerStore'
import { useRootTopicChipStore } from '~/components/header/rootTopicChipStore'

const userStore = useUserStore()
const config = useRuntimeConfig()
const spinnerStore = useSpinnerStore()
const rootTopicChipStore = useRootTopicChipStore()

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
	},
	onResponseError(context) {
		throw createError({ statusMessage: context.error?.message })
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
	},
	onResponseError(context) {
		throw createError({ statusMessage: context.error?.message })
	},
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
const route = useRoute()
const { $urlHelper } = useNuxtApp()
userStore.$onAction(({ name, after }) => {
	if (name == 'logout') {

		after(async (loggedOut) => {
			if (loggedOut) {
				userStore.reset()
				spinnerStore.showSpinner()

				try {
					await refreshNuxtData()
				} finally {
					spinnerStore.hideSpinner()
					if (page.value == Page.Topic && topicStore.visibility != Visibility.All)
						await navigateTo('/')
				}
			}
		})
	}
	if (name == 'login') {
		after(async (loginResult) => {
			if (loginResult.success == true) {
				await nextTick()
				handleLogin()
			}
		})
	}
	if (name == 'apiLogin') {
		after(async (loggedIn) => {
			if (loggedIn == true) {
				await nextTick()
				handleLogin()
			}

		})
	}
})

async function handleLogin() {
	if ((page.value == Page.Topic || page.value == Page.Register) && route.params.id == rootTopicChipStore.id.toString() && userStore.personalWiki && userStore.personalWiki.Id != rootTopicChipStore.id)
		await navigateTo($urlHelper.getTopicUrl(userStore.personalWiki.Name, userStore.personalWiki.Id))
	else
		await refreshNuxtData()
}

const { $vfm } = useNuxtApp()
const { openedModals } = $vfm
const modalIsOpen = ref(false)
watch(() => openedModals, (val) => {
	if (val.length > 0)
		modalIsOpen.value = true
	else modalIsOpen.value = false
}, { deep: true })

useHead(() => ({
	meta: [
		{
			name: 'theme-color',
			content: 'white',
			media: '(prefers-color-scheme: light)'
		}
	]
}))
</script>

<template>
	<Html lang="de">

	</Html>
	<HeaderGuest v-if="!userStore.isLoggedIn" />
	<HeaderMain :page="page" :question-page-data="questionPageData" :breadcrumb-items="breadcrumbItems" />
	<ClientOnly>
		<BannerInfo v-if="footerTopics" :documentation="footerTopics?.Documentation" />
	</ClientOnly>
	<NuxtPage @set-page="setPage" @set-question-page-data="setQuestionpageBreadcrumb" @set-breadcrumb="setBreadcrumb"
		:documentation="footerTopics?.Documentation" :class="{ 'open-modal': modalIsOpen }" />
	<ClientOnly>
		<LazyUserLogin v-if="!userStore.isLoggedIn" />
		<LazySpinner />
		<LazyAlert />
		<LazyActivityPointsLevelPopUp />
		<LazyImageLicenseDetailModal />
	</ClientOnly>

	<Footer :footer-topics="footerTopics" v-if="footerTopics" />
</template>
