import { Topic } from "~~/components/topic/topicStore";

export default defineNuxtRouteMiddleware(async (to) => {

    const { $config } = useNuxtApp()
    const { data: result } = await useFetch<string>(`/Login/GetLoginState/`, { 
            baseURL: $config.apiBase,
            credentials: 'include',
            headers: useRequestHeaders(['cookie']),
            mode: 'no-cors'
         }
    );

    var val = result.value == 'True'
    useState<boolean>('isLoggedIn', () => val)
  })