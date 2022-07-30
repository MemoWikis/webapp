import { Topic } from "~~/components/topic/topicStore";

export default defineNuxtRouteMiddleware(async (to) => {
    const { $config } = useNuxtApp()
    const { data: topic } = await useFetch<Topic>(`/Topic/GetTopic/${to.params.id}`, {
        baseURL: $config.apiBase,
        headers: useRequestHeaders(['cookie'])
      });
    useState<Topic>('topic', () => topic.value)
  })