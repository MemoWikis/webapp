<script lang="ts" setup>
import { AnswerBodyModel, SolutionData } from '~~/components/question/answerBody/answerBodyInterfaces'

const route = useRoute()
const config = useRuntimeConfig()
const headers = useRequestHeaders(['cookie']) as HeadersInit

interface Question {
  answerBodyModel: AnswerBodyModel
  solutionData: SolutionData
}

const { data: question } = await useFetch<Question>(`/apiVue/VueQuestion/GetQuestionPage/${route.params.id}`,
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
</script>

<template>
  <div class="container">
    <div v-if="question" class="question-page-container row">
      <QuestionAnswerBody :is-landing-page="true" :landing-page-model="question.answerBodyModel"
        :landing-page-solution-data="question.solutionData" />
    </div>
  </div>
</template>

<style scoped lang="less">
.question-page-container {
  padding-top: 40px;
  padding-bottom: 40px;
}
</style>
