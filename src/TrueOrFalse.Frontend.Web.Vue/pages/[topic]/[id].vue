<script lang="ts" setup>
import { ref, watch, nextTick, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { Tab } from '~~/components/topic/tabs/TabsEnum'
import { useTabsStore } from '~~/components/topic/tabs/tabsStore';
import { Topic, useTopicStore } from '~~/components/topic/topicStore';
import { useSpinnerStore } from '~~/components/spinner/spinnerStore';
import { useUserStore } from '~~/components/user/userStore';

const spinnerStore = useSpinnerStore()

const route = useRoute()
const categoryId = ref(parseInt(route.params.id.toString()))

const topicStore = useTopicStore()
const { data: topic } = await useFetch<Topic>(`/api/Topic/GetTopic/${categoryId}`, {
  headers: useRequestHeaders(['cookie'])
}
);
console.log(topic)
useState('topic', () => topic.value)
topicStore.setTopic(topic.value)

const tabsStore = useTabsStore()
const userStore = useUserStore()

const { data: result } = await useFetch<string>(`/api/Login/GetLoginState/`, {
  credentials: 'include',
  headers: useRequestHeaders(['cookie']),
  mode: 'no-cors'
}
);

var val = result.value == 'True'
useState<boolean>('isLoggedIn', () => val)
</script>

<template>
  <div>
    Topic {{ $route.params.id }}
    <br />
    <TopicHeader />
    <br />
    <button @click="spinnerStore.showSpinner()">showSpinner</button>
    <br />

    <button @click="spinnerStore.hideSpinner()">hideSpinner</button>
    <br />
    <!-- <button @click="getTopicData()">loadTopic</button>
            <br/> -->
    <LazyTopicTabsContent v-show="tabsStore.activeTab == Tab.Topic" :category-id="categoryId" />
    <LazyTopicTabsLearning v-show="tabsStore.activeTab == Tab.Learning" />
    <button @click="userStore.logout()">logout</button>

  </div>
</template>

<style scoped>
</style>
