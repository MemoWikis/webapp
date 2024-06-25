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

const { $urlHelper, $vfm, $logger } = useNuxtApp()

const headers = useRequestHeaders(['cookie']) as HeadersInit

if (import.meta.server && !!useCookie('persistentLogin').value) {

	interface SessionStartResult {
		success: boolean
		loginGuid?: string
		expiryDate?: string
		alreadyLoggedIn?: boolean
	}

	const { data: result } = await useFetch<SessionStartResult>('/apiVue/App/SessionStart', {
		method: 'POST',
		credentials: 'include',
		mode: 'no-cors',
		body: {
			sessionStartGuid: config.sessionStartGuid
		},
		onRequest({ options }) {
			options.headers = headers
			options.baseURL = config.public.serverBase
		},
		onResponseError(context) {
			throw createError({ statusMessage: context.error?.message })
		}
	})

	if (result.value?.success) {

		const loginGuid = result.value.loginGuid
		const expiryDate = result.value.expiryDate

		if (loginGuid && expiryDate) {
			setPersistentLoginCookie(loginGuid, expiryDate)
		}
	} else if (result.value?.success == false && result.value.alreadyLoggedIn == false) {
		deletePersistentLoginCookie()
	}
}

function deletePersistentLoginCookie() {
	useCookie('persistentLogin', { maxAge: -1 }).value = ""
	refreshCookie('persistentLogin')
}

function setPersistentLoginCookie(loginGuid: string, expiryDate: string) {
	refreshCookie('persistentLogin')

	useCookie('persistentLogin', {
		expires: new Date(expiryDate),
		sameSite: 'lax',
		secure: config.public.environment != 'development',
		httpOnly: true
	}).value = loginGuid

	refreshCookie('persistentLogin')
}

const { data: currentUser } = await useFetch<CurrentUser>('/apiVue/App/GetCurrentUser', {
	method: 'GET',
	credentials: 'include',
	mode: 'no-cors',
	onRequest({ options }) {
		if (import.meta.server) {
			options.headers = headers
			options.baseURL = config.public.serverBase
		}
	},
	onResponse({ response }) {
		const cookies = response.headers.get('set-cookie')
		$logger.info('cookieProblem: app.vue GetCurrentUser sessionHeader', [{ cookies: cookies }])

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
		if (import.meta.server) {
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
	if (page.value == Page.Error)
		return
	if ((page.value == Page.Topic || page.value == Page.Register) && route.params.id == rootTopicChipStore.id.toString() && userStore.personalWiki && userStore.personalWiki.id != rootTopicChipStore.id)
		await navigateTo($urlHelper.getTopicUrl(userStore.personalWiki.name, userStore.personalWiki.id))
	else
		await refreshNuxtData()
}

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
const { isMobile } = useDevice()
</script>

<template>
	<Html lang="de">

	</Html>
	<HeaderGuest v-if="!userStore.isLoggedIn" />
	<HeaderMain :page="page" :question-page-data="questionPageData" :breadcrumb-items="breadcrumbItems" />
	<ClientOnly>
		<BannerInfo v-if="footerTopics && !userStore.isLoggedIn" :documentation="footerTopics?.documentation" />
	</ClientOnly>
	<NuxtPage @set-page="setPage" @set-question-page-data="setQuestionpageBreadcrumb" @set-breadcrumb="setBreadcrumb"
		:documentation="footerTopics?.documentation"
		:class="{ 'open-modal': modalIsOpen, 'mobile-headings': isMobile }" />
	<ClientOnly>
		<LazyUserLogin v-if="!userStore.isLoggedIn" />
		<LazySpinner />
		<LazyAlert />
		<LazyActivityPointsLevelPopUp />
		<LazyImageLicenseDetailModal />
		<SnackBar />

	</ClientOnly>
	<Footer :footer-topics="footerTopics" v-if="footerTopics" />
</template>

<style lang="less">
.mobile-headings {
	h2 {
		font-size: 28px;
		line-height: 1.2;
		font-weight: unset;
	}

	h3 {
		font-size: 22px;
		line-height: 1.2;
	}
}
</style>
