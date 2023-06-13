<script lang="ts" setup>
import { VueElement } from 'vue'
import { useTopicStore } from '../topic/topicStore'
import { Page } from '../shared/pageEnum'
import { useUserStore } from '../user/userStore'
import { BreadcrumbItem as CustomBreadcrumbItem } from './breadcrumbItems'
import { useRootTopicChipStore } from './rootTopicChipStore'

interface Props {
	page: Page,
	showSearch: boolean,
	questionPageData?: {
		primaryTopicName: string
		primaryTopicUrl: string
		title: string
	}
	customBreadcrumbItems?: CustomBreadcrumbItem[]
	partialLeft?: VueElement
}

const props = defineProps<Props>()

const userStore = useUserStore()
const topicStore = useTopicStore()
interface BreadcrumbItem {
	Name: string
	Id: number
	width?: number
}
interface Breadcrumb {
	newWikiId: number
	personalWiki: BreadcrumbItem
	items: BreadcrumbItem[]
	rootTopic: BreadcrumbItem
	currentTopic: BreadcrumbItem
	breadcrumbHasGlobalWiki: boolean
	isInPersonalWiki: boolean
}
const breadcrumb = ref<Breadcrumb>()

const breadcrumbItems = ref<BreadcrumbItem[]>([])
const stackedBreadcrumbItems = ref<BreadcrumbItem[]>([])

const breadcrumbEl = ref<VueElement>()
const breadcrumbWidth = ref('')

function startUpdateBreadcrumb() {
	updateBreadcrumb()
}

function handleResize() {
	windowInnerWidth.value = window.innerWidth
	startUpdateBreadcrumb()
}

function handleScroll() {
	if (userStore.isLoggedIn || window?.pageYOffset > 105)
		return
	startUpdateBreadcrumb()
}
const personalWiki = ref<BreadcrumbItem | null>(null)

async function updateBreadcrumb() {
	await nextTick()
	if (document.getElementById('BreadCrumb') != null && props.partialLeft != null && props.partialLeft.clientWidth != null) {

		breadcrumbWidth.value = `max-width: ${0}px`
		const width = userStore.isLoggedIn ? props.partialLeft.clientWidth - document.getElementById('BreadCrumb')!.clientHeight - 30 : props.partialLeft.clientWidth - document.getElementById('BreadCrumb')!.clientHeight + 200
		if (width > 0)
			breadcrumbWidth.value = `max-width: ${width}px`

		if (document.getElementById('BreadCrumb')!.clientHeight > 30) {
			shiftToStackedBreadcrumbItems()
		} else if (document.getElementById('BreadCrumb')!.clientHeight == 30) {
			insertToBreadcrumbItems()
			await nextTick()
			if (breadcrumbEl.value && document.getElementById('BreadCrumb')!.clientHeight > 30) {
				shiftToStackedBreadcrumbItems(false)
			}
		}
	}
}

const rootWikiIsStacked = ref(false)

function shiftToStackedBreadcrumbItems(update: boolean = true) {
	if (breadcrumbItems.value.length > 0) {
		stackedBreadcrumbItems.value.push(breadcrumbItems.value.shift()!)

		if (breadcrumbEl.value!.clientHeight > 21 && breadcrumbItems.value.length > 0 && update) {
			updateBreadcrumb()
		}
	} else if (breadcrumb.value?.rootTopic && !rootWikiIsStacked.value) {
		rootWikiIsStacked.value = true
		stackedBreadcrumbItems.value.unshift(breadcrumb.value.rootTopic)
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
	startUpdateBreadcrumb()
	getBreadcrumb()

})

onBeforeUnmount(() => {
	if (typeof window !== 'undefined') {
		window.removeEventListener('resize', handleResize)
		window.removeEventListener('scroll', handleScroll)
	}
})

const route = useRoute()
watch(() => route.params, () => {
	if (props.page != Page.Topic)
		getBreadcrumb()
})
watch(() => topicStore.id, (newId, oldId) => {
	if (newId > 0 && newId != oldId && props.page == Page.Topic) {
		getBreadcrumb()
	}
})

watch(() => props.page, (newPage, oldPage) => {
	if (oldPage != newPage && (newPage == Page.Topic && topicStore.id > 0))
		getBreadcrumb()
})
const rootTopicChipStore = useRootTopicChipStore()

async function getBreadcrumb() {
	breadcrumbItems.value = []
	stackedBreadcrumbItems.value = []

	var sessionStorage = window.sessionStorage

	if (topicStore.isWiki)
		sessionStorage.setItem('currentWikiId', topicStore.id.toString())
	var sessionWikiId = parseInt(sessionStorage.getItem('currentWikiId')!)

	var currentWikiId = 0;
	if (!isNaN(sessionWikiId))
		currentWikiId = sessionWikiId

	const data = {
		wikiId: currentWikiId,
		currentCategoryId: topicStore.id,
	}

	if (props.page == Page.Topic) {
		const result = await $fetch<Breadcrumb>(`/apiVue/Breadcrumb/GetBreadcrumb/`,
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
		const result = await $fetch<BreadcrumbItem>(`/apiVue/Breadcrumb/GetPersonalWiki/`,
			{
				method: 'POST',
				body: data,
				credentials: 'include',
				mode: 'no-cors',
				onResponseError(context) {
					throw createError({ statusMessage: context.error?.message })
				}
			})

		personalWiki.value = result

	}
	if (personalWiki.value?.Id == 1 || breadcrumbItems.value.some(i => i.Id == 1))
		rootTopicChipStore.showRootTopicChip = false
	else rootTopicChipStore.showRootTopicChip = true

	setPageTitle()
	updateBreadcrumb()
}

function setPageTitle() {
	switch (props.page) {
		case Page.Topic:
			pageTitle.value = topicStore.name
			break
		case Page.Register:
			pageTitle.value = 'Registrieren'
			break
		case Page.Welcome:
			pageTitle.value = 'Willkommen'
			break
	}
}

watch(() => props.showSearch, (val) => {
	if (!val)
		startUpdateBreadcrumb()
})

const { isMobile } = useDevice()
const windowInnerWidth = ref(0)
onMounted(async () => {
	windowInnerWidth.value = window.innerWidth
	await nextTick()
	startUpdateBreadcrumb()
})

const shrinkBreadcrumb = ref(false)
watch(() => props.showSearch, (val) => {
	windowInnerWidth.value = window.innerWidth

	if (isMobile && val) {
		shrinkBreadcrumb.value = true
	} else
		shrinkBreadcrumb.value = false
	startUpdateBreadcrumb()
})

watch(() => userStore.isLoggedIn, () => {
	getBreadcrumb()
})

const { $urlHelper } = useNuxtApp()
</script>

<template>
	<div v-if="breadcrumb != null && props.page == Page.Topic" id="BreadCrumb" ref="breadcrumbEl" :style="breadcrumbWidth"
		:class="{ 'search-is-open': props.showSearch && windowInnerWidth < 768 }" v-show="!shrinkBreadcrumb">

		<NuxtLink :to="$urlHelper.getTopicUrl(breadcrumb.personalWiki.Name, breadcrumb.personalWiki.Id)"
			class="breadcrumb-item root-topic" v-tooltip="breadcrumb.personalWiki.Name" v-if="breadcrumb.personalWiki"
			:class="{ 'is-in-root-topic': topicStore.id == personalWiki?.Id }">
			<font-awesome-icon icon="fa-solid fa-house-user" v-if="userStore.isLoggedIn" class="home-btn" />
			<font-awesome-icon icon="fa-solid fa-house" v-else class="home-btn" />
			<span class="root-topic-label" v-if="topicStore.id == personalWiki?.Id">
				{{ topicStore.name }}
			</span>
		</NuxtLink>

		<template v-if="breadcrumb.rootTopic">
			<div
				v-if="breadcrumb.currentTopic && breadcrumb.rootTopic.Id != breadcrumb.currentTopic.Id && breadcrumb.isInPersonalWiki">
				<div>
					<font-awesome-icon icon="fa-solid fa-chevron-right" />
				</div>
			</div>

			<template
				v-else-if="breadcrumb.personalWiki && breadcrumb.rootTopic.Id != breadcrumb.personalWiki.Id && !breadcrumb.isInPersonalWiki">
				<div class="breadcrumb-divider"></div>
				<template v-if="topicStore.id != breadcrumb.rootTopic.Id && !rootWikiIsStacked">
					<NuxtLink :to="$urlHelper.getTopicUrl(breadcrumb.rootTopic.Name, breadcrumb.rootTopic.Id)"
						class="breadcrumb-item" v-tooltip="breadcrumb.rootTopic.Name">
						{{ breadcrumb.rootTopic.Name }}
					</NuxtLink>
					<div>
						<font-awesome-icon icon="fa-solid fa-chevron-right" />
					</div>
				</template>
			</template>
		</template>

		<VDropdown v-show="stackedBreadcrumbItems.length > 0" :distance="0">
			<div>
				<font-awesome-icon icon="fa-solid fa-ellipsis" class="breadcrumb-item" />
				<font-awesome-icon icon="fa-solid fa-chevron-right" />
			</div>

			<template #popper>

				<NuxtLink v-for="(s, i) in stackedBreadcrumbItems" :to="$urlHelper.getTopicUrl(s.Name, s.Id)"
					v-tooltip="s.Name">
					<div class="dropdown-row">
						{{ s.Name }}
					</div>
				</NuxtLink>
				<div></div>
			</template>
		</VDropdown>

		<template v-for="(b, i) in breadcrumbItems" :key="`breadcrumb-${i}`">
			<NuxtLink :to="$urlHelper.getTopicUrl(b.Name, b.Id)" class="breadcrumb-item" v-tooltip="b.Name">
				{{ b.Name }}
			</NuxtLink>
			<div>
				<font-awesome-icon icon="fa-solid fa-chevron-right" />
			</div>
		</template>

		<div class="breadcrumb-item last" v-tooltip="topicStore.name" v-if="topicStore.id != personalWiki?.Id">
			{{ topicStore.name }}
		</div>
	</div>
	<div v-else-if="personalWiki != null" id="BreadCrumb" :style="breadcrumbWidth">
		<NuxtLink :to="$urlHelper.getTopicUrl(personalWiki.Name, personalWiki.Id)" class="breadcrumb-item"
			v-tooltip="personalWiki.Name">
			<font-awesome-icon icon="fa-solid fa-house-user" v-if="userStore.isLoggedIn" class="home-btn" />
			<font-awesome-icon icon="fa-solid fa-house" v-else class="home-btn" />
		</NuxtLink>
		<div class="breadcrumb-divider"></div>
		<div class="breadcrumb-item last" v-tooltip="topicStore.name" v-if="props.page == Page.Topic">
			{{ topicStore.name }}
		</div>
		<template v-else-if="props.page == Page.Question && props.questionPageData != null">
			<NuxtLink :to="`${questionPageData?.primaryTopicUrl}`" class="breadcrumb-item"
				v-tooltip="questionPageData?.primaryTopicName">
				{{ questionPageData?.primaryTopicName }}
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
				<NuxtLink :to="`${e.url}`" class="breadcrumb-item" v-tooltip="e.name">
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

		&.root-topic {
			padding-left: 0px;
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

		&.is-in-root-topic {
			padding-right: 0px;
			display: block;

			.root-topic-label {
				padding-left: 6px;
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
</style>