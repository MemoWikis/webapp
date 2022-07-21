import { LoginState } from "~~/components/user/userStore";

export default defineNuxtRouteMiddleware(async (to) => {

    const { $config } = useNuxtApp()
    const { data: result } = await useFetch<LoginState>(`/SessionUser/GetCurrentUser/`, { 
            baseURL: $config.apiBase,
            credentials: 'include',
            headers: useRequestHeaders(['cookie']),
            mode: 'no-cors'
         }
    );

    useState<LoginState>('loginState', () => result.value)
  })