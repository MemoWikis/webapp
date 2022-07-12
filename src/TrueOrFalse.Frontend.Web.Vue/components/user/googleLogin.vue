<script setup lang="ts">
import { Google } from './Google'

const gapiLoaded = ref(false)
onMounted(async () => {
    const script = document.createElement("script");
    script.type = "text/javascript";
    script.src = "https://apis.google.com/js/platform.js";
    document.body.appendChild(script);
})
onUpdated(async () => {
    if (!gapiLoaded.value) {

        new Google()
        await nextTick()
        await nextTick()
        Google.AttachClickHandler('GoogleLogin')
        gapiLoaded.value = true
    }

})
</script>

<template>
</template>