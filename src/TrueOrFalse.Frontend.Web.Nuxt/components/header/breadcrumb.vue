<script lang="ts" setup>
import { VueElement } from 'vue'
import { usePageStore } from '../page/pageStore'
import { PageEnum } from '../shared/pageEnum'
import { useUserStore } from '../user/userStore'
import { BreadcrumbItem as CustomBreadcrumbItem } from './breadcrumbItems'
import { useRootPageChipStore } from './rootPageChipStore'

interface Props {
	page: PageEnum
	showSearch: boolean
	questionPageData?: {
		primaryPageName: string
		primaryPageUrl: string
		title: string
	}
	customBreadcrumbItems?: CustomBreadcrumbItem[]
	partialLeft?: VueElement
}

const props = defineProps<Props>()

const userStore = useUserStore()
const pageStore = usePageStore()
interface BreadcrumbItem {
	name: string
	id: number
	width?: number
}
interface Breadcrumb {
	newWikiId: number
	personalWiki: BreadcrumbItem
	items: BreadcrumbItem[]
	rootPage: BreadcrumbItem
	currentPage: BreadcrumbItem
	breadcrumbHasGlobalWiki: boolean
	isInPersonalWiki: boolean
}
const breadcrumb = ref<Breadcrumb>()

const breadcrumbItems = ref<BreadcrumbItem[]>([])
const stackedBreadcrumbItems = ref<BreadcrumbItem[]>([])

const breadcrumbEl = ref<VueElement>()
const breadcrumbWidth = ref('')

function handleResize() {
	windowInnerWidth.value = window?.innerWidth
	updateBreadcrumb()
}

function handleScroll() {
	if (userStore.isLoggedIn || window?.scrollY > 105)
		return
	updateBreadcrumb()
}
const personalWiki = ref<BreadcrumbItem | null>(null)

const isUpdating = ref(false)
const shouldCalc = ref(true)
const whiteOut = ref(true)
const whiteOutTimer = ref()
watch([isUpdating, shouldCalc], ([i, v]) => {
	clearTimeout(whiteOutTimer.value)
	if (i && v)
		whiteOut.value = true
	else
		whiteOutTimer.value = setTimeout(() => {
			whiteOut.value = false
		}, 100)
})
async function updateBreadcrumb() {

	isUpdating.value = true

	await nextTick()
	if (breadcrumbEl.value != null && props.partialLeft != null && props.partialLeft.clientWidth != null && window != null) {

		breadcrumbWidth.value = `max-width: ${0}px`
		let width = userStore.isLoggedIn ?
			props.partialLeft.clientWidth - breadcrumbEl.value.clientHeight - 30 :
			props.partialLeft.clientWidth - (breadcrumbEl.value.clientHeight + (window.innerWidth < 900 ? - 190 : 245))

		if (window.innerWidth < 900)
			width -= 47
		if (width > 0)
			breadcrumbWidth.value = `max-width: ${width}px`

		if (breadcrumbEl.value.clientHeight > 30) {
			shiftToStackedBreadcrumbItems()
		} else if (breadcrumbEl.value.clientHeight <= 30) {
			insertToBreadcrumbItems()
			await nextTick()
			if (breadcrumbEl.value && breadcrumbEl.value.clientHeight > 30) {
				shiftToStackedBreadcrumbItems(false)
			}
		}
	}
	await nextTick()
	isUpdating.value = false
	maxWidth.value = computedMaxWidth.value
}

const rootWikiIsStacked = ref(false)

function shiftToStackedBreadcrumbItems(update: boolean = true) {
	if (breadcrumbItems.value.length > 0) {
		stackedBreadcrumbItems.value.push(breadcrumbItems.value.shift()!)

		if (breadcrumbEl.value!.clientHeight > 21 && breadcrumbItems.value.length > 0 && update) {
			updateBreadcrumb()
		}
	} else if (breadcrumb.value?.rootPage && !rootWikiIsStacked.value) {
		rootWikiIsStacked.value = true
		stackedBreadcrumbItems.value.unshift(breadcrumb.value.rootPage)
	}
}
function insertToBreadcrumbItems() {
	if (stackedBreadcrumbItems.value.length > 0) {
		if (rootWikiIsStacked.value) {
			rootWikiIsStacked.value = false
			stackedBreadcrumbItems.value.shift()
		} else
			breadcrumbItems.value.unshift(stackedBreadcrumbItems.value.pop()!)
	}
}
const pageTitle = ref('')

onBeforeMount(async () => {
	if (typeof window !== 'undefined') {
		window.addEventListener('resize', handleResize)
		window.addEventListener('scroll', handleScroll)
	}
	await nextTick()
	updateBreadcrumb()
	getBreadcrumb()
})

const route = useRoute()
watch(() => route.params, () => {
	if (props.page != PageEnum.Page)
		getBreadcrumb()
})
watch(() => pageStore.id, (newId, oldId) => {
	if (newId > 0 && newId != oldId && props.page == PageEnum.Page) {
		getBreadcrumb()
	}
}, { immediate: true })

watch(() => props.page, (newPage, oldPage) => {
	if (oldPage != newPage && (newPage == PageEnum.Page && pageStore.id > 0))
		getBreadcrumb()
})
const rootPageChipStore = useRootPageChipStore()

async function getBreadcrumb() {
	shouldCalc.value = true

	rootWikiIsStacked.value = false
	breadcrumbItems.value = []
	stackedBreadcrumbItems.value = []

	await nextTick()

	const sessionStorage = window?.sessionStorage

	if (pageStore.isWiki)
		sessionStorage.setItem('currentWikiId', pageStore.id.toString())
	const sessionWikiId = parseInt(sessionStorage.getItem('currentWikiId')!)

	const data = {
		wikiId: !isNaN(sessionWikiId) ? sessionWikiId : 0,
		currentPageId: pageStore.id,
	}
	if (props.page == PageEnum.Page && pageStore.id > 0) {
		const result = await $api<Breadcrumb>(`/apiVue/Breadcrumb/GetBreadcrumb/`,
			{
				method: 'POST',
				body: data,
				credentials: 'include',
				onResponseError(context) {
					throw createError({ statusMessage: context.error?.message })
				}
			})

		if (result) {
			breadcrumb.value = result
			personalWiki.value = result.personalWiki
			breadcrumbItems.value = result.items
			sessionStorage.setItem('currentWikiId', result.newWikiId.toString())
		}
	} else {
		const result = await $api<BreadcrumbItem>(`/apiVue/Breadcrumb/GetPersonalWiki/`,
			{
				method: 'GET',
				credentials: 'include',
				mode: 'no-cors',
				onResponseError(context) {
					throw createError({ statusMessage: context.error?.message })
				}
			})
		personalWiki.value = result

	}
	if (personalWiki.value?.id == 1 || breadcrumbItems.value.some(i => i.id == 1))
		rootPageChipStore.showRootPageChip = false
	else rootPageChipStore.showRootPageChip = true

	setPageTitle()
	updateBreadcrumb()

	await nextTick()
	updateBreadcrumbTimer.value = setTimeout(() => {
		updateBreadcrumb()
		shouldCalc.value = false
	}, 300)
}

function setPageTitle() {
	pageTitle.value = ''
	switch (props.page) {
		case PageEnum.Welcome:
			pageTitle.value = 'Willkommen'
			break
		case PageEnum.Page:
			pageTitle.value = pageStore.name
			break
		// case Page.Question custom breadcrumb item set in the question/[id].vue
		// case Page.Question custom breadcrumb item set in the user/[id].vue
		case PageEnum.Imprint:
			pageTitle.value = 'Impressum'
			break
		case PageEnum.Terms:
			pageTitle.value = 'Geschäftsbedingungen'
			break
		case PageEnum.Register:
			pageTitle.value = 'Registrieren'
			break
		case PageEnum.Messages:
			pageTitle.value = 'Nachrichten'
			break
		// case Page.Default:
		// 	pageTitle.value = ''
		// 	break
		case PageEnum.Error:
			pageTitle.value = 'Fehler'
			break
		case PageEnum.ResetPassword:
			pageTitle.value = 'Passwort zurücksetzen'
			break
		case PageEnum.ConfirmEmail:
			pageTitle.value = 'E-Mail Bestätigung'
			break
		case PageEnum.Metrics:
			pageTitle.value = 'Metriken'
			break
	}
}

watch(() => props.showSearch, (val) => {
	if (!val)
		updateBreadcrumb()
})

const { isMobile } = useDevice()
const windowInnerWidth = ref(0)

const updateBreadcrumbTimer = ref()

onMounted(async () => {
	await nextTick()
	windowInnerWidth.value = window?.innerWidth
	await nextTick()
	updateBreadcrumb()
	await nextTick()
	updateBreadcrumbTimer.value = setTimeout(() => {
		updateBreadcrumb()
		shouldCalc.value = false
	}, 300)
})

onBeforeUnmount(() => clearTimeout(updateBreadcrumbTimer.value))

const shrinkBreadcrumb = ref(false)
watch(() => props.showSearch, (val) => {
	windowInnerWidth.value = window?.innerWidth

	if (isMobile && val) {
		shrinkBreadcrumb.value = true
	} else
		shrinkBreadcrumb.value = false
	updateBreadcrumb()
})

watch(() => userStore.isLoggedIn, async () => {

	windowInnerWidth.value = window?.innerWidth
	await nextTick()
	updateBreadcrumb()
	await nextTick()

	getBreadcrumb()
})

const { $urlHelper } = useNuxtApp()

const lastBreadcrumbItem = ref()
const computedMaxWidth = computed(() => {
	let width = 150
	if (lastBreadcrumbItem.value && !isUpdating.value && window != null) {
		const leftOffset = lastBreadcrumbItem.value.getBoundingClientRect().left > 100 ? lastBreadcrumbItem.value.getBoundingClientRect().left : 100
		const scrollTop = window.scrollY
		let rightOffset = 10
		if (userStore.isLoggedIn) {
			rightOffset = 90
		} else if (scrollTop > 59)
			rightOffset = 80
		const result = window.screen.width - leftOffset - rightOffset
		if (result > 150)
			return result
	}
	return width
})

const maxWidth = ref(150)
const ariaId = useId()
const ariaId2 = useId()
const ariaId3 = useId()
</script>

<template>
	<div v-if="breadcrumb != null && props.page == PageEnum.Page" id="BreadCrumb" ref="breadcrumbEl"
		:style="breadcrumbWidth"
		:class="{ 'search-is-open': props.showSearch && windowInnerWidth < 768, 'pseudo-white': whiteOut }"
		v-show="!shrinkBreadcrumb">

		<VDropdown :aria-id="ariaId" :distance="0" v-if="breadcrumb.personalWiki && pageStore.id == personalWiki?.id">
			<NuxtLink :to="$urlHelper.getPageUrl(breadcrumb.personalWiki.name, breadcrumb.personalWiki.id)"
				class="breadcrumb-item root-page" :class="{ 'is-in-root-page': pageStore.id == personalWiki?.id }"
				aria-label="home button">
				<font-awesome-icon icon="fa-solid fa-house-user" v-if="userStore.isLoggedIn" class="home-btn" />
				<font-awesome-icon icon="fa-solid fa-house" v-else class="home-btn" />
				<span class="root-page-label">
					{{ pageStore.name }}
				</span>
			</NuxtLink>
			<template #popper>
				<p class="breadcrumb-dropdown dropdown-row" v-if="pageStore.id == personalWiki?.id">
					{{ pageStore.name }}
				</p>
			</template>
		</VDropdown>

		<NuxtLink :to="$urlHelper.getPageUrl(breadcrumb.personalWiki.name, breadcrumb.personalWiki.id)" class="breadcrumb-item root-page" v-tooltip="breadcrumb.personalWiki.name" v-else-if="breadcrumb.personalWiki"
			:class="{ 'is-in-root-page': pageStore.id == personalWiki?.id }" aria-label="home button">
			<font-awesome-icon icon="fa-solid fa-house-user" v-if="userStore.isLoggedIn" class="home-btn" />
			<font-awesome-icon icon="fa-solid fa-house" v-else class="home-btn" />
			<span class="root-page-label" v-if="pageStore.id == personalWiki?.id">
				{{ pageStore.name }}
			</span>
		</NuxtLink>

		<template v-if="breadcrumb.rootPage">
			<div
				v-if="breadcrumb.currentPage && breadcrumb.rootPage.id != breadcrumb.currentPage.id && breadcrumb.isInPersonalWiki">
				<div>
					<font-awesome-icon icon="fa-solid fa-chevron-right" />
				</div>
			</div>

			<template
				v-else-if="breadcrumb.personalWiki && breadcrumb.rootPage.id != breadcrumb.personalWiki.id && !breadcrumb.isInPersonalWiki">
				<div class="breadcrumb-divider"></div>
				<template v-if="pageStore.id != breadcrumb.rootPage.id && !rootWikiIsStacked">
					<NuxtLink :to="$urlHelper.getPageUrl(breadcrumb.rootPage.name, breadcrumb.rootPage.id)"
						class="breadcrumb-item" v-tooltip="breadcrumb.rootPage.name" :aria-label="'root page button'">
						{{ breadcrumb.rootPage.name }}
					</NuxtLink>
					<div>
						<font-awesome-icon icon="fa-solid fa-chevron-right" />
					</div>
				</template>
			</template>
		</template>

		<VDropdown :aria-id="ariaId2" v-show="stackedBreadcrumbItems.length > 0" :distance="0">
			<div>
				<font-awesome-icon icon="fa-solid fa-ellipsis" class="breadcrumb-item" />
				<font-awesome-icon icon="fa-solid fa-chevron-right" />
			</div>

			<template #popper="{ hide }">
				<NuxtLink v-for="s in stackedBreadcrumbItems" :to="$urlHelper.getPageUrl(s.name, s.id)" :key="s.id"
					@click="hide" v-tooltip="s.name" :aria-label="s.name">
					<div class="dropdown-row">
						{{ s.name }}
					</div>
				</NuxtLink>
				<div></div>
			</template>
		</VDropdown>

		<template v-for="(b, i) in breadcrumbItems" :key="`breadcrumb-${i}`">
			<NuxtLink :to="$urlHelper.getPageUrl(b.name, b.id)" class="breadcrumb-item" v-tooltip="b.name"
				:aria-label="b.name">
				{{ b.name }}
			</NuxtLink>
			<div>
				<font-awesome-icon icon="fa-solid fa-chevron-right" />
			</div>
		</template>
		<div ref="lastBreadcrumbItem"></div>

		<VDropdown :aria-id="ariaId3" :distance="0" v-if="pageStore.id != personalWiki?.id">
			<div class="breadcrumb-item last" :style="`max-width: ${maxWidth}px`">
				{{ pageStore.name }}
			</div>
			<template #popper>
				<p class="breadcrumb-dropdown dropdown-row">
					{{ pageStore.name }}
				</p>
			</template>
		</VDropdown>

	</div>
	<div v-else-if="personalWiki != null" id="BreadCrumb" :style="breadcrumbWidth">
		<NuxtLink :to="$urlHelper.getPageUrl(personalWiki.name, personalWiki.id)" class="breadcrumb-item" v-tooltip="personalWiki.name" aria-label="personal home button">
			<font-awesome-icon icon="fa-solid fa-house-user" v-if="userStore.isLoggedIn" class="home-btn" />
			<font-awesome-icon icon="fa-solid fa-house" v-else class="home-btn" />
		</NuxtLink>
		<div class="breadcrumb-divider"></div>
		<div class="breadcrumb-item last" v-tooltip="pageStore.name" v-if="props.page == PageEnum.Page">
			{{ pageStore.name }}
		</div>
		<template v-else-if="props.page == PageEnum.Question && props.questionPageData != null">
			<NuxtLink :to="`${questionPageData?.primaryPageUrl}`" class="breadcrumb-item"
				v-tooltip="questionPageData?.primaryPageName" :aria-label="questionPageData?.primaryPageName">
				{{ questionPageData?.primaryPageName }}
			</NuxtLink>
			<div>
				<font-awesome-icon icon="fa-solid fa-chevron-right" />
			</div>
			<div class="breadcrumb-item last">
				{{ questionPageData?.title }}
			</div>
		</template>

		<template v-else-if="props.customBreadcrumbItems != null && props.customBreadcrumbItems.length > 0">
			<template v-for="e, index in props.customBreadcrumbItems">
				<NuxtLink :to="`${e.url}`" class="breadcrumb-item" v-tooltip="e.name" :aria-label="e.name">
					{{ e.name }}
				</NuxtLink>
				<div v-if="index != props.customBreadcrumbItems.length - 1">
					<font-awesome-icon icon="fa-solid fa-chevron-right" />
				</div>
			</template>
		</template>

		<div class="breadcrumb-item last" v-tooltip="pageTitle" v-else>
			{{ pageTitle }}
		</div>
	</div>
	<div ref="remainingWidthDiv"></div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.home-btn {
	font-size: 16px;
}

#BreadCrumb {
	display: flex;
	justify-content: flex-start;
	font-size: 14px;
	color: @memo-grey-dark;
	flex-wrap: wrap;
	opacity: 1;
	transition: opacity 0.5s;
	visibility: visible;
	overflow: hidden;
	min-width: 280px;
	align-items: center;

	.search-is-open {
		width: 0;
		padding: 0;
		margin: 0;
		visibility: hidden;
	}

	.breadcrumb-item {
		padding: 0 12px;
		text-overflow: ellipsis;
		overflow: hidden;
		cursor: pointer;

		color: @memo-grey-dark;
		text-decoration: none;
		font-family: "Open Sans";
		font-size: 14px;
		font-weight: 600;
		padding-left: 10px;
		transition: all 0.1s ease-in-out;

		flex-shrink: 1;

		&.root-page {
			padding-left: 0px;
		}

		@media (max-width: 479px) {
			max-width: 260px;
		}

		&.last {
			max-width: 300px;
			color: @memo-grey-darker;

			@media (max-width: 479px) {
				max-width: 140px;
			}
		}

		&:hover {
			color: @memo-blue;
		}

		&.is-in-root-page {
			padding-right: 0px;
			display: block;

			.root-page-label {
				padding-left: 8px;
			}
		}

	}

	.breadcrumb-divider {
		min-height: 30px;
		height: 60%;
		background: #ddd;
		max-width: 1px;
		border-radius: 4px;
		min-width: 1px;
	}
}

.breadcrumb-dropdown {
	padding: 10px 25px;

}
</style>

<style lang="less">
.pseudo-white {

	.breadcrumb-item,
	.fa-chevron-right {
		color: white !important;

	}
}
</style>