<script lang="ts" setup>
import { useTabsStore, Tab } from './tabsStore'
import { useTopicStore } from '../topicStore'
import { VueElement } from 'vue'
import { ChartData } from '~~/components/chart/chartData'

const tabsStore = useTabsStore()
const topicStore = useTopicStore()

const { isMobile } = useDevice()

const topicLabelEl = ref()
const learningLabelEl = ref()
const analyticsLabelEl = ref()

function getWidth(e: VueElement) {
	if (e != null)
		return `width: ${e.clientWidth}px`
}
const chartData = ref<ChartData[]>([])

function setChartData() {

	chartData.value = []
	for (const [key, value] of Object.entries(topicStore.knowledgeSummary)) {
		chartData.value.push({
			value: value,
			class: key,
		})
	}
	chartData.value = chartData.value.slice().reverse()
}

function getTooltipLabel(key: string, count: number) {
	switch (key) {
		case 'solid':
			return `Sicheres Wissen: ${count} Fragen`
		case 'needsConsolidation':
			return `Solltest du festigen: ${count} Fragen`
		case 'needsLearning':
			return `Solltest du lernen: ${count} Fragen`
		case 'notLearned':
			return `Noch nicht gelernt: ${count} Fragen`
	}
}

onBeforeMount(() => setChartData())
watch(() => topicStore.knowledgeSummary, () => setChartData(), { deep: true })
const ariaId = useId()
const ariaId2 = useId()

</script>

<template>
	<ClientOnly>
		<div>
			<div class="tabs-bar" :class="{ 'is-mobile': isMobile }">
				<perfect-scrollbar>
					<div id="TopicTabBar" class="col-xs-12" :class="{ 'is-mobile': isMobile }">

						<div class="tab" @click="tabsStore.activeTab = Tab.Topic">

							<div class="tab-label active" v-if="tabsStore.activeTab == Tab.Topic"
								:style="getWidth(topicLabelEl)">
								Thema
							</div>
							<div class="tab-label" :class="{ 'invisible-tab': tabsStore.activeTab == Tab.Topic }"
								ref="topicLabelEl">
								Thema
							</div>

							<div class="active-tab" v-if="tabsStore.activeTab == Tab.Topic"></div>
							<div class="inactive-tab" v-else>
								<div class="tab-border"></div>
							</div>
						</div>

						<div class="tab" @click="tabsStore.activeTab = Tab.Learning">

							<div class="tab-label chip-tab active" v-if="tabsStore.activeTab == Tab.Learning"
								:style="getWidth(learningLabelEl)">
								Fragen
								<div class="chip" v-if="topicStore.questionCount > 0">
									{{ topicStore.questionCount }}
								</div>
							</div>
							<div class="tab-label chip-tab"
								:class="{ 'invisible-tab': tabsStore.activeTab == Tab.Learning }" ref="learningLabelEl">
								Fragen
								<div class="chip" v-if="topicStore.questionCount > 0">
									{{ topicStore.questionCount }}
								</div>
							</div>

							<div class="active-tab" v-if="tabsStore.activeTab == Tab.Learning"></div>
							<div class="inactive-tab" v-else>
								<div class="tab-border"></div>
							</div>
						</div>

						<div class="tab" @click="tabsStore.activeTab = Tab.Analytics">

							<div class="tab-label active analytics-tab" v-if="tabsStore.activeTab == Tab.Analytics"
								:style="getWidth(analyticsLabelEl)">
								<template v-if="!isMobile">
									Analytics
								</template>
								<VTooltip :aria-id="ariaId" class="tooltip-container">
									<div class="pie-container">
										<LazyChartPie class="pie-chart" :data="chartData" :height="24" :width="24" />
									</div>
									<template #popper>
										<b>Dein Wissenstand:</b>
										<div v-for="d in chartData" v-if="chartData.some(d => d.value > 0)"
											class="knowledgesummary-info">
											<div class="color-container" :class="`color-${d.class}`"></div>
											<div>{{ getTooltipLabel(d.class!, d.value) }}</div>
										</div>
										<div v-else>
											Du hast noch keine Fragen in diesem Thema
										</div>
									</template>
								</VTooltip>
							</div>
							<div class="tab-label analytics-tab" :class="{ 'invisible-tab': tabsStore.activeTab == Tab.Analytics }" ref="analyticsLabelEl">
								<template v-if="!isMobile">
									Analytics
								</template>
								<VTooltip :aria-id="ariaId2" class="tooltip-container">
									<div class="pie-container">
										<LazyChartPie class="pie-chart" :data="chartData" :height="24" :width="24" />
									</div>
									<template #popper>
										<b>Dein Wissenstand:</b>
										<div v-for="d in chartData" v-if="chartData.some(d => d.value > 0)"
											class="knowledgesummary-info">
											<div class="color-container" :class="`color-${d.class}`"></div>
											<div>{{ getTooltipLabel(d.class!, d.value) }}</div>
										</div>
										<div v-else>
											Du hast noch keine Fragen
											<br />
											in diesem Thema
										</div>
									</template>
								</VTooltip>
							</div>

							<div class="active-tab" v-if="tabsStore.activeTab == Tab.Analytics"></div>
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
				</perfect-scrollbar>
			</div>
		</div>

		<template #fallback>
			<div id="TopicTabBar" class="col-xs-12 fallback" :class="{ 'is-mobile': isMobile }">

				<div class="tab">

					<div class="tab-label active" v-if="tabsStore.activeTab == Tab.Topic && isMobile" style="width:82px"
						:style="getWidth(topicLabelEl)">
						Thema
					</div>
					<div class="tab-label active" v-else-if="tabsStore.activeTab == Tab.Topic" style="width:90px"
						:style="getWidth(topicLabelEl)">
						Thema
					</div>
					<div class="tab-label" :class="{ 'invisible-tab': tabsStore.activeTab == Tab.Topic }"
						ref="topicLabelEl">
						Thema
					</div>

					<div class="active-tab" v-if="tabsStore.activeTab == Tab.Topic"></div>
					<div class="inactive-tab" v-else>
						<div class="tab-border"></div>
					</div>
				</div>

				<div class="tab">

					<div class="tab-label chip-tab active learning-tab" v-if="tabsStore.activeTab == Tab.Learning"
						:style="getWidth(learningLabelEl)">
						Fragen
						<div class="chip" v-if="topicStore.questionCount > 0">
							{{ topicStore.questionCount }}
						</div>
					</div>
					<div class="tab-label chip-tab" :class="{ 'invisible-tab': tabsStore.activeTab == Tab.Learning }"
						ref="learningLabelEl">
						Fragen
						<div class="chip" v-if="topicStore.questionCount > 0">
							{{ topicStore.questionCount }}
						</div>
					</div>

					<div class="active-tab" v-if="tabsStore.activeTab == Tab.Learning"></div>
					<div class="inactive-tab" v-else>
						<div class="tab-border"></div>
					</div>

				</div>

				<div class="tab">

					<div class="tab-label active analytics-tab" v-if="tabsStore.activeTab == Tab.Analytics"
						:style="getWidth(analyticsLabelEl)">

						<template v-if="!isMobile">
							Analytics
						</template>
						<!-- <ChartPie class="pie-chart" :data="chartData" :height="24" :width="24" /> -->
					</div>
					<div class="tab-label analytics-tab" :class="{ 'invisible-tab': tabsStore.activeTab == Tab.Analytics }" ref="analyticsLabelEl">
						<template v-if="!isMobile">
							Analytics
						</template>
						<!-- <ChartPie class="pie-chart" :data="chartData" :height="24" :width="24" /> -->
					</div>

					<div class="active-tab" v-if="tabsStore.activeTab == Tab.Analytics"></div>
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

.fallback {
	.learning-tab {
		margin-left: 15px;
		margin-right: -15px;
	}
	
	.analytics-tab {
		&.active {
			margin-left: 15px;
			margin-right: -15px;
		}
	}
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

		.pie-container {
			width: 24px;
			height: 24px;
		}
	}

	.pie-chart {
		height: 24px;
		width: 24px;
	}
}

.fallback {
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
	}
}
</style>
