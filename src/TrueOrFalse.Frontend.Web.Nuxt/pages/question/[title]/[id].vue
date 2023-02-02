<script lang="ts" setup>
import { Question, SolutionType } from '~~/components/question/question'

const route = useRoute()
const config = useRuntimeConfig()
const headers = useRequestHeaders(['cookie']) as HeadersInit
const { data: question } = await useFetch<Question>(`/apiVue/VueQuestion/GetQuestion/${route.params.id}`,
  {
    credentials: 'include',
    mode: 'no-cors',
    onRequest({ options }) {
      if (process.server) {
        options.headers = headers
        options.baseURL = config.public.serverBase
      }
    }
  })
const markFlashCardAsCorrect = ref(false)
</script>

<template>
  <div v-if="question" class="question-page-container">
    <QuestionAnswerBodyFlashcard v-if="question?.SolutionType == SolutionType.FlashCard" :solution="question.Solution"
      :text="question.Text" :marked-as-correct="markFlashCardAsCorrect" />
  </div>
</template>

<style scoped lang="less">
.question-page-container {
  flex-grow: 2;
}
</style>
