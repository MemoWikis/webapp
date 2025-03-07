<script lang="ts" setup>
import { AnswerBodyModel, SolutionData } from '~~/components/question/answerBody/answerBodyInterfaces'
import { Site } from '~/components/shared/siteEnum'
import { SolutionType } from '~~/components/question/solutionTypeEnum'
import { useUserStore } from '~/components/user/userStore'
import { handleNewLine, getHighlightedCode } from '~/components/shared/utils'
import { AnswerQuestionDetailsResult } from '~/components/question/answerBody/answerQuestionDetailsResult'
import { ErrorCode } from '~/components/error/errorCodeEnum'


const { $logger } = useNuxtApp()
const userStore = useUserStore()

interface Props {
	site: Site
}

const props = defineProps<Props>()

const { t } = useI18n()

const route = useRoute()
const config = useRuntimeConfig()
const headers = useRequestHeaders(['cookie']) as HeadersInit

interface Question {
	answerBodyModel?: AnswerBodyModel
	solutionData?: SolutionData
	answerQuestionDetailsModel?: AnswerQuestionDetailsResult,
	messageKey?: string
	errorCode?: ErrorCode
}

const { data: question } = await useFetch<Question>(`/apiVue/QuestionLandingPage/GetQuestionPage/${route.params.id}`,
	{
		credentials: 'include',
		mode: 'cors',
		onRequest({ options }) {
			if (import.meta.server) {
				options.headers = headers
				options.baseURL = config.public.serverBase
			}
		},
		onResponseError(context) {
			throw createError({ statusMessage: context.error?.message })
		},
	})

if (question.value && question.value.messageKey && question.value.messageKey.length > 0 && question.value.errorCode != null) {
	$logger.warn(`Question: ${question.value.messageKey} route ${route.fullPath}`)

	throw createError({ statusCode: question.value.errorCode, statusMessage: t(question.value.messageKey) })
}

function highlightCode(id: string) {

	const el = document.getElementById(id)
	if (el != null)
		el.querySelectorAll('code').forEach(block => {
			if (block.textContent != null)
				block.innerHTML = getHighlightedCode(block.textContent)
		})
}
const emit = defineEmits(['setQuestionPageData', 'setPage', 'setBreadcrumb'])
onBeforeMount(() => {
	emit('setPage', Site.Question)

	if (question.value?.answerBodyModel != null)
		emit('setQuestionPageData', {
			primaryPageName: question.value.answerBodyModel?.primaryPageName,
			primaryPageUrl: $urlHelper.getPageUrlWithQuestionId(question.value.answerBodyModel.primaryPageName, question.value.answerBodyModel.primaryPageId, question.value.answerBodyModel.id),
			title: question.value.answerBodyModel.title,
			isPrivate: question.value.answerBodyModel.isPrivate
		})
	if (!question.value)
		emit('setBreadcrumb', [{ name: 'Fehler', url: '' }])

	highlightCode('AnswerBody')
	highlightCode('SolutionContent')

	if (question.value?.answerBodyModel != null && question.value?.solutionData != null) {
		if (question.value.answerBodyModel.solutionType != SolutionType.FlashCard && question.value.answerBodyModel.renderedQuestionTextExtended.length > 0)
			highlightCode('ExtendedQuestionContainer')
		if (question.value.solutionData.answerDescription?.trim().length > 0)
			highlightCode('ExtendedSolutionContent')
	}

})
const { $urlHelper } = useNuxtApp()
useHead(() => ({
	link: [
		{
			rel: 'canonical',
			href: `${config.public.officialBase}${$urlHelper.getQuestionUrl(question.value?.answerBodyModel?.title!, question.value?.answerBodyModel?.id!)}`
		},
	],
	meta: [
		{
			name: 'description',
			content: question.value?.answerBodyModel?.description
		},
		{
			property: 'og:title',
			content: $urlHelper.sanitizeUri(question.value?.answerBodyModel?.title)
		},
		{
			property: 'og:url',
			content: `${config.public.officialBase}${$urlHelper.getQuestionUrl(question.value?.answerBodyModel?.title!, question.value?.answerBodyModel?.id!)}`
		},
		{
			property: 'og:type',
			content: 'article'
		},
		{
			property: 'og:image',
			content: question.value?.answerBodyModel?.imgUrl
		}
	]
}))
</script>

<template>
	<title v-if="question && question?.answerBodyModel != null">
		Frageseite zu '{{ question.answerBodyModel.title }}'
	</title>
	<div class="container page-container">
		<div class="question-page-container row main-page">
			<template v-if="question && question?.answerBodyModel != null">
				<div class="col-lg-9 col-md-12 container main-content">

					<div id="AnswerBody" class="col-xs-12 landing-page">
						<div class="answerbody-header">

							<div class="answerbody-text">
								<h3 v-if="question.answerBodyModel.solutionType != SolutionType.FlashCard"
									class="QuestionText">
									{{ question.answerBodyModel.text }}
								</h3>
							</div>
						</div>

						<div class="row">

							<div id="MarkdownCol"
								v-if="question.answerBodyModel.solutionType != SolutionType.FlashCard && question.answerBodyModel.renderedQuestionTextExtended.length > 0">

								<div id="ExtendedQuestionContainer" class="RenderedMarkdown"
									v-html="handleNewLine(question.answerBodyModel.renderedQuestionTextExtended)">
								</div>
							</div>


							<div id="AnswerAndSolutionCol">
								<div id="AnswerAndSolution">
									<div class="row"
										:class="{ 'hasFlashCard': question.answerBodyModel.solutionType === SolutionType.FlashCard }">
										<div id="AnswerInputSection">

											<QuestionAnswerBodyFlashcard
												:key="question.answerBodyModel.id + 'flashcard'"
												v-if="question.answerBodyModel.solutionType === SolutionType.FlashCard"
												ref="flashcard" :solution="question.answerBodyModel.solution"
												:front-content="question.answerBodyModel.textHtml"
												:marked-as-correct="true" />
											<QuestionAnswerBodyMatchlist
												:key="question.answerBodyModel.id + 'matchlist'"
												v-else-if="question.answerBodyModel.solutionType === SolutionType.MatchList"
												ref="matchList" :solution="question.answerBodyModel.solution"
												:show-answer="true" />
											<QuestionAnswerBodyMultipleChoice
												:key="question.answerBodyModel.id + 'multiplechoice'"
												v-else-if="question.answerBodyModel.solutionType === SolutionType.MultipleChoice"
												:solution="question.answerBodyModel.solution" :show-answer="true"
												ref="multipleChoice" />
											<QuestionAnswerBodyText :key="question.answerBodyModel.id + 'text'"
												v-else-if="question.answerBodyModel.solutionType === SolutionType.Text"
												ref="text" :show-answer="true" />

										</div>
										<div id="ButtonsAndSolutionCol">
											<div id="ButtonsAndSolution" class="Clearfix">
												<div id="Buttons">
													<div id="btnGoToTestSession">

														<NuxtLink v-if="question.answerBodyModel.hasPages"
															:to="$urlHelper.getPageUrlWithQuestionId(question.answerBodyModel.primaryPageName, question.answerBodyModel.primaryPageId, question.answerBodyModel.id)"
															id="btnStartTestSession"
															class="btn btn-primary show-tooltip" rel="nofollow"
															v-tooltip="userStore.isLoggedIn ? 'Lerne alle Fragen auf dieser Seite' : 'Lerne 5 zufällig ausgewählte Fragen von ' + question.answerBodyModel.primaryPageName">
															<b>Weiterlernen</b>
														</NuxtLink>
													</div>
												</div>

												<div id="AnswerFeedbackAndSolutionDetails">
													<div v-if="question.answerBodyModel.solutionType != SolutionType.FlashCard"
														id="AnswerFeedback">

														<div id="Solution">
															<div class="solution-label">
																Richtige Antwort:
															</div>

															<div class="Content body-m" id="SolutionContent"
																v-html="handleNewLine(question.solutionData?.answerAsHTML)">
															</div>

														</div>
													</div>

													<div id="SolutionDetails"
														v-if="question.solutionData != null && question.solutionData.answerDescription.trim().length > 0">
														<div id="Description">
															<div class="solution-label">
																Ergänzungen zur Antwort:
															</div>

															<div class="Content body-m" id="ExtendedSolutionContent"
																v-html="handleNewLine(question.solutionData?.answerDescription)">
															</div>
														</div>
													</div>
												</div>
											</div>
										</div>
									</div>
								</div>
							</div>
						</div>

						<QuestionAnswerBodyAnswerQuestionDetailsLandingPage
							v-if="question.answerQuestionDetailsModel != null"
							:model="question.answerQuestionDetailsModel" />

					</div>
				</div>
				<Sidebar :site="props.site" />
			</template>
		</div>
	</div>
</template>

<style scoped lang="less">
#AnswerBody {
	&.landing-page {
		margin-top: 60px;
	}
}

.question-page-container {
	padding-bottom: 45px;
	margin-top: 20px;
}

#AnswerAndSolutionCol {
	margin-bottom: 45px;
}
</style>

<style lang="less">
@import '~~/assets/views/answerQuestion.less';

#ButtonsAndSolution {
	.btn-primary {
		margin-right: 22px;
	}
}

.ButtonGroup {
	display: flex;
	justify-content: flex-start;
	flex-wrap: wrap;
}

#AnswerBody {
	transition: all 1s ease-in-out;
}

.answerbody-header {
	display: flex;
	flex-wrap: nowrap;
	justify-content: space-between;

	.answerbody-text {
		margin-right: 8px;

		h3 {
			margin-top: 0;
			margin-bottom: 34px;
		}
	}

	.answerbody-btn {
		font-size: 18px;

		.answerbody-btn-inner {
			cursor: pointer;
			background: white;
			height: 32px;
			width: 32px;
			display: flex;
			justify-content: center;
			align-items: center;
			border-radius: 15px;

			.fa-ellipsis-vertical {
				color: @memo-grey-dark;
			}

			&:hover {
				filter: brightness(0.95)
			}

			&:active {
				filter: brightness(0.85)
			}
		}
	}
}

.activity-points-icon {
	font-size: 14px;
}

#ActivityPointsDisplay {
	.activitypoints-display-detail {
		display: flex;
		justify-content: flex-end;
		align-items: center;

		#ActivityPoints {
			margin-right: 8px;
		}
	}
}

#ulAnswerHistory {
	padding-top: 5px;
}

.solution-label {
	font-weight: bold;
	padding-right: 5px;
}

.correct-answer-label {
	color: @memo-green-correct;
}

.wrong-answer-label {
	color: @memo-red-wrong;
}
</style>
