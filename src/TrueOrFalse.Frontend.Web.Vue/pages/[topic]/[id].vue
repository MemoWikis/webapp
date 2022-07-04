<script lang="ts" setup>
import { ref, watch, nextTick, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { Tab } from '~~/components/topic/tabs/TabsEnum'
import { useTabsStore } from '~~/components/topic/tabs/tabsStore';
import { useTopicStore } from '~~/components/topic/topicStore';
import { useSpinnerStore } from '~~/components/spinner/spinnerStore';
import { useUserStore } from '~~/components/user/userStore'

const userStore = useUserStore()
userStore.getCurrentState()

const spinnerStore = useSpinnerStore()

const route = useRoute()  
const categoryId = ref(parseInt(route.params.id.toString()))
const config = useRuntimeConfig()
const { data: topic } = await useFetch(`/Topic/GetTopic/${route.params.id}`, 
{ baseURL: config.apiBase }
);

const topicStore = useTopicStore()
topicStore.setTopic(topic.value)

const tabsStore = useTabsStore()

</script>

<template>
  <div>
    Topic {{ $route.params.id }}
    <br/>
    <TopicHeader />
    <br/>
            <button @click="spinnerStore.showSpinner()">showSpinner</button>
                <br/>

        <button @click="spinnerStore.hideSpinner()">hideSpinner</button>
            <br/>
        <!-- <button @click="getTopicData()">loadTopic</button>
            <br/> -->
    <TopicTabsContent v-show="tabsStore.activeTab == Tab.Topic" :category-id="categoryId"/>
    <LazyTopicTabsLearning v-show="tabsStore.activeTab == Tab.Learning"/>

  </div>
</template>

<style scoped></style>
