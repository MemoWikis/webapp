<script lang="ts" setup>
import { SiteType } from '~/components/shared/siteEnum'
import { useUserStore } from '~/components/user/userStore'

const userStore = useUserStore()
interface Props {
    site: SiteType
}
const props = defineProps<Props>()
const { t } = useI18n()

const emit = defineEmits(['setPage'])
onBeforeMount(() => {
    emit('setPage', SiteType.ConfirmEmail)
})

const { $logger } = useNuxtApp()
const route = useRoute()

const { data: verificationResult, status, error } = useFetch<boolean>(`/apiVue/ConfirmEmail/Run`, {
    body: {
        token: route.params.token
    },
    method: 'POST',
    mode: 'cors',
    onResponseError(context) {
        $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
    }
})
const success = computed(() => verificationResult.value === true && !error.value)

watch(success, async (val) => {
    if (val)
        await refreshNuxtData()
})

onMounted(async () => {
    if (!route.params.token) {
        throw createError({ statusCode: 404, statusMessage: 'Not Found' })
    }
})

const newVerificationMailSent = ref(false)
const messageKey = ref('')
async function requestVerificationMail() {
    if (!userStore.isLoggedIn) {
        userStore.openLoginModal()
        return
    }
    const result = await userStore.requestVerificationMail()
    newVerificationMailSent.value = true
    messageKey.value = result.messageKey
}

const config = useRuntimeConfig()

const contact = () => {
    window.location.href = `mailto:${config.public.teamEmail}`
}
</script>


<template>
    <div class="main-content">

        <div class="row content">
            <div class="form-horizontal col-md-12">
                <template v-if="status === 'pending'">
                    <div class="row" style="margin-bottom: 23px; margin-top: -13px;">
                        <h1 class="col-sm-offset-2 col-sm-8 reset-title">
                            {{ t('user.confirmEmail.pending.title') }}
                        </h1>
                    </div>
                    <div class="alert alert-info col-sm-offset-2 col-sm-8 ">
                        {{ t('user.confirmEmail.pending.message') }}
                    </div>
                </template>
                <template v-else>
                    <div v-if="success">
                        <div class="row" style="margin-bottom: 23px; margin-top: -13px;">
                            <h1 class="col-sm-offset-2 col-sm-8 reset-title">
                                {{ t('user.confirmEmail.success.title') }}
                            </h1>
                        </div>
                        <div class="alert alert-success col-sm-offset-2 col-sm-8 ">
                            {{ t('user.confirmEmail.success.message') }}
                        </div>
                        <div class="confirmEmail-container col-sm-offset-2 col-sm-8">
                            <div class="confirmEmail-divider">
                                <div class="confirmEmail-divider-line"></div>
                            </div>
                        </div>
                        <div class="col-sm-offset-2 col-sm-8 request-verification-mail-container">
                            <NuxtLink to="/" class="memo-button btn-primary">
                                {{ t('user.confirmEmail.success.continue', { destination: userStore.isLoggedIn ? t('user.confirmEmail.success.yourWiki') : t('user.confirmEmail.success.homepage') }) }}
                            </NuxtLink>
                        </div>
                    </div>
                    <div v-else>

                        <template v-if="newVerificationMailSent">
                            <div class="row" style="margin-bottom: 23px; margin-top: -13px;">
                                <h1 class="col-sm-offset-2 col-sm-8 reset-title">
                                    {{ t('user.confirmEmail.newMail.title') }}
                                </h1>
                            </div>

                            <div class="alert alert-success col-sm-offset-2 col-sm-8 ">
                                {{ t(messageKey) }}
                            </div>
                        </template>
                        <template v-else>
                            <div class="row" style="margin-bottom: 23px; margin-top: -13px;">
                                <h1 class="col-sm-offset-2 col-sm-8 reset-title">
                                    {{ t('user.confirmEmail.failed.title') }}
                                </h1>
                            </div>
                            <div class="alert alert-danger col-sm-offset-2 col-sm-8">
                                {{ t('user.confirmEmail.failed.message1') }}
                                <br />

                                {{ t('user.confirmEmail.failed.message2') }}
                                <br />

                                {{ t('user.confirmEmail.failed.message3') }} <NuxtLink to="#" @click="contact">{{ config.public.teamEmail }}</NuxtLink>,
                                {{ t('user.confirmEmail.failed.message4') }}

                                <br />
                                {{ t('user.confirmEmail.failed.message5') }}
                            </div>

                            <div class="confirmEmail-container col-sm-offset-2 col-sm-8">
                                <div class="confirmEmail-divider">
                                    <div class="confirmEmail-divider-line"></div>
                                </div>
                            </div>
                            <div class="col-sm-offset-2 col-sm-8 request-verification-mail-container">
                                <button class="memo-button btn-primary" @click="requestVerificationMail()">
                                    {{ t('user.confirmEmail.failed.resendButton') }}
                                </button>
                            </div>
                        </template>

                    </div>
                </template>

            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.confirmEmail-container {
    margin-top: 24px;

    .confirmEmail-divider-label-container,
    .confirmEmail-divider {
        position: absolute;
        display: flex;
        justify-content: center;
        align-items: center;
        width: 100%;
        padding: 0 10px;
        margin: 0 -10px;
        height: 100%;

        .confirmEmail-divider-label {
            background: white;
            height: 53px;
            width: 53px;
            border-radius: 27px;
            display: flex;
            align-items: center;
            justify-content: center;
        }

        .confirmEmail-divider-line {
            height: 1px;
            background: @memo-grey-light;
            width: 100%;
        }
    }
}

.request-verification-mail-container {
    display: flex;
    justify-content: center;
    align-items: center;
    margin-top: 48px;
}
</style>
