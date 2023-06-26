<script lang="ts" setup>
import { Page } from '~/components/shared/pageEnum'
import { TopicChangeType } from '~~/components/topic/history/topicChangeTypeEnum'

interface ChangeDetail {
    topicName: string
    imageWasUpdated: boolean
    isCurrent: boolean
    changeType: TopicChangeType

    currentName?: string
    previousName?: string

    currentMarkdown?: string
    previousMarkdown?: string

    currentContent?: string
    previousContent?: string

    currentSegments?: string
    previousSegments?: string

    currentDescription?: string
    previousDescription?: string

    currentRelations?: string
    previousRelations?: string
}
const { $logger } = useNuxtApp()

const route = useRoute()
const config = useRuntimeConfig()
const headers = useRequestHeaders(['cookie']) as HeadersInit
const { data: changeDetail } = await useFetch<ChangeDetail>(`/apiVue/HistoryTopicDetail/Get?topicId=${route.params.topicId}&currentRevisionId=${route.params.currentRevisionId}&firstEditId=${route.params.firstEditId ? route.params.firstEditId : 0}`, {
    credentials: 'include',
    mode: 'cors',
    onRequest({ options }) {
        if (process.server) {
            options.headers = headers
            options.baseURL = config.public.serverBase
        }
    },
    onResponseError(context) {
        $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])

    },
})

const outputFormat = ref('side-by-side')

const emit = defineEmits(['setBreadcrumb', 'setPage'])

onMounted(() => {
    emit('setPage', Page.Default)
    if (changeDetail.value != null)
        emit('setBreadcrumb', [{ name: `Änderungen für ${changeDetail.value.topicName}`, url: route.fullPath }])

})

</script>

<template>
    <div class="container">
        <div class="row main-page">
            <div class="col-xs-12">
                <h1>Änderungen für '{{ changeDetail?.topicName }}'</h1>
            </div>
            <div class="col-xs-12"></div>

            <div class="col-xs-12" v-if="changeDetail">
                <ClientOnly>
                    <code-diff v-if="changeDetail.currentName != null" :old-string="changeDetail.previousName"
                        :new-string="changeDetail.currentName" :output-format="outputFormat" language="plaintext" />

                    <code-diff v-if="changeDetail.currentContent != null" :old-string="changeDetail.previousContent"
                        :new-string="changeDetail.currentContent" :output-format="outputFormat" language="html"
                        maxHeight="300px" />
                </ClientOnly>

            </div>

        </div>
    </div>
</template>