<script lang="ts" setup>
import { SiteType } from '~/components/shared/siteEnum'
import { ErrorCode } from '~~/components/error/errorCodeEnum'
import type { NuxtError } from '#app'
'../alert/messages'

const props = defineProps({
    error: Object as () => NuxtError,
    inErrorBoundary: Boolean
})

const emit = defineEmits(['setPage', 'clearError'])
emit('setPage', SiteType.Error)

const { t } = useI18n()

onBeforeMount(() => {
    if (props.error?.statusCode)
        setErrorImage(props.error.statusCode)
    if (props.error?.message)
        description.value = props.error.message

    if (props.inErrorBoundary) {
        const router = useRouter()

        router.afterEach(() => {
            emit('clearError')
        })
    }
})

function setErrorImage(statusCode: number) {
    switch (statusCode) {
        case ErrorCode.NotFound:
            errorImgSrc.value = '/Images/Error/memo-404_german_600.png'
            break
        case ErrorCode.Unauthorized:
            errorImgSrc.value = '/Images/Error/memo-401_german_600.png'
            break
        case ErrorCode.Error:
        default:
            errorImgSrc.value = '/Images/Error/memo-500_german_600.png'
            break
    }
}

const errorImgSrc = ref<string>('/Images/Error/memo-500_german_600.png')
const description = ref<string>(t('errorContent.route.notFound'))

function handleError() {
    clearError({ redirect: '/' })
} 
</script>

<template>
    <div class="container">
        <div class="row">
            <div class="col-xs-12 container">
                <div class="error-page">
                    <Image v-if="errorImgSrc" :src="errorImgSrc" class="error-image" />
                    <button navigate class="btn back-btn" @click="handleError">
                        {{ t('errorContent.backToHome') }}
                    </button>
                    <h2 class="error-message">{{ description }}</h2>
                    <p class="email">{{ t('errorContent.emailContact', { email: 'team@memoWikis.de' }) }}</p>
                    <ul>
                        <li>{{ t('errorContent.notifications.emailSent') }}</li>
                        <li>{{ t('errorContent.notifications.urgentContact') }}</li>
                        <li>{{ t('errorContent.notifications.emailOption', { email: 'team@memoWikis.de' }) }}</li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) './assets/includes/imports.less';

.error-page {
    display: flex;
    justify-content: center;
    flex-direction: column;
    padding: 60px 20px;
    align-items: center;
    text-align: center;

    .back-btn {
        background: @memo-green;
        margin-bottom: 20px;
        padding: 10px 20px;
        border: none;
        border-radius: 4px;
        color: white;
        font-size: 16px;
        cursor: pointer;
        transition: background 0.3s ease;

        &:hover {
            background: darken(@memo-green, 10%);
        }
    }

    .error-image {
        max-width: 100%;
        height: auto;
        margin-bottom: 40px;
    }

    .error-message {
        font-size: 24px;
        font-weight: bold;
        margin-bottom: 20px;
    }

    .email {
        font-size: 16px;
        margin-bottom: 40px;
    }

    ul {
        list-style-type: none;
        padding: 0;
        font-size: 16px;

        li {
            margin-bottom: 10px;
        }
    }
}
</style>
