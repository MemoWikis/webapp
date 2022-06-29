<script lang="ts" setup>
import { ref, watch, nextTick, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { Tab } from '~~/components/topic/tabs/TabsEnum'
import { useTabsStore } from '~~/components/topic/tabs/tabsStore';
import { useTopicStore } from '~~/components/topic/topicStore';

const route = useRoute()  
const categoryId = ref(parseInt(route.params.id.toString()))

const { data: topic } = await useFetch(() => `/api/Topic/GetTopic/${route.params.id}`);

const topicStore = useTopicStore()
topicStore.setTopic(topic.value)

const tabsStore = useTabsStore()

</script>

<template>
  <div>
    Topic {{ $route.params.id }}
    {{topic}}
    <br/>
    <TopicHeader />
    <br/>
    <TopicTabsContent v-show="tabsStore.activeTab == Tab.Topic" :category-id="categoryId"/>
    <LazyTopicTabsLearning v-show="tabsStore.activeTab == Tab.Learning"/>

  </div>
</template>

<style scoped></style>
