<script lang="ts" setup>
const headers = useRequestHeaders(['cookie']) as HeadersInit
const config = useRuntimeConfig()

interface NameAndLink {
    link: string;
    name: string;
}
const topicArray = ref<NameAndLink[]>()

const { data: result } = await useFetch<FetchResult<NameAndLink[]>>('/apiVue/Cancel/GetHelperTopics', {
    method: 'GET',
    credentials: 'include',
    onRequest({ options }) {
        if (process.server) {
            options.headers = headers
            options.baseURL = config.public.serverBase
        }
    },
});
if (result.value?.success == true) {
    topicArray.value = result.value.data
}

</script>

<template>
    <div class="container">
        <div class="row topic-container main-page">
            <h1>Abbruch</h1>
            <p>Es tut uns leid zu sehen, dass Sie Ihre Zahlung abgebrochen haben.
                Wenn Sie auf Probleme gestoßen sind oder Fragen haben,
                zögern Sie bitte nicht, uns zu kontaktieren.
                Unser freundliches Support-Team steht Ihnen gerne zur Verfügung.</p>
            <div class="helper-links">
                <a href="https://discord.com/invite/nXKwGrN" target="_blank"><i class="fab fa-discord"
                        aria-hidden="true"></i>Discord</a>

                <nuxt-link v-for="(nameAndLink) in  topicArray " :to="nameAndLink.link" target="_blank"
                    v-if="result?.success == true">{{
                        nameAndLink.name
                    }}</nuxt-link><br />
            </div>
        </div>
    </div>
</template>
<style lang="less">
.helper-links {
    display: flex;
    flex-direction: column;
    font-size: 16px;
    font-weight: 600;
}

p {
    font-size: 16px;
    font-weight: 600;
}
</style>
