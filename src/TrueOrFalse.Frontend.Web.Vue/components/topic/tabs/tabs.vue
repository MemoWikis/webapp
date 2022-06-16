<script lang="ts" setup>
import {Tab} from './TabsEnum'
import {useTabsStore} from './tabsStore'
const tabsStore = useTabsStore()
</script>

<template>
  <div>
    Component: topic/tab
    <div @click="tabsStore.activeTab = Tab.Topic">Thema</div>
    <div @click="tabsStore.activeTab = Tab.Learning">Lernen</div>
    <div @click="tabsStore.activeTab = Tab.Analytics">Analytics</div>
  </div>
</template>

<style scoped></style>
