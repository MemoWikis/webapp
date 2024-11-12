import { useUserStore } from "~/components/user/userStore"

export default defineNuxtRouteMiddleware((to, from) => {
    if (Object.keys(to.params).length == 0) {

        const userStore = useUserStore()
        if (userStore.isLoggedIn)
            return `/apiVue/Page/GetPage/${userStore.personalWiki?.id}`
        else return `/apiVue/Page/GetPage/1`
    }
    return
})