<script lang="ts" setup>
import { AnswerBodyModel, SolutionData } from '~~/components/question/answerBody/answerBodyInterfaces'
import { Page } from '~~/components/shared/pageEnum'

const route = useRoute()
const config = useRuntimeConfig()
const headers = useRequestHeaders(['cookie']) as HeadersInit

interface Question {
  answerBodyModel: AnswerBodyModel
  solutionData: SolutionData
  answerQuestionDetailsModel: any
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
const emit = defineEmits(['setQuestionPageData', 'setPage'])
onBeforeMount(() => {
  emit('setPage', Page.Question)

  if (question.value?.answerBodyModel != null)
    emit('setQuestionPageData', {
      primaryTopicName: question.value.answerBodyModel?.primaryTopicName,
      primaryTopicUrl: question.value.answerBodyModel?.primaryTopicUrl,
      title: question.value.answerBodyModel.title
    })
})

const { isDesktop } = useDevice()
</script>

<template>
  <div class="container">
    <div class="question-page-container row mt-45 main-page">
      <div class="col-lg-9 col-md-12 container" v-if="question">
        <QuestionAnswerBody :is-landing-page="true" :landing-page-model="question.answerBodyModel"
          :landing-page-solution-data="question.solutionData" />
      </div>
      <Sidebar />
    </div>
  </div>
</template>

<style scoped lang="less">
.question-page-container {
  padding-bottom: 80px;
}
</style>
