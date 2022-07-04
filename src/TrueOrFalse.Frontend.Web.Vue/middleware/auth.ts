
export default defineNuxtRouteMiddleware(async (to) => {

    const { $config } = useNuxtApp()
    const { data: result } = await useFetch<string>(`/Topic/CanAccess/${to.params.id}`, { 
            baseURL: $config.apiBase,
            credentials: 'include',
            headers: useRequestHeaders(['cookie'])
         }
    );

    var noAccess = result.value == 'False'
    
    if (noAccess) {
        return abortNavigation()
    }
  })