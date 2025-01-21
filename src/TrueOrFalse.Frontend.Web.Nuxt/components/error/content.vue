<script lang="ts" setup>
import { Site } from '~/components/shared/siteEnum'
import { ErrorCode } from '~~/components/error/errorCodeEnum'
import type { NuxtError } from '#app'
import { messages } from '../alert/messages'

const props = defineProps({
    error: Object as () => NuxtError,
    inErrorBoundary: Boolean
})

const emit = defineEmits(['setPage', 'clearError'])
emit('setPage', Site.Error)


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
const description = ref<string>(messages.error.route.notFound)

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
                        Zurück zur Startseite
                    </button>
                    <h2 class="error-message">{{ description }}</h2>
                    <p class="email">Oder schicke eine E-Mail an team@memowikis.net.</p>
                    <ul>
                        <li>Wir wurden per E-Mail informiert.</li>
                        <li>Bei dringenden Fragen kannst du Robert unter 0178-1866848 erreichen.</li>
                        <li>Oder schicke eine E-Mail an team@memowikis.net.</li>
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
