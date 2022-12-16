<script lang="ts" setup>
import { useTabsStore, Tab } from './tabsStore'
import { useTopicStore } from '../topicStore'

const tabsStore = useTabsStore()
const topicStore = useTopicStore()
</script>

<template>
  <div id="TopicTabBar" class="col-xs-12">

    <div class="tab" @click="tabsStore.activeTab = Tab.Topic">
      <div class="tab-label">Thema</div>
      <div class="active-tab" v-if="tabsStore.activeTab == Tab.Topic"></div>
      <div class="inactive-tab" v-else>
        <div class="tab-border"></div>
      </div>
    </div>

    <div class="tab" @click="tabsStore.activeTab = Tab.Learning">
      <div class="tab-label">Fragen <template
          v-if="topicStore.questionCount > 0">({{ topicStore.questionCount }})</template>
      </div>
      <div class="active-tab" v-if="tabsStore.activeTab == Tab.Learning"></div>
      <div class="inactive-tab" v-else>
        <div class="tab-border"></div>
      </div>
    </div>

    <div class="tab" @click="tabsStore.activeTab = Tab.Feed">
      <div class="tab-label">Feed</div>
      <div class="active-tab" v-if="tabsStore.activeTab == Tab.Feed"></div>
      <div class="inactive-tab" v-else>
        <div class="tab-border"></div>
      </div>
    </div>

    <div class="tab" @click="tabsStore.activeTab = Tab.Analytics">
      <div class="tab-label">Analytics</div>
      <div class="active-tab" v-if="tabsStore.activeTab == Tab.Analytics"></div>
      <div class="inactive-tab" v-else>
        <div class="tab-border"></div>
      </div>
    </div>

    <div class="tab-filler-container">
      <div class="tab-filler"></div>
      <div class="inactive-tab">
        <div class="tab-border"></div>
      </div>
    </div>

  </div>
</template>

<style scoped lang="less">
@import (reference) '~~/assets/includes/imports.less';

#TopicTabBar {
  text-align: center;
  flex-grow: 1;
  color: #555555;
  display: flex;
  margin-top: 30px;

  .tab,
  .tab-filler-container {
    height: 100%;
    justify-content: center;
    align-items: center;
    text-align: center;
    font-weight: 700;
    font-size: 18px;

    .tab-label,
    .tab-filler {
      padding: 4px 20px;
      height: 34px;
      white-space: nowrap;
    }

    .tab-filler {
      width: 100%;
    }

    .active-tab,
    .inactive-tab {
      height: 5px;
      width: inherit;
      z-index: 2;
      bottom: 0;
    }

    .active-tab {
      background: @memo-blue;
      border-radius: 4px;
    }

    .inactive-tab {
      display: flex;
      justify-content: center;
      align-items: center;
      width: 100%;

      .tab-border {
        height: 1px;
        background: @memo-grey-light;
        width: 100%;
      }
    }
  }

  .tab {

    .tab-label {
      border-radius: 12px;
    }

    &:hover {
      color: @memo-blue;
      cursor: pointer;
    }

    &:active {
      .tab-label {
        transition: filter 0.1s;
        background: white;
        border-radius: 24px;
        filter: brightness(0.95)
      }

    }
  }

  .tab-filler-container {
    width: 100%;
  }
}
</style>