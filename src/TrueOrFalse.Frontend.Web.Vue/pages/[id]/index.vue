<script lang="ts" setup>
import {ref, reactive} from 'vue'
import {Tab} from '../../enum/TabsEnum'
const category = ref({
  id: 1,
  text: 'foo'
})
const selectedTab = ref(Tab.Topic)

function selectTab(e) {
 selectedTab.value = e;
 nextTick(() => console.log(e));
}

</script>

<template>
  <div>
    Topic {{ $route.params.id }}
    <br/>
    <TopicTabs @select-tab="selectTab"/>
    <TopicContent v-if="selectedTab == Tab.Topic" :category="category"/>
    <LazyTopicLearning v-else-if="selectedTab == Tab.Learning"/>

  </div>
</template>

<style scoped></style>
