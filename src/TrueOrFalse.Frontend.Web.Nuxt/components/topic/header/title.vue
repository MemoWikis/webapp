<script lang="ts" setup>
import { VueElement } from 'vue'
import { Topic, useTopicStore } from '../topicStore'
import { useTabsStore, Tab } from '../tabs/tabsStore'
import { Author } from '~~/components/author/author'
import { ImageFormat } from '~~/components/image/imageFormatEnum'

const topicStore = useTopicStore()
const tabsStore = useTabsStore()
const textArea = ref()
const firstAuthors = computed(() => topicStore.authors.length <= 4 ? topicStore.authors : topicStore.authors.slice(0, 4))
const lastAuthors = computed(() => topicStore.authors.length > 4 ? topicStore.authors.slice(4, topicStore.authors.length + 1) : [] as Author[])

function resize() {
    let element = textArea.value as VueElement
    if (element) {
        element.style.height = "56px"
        element.style.height = element.scrollHeight + "px"
    }
}

const readonly = ref(false)
watch(() => tabsStore.activeTab, (val: any) => {

    if (val == Tab.Topic)
        readonly.value = false
    else {
        readonly.value = true
    }
})

onBeforeMount(() => {
    window.addEventListener('resize', resize);

    watch(() => topicStore.name, (newName) => {
        if (topicStore.initialName != newName) {
            topicStore.contentHasChanged = true
        }
    })

})

onMounted(async () => {
    await nextTick()
    resize()
})

onUnmounted(() => {
    window.removeEventListener('resize', resize);
})

async function scrollToChildTopics() {
    if (tabsStore.activeTab != Tab.Topic) {
        tabsStore.activeTab = Tab.Topic
        await nextTick()
        await nextTick()
    }
    const s = document.getElementById('TopicGrid')
    if (s)
        s.scrollIntoView({ behavior: 'smooth' })
}

const { isDesktopOrTablet, isMobile } = useDevice()

const topic = useState<Topic>('topic')
</script>

<template>
    <div id="TopicHeaderContainer">
        <h1 id="TopicTitle">
            <textarea placeholder="Gib deinem Thema einen Namen" @input="resize()" ref="textArea" v-model="topicStore.name"
                v-if="topicStore" :readonly="readonly"></textarea>
            <template v-else-if="topic">
                {{ topic.Name }}
            </template>
        </h1>
        <div id="TopicHeaderDetails" :class="{ 'is-mobile': isMobile }">
            <div v-if="topicStore.childTopicCount > 0" class="topic-detail clickable" @click="scrollToChildTopics()"
                v-tooltip="'Alle Unterthemen'">
                <font-awesome-icon icon="fa-solid fa-sitemap" />
                <div class="topic-detail-label">{{ topicStore.childTopicCount }}</div>
            </div>

            <div class="topic-detail-spacer" v-if="topicStore.parentTopicCount > 0 && topicStore.childTopicCount > 0">
            </div>

            <!-- <div v-if="topicStore.parentTopicCount > 0" class="topic-detail ">
                <font-awesome-icon icon="fa-solid fa-sitemap" rotation="180" />
                <div class="topic-detail-label">{{ topicStore.parentTopicCount }}</div>
            </div> -->
            <VDropdown :distance="6">
                <button v-show="topicStore.parentTopicCount > 0" class="parent-tree-btn">

                    <div class="topic-detail">
                        <font-awesome-icon icon="fa-solid fa-sitemap" rotation="180" />
                        <div class="topic-detail-label">{{ topicStore.parentTopicCount }}</div>
                    </div>

                </button>
                <template #popper>
                    <template v-for="parent in topicStore.parents">
                        <LazyNuxtLink class="dropdown-row" v-if="parent.id > 0"
                            :to="$urlHelper.getTopicUrl(parent.name, parent.id)">
                            <div class="dropdown-icon">
                                <Image :src="parent.imgUrl" :format="ImageFormat.Topic" class="header-topic-icon" />
                            </div>
                            <div class="dropdown-label">{{ parent.name }}</div>
                        </LazyNuxtLink>
                    </template>


                </template>
            </VDropdown>

            <div class="topic-detail-spacer"
                v-if="topicStore.views > 0 && (topicStore.childTopicCount > 0 || topicStore.parentTopicCount > 0)">
            </div>

            <div v-if="topicStore.views > 0" class="topic-detail">
                <font-awesome-icon icon="fa-solid fa-eye" />
                <div class="topic-detail-label">{{ topicStore.views }}</div>
            </div>
            <div v-if="isMobile" class="topic-detail-flex-breaker"></div>
            <div v-if="isDesktopOrTablet" class="topic-detail-spacer"></div>

            <template v-for="author in firstAuthors">
                <LazyNuxtLink v-if="author.Id > 0" :to="$urlHelper.getUserUrl(author.Name, author.Id)"
                    v-tooltip="author.Name" class="header-author-icon-link">
                    <Image :src="author.ImgUrl" :format="ImageFormat.Author" class="header-author-icon"
                        :alt="`${author.Name}'s profile picture'`" />
                </LazyNuxtLink>
            </template>

            <VDropdown :distance="6">
                <button v-show="(lastAuthors.length > 1)" class="additional-authors-btn"
                    :class="{ 'long': lastAuthors.length > 9 }">
                    <span>
                        +{{ lastAuthors.length }}
                    </span>
                </button>
                <template #popper>
                    <template v-for="author in lastAuthors">
                        <LazyNuxtLink class="dropdown-row" v-if="author.Id > 0"
                            :to="$urlHelper.getUserUrl(author.Name, author.Id)">
                            <div class="dropdown-icon">
                                <Image :src="author.ImgUrl" :format="ImageFormat.Author" class="header-author-icon" />
                            </div>
                            <div class="dropdown-label">{{ author.Name }}</div>
                        </LazyNuxtLink>
                    </template>
                </template>
            </VDropdown>


        </div>
    </div>
</template>
                           
<style scoped lang="less">
@import (reference) '~~/assets/includes/imports.less';

#TopicHeaderContainer {
    padding-left: 20px;
    width: 100%;
    color: @memo-grey-dark;

    #TopicTitle {
        min-height: 60px;
        margin: 0;

        textarea {
            width: 100%;
            border: none;
            outline: none;
            min-height: 18px;
            resize: none;
            margin-top: -8px;
            padding: 0;
            padding-left: 0;
            height: 54px;
            overflow: hidden;
        }
    }

    .parent-tree-btn {
        background: none;
    }

    .additional-authors-btn {
        border-radius: 10px;
        min-width: 20px;
        height: 20px;
        line-height: 20px;
        text-align: center;
        justify-content: center;
        align-items: center;
        display: inline-flex;
        font-size: 10px;
        font-weight: 700;
        border: solid 1px @memo-grey-light;
        padding: 0 2px;
        background: none;
        cursor: pointer;
        transition: all .1s ease-in-out;

        &.long {
            padding: 0 4px;
        }

        &:hover {
            background: @memo-blue;
            color: white;
            transition: all .1s ease-in-out;
            border: solid 1px @memo-blue;
        }

        span {
            position: relative;
            left: -1px;
        }
    }

    #TopicHeaderDetails {
        display: flex;
        flex-wrap: wrap;
        align-items: center;
        min-height: 21px;

        &.is-mobile {
            .topic-detail {
                margin-bottom: 8px;
            }
        }


        .header-author-icon-link {
            margin-right: 8px;
        }

        .header-author-icon,
        .header-topic-icon {
            height: 20px;
            width: 20px;
            min-height: 20px;
            min-width: 20px;
            margin-left: 0px;
        }

        .topic-detail {
            margin-right: 8px;
            display: flex;
            flex-wrap: nowrap;
            align-items: center;

            .topic-detail-label {
                padding-left: 6px;
            }

            &.clickable {
                cursor: pointer;

                &:hover {
                    color: @memo-blue-link;
                }

                transition: color ease-in-out 0.2s;
            }
        }

        .topic-detail-spacer {
            height: 100%;
            width: 1px;
            background: @memo-grey-light;
            margin-right: 8px;
            min-height: 12px;

        }

        .topic-detail-flex-breaker {
            flex-basis: 100%;
        }
    }
}
</style>