<script lang="ts" setup>
import { VueElement } from 'vue'
import { TinyPageModel, usePageStore } from '../page/pageStore'
import { SiteType } from '../shared/siteEnum'
import { useUserStore } from '../user/userStore'
import { BreadcrumbItem as CustomBreadcrumbItem } from './breadcrumbItems'
import { useConvertStore } from '../page/convert/convertStore'

interface Props {
	site: SiteType
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
const convertStore = useConvertStore()
const { t } = useI18n()

interface BreadcrumbItem {
	name: string
	id: number
	width?: number
}
interface Breadcrumb {
	newWikiId: number
	items: BreadcrumbItem[]
	currentWiki: TinyPageModel
	currentPage: BreadcrumbItem
	breadcrumbHasGlobalWiki: boolean
	isInPersonalWiki: boolean
}
const breadcrumb = ref<Breadcrumb>()

const breadcrumbItems = ref<BreadcrumbItem[]>([])
const stackedBreadcrumbItems = ref<BreadcrumbItem[]>([])

const breadcrumbEl = ref<VueElement>()
const breadcrumbWidth = ref('')

const clearBreadcrumb = () => {
	breadcrumb.value = undefined
	breadcrumbItems.value = []
	stackedBreadcrumbItems.value = []
	rootWikiIsStacked.value = false
	pageTitle.value = ''
}

const handleResize = () => {
	windowInnerWidth.value = window?.innerWidth
	updateBreadcrumb()
}

const handleScroll = () => {
	if (userStore.isLoggedIn || window?.scrollY > 105)
		return
	updateBreadcrumb()
}

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

const updateBreadcrumb = async () => {

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

const shiftToStackedBreadcrumbItems = (update: boolean = true) => {
	if (breadcrumbItems.value.length > 0) {
		stackedBreadcrumbItems.value.push(breadcrumbItems.value.shift()!)

		if (breadcrumbEl.value!.clientHeight > 21 && breadcrumbItems.value.length > 0 && update) {
			updateBreadcrumb()
		}
	} else if (breadcrumb.value?.currentWiki && !rootWikiIsStacked.value) {
		rootWikiIsStacked.value = true
		stackedBreadcrumbItems.value.unshift(breadcrumb.value.currentWiki)
	}
}
const insertToBreadcrumbItems = () => {
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
	if (props.site != SiteType.Page)
		getBreadcrumb()
})

watch(() => pageStore.id, (newId, oldId) => {
	if (newId > 0 && newId != oldId && props.site === SiteType.Page)
		getBreadcrumb()
})

watch(() => props.site, (newPage, oldPage) => {
	if (oldPage != newPage && (newPage === SiteType.Page && pageStore.id > 0))
		getBreadcrumb()
})

const getBreadcrumb = async () => {
	clearBreadcrumb()

	await nextTick()

	const sessionStorage = window?.sessionStorage

	if (pageStore.isWiki)
		sessionStorage.setItem('currentWikiId', pageStore.id.toString())
	const sessionWikiId = parseInt(sessionStorage.getItem('currentWikiId')!)

	const data = {
		wikiId: !isNaN(sessionWikiId) ? sessionWikiId : 0,
		currentPageId: pageStore.id,
	}
	if (props.site === SiteType.Page && pageStore.id > 0) {
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
			breadcrumbItems.value = result.items
			sessionStorage.setItem('currentWikiId', result.newWikiId.toString())
			pageStore.currentWiki = result.currentWiki
		}
	}

	setPageTitle()
	updateBreadcrumb()

	await nextTick()
	updateBreadcrumbTimer.value = setTimeout(() => {
		updateBreadcrumb()
		shouldCalc.value = false
	}, 300)
}

const setPageTitle = () => {
	pageTitle.value = ''
	switch (props.site) {
		case SiteType.Welcome:
			pageTitle.value = t('breadcrumb.titles.welcome')
			break
		case SiteType.Page:
			pageTitle.value = pageStore.name
			break
		// case Page.Question custom breadcrumb item set in the question/[id].vue
		// case Page.Question custom breadcrumb item set in the user/[id].vue
		case SiteType.Imprint:
			pageTitle.value = t('breadcrumb.titles.imprint')
			break
		case SiteType.Terms:
			pageTitle.value = t('breadcrumb.titles.terms')
			break
		case SiteType.Register:
			pageTitle.value = t('breadcrumb.titles.register')
			break
		case SiteType.Messages:
			pageTitle.value = t('breadcrumb.titles.messages')
			break
		case SiteType.MissionControl:
			pageTitle.value = t('breadcrumb.titles.missionControl')
			break
		// case Page.Default:
		// 	pageTitle.value = ''
		// 	break
		case SiteType.Error:
			pageTitle.value = t('breadcrumb.titles.error')
			break
		case SiteType.ResetPassword:
			pageTitle.value = t('breadcrumb.titles.resetPassword')
			break
		case SiteType.ConfirmEmail:
			pageTitle.value = t('breadcrumb.titles.confirmEmail')
			break
		case SiteType.Metrics:
			pageTitle.value = t('breadcrumb.titles.metrics')
			break
		case SiteType.AllWishknowledgeLearning:
			pageTitle.value = t('breadcrumb.titles.allWishknowledgeLearning')
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

convertStore.$onAction(({ name, after }) => {
	if (name === 'confirmConversion') {
		after(async () => {
			await getBreadcrumb()
		})
	}
})

</script>

<template>
	<div v-if="breadcrumb != null && props.site === SiteType.Page" id="BreadCrumb" ref="breadcrumbEl"
		:style="breadcrumbWidth"
		:class="{ 'search-is-open': props.showSearch && windowInnerWidth < 768, 'pseudo-white': whiteOut }"
		v-show="!shrinkBreadcrumb">

		<template v-if="breadcrumb.currentWiki">
			<template v-if="pageStore.id != breadcrumb.currentWiki.id && !rootWikiIsStacked">
				<NuxtLink :to="$urlHelper.getPageUrl(breadcrumb.currentWiki.name, breadcrumb.currentWiki.id)"
					class="breadcrumb-item current-wiki" v-tooltip="breadcrumb.currentWiki.name"
					:aria-label="t('breadcrumb.aria.rootPageButton')">
					{{ breadcrumb.currentWiki.name }}
				</NuxtLink>
				<div>
					<font-awesome-icon icon="fa-solid fa-chevron-right" />
				</div>
			</template>
		</template>

		<VDropdown :aria-id="ariaId" v-show="stackedBreadcrumbItems.length > 0" :distance="0">
			<div>
				<font-awesome-icon icon="fa-solid fa-ellipsis" class="breadcrumb-item" />
				<font-awesome-icon icon="fa-solid fa-chevron-right" />
			</div>

			<template #popper="{ hide }">
				<NuxtLink v-for="s in stackedBreadcrumbItems" :to="$urlHelper.getPageUrl(s.name, s.id)" :key="s.id"
					@click="hide" v-tooltip="s.name" :aria-label="t('breadcrumb.aria.pageLink', { pageName: s.name })">
					<div class="dropdown-row">
						{{ s.name }}
					</div>
				</NuxtLink>
				<div></div>
			</template>
		</VDropdown>

		<template v-for="(b, i) in breadcrumbItems" :key="`breadcrumb-${i}`">
			<NuxtLink :to="$urlHelper.getPageUrl(b.name, b.id)" class="breadcrumb-item" v-tooltip="b.name"
				:aria-label="t('breadcrumb.aria.pageLink', { pageName: b.name })">
				{{ b.name }}
			</NuxtLink>
			<div>
				<font-awesome-icon icon="fa-solid fa-chevron-right" />
			</div>
		</template>
		<div ref="lastBreadcrumbItem"></div>

		<VDropdown :aria-id="ariaId2" :distance="0">
			<div class="breadcrumb-item last" :style="`max-width: ${maxWidth}px`" :class="{ 'current-wiki': pageStore.id === pageStore.currentWiki?.id }">
				{{ pageStore.name }}
			</div>
			<template #popper>
				<p class="breadcrumb-dropdown dropdown-row">
					{{ pageStore.name }}
				</p>
			</template>
		</VDropdown>

	</div>
	<div v-else-if="props.customBreadcrumbItems && props.customBreadcrumbItems.length > 0" id="BreadCrumb" ref="breadcrumbEl" :style="breadcrumbWidth">
		<template v-for="(item, index) in props.customBreadcrumbItems" :key="`custom-breadcrumb-${index}`">
			<NuxtLink v-if="item.url" :to="item.url" class="breadcrumb-item" :class="{ 'no-pl': index === 0 }" v-tooltip="item.name"
				:aria-label="item.name">
				{{ item.name }}
			</NuxtLink>
			<div v-else class="breadcrumb-item no-pl">
				{{ item.name }}
			</div>
			<div v-if="index < props.customBreadcrumbItems.length - 1">
				<font-awesome-icon icon="fa-solid fa-chevron-right" />
			</div>
		</template>
	</div>
	<div v-else id="BreadCrumb" ref="breadcrumbEl" :style="breadcrumbWidth">
		<div class="breadcrumb-item no-pl">
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
	min-width: 220px;
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

		&.current-wiki {
			padding-left: 0px;
		}

		&.no-pl {
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

		&.is-in-current-wiki {
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