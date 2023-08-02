<script  lang="ts" setup>
const { $logger } = useNuxtApp()
const route = useRoute()

interface VerificationResponse {
    success: boolean;
    message: string;
}

const { data: verificationResult, pending, error } = useFetch<VerificationResponse>(`/apiVue/VerifyEmail/${route.params.token}`, {
    mode: 'cors',
    onResponseError(context) {
        $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
    }
})

const success = computed(() => verificationResult.value?.success && !error.value)

onMounted(async () => {
    if (!route.params.code) {
        return navigateTo('/Fehler')// Redirect the user to an error page if the code isn't present.
    }
})
</script>
  

<template>
    <div>
        <div v-if="pending">
            Verifying...
        </div>
        <div v-else>
            <div v-if="success">
                Your email was successfully verified!
            </div>
            <div v-else>
                Failed to verify email.
            </div>
        </div>
    </div>
</template>
