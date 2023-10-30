<script lang="ts" setup>
import { BreadcrumbItem } from '~~/components/header/breadcrumbItems';
import { Page } from '~~/components/shared/pageEnum'
import { Message } from '~~/components/user/messages/message'

const emit = defineEmits(['setPage', 'setBreadcrumb'])
onBeforeMount(() => {
    emit('setPage', Page.Messages)

    const breadcrumbItems: BreadcrumbItem[] = [
        {
            name: 'Nachrichten',
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
        if (process.server) {
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
                        <span class="ColoredUnderline Message">Nachrichten</span>
                    </h1>
                    <div id="messagesWrapper">
                        <UserMessagesRow v-if="model.messages != null" v-for="message in model.messages" :message="message"
                            :force-show="forceShow" :key="message.id" />
                        <div class="alert alert-info" v-if="model.messages?.filter((m: any) => !m.read).length == 0">
                            Du hast aktuell keine ungelesenen Nachrichten.
                        </div>

                        <p v-if="model.readCount > 0">
                            Du hast {{ model.readCount }} gelesene Nachricht{{
                                model.readCount == 0 ||
                                model.readCount > 1 ? 'en' : ''
                            }}.
                            <div v-if="!forceShow" @click="forceShow = true" class="click">Alle anzeigen</div>.
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