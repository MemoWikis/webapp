<script lang="ts" setup>
import { AnswerBodyModel, SolutionData } from '~~/components/question/answerBody/answerBodyInterfaces'
import { Page } from '~~/components/shared/pageEnum'
import { Topic } from '~~/components/topic/topicStore'

interface Props {
	documentation: Topic
}

const props = defineProps<Props>()

const route = useRoute()
const config = useRuntimeConfig()
const headers = useRequestHeaders(['cookie']) as HeadersInit

interface Question {
	answerBodyModel: AnswerBodyModel
	solutionData: SolutionData
	answerQuestionDetailsModel: any
}

const { data: question } = await useFetch<Question>(`/apiVue/QuestionLandingPage/GetQuestionPage/${route.params.id}`,
	{
		credentials: 'include',
		mode: 'no-cors',
		onRequest({ options }) {
			if (process.server) {
				options.headers = headers
				options.baseURL = config.public.serverBase
			}
		},
		onResponseError(context) {
			throw createError({ statusMessage: context.error?.message })
		},
	})

const emit = defineEmits(['setQuestionPageData', 'setPage', 'setBreadcrumb'])
onBeforeMount(() => {
	emit('setPage', Page.Question)

	if (question.value?.answerBodyModel != null)
		emit('setQuestionPageData', {
			primaryTopicName: question.value.answerBodyModel?.primaryTopicName,
			primaryTopicUrl: question.value.answerBodyModel?.primaryTopicUrl,
			title: question.value.answerBodyModel.title
		})
	if (!question.value) emit('setBreadcrumb', [{ name: 'Fehler', url: '' }])
})
const { $urlHelper } = useNuxtApp()
useHead(() => ({
	link: [
		{
			rel: 'canonical',
			href: `${config.public.serverBase}/${$urlHelper.getQuestionUrl(question.value?.answerBodyModel.title!, question.value?.answerBodyModel.id!)}`
		},
	],
	meta: [
		{
			property: 'og:title',
			content: $urlHelper.sanitizeUri(question.value?.answerBodyModel.title)
		},
		{
			property: 'og:url',
			content: `${config.public.serverBase}/${$urlHelper.getQuestionUrl(question.value?.answerBodyModel.title!, question.value?.answerBodyModel.id!)}`
		},
		{
			property: 'og:type',
			content: 'article'
		},
		{
			property: 'og:image',
			content: question.value?.answerBodyModel.imgUrl
		}
	]
}))

onBeforeMount(() => {
	if (question.value == null || question.value.answerBodyModel == null)
		throw createError({ statusCode: 404, statusMessage: 'Seite nicht gefunden' })
})
</script>

<template>
	<div class="container">
		<div class="question-page-container row main-page">
			<template v-if="question && question.answerBodyModel != null">
				<div class="col-lg-9 col-md-12 container main-content">
					<QuestionAnswerBody :is-landing-page="true" :landing-page-model="question.answerBodyModel"
						:landing-page-solution-data="question.solutionData" />
				</div>
				<Sidebar :documentation="props.documentation" />
			</template>
		</div>
	</div>
</template>

<style scoped lang="less">
.question-page-container {
	padding-bottom: 45px;
}
</style>
