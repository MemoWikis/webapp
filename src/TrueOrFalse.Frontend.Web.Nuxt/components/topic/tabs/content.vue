<script setup lang="ts">
import { useTopicStore } from '../topicStore'

const topicStore = useTopicStore()
const { isMobile } = useDevice()
</script>

<template>
    <div id="TopicContent" class="row" :class="{ 'is-mobile': isMobile }">
        <ClientOnly>
            <TopicContentEditor />
            <TopicContentEditBar />
            <template #fallback>
                <div class="col-xs-12">
                    <div class="ProseMirror content-placeholder" v-html="topicStore.content" id="TopicContentPlaceholder">
                    </div>
                </div>
            </template>
        </ClientOnly>
    </div>
</template>

<style lang="less">
#TopicContentPlaceholder {
    padding: 0px;

    p {
        min-height: 30px;
    }

    ul,
    pre {
        margin-bottom: 20px;
    }
}
</style>