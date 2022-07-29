<script lang="ts" setup>
import { Tab } from './TabsEnum'
import { useTabsStore } from './tabsStore'
const tabsStore = useTabsStore()
</script>

<template>
  <div class="col-xs-12 row">
    <div class="col-sm-4" @click="tabsStore.activeTab = Tab.Topic">Thema</div>
    <div class="col-sm-4" @click="tabsStore.activeTab = Tab.Learning">Lernen</div>
    <div class="col-sm-4" @click="tabsStore.activeTab = Tab.Analytics">Analytics</div>
  </div>
</template>

<style scoped>
</style>
