<script lang="ts" setup>
import { ImageFormat } from '~/components/image/imageFormatEnum'
import { Page } from '~/components/shared/pageEnum'
import { useUserStore } from '~/components/user/userStore'
import { TopicChangeType } from '~~/components/topic/history/topicChangeTypeEnum'

const userStore = useUserStore()
interface ChangeDetail {
    topicName: string
    imageWasUpdated: boolean
    isCurrent: boolean
    changeType: TopicChangeType
    currentChangeDate: string
    previousChangeDate: string

    authorName: string
    authorId: number
    authorImgUrl: string

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
const { $logger, $urlHelper } = useNuxtApp()

const route = useRoute()
const config = useRuntimeConfig()
const headers = useRequestHeaders(['cookie']) as HeadersInit
const { data: changeDetail } = await useFetch<ChangeDetail>(route.params.firstEditId ? `/apiVue/HistoryTopicDetail/Get?topicId=${route.params.topicId}&currentRevisionId=${route.params.currentRevisionId}&firstEditId=${route.params.firstEditId}` : `/apiVue/HistoryTopicDetail/Get?topicId=${route.params.topicId}&currentRevisionId=${route.params.currentRevisionId}`, {
    credentials: 'include',
    mode: 'cors',
    onRequest({ options }) {
        if (import.meta.server) {
            options.headers = headers
            options.baseURL = config.public.serverBase
        }
    },
    onResponseError(context) {
        $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
    },
})

const outputFormat = ref('line-by-line')

const emit = defineEmits(['setBreadcrumb', 'setPage'])

onMounted(() => {
    emit('setPage', Page.Default)
    if (changeDetail.value != null)
        emit('setBreadcrumb', [{ name: `Bearbeitungshistorie für ${changeDetail.value.topicName}`, url: `/Historie/Thema/${route.params.topicId}` }, { name: `Änderungen vom ${changeDetail.value.currentChangeDate}`, url: route.fullPath }])

})

async function restore() {
    if (!userStore.isLoggedIn) {
        userStore.openLoginModal()
        return
    }
    await $api(`/apiVue/HistoryTopicDetail/RestoreTopic?topicChangeId=${route.params.currentRevisionId}`, {
        method: 'GET',
        credentials: 'include',
        mode: 'cors',
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
        },
    })
}


</script>

<template>
    <div class="container">
        <div class="row main-page">
            <div class="col-xs-12">
                <h1>Änderungen für '{{ changeDetail?.topicName }}'</h1>
            </div>

            <template v-if="changeDetail">
                <div class="col-xs-12">
                    <div class="header">
                        <div>
                            <NuxtLink :to="$urlHelper.getUserUrl(changeDetail.authorName, changeDetail.authorId)"
                                class="author">
                                <Image :src="changeDetail.authorImgUrl" :format="ImageFormat.Author"
                                    class="author-img" />
                                {{ changeDetail.authorName }}
                            </NuxtLink>
                            <div>
                                vom {{ changeDetail.currentChangeDate }}
                            </div>
                        </div>
                        <div>
                            <!-- <button class="memo-button btn btn-default" @click="restore"
                                v-if="changeDetail.changeType == TopicChangeType.Renamed || changeDetail.changeType == TopicChangeType.Text">
                                Wiederherstellen
                            </button> -->
                        </div>
                    </div>
                </div>

                <div class="col-xs-12">
                    <ClientOnly>
                        <code-diff v-if="changeDetail.currentName != null" :old-string="changeDetail.previousName"
                            :new-string="changeDetail.currentName" :output-format="outputFormat" language="plaintext" />

                        <code-diff v-if="changeDetail.currentContent != null" :old-string="changeDetail.previousContent"
                            :new-string="changeDetail.currentContent" :output-format="outputFormat" language="html" />
                    </ClientOnly>

                    <div class="alert alert-info"
                        v-if="changeDetail.currentContent == changeDetail.previousContent && changeDetail.currentName == changeDetail.previousContent">
                        Zwischen den beiden Revisionen (vom {{ changeDetail.previousChangeDate }} und
                        vom {{ changeDetail.currentChangeDate }}) gibt es <b>keine inhaltlichen Unterschiede</b>.
                    </div>
                </div>
            </template>


        </div>
    </div>
</template>

<style lang="less" scoped>
.header {
    display: flex;
    justify-content: space-between;
    align-items: center;

    .author {
        display: flex;

        .author-img {
            height: 20px;
            width: 20px;
            margin-right: 8px;
        }
    }
}

.alert {
    margin-top: 36px;
}
</style>