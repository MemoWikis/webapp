<script lang="ts" setup>
import {ref, watch} from 'vue'
import {Tab} from '../../../enum/TabsEnum'

const emit = defineEmits(['selectTab'])
const selectedTab = ref(null)

watch(selectedTab, async(val: typeof Tab) => {
  emit('selectTab', val)
})

</script>

<template>
  <div>
    Component: topic/tab
    <div @click="selectedTab = Tab.Topic">Thema</div>
    <div @click="selectedTab = Tab.Learning">Lernen</div>
    <div @click="selectedTab = Tab.Analytics">Analytics</div>
  </div>
</template>

<style scoped></style>
