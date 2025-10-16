<script lang="ts" setup>
import { useTabsStore, Tab } from './tabsStore'
import { usePageStore } from '../pageStore'
import { ChartData } from '~/components/chart/chartData'
import { VueElement } from 'vue'

const tabsStore = useTabsStore()
const pageStore = usePageStore()

const { isMobile } = useDevice()

const chartData = computed((): ChartData[] => {
	return convertKnowledgeSummaryToChartData(pageStore.knowledgeSummary)
})

const { t } = useI18n()

function getTooltipLabel(key: string, count: number) {
	switch (key) {
		case 'solid':
			return t('knowledgeStatus.solidCount', count)
		case 'needsConsolidation':
			return t('knowledgeStatus.needsConsolidationCount', count)
		case 'needsLearning':
			return t('knowledgeStatus.needsLearningCount', count)
		case 'notLearned':
			return t('knowledgeStatus.notLearnedCount', count)
		case 'notInWishknowledge':
			return t('knowledgeStatus.notInWishknowledgeCount', count)
	}
}

const ariaId = useId()
const ariaId2 = useId()

// Element refs
const pageTabsBarEl = ref<VueElement | null>(null)
const pageTextTabEl = ref<VueElement | null>(null)
const pageLearningTabEl = ref<VueElement | null>(null)
const pageFeedTabEl = ref<VueElement | null>(null)
const pageAnalyticsTabEl = ref<VueElement | null>(null)

const { sideSheetOpen } = useSideSheetState()
const { suppressScrollX } = useScrollbarSuppression(
	pageTabsBarEl,
	[pageTextTabEl, pageLearningTabEl, pageFeedTabEl, pageAnalyticsTabEl],
	{
		buffer: 2,
		debounceDelay: 150,
		watchSources: [() => pageStore.questionCount, () => chartData.value, () => sideSheetOpen.value]
	}
)

</script>

<template>
	<ClientOnly>
		<PerfectScrollbar class="tabs-bar" :options="{ suppressScrollX: suppressScrollX }">
			<div id="PageTabBar" :class="{ 'is-mobile': isMobile }" ref="pageTabsBarEl">

				<div class="tab" @click="tabsStore.activeTab = Tab.Text" ref="pageTextTabEl">

					<div class="tab-label active" v-if="tabsStore.activeTab === Tab.Text">
						{{ t('page.tabs.text') }}
					</div>
					<div class="tab-label" :class="{ 'invisible-tab': tabsStore.activeTab === Tab.Text }" ref="pageLabelEl">
						{{ t('page.tabs.text') }}
					</div>

					<div class="active-tab" v-if="tabsStore.activeTab === Tab.Text"></div>
					<div class="inactive-tab" v-else>
						<div class="tab-border"></div>
					</div>
				</div>

				<div class="tab" @click="tabsStore.activeTab = Tab.Learning" ref="pageLearningTabEl">

					<div class="tab-label chip-tab active" v-if="tabsStore.activeTab === Tab.Learning">
						{{ t('page.tabs.questions') }}
						<div class="chip" v-if="pageStore.questionCount > 0">
							{{ pageStore.questionCount }}
						</div>
					</div>
					<div class="tab-label chip-tab"
						:class="{ 'invisible-tab': tabsStore.activeTab === Tab.Learning }" ref="learningLabelEl">
						{{ t('page.tabs.questions') }}
						<div class="chip" v-if="pageStore.questionCount > 0">
							{{ pageStore.questionCount }}
						</div>
					</div>

					<div class="active-tab" v-if="tabsStore.activeTab === Tab.Learning"></div>
					<div class="inactive-tab" v-else>
						<div class="tab-border"></div>
					</div>
				</div>

				<div class="tab" @click="tabsStore.activeTab = Tab.Feed" ref="pageFeedTabEl">

					<div class="tab-label active" v-if="tabsStore.activeTab === Tab.Feed">
						{{ t('page.tabs.feed') }}
					</div>
					<div class="tab-label" :class="{ 'invisible-tab': tabsStore.activeTab === Tab.Feed }"
						ref="feedLabelEl">
						{{ t('page.tabs.feed') }}
					</div>

					<div class="active-tab" v-if="tabsStore.activeTab === Tab.Feed"></div>
					<div class="inactive-tab" v-else>
						<div class="tab-border"></div>
					</div>
				</div>

				<div class="tab" @click="tabsStore.activeTab = Tab.Analytics" ref="pageAnalyticsTabEl">

					<div class="tab-label active analytics-tab" v-if="tabsStore.activeTab === Tab.Analytics">
						<template v-if="!isMobile">
							{{ t('page.tabs.analytics') }}
						</template>
						<Suspense>
							<template #default>
								<VTooltip :aria-id="ariaId" class="tooltip-container">
									<div class="pie-container">
										<ChartPie class="pie-chart" :data="chartData" :height="24" :width="24" />
									</div>
									<template #popper>
										<b>{{ t('page.tabs.yourKnowledgeStatus') }}:</b>
										<div v-for="d in chartData" v-if="chartData.some(d => d.value > 0)"
											class="knowledgesummary-info">
											<div class="color-container" :class="`color-${d.class}`"></div>
											<div>{{ getTooltipLabel(d.class!, d.value) }}</div>
										</div>
										<div v-else>
											{{ t('page.tabs.knowledgeStatus.noQuestionAnswered') }}
										</div>
									</template>
								</VTooltip>
							</template>
							<template #fallback>
								<div class="pie-container">
									<div class="pie-chart-placeholder"></div>
								</div>
							</template>
						</Suspense>
					</div>
					<div class="tab-label analytics-tab" :class="{ 'invisible-tab': tabsStore.activeTab === Tab.Analytics }" ref="analyticsLabelEl">
						<template v-if="!isMobile">
							{{ t('page.tabs.analytics') }}
						</template>
						<Suspense>
							<template #default>
								<VTooltip :aria-id="ariaId2" class="tooltip-container">
									<div class="pie-container">
										<ChartPie class="pie-chart" :data="chartData" :height="24" :width="24" />
									</div>
									<template #popper>
										<b>{{ t('page.tabs.yourKnowledgeStatus') }}:</b>
										<div v-for="d in chartData" v-if="chartData.some(d => d.value > 0)"
											class="knowledgesummary-info">
											<div class="color-container" :class="`color-${d.class}`"></div>
											<div>{{ getTooltipLabel(d.class!, d.value) }}</div>
										</div>
										<div v-else>
											{{ t('page.tabs.knowledgeStatus.noQuestionAnswered') }}
										</div>
									</template>
								</VTooltip>
							</template>
							<template #fallback>
								<div class="pie-container">
									<div class="pie-chart-placeholder"></div>
								</div>
							</template>
						</Suspense>
					</div>

					<div class="active-tab" v-if="tabsStore.activeTab === Tab.Analytics"></div>
					<div class="inactive-tab" v-else>
						<div class="tab-border"></div>
					</div>
				</div>

				<div class="tab-filler-container">
					<div class="tab-filler" :class="{ 'mobile': isMobile }"></div>
					<div class="inactive-tab">
						<div class="tab-border"></div>
					</div>
				</div>

			</div>
		</PerfectScrollbar>

		<template #fallback>
			<div id="PageTabBar" class="fallback" :class="{ 'is-mobile': isMobile }">

				<div class="tab">

					<div class="tab-label active" v-if="tabsStore.activeTab === Tab.Text && isMobile">
						{{ t('page.tabs.text') }}
					</div>
					<div class="tab-label active" v-else-if="tabsStore.activeTab === Tab.Text">
						{{ t('page.tabs.text') }}
					</div>
					<div class="tab-label" :class="{ 'invisible-tab': tabsStore.activeTab === Tab.Text }"
						ref="pageLabelEl">
						{{ t('page.tabs.text') }}
					</div>

					<div class="active-tab" v-if="tabsStore.activeTab === Tab.Text"></div>
					<div class="inactive-tab" v-else>
						<div class="tab-border"></div>
					</div>
				</div>

				<div class="tab">

					<div class="tab-label chip-tab active learning-tab" v-if="tabsStore.activeTab === Tab.Learning">
						{{ t('page.tabs.questions') }}
						<div class="chip" v-if="pageStore.questionCount > 0">
							{{ pageStore.questionCount }}
						</div>
					</div>
					<div class="tab-label chip-tab" :class="{ 'invisible-tab': tabsStore.activeTab === Tab.Learning }"
						ref="learningLabelEl">
						{{ t('page.tabs.questions') }}
						<div class="chip" v-if="pageStore.questionCount > 0">
							{{ pageStore.questionCount }}
						</div>
					</div>

					<div class="active-tab" v-if="tabsStore.activeTab === Tab.Learning"></div>
					<div class="inactive-tab" v-else>
						<div class="tab-border"></div>
					</div>

				</div>

				<div class="tab">

					<div class="tab-label active" v-if="tabsStore.activeTab === Tab.Feed && isMobile">
						{{ t('page.tabs.feed') }}
					</div>
					<div class="tab-label active" v-else-if="tabsStore.activeTab === Tab.Feed">
						{{ t('page.tabs.feed') }}
					</div>
					<div class="tab-label" :class="{ 'invisible-tab': tabsStore.activeTab === Tab.Feed }"
						ref="feedLabelEl">
						{{ t('page.tabs.feed') }}
					</div>

					<div class="active-tab" v-if="tabsStore.activeTab === Tab.Feed"></div>
					<div class="inactive-tab" v-else>
						<div class="tab-border"></div>
					</div>
				</div>

				<div class="tab">

					<div class="tab-label active analytics-tab" v-if="tabsStore.activeTab === Tab.Analytics">
						<template v-if="!isMobile">
							{{ t('page.tabs.analytics') }}
						</template>
						<div class="tooltip-container">
							<div class="pie-container">
								<div class="pie-chart-placeholder"></div>
							</div>
						</div>
					</div>
					<div class="tab-label analytics-tab" :class="{ 'invisible-tab': tabsStore.activeTab === Tab.Analytics }" ref="analyticsLabelEl">
						<template v-if="!isMobile">
							{{ t('page.tabs.analytics') }}
						</template>
						<div class="tooltip-container">
							<div class="pie-container">
							</div>
						</div>
					</div>

					<div class="active-tab" v-if="tabsStore.activeTab === Tab.Analytics"></div>
					<div class="inactive-tab" v-else>
						<div class="tab-border"></div>
					</div>
				</div>

				<div class="tab-filler-container">
					<div class="tab-filler" :class="{ 'mobile': isMobile }"></div>
					<div class="inactive-tab">
						<div class="tab-border"></div>
					</div>
				</div>

			</div>
		</template>
	</ClientOnly>
</template>

<style lang="less">
@import '~~/assets/tabs-bar.less';
</style>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.tabs-bar {
	padding-top: 35px;
}

#PageTabBar {
	width: 100%;
	margin-top: 0;
}

.tab {
	user-select: none;
}

.analytics-tab {
	display: flex;
	flex-wrap: nowrap;
	justify-content: center;
	align-items: center;

	.tooltip-container {
		width: 24px;
		height: 24px;
		margin-left: 4px;
		margin-right: -8px;

		.pie-container {
			width: 24px;
			height: 24px;
			margin-right: -8px;

			.pie-chart-placeholder {
				height: 24px;
				width: 24px;
				border-radius: 50%;
				background: @memo-grey-lighter;
			}
		}
	}

	.pie-chart {
		height: 24px;
		width: 24px;
	}
}

.fallback {
	padding-top: 35px;

	.pie-chart {
		margin-left: 4px;
	}
}

.knowledgesummary-info {

	display: flex;
	flex-wrap: nowrap;
	align-items: center;

	.color-container {
		height: 12px;
		width: 12px;
		margin-right: 4px;
		border-radius: 50%;

		&.color-notLearned {
			background: @memo-grey-light;
		}

		&.color-needsLearning {
			background: @memo-salmon;
		}

		&.color-needsConsolidation {
			background: @memo-yellow;
		}

		&.color-solid {
			background: @memo-green;
		}

		&.color-notInWishknowledge {
			background: @memo-grey-dark;
		}
	}
}
</style>
