import { useUserStore } from "~/components/user/userStore"

export default defineNuxtRouteMiddleware((to, from) => {
    if (Object.keys(to.params).length == 0) {

        const userStore = useUserStore()
        if (userStore.isLoggedIn)
            return `/apiVue/Topic/GetTopic/${userStore.personalWiki?.id}`
        else return `/apiVue/Topic/GetTopic/1`
    }

    return
})