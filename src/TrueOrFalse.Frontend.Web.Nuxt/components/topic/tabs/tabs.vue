<script lang="ts" setup>
import { useTabsStore, Tab } from './tabsStore'
import { useTopicStore } from '../topicStore'

const tabsStore = useTabsStore()
const topicStore = useTopicStore()

const { isMobile } = useDevice()
</script>

<template>
  <div id="TopicTabBar" class="col-xs-12" :class="{ 'is-mobile': isMobile }">

    <div class="tab-scroll">
      <div class="tab" @click="tabsStore.activeTab = Tab.Topic">
        <div class="tab-label">Thema</div>
        <div class="active-tab" v-if="tabsStore.activeTab == Tab.Topic"></div>
        <div class="inactive-tab" v-else>
          <div class="tab-border"></div>
        </div>
      </div>

      <div class="tab" @click="tabsStore.activeTab = Tab.Learning">
        <div class="tab-label">Fragen <template v-if="topicStore.questionCount > 0">({{
          topicStore.questionCount
        }})</template>
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
        <div class="tab-filler" :class="{ 'mobile': isMobile }"></div>
        <div class="inactive-tab">
          <div class="tab-border"></div>
        </div>
      </div>
    </div>

  </div>
</template>

<style scoped lang="less">
@import (reference) '~~/assets/includes/imports.less';

#TopicTabBar {
  text-align: center;
  flex-grow: 1;
  color: @memo-grey-dark;
  display: flex;
  margin-top: 30px;

  ::-webkit-scrollbar {
    height: 4px;
  }

  ::-webkit-scrollbar-thumb {
    background-color: @memo-grey-dark;
    cursor: pointer;
    border-radius: 4px;

    &:hover {
      background-color: @memo-grey-darker;

    }
  }

  .tab-scroll {
    overflow-y: auto;
    max-width: 100vw;

    flex-grow: 1;
    display: flex;
  }

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

      &.mobile {
        padding: 0;
      }
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

  &.is-mobile {
    font-size: 16px;

    .tab-label,
    .tab-filler {
      padding: 4px 12px;
    }
  }

  .tab {

    .tab-label {
      border-radius: 12px;

      &.chip {
        border-radius: 24px;
        display: flex;
        justify-content: center;
        align-items: center;
        padding: 0 8px;
        background: @memo-grey-light;
        font-size: 14px;
      }
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
