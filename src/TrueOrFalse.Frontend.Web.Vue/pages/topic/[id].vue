<script lang="ts" setup>
import { ref, watch, nextTick, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { Tab } from '~~/components/topic/tabs/TabsEnum'
import { useTabsStore } from '~~/components/topic/tabs/tabsStore';

const category = ref({
  id: 1,
  text: 'foo'
})

const route = useRoute()  
const categoryId = ref(parseInt(route.params.id.toString()))
const { data } = await useFetch('/api/GetTopic', { params: { id: route.params.id }, server: false,  } )
onMounted(() => {
  console.log(data)
})
watch(data, (e) => {
  console.log(e);
})

const tabsStore = useTabsStore()

</script>

<template>
  <div>
    Topic {{ $route.params.id }}
    <br/>
    <TopicTabs/>
    <TopicTabsContent v-show="tabsStore.activeTab == Tab.Topic" :category-id="categoryId"/>
    <LazyTopicTabsLearning v-show="tabsStore.activeTab == Tab.Learning"/>

  </div>
</template>

<style scoped></style>
