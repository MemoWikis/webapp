<script lang="ts" setup>
import { VueElement } from 'vue'
import { useTopicStore } from '../topic/topicStore'
import _ from 'underscore'
import { Page } from '../shared/pageEnum'

const props = defineProps(['headerContainer', 'headerExtras', 'page'])

const topicStore = useTopicStore()
interface BreadcrumbItem {
  Name: string
  Id: number
}
class Breadcrumb {
  newWikiId: number = 0
  personalWiki: BreadcrumbItem | null = null
  items: BreadcrumbItem[] = []
  rootTopic: BreadcrumbItem | null = null
  currentTopic: BreadcrumbItem | null = null
  breadcrumbHasGlobalWiki: boolean = false
  isInPersonalWiki: boolean = false
}
const breadcrumb = ref(null as Breadcrumb | null)

const breadcrumbItems = ref([] as BreadcrumbItem[])
const stackedBreadcrumbItems = ref([] as BreadcrumbItem[])

const breadcrumbEl = ref(null as VueElement | null)
const breadcrumbWidth = ref('')

function startUpdateBreadcrumb() {
  updateBreadcrumb()
}

const personalWiki = ref(null as BreadcrumbItem | null)

const updateBreadcrumb = _.throttle(async () => {
  if (breadcrumbEl.value != null && breadcrumbEl.value.clientHeight != null) {
    const width = props.headerContainer.clientWidth - props.headerExtras.clientWidth - 30

    if (width > 0)
      breadcrumbWidth.value = `width: ${width}px`

    await nextTick()

    if (breadcrumbEl.value.clientHeight > 21) {
      shiftToStackedBreadcrumbItems()
    } else if (breadcrumbEl.value.clientHeight < 22) {
      insertToBreadcrumbItems()
      setTimeout(() => {
        if (breadcrumbEl.value && breadcrumbEl.value.clientHeight > 21) {
          shiftToStackedBreadcrumbItems()
        }
      }, 200)
    }
  }
}, 10)

function shiftToStackedBreadcrumbItems() {
  if (breadcrumbItems.value.length > 0)
    stackedBreadcrumbItems.value.push(breadcrumbItems.value.shift()!)

}
function insertToBreadcrumbItems() {
  if (stackedBreadcrumbItems.value.length > 0)
    breadcrumbItems.value.unshift(stackedBreadcrumbItems.value.pop()!)
}
const pageTitle = ref('')


onMounted(async () => {
  if (typeof window !== 'undefined') {
    window.addEventListener('resize', startUpdateBreadcrumb)
    window.addEventListener('scroll', startUpdateBreadcrumb)
  }
  await nextTick()
  startUpdateBreadcrumb()
  getBreadcrumb()
})
onBeforeUnmount(() => {
  if (typeof window !== 'undefined') {
    window.removeEventListener('resize', startUpdateBreadcrumb)
    window.removeEventListener('scroll', startUpdateBreadcrumb)
  }
})

const route = useRoute()
watch(() => route.params, () => {
  if (props.page != Page.Topic)
    getBreadcrumb()
})
watch(() => topicStore.id, (newId, oldId) => {
  if (newId != oldId && props.page == Page.Topic)
    getBreadcrumb()
})
watch(() => props.page, () => {
  getBreadcrumb()
})
async function getBreadcrumb() {
  await nextTick()

  var sessionStorage = window.sessionStorage

  if (topicStore.isWiki)
    sessionStorage.setItem('currentWikiId', topicStore.id.toString())
  var sessionWikiId = parseInt(sessionStorage.getItem('currentWikiId')!)

  var currentWikiId = 0;
  if (!isNaN(sessionWikiId))
    currentWikiId = sessionWikiId

  const data = {
    wikiId: currentWikiId,
    currentCategoryId: topicStore.id,
  }

  if (props.page == Page.Topic) {
    const result = await $fetch<Breadcrumb>(`/apiVue/Breadcrumb/GetBreadcrumb/`,
      {
        method: 'POST',
        body: data,
        credentials: 'include',
      })

    breadcrumb.value = result
    personalWiki.value = result.personalWiki
    breadcrumbItems.value = result.items
    sessionStorage.setItem('currentWikiId', result.newWikiId.toString())
    updateBreadcrumb()
  } else {
    const result = await $fetch<BreadcrumbItem>(`/apiVue/Breadcrumb/GetPersonalWiki/`,
      {
        method: 'POST',
        body: data,
        credentials: 'include',
        mode: 'no-cors',
      })

    personalWiki.value = result
    setPageTitle()

  }
}

function setPageTitle() {
  switch (props.page) {
    case Page.Topic:
      pageTitle.value = topicStore.name
      break
    case Page.Register:
      pageTitle.value = 'Registrieren'
      break
    case Page.Welcome:
      pageTitle.value = 'Willkommen'
      break
  }
}

</script>

<template>
  <div v-if="breadcrumb != null && props.page == Page.Topic" id="BreadCrumb" ref="breadcrumbEl"
    :style="breadcrumbWidth">

    <NuxtLink :to="`/${encodeURI(breadcrumb.personalWiki.Name.replaceAll(' ', '-'))}/${breadcrumb.personalWiki.Id}`"
      class="breadcrumb-item" v-tooltip="breadcrumb.personalWiki.Name" v-if="breadcrumb.personalWiki">
      <font-awesome-icon icon="fa-solid fa-house" />
    </NuxtLink>

    <template v-if="breadcrumb.rootTopic">
      <div
        v-if="breadcrumb.currentTopic && breadcrumb.rootTopic.Id != breadcrumb.currentTopic.Id && breadcrumb.isInPersonalWiki">
        <font-awesome-icon icon="fa-solid fa-chevron-right" />
      </div>

      <template
        v-else-if="breadcrumb.personalWiki && breadcrumb.rootTopic.Id != breadcrumb.personalWiki.Id && !breadcrumb.isInPersonalWiki">
        <div class="breadcrumb-divider"></div>
        <template v-if="topicStore.id != breadcrumb.rootTopic.Id">
          <NuxtLink :to="`/${encodeURI(breadcrumb.rootTopic.Name.replaceAll(' ', '-'))}/${breadcrumb.rootTopic.Id}`"
            class="breadcrumb-item" v-tooltip="breadcrumb.rootTopic.Name">
            {{ breadcrumb.rootTopic.Name }}
          </NuxtLink>
          <font-awesome-icon icon="fa-solid fa-chevron-right" />
        </template>
      </template>
    </template>


    <V-Dropdown v-if="stackedBreadcrumbItems.length > 0" :distance="0">
      <font-awesome-icon icon="fa-solid fa-ellipsis" class="breadcrumb-item" />
      <font-awesome-icon icon="fa-solid fa-chevron-right" />
      <template #popper>
        <ul>
          <li v-for="s in stackedBreadcrumbItems">
            <NuxtLink :to="`/${encodeURI(s.Name.replaceAll(' ', '-'))}/${s.Id}`" v-tooltip="s.Name">
              {{ s.Name }}
            </NuxtLink>
          </li>
        </ul>
      </template>
    </V-Dropdown>

    <template v-for="b in breadcrumbItems">
      <NuxtLink :to="`/${encodeURI(b.Name.replaceAll(' ', '-'))}/${b.Id}`" class="breadcrumb-item" v-tooltip="b.Name">
        {{ b.Name }}
      </NuxtLink>
      <font-awesome-icon icon="fa-solid fa-chevron-right" />
    </template>

    <div class="breadcrumb-item last" v-tooltip="topicStore.name">
      {{ topicStore.name }}
    </div>

  </div>
  <div v-else-if="personalWiki != null" id="BreadCrumb" :style="breadcrumbWidth">
    <NuxtLink :to="`/${encodeURI(personalWiki.Name.replaceAll(' ', '-'))}/${personalWiki.Id}`" class="breadcrumb-item"
      v-tooltip="personalWiki.Name">
      <font-awesome-icon icon="fa-solid fa-house" />
    </NuxtLink>
    <div class="breadcrumb-divider"></div>
    <div class="breadcrumb-item last" v-tooltip="topicStore.name" v-if="props.page == Page.Topic">
      {{ topicStore.name }}
    </div>
    <div class="breadcrumb-item last" v-tooltip="pageTitle" v-else>
      {{ pageTitle }}
    </div>
  </div>


</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

#BreadCrumb {
  display: flex;
  justify-content: flex-start;
  align-items: center;
  font-size: 14px;
  color: @memo-grey-dark;
  flex-wrap: wrap;
  opacity: 1;
  transition: opacity 0.5s;


  .breadcrumb-item {
    padding: 0 12px;
    max-width: 100px;
    text-overflow: ellipsis;
    overflow: hidden;
    cursor: pointer;

    color: @memo-grey-dark;
    text-decoration: none;
    font-family: "Open Sans";
    font-size: 14px;
    font-weight: 600;
    padding-left: 10px;
    transition: all 0.1s ease-in-out;

    &.last {
      max-width: 300px;
      color: @memo-grey-darker;
    }

    &:hover {
      color: @memo-blue;
    }

  }

  .breadcrumb-divider {
    min-height: 30px;
    height: 60%;
    background: #ddd;
    max-width: 1px;
    border-radius: 4px;
    min-width: 1px;
  }
}
</style>