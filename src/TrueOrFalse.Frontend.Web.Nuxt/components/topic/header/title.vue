<script lang="ts" setup>
import { VueElement } from 'vue'
import { Topic, useTopicStore } from '../topicStore'
import { useTabsStore, Tab } from '../tabs/tabsStore'
import { Author } from '~~/components/author/author'
import { ImageFormat } from '~~/components/image/imageFormatEnum'
import { useOutlineStore } from '~/components/sidebar/outlineStore'
import { Visibility } from '~/components/shared/visibilityEnum'

const topicStore = useTopicStore()
const tabsStore = useTabsStore()
const outlineStore = useOutlineStore()

const textArea = ref()

const mobileFirstAuthor = ref<Author>()
const firstAuthors = computed(() => {
    if (isMobile) {
        mobileFirstAuthor.value = topicStore.authors[0]
        return [] as Author[]
    }
    else
        return topicStore.authors.length <= 4 ? topicStore.authors : topicStore.authors.slice(0, 4)
})

const groupedAuthors = computed(() => {
    if (isMobile)
        return topicStore.authors
    else
        return topicStore.authors.length > 4 ? topicStore.authors.slice(4, topicStore.authors.length + 1) : [] as Author[]
})

function resize() {
    outlineStore.titleIsFocused = true
    let element = textArea.value as VueElement
    if (element) {
        element.style.height = "42px"
        element.style.height = element.scrollHeight + "px"
    }
}

const readonly = ref(false)
onMounted(() => {
    if (tabsStore.activeTab != Tab.Text) {
        readonly.value = true
    }
})
watch(() => tabsStore.activeTab, (val: any) => {

    if (val == Tab.Text)
        readonly.value = false
    else {
        readonly.value = true
    }
})

const autoSaveTimer = ref()
const autoSave = () => {
    if (topicStore.visibility != Visibility.Owner)
        return

    if (autoSaveTimer.value) {
        clearTimeout(autoSaveTimer.value)
    }
    autoSaveTimer.value = setTimeout(() => {
        topicStore.saveName()
    }, 200)
}

onBeforeMount(() => {
    window.addEventListener('resize', resize)

    watch(() => topicStore.name, (newName) => {
        if (topicStore.initialName != newName) {
            if (topicStore.visibility == Visibility.Owner)
                autoSave()
            else
                topicStore.nameHasChanged = true
        }
    })
})

onMounted(async () => {
    await nextTick()
    resize()
})

onUnmounted(() => {
    window.removeEventListener('resize', resize)
})

async function scrollToChildTopics() {
    if (tabsStore.activeTab != Tab.Text) {
        tabsStore.activeTab = Tab.Text
        await nextTick()
        await nextTick()
    }
    const s = document.getElementById('TopicGrid')
    if (s)
        s.scrollIntoView({ behavior: 'smooth' })
}

const { isMobile } = useDevice()

const topic = useState<Topic>('topic')

const viewsLabel = computed(() => {
    if (topicStore.views === 1)
        return `1 Aufruf`

    let viewCount = topicStore.views.toString()

    if (topicStore.views >= 10000) {
        const formatter = new Intl.NumberFormat('de-DE')
        viewCount = formatter.format(topicStore.views)
    }
    return `${viewCount} Aufrufe`
})

const topicTitle = ref()
function focus() {
    outlineStore.titleIsFocused = true
}
function blur() {
    outlineStore.titleIsFocused = false
}

const showParents = computed(() => {
    if (topicStore.isWiki)
        return topicStore.parentTopicCount > 0
    else return topicStore.parentTopicCount > 1
})
const ariaId = useId()
const ariaId2 = useId()

</script>

<template>
    <div id="TopicHeaderContainer">
        <h1 id="TopicTitle" ref="topicTitle">
            <textarea placeholder="Gib deinem Thema einen Namen" @input="resize()" ref="textArea"
                v-model="topicStore.name" v-if="topicStore" :readonly="readonly" @focus="focus" @blur="blur"></textarea>
            <template v-else-if="topic">
                {{ topic.name }}
            </template>
        </h1>
        <div id="TopicHeaderDetails" :class="{ 'is-mobile': isMobile }">
            <div v-if="topicStore.childTopicCount > 0 && !isMobile" class="topic-detail clickable" @click="scrollToChildTopics()" v-tooltip="'Alle Unterthemen'">
                <font-awesome-icon icon="fa-solid fa-sitemap" class="topic-fa-icon" />
                <div class="topic-detail-label">{{ topicStore.childTopicCount }}</div>
            </div>

            <div class="topic-detail-spacer" v-if="showParents && topicStore.childTopicCount > 0">
            </div>

            <VDropdown :aria-id="ariaId" :distance="6">
                <button v-show="showParents" class="parent-tree-btn">

                    <div class="topic-detail">
                        <font-awesome-icon icon="fa-solid fa-sitemap" rotation="180" class="topic-fa-icon" />
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

            <div class="topic-detail-spacer" v-if="topicStore.views > 0 && (topicStore.childTopicCount > 1 && !isMobile || topicStore.parentTopicCount > 1)">
            </div>

            <div v-if="topicStore.views > 0" class="topic-detail">
                <div class="topic-detail-label">
                    {{ viewsLabel }}
                </div>
            </div>

            <div v-if="topicStore.views > 0 ||
                (topicStore.childTopicCount > 0 || topicStore.parentTopicCount > 0)" class="topic-detail-spacer">
            </div>

            <template v-for="author in firstAuthors">
                <LazyNuxtLink v-if="author.id > 0" :to="$urlHelper.getUserUrl(author.name, author.id)"
                    v-tooltip="author.name" class="header-author-icon-link">
                    <Image :src="author.imgUrl" :format="ImageFormat.Author" class="header-author-icon"
                        :alt="`${author.name}'s profile picture'`" />
                </LazyNuxtLink>
            </template>

            <VDropdown :aria-id="ariaId2" :distance="6">
                <div v-if="isMobile && groupedAuthors.length == 1 && mobileFirstAuthor && mobileFirstAuthor.id > 0" :to="$urlHelper.getUserUrl(mobileFirstAuthor.name, mobileFirstAuthor.id)" class="header-author-icon-link">
                    <Image :src="mobileFirstAuthor.imgUrl" :format="ImageFormat.Author" class="header-author-icon" :alt="`${mobileFirstAuthor.name}'s profile picture'`" />
                </div>
                <div v-else-if="groupedAuthors.length > 1" class="additional-authors-btn" :class="{ 'long': groupedAuthors.length > 9 }">
                    <span>
                        +{{ groupedAuthors.length }}
                    </span>
                </div>
                <template #popper>
                    <template v-for="author in groupedAuthors">
                        <LazyNuxtLink class="dropdown-row" v-if="author.id > 0"
                            :to="$urlHelper.getUserUrl(author.name, author.id)">
                            <div class="dropdown-icon">
                                <Image :src="author.imgUrl" :format="ImageFormat.Author" class="header-author-icon" />
                            </div>
                            <div class="dropdown-label">{{ author.name }}</div>
                        </LazyNuxtLink>
                    </template>
                </template>
            </VDropdown>


        </div>
    </div>
</template>

<style scoped lang="less">
@import (reference) '~~/assets/includes/imports.less';

.header-author-icon {
    height: 20px;
    width: 20px;
    min-height: 20px;
    min-width: 20px;
    margin: 0 4px;
    max-height: 20px;
    max-width: 20px;
}

#TopicHeaderContainer {
    padding-left: 20px;
    width: 100%;
    color: @memo-grey-dark;

    #TopicTitle {
        min-height: 49px;
        margin: 0;
        line-height: 1.1;

        textarea {
            line-height: 1.1;
            width: 100%;
            border: none;
            outline: none;
            min-height: 18px;
            resize: none;
            padding: 0;
            padding-left: 0;
            height: 42px;
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

        .header-author-icon-link {
            margin-right: 8px;
        }

        .header-author-icon,
        .header-topic-icon {
            height: 20px;
            width: 20px;
            min-height: 20px;
            min-width: 20px;
            margin: 0 2px;
        }

        .topic-detail {
            margin-right: 8px;
            display: flex;
            flex-wrap: nowrap;
            align-items: center;

            .topic-fa-icon {
                margin-right: 6px;
            }

            .topic-detail-label {
                padding-left: 0px;
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