<script lang="ts" setup>
definePageMeta({
    middleware:
        // ['admin-auth']

        async () => {
            const headers = useRequestHeaders(['cookie']) as HeadersInit
            const { $config } = useNuxtApp()
            console.log('hello')

            const isAdmin = await $fetch<any>('/apiVue/AdminAuth/Get',
                {
                    credentials: 'include',
                    mode: 'cors',
                    onRequest({ options }) {
                        if (process.server) {
                            options.headers = headers
                            options.baseURL = $config.public.serverBase
                        }
                    },
                })

            console.log(isAdmin)
            if (isAdmin) {
                return false
            }
        }
})
</script>


<template>
    <div>
        Adminseite
    </div>
</template>
  
