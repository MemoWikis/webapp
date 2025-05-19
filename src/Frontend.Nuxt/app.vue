<script lang="ts" setup>
import { CurrentUser, useUserStore } from '~/components/user/userStore'
import { Page, usePageStore, FooterPages } from '~/components/page/pageStore'
import { SiteType } from './components/shared/siteEnum'
import { BreadcrumbItem } from './components/header/breadcrumbItems'
import { Visibility } from './components/shared/visibilityEnum'
import { useLoadingStore } from './components/loading/loadingStore'
import { useRootPageChipStore } from '~/components/header/rootPageChipStore'
import { AlertType, useAlertStore } from './components/alert/alertStore'
import { ErrorCode } from './components/error/errorCodeEnum'
import { NuxtError } from '#app'

const { t, locale, setLocale } = useI18n()

const userStore = useUserStore()
const config = useRuntimeConfig()
const loadingStore = useLoadingStore()
const rootPageChipStore = useRootPageChipStore()

const { $urlHelper, $vfm, $logger } = useNuxtApp()

const headers = useRequestHeaders(['cookie']) as HeadersInit

const { data: currentUser } = await useFetch<CurrentUser>('/apiVue/App/GetCurrentUser', {
	method: 'GET',
	credentials: 'include',
	mode: 'no-cors',
	onRequest({ options }) {
		if (import.meta.server) {
			options.headers = new Headers(headers)
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
	if (userStore.isLoggedIn) {
		const fontSizeCookie = useCookie('fontSize').value

		if (fontSizeCookie != null) {
			const cookieValues = fontSizeCookie.split('-')
			const fontSize = cookieValues[0]
			const userId = cookieValues[1]

			if (parseInt(userId) === userStore.id)
				userStore.setFontSize(parseInt(fontSize))
		}
	}
}

const { data: footerPages } = await useFetch<FooterPages>(`/apiVue/App/GetFooterPages`, {
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

const siteType = ref(SiteType.Default)

const pageStore = usePageStore()

function setPage(type: SiteType | undefined | null = null) {
	if (type != null && type != undefined) {
		siteType.value = type
		if (type != SiteType.Page) {
			pageStore.setPage(new Page())
		}
	}
}

interface QuestionPageData {
	primaryPageName: string
	primaryPageUrl: string
	title: string
	isPrivate: boolean
}
const questionPageData = ref<QuestionPageData>()
function setQuestionpageBreadcrumb(e: QuestionPageData) {
	questionPageData.value = e
}

const breadcrumbItems = ref<BreadcrumbItem[]>()
function setBreadcrumb(e: BreadcrumbItem[]) {
	breadcrumbItems.value = e
}
const route = useRoute()
userStore.$onAction(({ name, after }) => {
	if (name === 'logout') {

		after(async (loggedOut) => {
			if (loggedOut) {
				userStore.reset()
				loadingStore.startLoading()

				try {
					await refreshNuxtData()
				} finally {
					loadingStore.stopLoading()
					if (siteType.value === SiteType.Page && pageStore.visibility != Visibility.Public)
						await navigateTo('/')
				}
			}
		})
	}

	if (name === 'login') {
		after(async (loginResult) => {
			if (loginResult.success === true) {
				await nextTick()
				handleLogin()
			}
		})
	}

	if (name === 'apiLogin') {
		after(async (loggedIn) => {
			if (loggedIn === true) {
				await nextTick()
				handleLogin()
			}

		})
	}

	if (name === 'deleteUser') {
		after(async () => {
			userStore.reset()
			loadingStore.startLoading()

			try {
				await refreshNuxtData()
			} finally {
				loadingStore.stopLoading()
				await navigateTo('/')
			}
		})
	}
})

async function handleLogin() {
	if (siteType.value === SiteType.Error)
		return
	if ((siteType.value === SiteType.Page || siteType.value === SiteType.Register) && route.params.id === rootPageChipStore.id.toString() && userStore.personalWiki && userStore.personalWiki.id != rootPageChipStore.id)
		await navigateTo($urlHelper.getPageUrl(userStore.personalWiki.name, userStore.personalWiki.id))
	else
		await refreshNuxtData()

	if (locale.value != userStore.uiLanguage)
		setLocale(userStore.uiLanguage)
}

const { openedModals } = $vfm
const modalIsOpen = ref(false)

watch(openedModals, (val) => {
	if (val.length > 0)
		modalIsOpen.value = true
	else
		modalIsOpen.value = false
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
const statusCode = ref<number>(0)

function clearErr() {
	statusCode.value = 0
	clearError()
}

function logError(error: any) {

	const errorObject = {
		error: error,
		userId: userStore.isLoggedIn ? userStore.id : null,
		location: window.location.href,
		userAgent: navigator.userAgent,
	}

	$logger.error('Nuxt non Fatal Error', [errorObject])

	const nuxtError = error as NuxtError

	if (nuxtError.statusCode)
		statusCode.value = nuxtError.statusCode

	if (statusCode.value === ErrorCode.NotFound || statusCode.value === ErrorCode.Unauthorized)
		return

	if (import.meta.client) {
		const alertStore = useAlertStore()
		alertStore.openAlert(AlertType.Error, { text: null, customDetails: error, texts: [t('error.api.body.title'), t('error.api.body.suggestion')] }, t('label.reloadPage'), true, t('error.api.title'), 'reloadPage', t('label.back'))

		alertStore.$onAction(({ name, after }) => {
			if (name === 'closeAlert') {

				after((result) => {
					if (result.cancelled === false && result.id === 'reloadPage')
						window.location.reload()
					else clearErr()
				})
			}
		})
	}
}

useHead(() => ({
	meta: [
		{
			name: 'viewport',
			content: 'width=device-width, initial-scale=1.0, interactive-widget=resizes-content'
		},
	]
}))

const windowLoaded = ref(false)
onMounted(() => {
	if (window)
		windowLoaded.value = true
})
watch(locale, () => {
	userStore.updateLanguageSetting(locale.value)
})
</script>

<template>
	<HeaderGuest v-if="!userStore.isLoggedIn" />
	<HeaderMain :site="siteType" :question-page-data="questionPageData" :breadcrumb-items="breadcrumbItems" />

	<SideSheet v-if="footerPages" :footer-pages="footerPages" />
	<div class="nuxt-page" :class="{ 'modal-is-open': modalIsOpen }">

		<NuxtErrorBoundary @error="logError">
			<BannerLoginToEditReminder v-if="siteType === SiteType.Page && userStore.showLoginToEditReminderBanner" />

			<NuxtPage @set-page="setPage" @set-question-page-data="setQuestionpageBreadcrumb"
				@set-breadcrumb="setBreadcrumb" :site="siteType"
				:class="{ 'open-modal': modalIsOpen, 'mobile-headings': isMobile, 'window-loading': !windowLoaded }" />

			<template #error="{ error }">
				<ErrorContent v-if="statusCode === ErrorCode.NotFound || statusCode === ErrorCode.Unauthorized"
					:error="error as NuxtError<unknown>" :in-error-boundary="true" @clear-error="clearErr" />
				<NuxtPage v-else @set-page="setPage" @set-question-page-data="setQuestionpageBreadcrumb"
					@set-breadcrumb="setBreadcrumb" :footer-pages="footerPages"
					:class="{ 'open-modal': modalIsOpen, 'mobile-headings': isMobile }" :site="SiteType.Error" />
			</template>
		</NuxtErrorBoundary>
	</div>

	<FooterGlobalLicense :site="siteType" :question-page-is-private="questionPageData?.isPrivate" v-show="!modalIsOpen" />
	<Footer :footer-pages="footerPages" v-if="footerPages" :site="siteType" :question-page-is-private="questionPageData?.isPrivate" v-show="!modalIsOpen" />

	<ClientOnly>
		<LazyUserLoginModal v-if="!userStore.isLoggedIn" />
		<LazyLoading />
		<LazyAlert />
		<LazyActivityPointsLevelPopUp />
		<LazyImageLicenseDetailModal />
		<SnackBar />
		<LazyPageConvertModal />
	</ClientOnly>
</template>

<style lang="less">
.nuxt-page {
	transition: all 0.3s ease-in-out;

	@media (min-width: 900px) and (max-width: 1650px) {
		padding-left: clamp(100px, 10vw, 320px);
	}

	@media (min-width: 1651px) {
		padding-left: clamp(100px, 20vw, 320px);
	}

	&.window-loading {
		padding-left: 0px;
	}

	min-height: 86vh;

	&.modal-is-open {
		min-height: unset;
	}
}

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
