<script  lang="ts" setup>
import { Page } from '~/components/shared/pageEnum';
import { Topic } from '~/components/topic/topicStore'

interface Props {
    documentation: Topic
}
const props = defineProps<Props>()

const emit = defineEmits(['setPage'])
onBeforeMount(() => {
    emit('setPage', Page.ConfirmEmail)
})

const { $logger } = useNuxtApp()
const route = useRoute()

const { data: verificationResult, pending, error } = useFetch<boolean>(`/apiVue/ConfirmEmail/${route.params.token}`, {
    mode: 'cors',
    onResponseError(context) {
        $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
    }
})

const success = computed(() => verificationResult.value == true && !error.value)

onMounted(async () => {
    if (!route.params.token) {
        return navigateTo('/Fehler')// Redirect the user to an error page if the code isn't present.
    }
})

</script>
  

<template>
    <div class="container">
        <div class="row main-page">
            <div class="col-lg-9 col-md-12 container main-content">

                <div class="row content">
                    <div class="form-horizontal col-md-12">
                        <template v-if="pending">
                            <div class="row" style="margin-bottom: 23px; margin-top: -13px;">
                                <h1 class="col-sm-offset-2 col-sm-8 reset-title">
                                    Bestätigung läuft
                                </h1>
                            </div>
                            <div class="alert alert-info col-sm-offset-2 col-sm-8 ">
                                Hallo! Wir sind gerade dabei, deine E-Mail-Adresse zu bestätigen. Nur einen kleinen
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
                                    Hey, super! Deine E-Mail-Adresse wurde erfolgreich bestätigt. Jetzt kannst du alle
                                    unsere coolen Features nutzen. Viel Spaß!
                                </div>
                            </div>
                            <div v-else>

                                <div class="row" style="margin-bottom: 23px; margin-top: -13px;">
                                    <h1 class="col-sm-offset-2 col-sm-8 reset-title">
                                        Ups! <br />
                                        Bestätigung fehlgeschlagen
                                    </h1>
                                </div>

                                <div class="alert alert-danger col-sm-offset-2 col-sm-8 ">
                                    Oh nein, etwas hat mit der Bestätigung deiner E-Mail-Adresse nicht geklappt.
                                    <br />

                                    Es könnte sein, dass der Bestätigungslink nicht korrekt oder abgelaufen ist.
                                    <br />

                                    Keine Sorge, probier es einfach nochmal oder wende dich an <b>team@memucho.de</b>,
                                    falls das Problem weiterhin besteht.

                                    <br />
                                    Wir helfen dir gerne weiter!
                                </div>
                            </div>
                        </template>

                    </div>
                </div>
            </div>
            <Sidebar :documentation="props.documentation" />

        </div>
    </div>

    <div>

    </div>
</template>
