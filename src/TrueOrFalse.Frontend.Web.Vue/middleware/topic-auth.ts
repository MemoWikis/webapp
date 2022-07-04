
import { Topic } from "~~/components/topic/topicStore";
export default defineNuxtRouteMiddleware(async (to) => {

    const { $config } = useNuxtApp()
    const { data: result } = await useFetch<string>(`/Topic/CanAccess/${to.params.id}`, { 
            baseURL: $config.apiBase,
            headers: useRequestHeaders(['cookie'])
         }
    );

    var noAccess = result.value == 'False'
    
    if (noAccess) {
        return abortNavigation()
    }

    const { data: topic } = await useFetch<Topic>(`/Topic/GetTopic/${to.params.id}`, { 
        baseURL: $config.apiBase,
        headers: useRequestHeaders(['cookie'])
        }
      );

    useState('topic', () => topic.value)
  })