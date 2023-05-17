<script lang="ts" setup>

import { useUserStore } from '~/components/user/userStore'

const userStore = useUserStore()
const router = useRouter()

onBeforeMount(() => {
    if (window.location.pathname != '/Fehler') {
        router.push({ path: "/Fehler" })
    }

})

function handleError() {
    clearError({ redirect: '/' })
}
watch(() => userStore.isLoggedIn, () => {
    handleError()
})

</script>

<template>
    <div class="container">
        <div class="row topic-container main-page">
            <div class="col-xs-12 container main-content">
                <div class="error-page">
                    <Image src="/Images/Error/memo-500_german_600.png" class="error-image" />

                    <div class="button-container">
                        <button class="memo-button btn-primary" @click="handleError">Zurück zur Startseite</button>
                    </div>
                    <div class="error-message">
                        <p>Oder schicke eine E-Mail an team@memucho.de.</p>
                        <ul>
                            <li>Wir wurden per E-Mail informiert.</li>
                            <li>Bei dringenden Fragen kannst du Robert unter 0178-1866848 erreichen.</li>
                            <li>Oder schicke eine E-Mail an team@memucho.de.</li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
.error-page {
    display: flex;
    justify-content: center;
    align-items: center;
    padding-top: 60px;
    padding-bottom: 60px;
    flex-direction: column;

    .error-image {
        max-width: 600px;
        margin-bottom: 60px;
    }

    .error-message {
        padding-top: 45px;
        width: 100%;
    }
}
</style>