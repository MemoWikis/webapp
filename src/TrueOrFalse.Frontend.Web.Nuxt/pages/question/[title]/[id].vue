<script lang="ts" setup>
import { Question, SolutionType } from '~~/components/question/question'

const route = useRoute()
const config = useRuntimeConfig()
const headers = useRequestHeaders(['cookie']) as HeadersInit
const { data: question } = await useFetch<Question>(`/apiVue/VueQuestion/GetQuestion/${route.params.id}`,
  {
    baseURL: process.server ? config.public.serverBase : config.public.clientBase,
    credentials: 'include',
    mode: 'no-cors',
    onResponse({ options }) {
      if (process.server)
        options.headers = headers
    }
  })


</script>

<template>
  <div>
    <QuestionAnswerBodyFlashcard v-if="question?.SolutionType == SolutionType.FlashCard" :question="question" />
  </div>
</template>

<style scoped>

</style>
