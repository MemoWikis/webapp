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
        return navigateTo('/Fehler')// Redirect the user to an error page if the code isn't present.
    }
})
const newVerificationMailSent = ref(false)
const msg = ref('')
async function requestVerificationMail() {
    if (!userStore.isLoggedIn) {
        userStore.openLoginModal()
        return
    }
    const result = await userStore.requestVerificationMail()
    newVerificationMailSent.value = true
    msg.value = t(result.messageKey)
}
</script>


<template>
    <div class="container">
        <div class="row main-page">
            <div class="col-lg-9 col-md-12 container main-content">

                <div class="row content">
                    <div class="form-horizontal col-md-12">
                        <template v-if="status === 'pending'">
                            <div class="row" style="margin-bottom: 23px; margin-top: -13px;">
                                <h1 class="col-sm-offset-2 col-sm-8 reset-title">
                                    Bestätigung läuft
                                </h1>
                            </div>
                            <div class="alert alert-info col-sm-offset-2 col-sm-8 ">
                                Wir sind gerade dabei, deine E-Mail-Adresse zu bestätigen. Einen
                                Augenblick Geduld bitte.
                            </div>
                        </template>
                        <template v-else>
                            <div v-if="success">

                                <div class="row" style="margin-bottom: 23px; margin-top: -13px;">
                                    <h1 class="col-sm-offset-2 col-sm-8 reset-title">
                                        Bestätigung erfolgreich
                                    </h1>
                                </div>
                                <div class="alert alert-success col-sm-offset-2 col-sm-8 ">
                                    Deine E-Mail-Adresse wurde erfolgreich bestätigt.
                                </div>
                                <div class="confirmEmail-container col-sm-offset-2 col-sm-8">
                                    <div class="confirmEmail-divider">
                                        <div class="confirmEmail-divider-line"></div>
                                    </div>
                                </div>
                                <div class="col-sm-offset-2 col-sm-8 request-verification-mail-container">
                                    <NuxtLink to="/" class="memo-button btn-primary">
                                        Weiter {{ userStore.isLoggedIn ? "zu deinem Wiki" : "zur Startseite" }}
                                    </NuxtLink>
                                </div>
                            </div>
                            <div v-else>

                                <template v-if="newVerificationMailSent">
                                    <div class="row" style="margin-bottom: 23px; margin-top: -13px;">
                                        <h1 class="col-sm-offset-2 col-sm-8 reset-title">
                                            Neue Verifizierungs-E-Mail angefordert!
                                        </h1>
                                    </div>

                                    <div class="alert alert-success col-sm-offset-2 col-sm-8 ">
                                        {{ msg }}
                                    </div>
                                </template>
                                <template v-else>
                                    <div class="row" style="margin-bottom: 23px; margin-top: -13px;">
                                        <h1 class="col-sm-offset-2 col-sm-8 reset-title">
                                            Bestätigung fehlgeschlagen
                                        </h1>
                                    </div>

                                    <div class="alert alert-danger col-sm-offset-2 col-sm-8 ">
                                        Es tut uns leid, die Bestätigung deiner E-Mail-Adresse ist fehlgeschlagen.
                                        <br />

                                        Es könnte sein, dass der Bestätigungslink nicht korrekt oder abgelaufen ist.
                                        <br />

                                        Versuche es bitte noch einmal oder wende dich an <b>team@memoWikis.de</b>,
                                        falls das Problem weiterhin besteht.

                                        <br />
                                        Wir helfen dir gerne weiter!
                                    </div>

                                    <div class="confirmEmail-container col-sm-offset-2 col-sm-8">
                                        <div class="confirmEmail-divider">
                                            <div class="confirmEmail-divider-line"></div>
                                        </div>
                                    </div>
                                    <div class="col-sm-offset-2 col-sm-8 request-verification-mail-container">
                                        <button class="memo-button btn-primary" @click="requestVerificationMail()">
                                            neue Verifizierungs-Email senden
                                        </button>
                                    </div>
                                </template>

                            </div>
                        </template>

                    </div>
                </div>
            </div>
            <Sidebar :site="props.site" />

        </div>
    </div>

    <div>

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
