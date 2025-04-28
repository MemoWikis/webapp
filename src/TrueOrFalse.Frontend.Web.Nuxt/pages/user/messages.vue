<script lang="ts" setup>
import { BreadcrumbItem } from '~~/components/header/breadcrumbItems'
import { SiteType } from '~/components/shared/siteEnum'
import { Message } from '~~/components/user/messages/message'

const emit = defineEmits(['setPage', 'setBreadcrumb'])
const { t } = useI18n()

onBeforeMount(() => {
    emit('setPage', SiteType.Messages)

    const breadcrumbItems: BreadcrumbItem[] = [
        {
            name: t('breadcrumb.titles.messages'),
            url: '/Nachrichten'
        }]
    emit('setBreadcrumb', breadcrumbItems)
})
const config = useRuntimeConfig()
const headers = useRequestHeaders(['cookie']) as HeadersInit

interface MessageResult {
    notLoggedIn?: boolean
    messages?: Message[]
    readCount?: number
}
const { $logger } = useNuxtApp()

const { data: model } = await useFetch<MessageResult>(`/apiVue/UserMessages/Get/`, {
    credentials: 'include',
    mode: 'no-cors',
    onRequest({ options }) {
        if (import.meta.server) {
            options.headers = headers
            options.baseURL = config.public.serverBase
        }
    },
    onResponseError(context) {
        const { $logger } = useNuxtApp()
        $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])

    },
})

const forceShow = ref(false)

</script>

<template>
    <div class="container">
        <div class="row messages-container main-page">
            <div class="main-content row">
                <div class="col-md-9" v-if="model != null && model.messages != null && model.readCount != null">
                    <h1>
                        <span class="ColoredUnderline Message">{{ t('breadcrumb.titles.messages') }}</span>
                    </h1>
                    <div id="messagesWrapper">
                        <UserMessagesRow v-if="model.messages != null" v-for="message in model.messages"
                            :message="message" :force-show="forceShow" :key="message.id" />
                        <div class="alert alert-info" v-if="model.messages?.filter((m: any) => !m.read).length === 0">
                            {{ t('messages.noUnread') }}
                        </div>

                        <p v-if="model.readCount > 0">
                            {{ t('messages.readCount', { count: model.readCount }) }}
                            <span v-if="!forceShow" @click="forceShow = true" class="click">{{ t('messages.showAll') }}</span>.
                        </p>

                    </div>

                </div>
                <div class="col-md-3"></div>
            </div>

        </div>
    </div>
</template>

<style lang="less" scoped>
@import '~~/assets/views/answerQuestion.less';

.click {
    cursor: pointer;

    &:hover {
        color: @memo-blue-link;
    }
}

h1 {
    margin-top: 0;
    margin-bottom: 20px;
}

.Message {
    padding-right: 3px;
}

.container {
    min-height: 400px;
}

.messages-container {
    padding-bottom: 45px;
}
</style>