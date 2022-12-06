<script lang="ts" setup>
import { VueElement } from 'vue'
import { useTopicStore, Topic } from '../topicStore'
import { useTabsStore, Tab } from '../tabs/tabsStore'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import { Author } from '~~/components/author/author';

const topicStore = useTopicStore()
const tabsStore = useTabsStore()
const textArea = ref()
const topic = useState<Topic>('topic')
const firstAuthors = computed(() => topicStore.authors.length <= 4 ? topicStore.authors : topicStore.authors.slice(0, 4));
const lastAuthors = computed(() => topicStore.authors.length > 4 ? topicStore.authors.slice(4, topicStore.authors.length + 1) : [] as Author[])



function resize() {
    let element = textArea.value as VueElement
    if (element) {
        element.style.height = "54px"
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

onUnmounted(() => {
    window.removeEventListener('resize', resize);
})

</script>

<template>
    <div id="TopicHeaderContainer">
        <h1 id="TopicTitle">
            <textarea v-if="topicStore" placeholder="Gib deinem Thema einen Namen" @input="resize()" ref="textArea"
                v-model="topicStore.name" :readonly="readonly"></textarea>
            <textarea v-else ref="textArea" v-model="topic.Name"></textarea>
        </h1>
        <div id="TopicHeaderDetails">

            <div v-if="topicStore.childTopicCount > 0" class="topic-detail">
                <font-awesome-icon icon="fa-solid fa-sitemap" />
                <div class="topic-detail-label">{{ topicStore.childTopicCount }}</div>
            </div>

            <div class="topic-detail-spacer" v-if="topicStore.parentTopicCount > 0 && topicStore.childTopicCount > 0">
            </div>

            <div v-if="topicStore.parentTopicCount > 0" class="topic-detail">
                <font-awesome-icon icon="fa-solid fa-sitemap" rotation="180" />
                <div class="topic-detail-label">{{ topicStore.parentTopicCount }}</div>
            </div>

            <div class="topic-detail-spacer"
                v-if="topicStore.views > 0 && (topicStore.childTopicCount > 0 || topicStore.parentTopicCount > 0)">
            </div>

            <div v-if="topicStore.views > 0" class="topic-detail">
                <font-awesome-icon icon="fa-solid fa-eye" />
                <div class="topic-detail-label">{{ topicStore.views }}</div>
            </div>

            <div class="topic-detail-spacer"></div>

            <AuthorIcon v-for="(author) in firstAuthors" :author="author" />

            <VDropdown :distance="6">
                <button class="additional-authors-btn">+{{ lastAuthors.length }}</button>
                <template #popper>
                    <div v-for="(author) in lastAuthors" class="dropdown-row">
                        <NuxtLink :author="author" :to="'/user/' + author.Id">
                            <AuthorIcon :author="author" />{{ author.Name }}
                        </NuxtLink>
                    </div>
                </template>
            </VDropdown>
        </div>
    </div>


</template>
                           
<style scoped lang="less">
@import (reference) '~~/assets/includes/imports.less';

#TopicHeaderContainer {
    width: 100%;

    #TopicTitle {
        margin: 0;

        textarea {
            width: 100%;
            border: none;
            outline: none;
            min-height: 18px;
            resize: none;
            margin-top: -8px;
            padding: 0;
            padding-left: 12px;
            height: 54px;
            overflow: hidden;
        }
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
        font-size: 11px;
        font-weight: 600;
        border: solid 1px @memo-grey-light;
        padding: 0 2px;
        cursor: pointer;
        transition: all .1s ease-in-out;

        &:hover {
            background: @memo-blue;
            color: white;
            transition: all .1s ease-in-out;
            border: solid 1px @memo-blue;
        }
    }

    #TopicHeaderDetails {
        display: flex;
        padding-left: 12px;
        font-size: 14px;
        color: @memo-grey-dark;
        height: 20px;

        .topic-detail {
            margin-right: 8px;
            display: flex;
            flex-wrap: nowrap;
            align-items: center;

            .topic-detail-label {
                padding-left: 6px;
            }
        }

        .topic-detail-spacer {
            height: 100%;
            width: 1px;
            background: @memo-grey-light;
            margin-right: 8px;
        }
    }
}
</style>