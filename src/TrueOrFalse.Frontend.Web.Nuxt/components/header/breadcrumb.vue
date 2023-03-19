<script lang="ts" setup>
import { VueElement } from 'vue'
import { useTopicStore } from '../topic/topicStore'
import _ from 'underscore'
import { Page } from '../shared/pageEnum'
import { useUserStore } from '../user/userStore'
import { BreadcrumbItem as CustomBreadcrumbItem } from './breadcrumbItems'

interface Props {
	headerContainer?: VueElement,
	headerExtras?: VueElement,
	page: Page,
	showSearch: boolean,
	partialSpacer?: VueElement,
	questionPageData?: {
		primaryTopicName: string
		primaryTopicUrl: string
		title: string
	}
	customBreadcrumbItems?: CustomBreadcrumbItem[]
}

const props = defineProps<Props>()

const userStore = useUserStore()
const topicStore = useTopicStore()
interface BreadcrumbItem {
	Name: string
	Id: number
}
class Breadcrumb {
	newWikiId: number = 0
	personalWiki: BreadcrumbItem | null = null
	items: BreadcrumbItem[] = []
	rootTopic: BreadcrumbItem | null = null
	currentTopic: BreadcrumbItem | null = null
	breadcrumbHasGlobalWiki: boolean = false
	isInPersonalWiki: boolean = false
}
const breadcrumb = ref<Breadcrumb | null>(null)

const breadcrumbItems = ref<BreadcrumbItem[]>([])
const stackedBreadcrumbItems = ref<BreadcrumbItem[]>([])

const breadcrumbEl = ref<VueElement | null>(null)
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

const updateBreadcrumb = _.throttle(async () => {

	if (breadcrumbEl.value != null && breadcrumbEl.value.clientHeight != null && props.headerContainer != null && props.headerExtras != null) {
		const width = props.headerContainer.clientWidth - props.headerExtras.clientWidth - 30

		if (width > 0)
			breadcrumbWidth.value = `width: ${width}px`

		if (breadcrumbEl.value.clientHeight > 21) {
			shiftToStackedBreadcrumbItems()
		} else if (breadcrumbEl.value.clientHeight < 22) {
			insertToBreadcrumbItems()
			await nextTick()
			if (breadcrumbEl.value && breadcrumbEl.value.clientHeight > 21) {
				shiftToStackedBreadcrumbItems()
			}
		}
	}
	await nextTick()
}, 200)

function shiftToStackedBreadcrumbItems() {
	if (breadcrumbItems.value.length > 0)
		stackedBreadcrumbItems.value.push(breadcrumbItems.value.shift()!)
}
function insertToBreadcrumbItems() {
	if (stackedBreadcrumbItems.value.length > 0)
		breadcrumbItems.value.unshift(stackedBreadcrumbItems.value.pop()!)
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

async function getBreadcrumb() {
	breadcrumbItems.value = []
	stackedBreadcrumbItems.value = []
	await nextTick()

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
			})

		breadcrumb.value = result
		personalWiki.value = result.personalWiki
		breadcrumbItems.value = result.items
		sessionStorage.setItem('currentWikiId', result.newWikiId.toString())
	} else {
		const result = await $fetch<BreadcrumbItem>(`/apiVue/Breadcrumb/GetPersonalWiki/`,
			{
				method: 'POST',
				body: data,
				credentials: 'include',
				mode: 'no-cors',
			})

		personalWiki.value = result

	}
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

const { isDesktopOrTablet, isMobile } = useDevice()
const windowInnerWidth = ref(0)
onMounted(async () => {
	windowInnerWidth.value = window.innerWidth
	await nextTick()
	startUpdateBreadcrumb()
})
onUpdated(() => {
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

function showBreadcrumb(e: any) {
	return true
}

watch(() => userStore.isLoggedIn, () => {
	getBreadcrumb()
})
</script>

<template>
	<div v-if="breadcrumb != null && props.page == Page.Topic" id="BreadCrumb" ref="breadcrumbEl" :style="breadcrumbWidth"
		:class="{ 'search-is-open': props.showSearch && windowInnerWidth < 768 }" v-show="!shrinkBreadcrumb">

		<NuxtLink :to="`/${encodeURI(breadcrumb.personalWiki.Name.replaceAll(' ', '-'))}/${breadcrumb.personalWiki.Id}`"
			class="breadcrumb-item" v-tooltip="breadcrumb.personalWiki.Name" v-if="breadcrumb.personalWiki"
			:class="{ 'is-in-root-topic': topicStore.id == personalWiki?.Id }">
			<font-awesome-icon icon="fa-solid fa-house-user" v-if="userStore.isLoggedIn" class="home-btn" />
			<font-awesome-icon icon="fa-solid fa-house" v-else class="home-btn" />

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
				<template v-if="topicStore.id != breadcrumb.rootTopic.Id">
					<NuxtLink
						:to="`/${encodeURI(breadcrumb.rootTopic.Name.replaceAll(' ', '-'))}/${breadcrumb.rootTopic.Id}`"
						class="breadcrumb-item" v-tooltip="breadcrumb.rootTopic.Name">
						{{ breadcrumb.rootTopic.Name }}
					</NuxtLink>
					<div>
						<font-awesome-icon icon="fa-solid fa-chevron-right" />
					</div>
				</template>
			</template>
		</template>

		<V-Dropdown v-show="stackedBreadcrumbItems.length > 0" :distance="0">
			<div>
				<font-awesome-icon icon="fa-solid fa-ellipsis" class="breadcrumb-item" />
				<font-awesome-icon icon="fa-solid fa-chevron-right" />
			</div>

			<template #popper>

				<NuxtLink v-for="s in stackedBreadcrumbItems" :to="`/${encodeURI(s.Name.replaceAll(' ', '-'))}/${s.Id}`"
					v-tooltip="s.Name">
					<div class="dropdown-row">
						{{ s.Name }}
					</div>
				</NuxtLink>
				<div></div>
			</template>
		</V-Dropdown>

		<template v-for="(b, i) in breadcrumbItems" :key="`breadcrumb-${i}`">
			<NuxtLink :to="`/${encodeURI(b.Name.replaceAll(' ', '-'))}/${b.Id}`" class="breadcrumb-item" v-tooltip="b.Name"
				:ref="el => showBreadcrumb(el)">
				{{ b.Name }}
			</NuxtLink>
			<div>
				<font-awesome-icon icon="fa-solid fa-chevron-right" />
			</div>
		</template>

		<div class="breadcrumb-item last" v-tooltip="topicStore.name">
			{{ topicStore.name }}
		</div>
	</div>
	<div v-else-if="personalWiki != null" id="BreadCrumb" :style="breadcrumbWidth">
		<NuxtLink :to="`/${encodeURI(personalWiki.Name.replaceAll(' ', '-'))}/${personalWiki.Id}`" class="breadcrumb-item"
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
				v-tooltip="questionPageData?.primaryTopicName" :ref="el => showBreadcrumb(el)">
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
				<NuxtLink :to="`${e.url}`" class="breadcrumb-item" v-tooltip="e.name" :ref="el => showBreadcrumb(el)">
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
	max-height: 22px;
	overflow: hidden;
	min-width: 240px;

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

		&.last {
			max-width: 300px;
			color: @memo-grey-darker;
		}

		&:hover {
			color: @memo-blue;
		}

		&.is-in-root-topic {
			padding-right: 0px;
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