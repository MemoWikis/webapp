<script lang="ts" setup>
import { useTabsStore, Tab } from './tabsStore'
import { useTopicStore } from '../topicStore'

const tabsStore = useTabsStore()
const topicStore = useTopicStore()

const { isMobile } = useDevice()
</script>

<template>
  <perfect-scrollbar>
    <div id="TopicTabBar" class="col-xs-12" :class="{ 'is-mobile': isMobile }">

      <div class="tab" @click="tabsStore.activeTab = Tab.Topic">
        <div class="tab-label" :class="{ 'active': tabsStore.activeTab == Tab.Topic }">Thema</div>
        <div class="active-tab" v-if="tabsStore.activeTab == Tab.Topic"></div>
        <div class="inactive-tab" v-else>
          <div class="tab-border"></div>
        </div>
      </div>

      <div class="tab" @click="tabsStore.activeTab = Tab.Learning">
        <div class="tab-label learning-tab" :class="{ 'active': tabsStore.activeTab == Tab.Learning }">Fragen
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
        <div class="tab-label" :class="{ 'active': tabsStore.activeTab == Tab.Feed }">Feed</div>
        <div class="active-tab" v-if="tabsStore.activeTab == Tab.Feed"></div>
        <div class="inactive-tab" v-else>
          <div class="tab-border"></div>
        </div>
      </div>

      <div class="tab" @click="tabsStore.activeTab = Tab.Analytics">
        <div class="tab-label" :class="{ 'active': tabsStore.activeTab == Tab.Analytics }">Analytics</div>
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

</template>

<style scoped lang="less">
@import '~~/assets/tabs-bar.less';
</style>
