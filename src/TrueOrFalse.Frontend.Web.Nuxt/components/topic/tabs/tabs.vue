<script lang="ts" setup>
import { useTabsStore, Tab } from './tabsStore'
import { useTopicStore } from '../topicStore'
import { VueElement } from 'vue'

const tabsStore = useTabsStore()
const topicStore = useTopicStore()

const { isMobile } = useDevice()

const topicLabelEl = ref()
const learningLabelEl = ref()
const feedLabelEl = ref()
const analyticsLabelEl = ref()

function getWidth(e: VueElement) {
	if (e != null)

		return `width: ${e.clientWidth}px`
}
</script>

<template>
	<ClientOnly>
		<div class="tabs-bar">
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

					<div class="tab" @click="tabsStore.activeTab = Tab.Feed">

						<div class="tab-label active" v-if="tabsStore.activeTab == Tab.Feed" :style="getWidth(feedLabelEl)">
							Feed</div>
						<div class="tab-label" :class="{ 'invisible-tab': tabsStore.activeTab == Tab.Feed }"
							ref="feedLabelEl">Feed
						</div>

						<div class="active-tab" v-if="tabsStore.activeTab == Tab.Feed"></div>
						<div class="inactive-tab" v-else>
							<div class="tab-border"></div>
						</div>
					</div>

					<div class="tab" @click="tabsStore.activeTab = Tab.Analytics">

						<div class="tab-label active" v-if="tabsStore.activeTab == Tab.Analytics"
							:style="getWidth(analyticsLabelEl)">
							Analytics</div>
						<div class="tab-label" :class="{ 'invisible-tab': tabsStore.activeTab == Tab.Analytics }"
							ref="analyticsLabelEl">
							Analytics
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

		<template #fallback>
			<div id="TopicTabBar" class="col-xs-12" :class="{ 'is-mobile': isMobile }">

				<div class="tab" @click="tabsStore.activeTab = Tab.Topic">

					<div class="tab-label active" v-if="tabsStore.activeTab == Tab.Topic" style="width:90px"
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

				<div class="tab" @click="tabsStore.activeTab = Tab.Feed">

					<div class="tab-label active" v-if="tabsStore.activeTab == Tab.Feed" :style="getWidth(feedLabelEl)">Feed
					</div>
					<div class="tab-label" :class="{ 'invisible-tab': tabsStore.activeTab == Tab.Feed }" ref="feedLabelEl">
						Feed
					</div>

					<div class="active-tab" v-if="tabsStore.activeTab == Tab.Feed"></div>
					<div class="inactive-tab" v-else>
						<div class="tab-border"></div>
					</div>
				</div>

				<div class="tab" @click="tabsStore.activeTab = Tab.Analytics">

					<div class="tab-label active" v-if="tabsStore.activeTab == Tab.Analytics"
						:style="getWidth(analyticsLabelEl)">
						Analytics</div>
					<div class="tab-label" :class="{ 'invisible-tab': tabsStore.activeTab == Tab.Analytics }"
						ref="analyticsLabelEl">
						Analytics
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
