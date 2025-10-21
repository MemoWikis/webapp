<script lang="ts" setup>
import { BreadcrumbItem } from '~~/components/header/breadcrumbItems'
import { useUserStore } from '~~/components/user/userStore'
import { Content } from '~/components/user/settings/contentEnum'
import { SiteType } from '~/components/shared/siteEnum'
import { ErrorCode } from '~/components/error/errorCodeEnum'

const route = useRoute()
const config = useRuntimeConfig()
const headers = useRequestHeaders(['cookie']) as HeadersInit
const userStore = useUserStore()
const { $logger, $urlHelper } = useNuxtApp()
const { t } = useI18n()

interface Props {
    content?: Content
}
const props = withDefaults(defineProps<Props>(), {})

interface User {
    id: number
    name: string
    wikiName: string
    wikiId?: number
    imageUrl: string
    reputationPoints: number
    rank: number
    showWishKnowledge: boolean
}
interface ProfileData {
    user: User
    isCurrentUser: boolean,
    messageKey?: string
    errorCode?: ErrorCode
}

// Redirect to login if not logged in
if (!userStore.isLoggedIn) {
    throw createError({ statusCode: 401, statusMessage: t('error.unauthorized') })
}

const { data: profile, refresh: refreshProfile } = await useFetch<ProfileData>(`/apiVue/User/Get/${userStore.id}`, {
    credentials: 'include',
    mode: 'no-cors',
    onRequest({ options }) {
        if (import.meta.server) {
            options.headers = new Headers(headers)
            options.baseURL = config.public.serverBase
        }
    },
    onResponseError(context) {
        $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
    },
})

if (profile.value && profile.value.messageKey && profile.value?.messageKey != "") {
    $logger.warn(`User: ${profile.value.messageKey} route ${route.fullPath}`)
    throw createError({ statusCode: profile.value.errorCode, statusMessage: t(profile.value.messageKey) })
}

const emit = defineEmits(['setBreadcrumb', 'setPage'])

onMounted(() => {
    emit('setPage', SiteType.User)

    const breadcrumbItem: BreadcrumbItem = {
        name: t('label.accountSettings'),
        url: t('label.accountSettings')
    }
    emit('setBreadcrumb', [breadcrumbItem])
})

watch(() => userStore.isLoggedIn, () => {
    if (!userStore.isLoggedIn) {
        navigateTo('/')
    } else {
        refreshProfile()
    }
})

useHead(() => ({
    title: t('label.accountSettings'),
}))

userStore.$onAction(({ name, after }) => {
    if (name === 'logout') {
        after(async (loggedOut) => {
            if (loggedOut) {
                navigateTo('/')
            }
        })
    }
})

</script>

<template>
    <div class="main-content" v-if="profile && profile.user.id > 0 && profile.isCurrentUser">
        <div class="settings-header">
            <h1>{{ t('label.accountSettings') }}</h1>
        </div>

        <UserSettings
            :image-url="profile.user.imageUrl"
            :content="props.content"
            @update-profile="refreshProfile" />
    </div>

    <div v-else class="main-content">
        <div class="error-message">
            <h1>{{ t('error.unauthorized') }}</h1>
            <p>{{ t('user.settings.unauthorized') }}</p>
        </div>
    </div>
</template>

<style scoped lang="less">
@import (reference) '~~/assets/includes/imports.less';

.settings-header {
    margin-bottom: 30px;

    h1 {
        margin-bottom: 10px;
    }

    .settings-description {
        color: @memo-grey-dark;
        font-size: 16px;
        margin-bottom: 0;
    }
}

.error-message {
    text-align: center;
    padding: 50px 20px;

    h1 {
        color: @memo-wuwi-red;
        margin-bottom: 20px;
    }

    p {
        color: @memo-grey-dark;
        font-size: 16px;
    }
}
</style>
