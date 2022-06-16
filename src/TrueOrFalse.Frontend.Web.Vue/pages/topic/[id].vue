<script lang="ts" setup>
import { ref, watch, nextTick } from 'vue'
import { useRoute } from 'vue-router'
import { Tab } from '../../components/topic/tabs/TabsEnum'
const category = ref({
  id: 1,
  text: 'foo'
})

const route = useRoute()  
const categoryId = ref(parseInt(route.params.id.toString()))
const { data } = await useFetch('http://memucho.local/Api/GetTopic', { params: { id: categoryId } } )

watch(data, (e) => {
  console.log(e);
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
    <TopicTabsContent v-if="selectedTab == Tab.Topic" :category-id="categoryId"/>
    <LazyTopicTabs Learning v-else-if="selectedTab == Tab.Learning"/>

  </div>
</template>

<style scoped></style>
