import { defineStore } from 'pinia'
import { useUserStore } from '../user/userStore'
import { Visibility } from '../shared/visibilityEnum'
import { Author } from '../author/author'
import { TopicItem } from '../search/searchHelper'
import { GridTopicItem } from './content/grid/item/gridTopicItem'
import { AlertType, messages, useAlertStore } from '../alert/alertStore'
import { useSnackbarStore, SnackbarData } from '../snackBar/snackBarStore'

export class Topic {
	canAccess: boolean = false
	id: number = 0
	name: string = ''
	imageUrl: string = ''
	imageId: number = 0
	content: string = ''
	parentTopicCount: number = 0
	parents: TinyTopicModel[] = []
	childTopicCount: number = 0
	directVisibleChildTopicCount: number = 0
	views: number = 0
	commentCount: number = 0
	visibility: Visibility = Visibility.Owner
	authorIds: number[] = []
	isWiki: boolean = false
	currentUserIsCreator: boolean = false
	canBeDeleted: boolean = false
	questionCount: number = 0
	directQuestionCount: number = 0
	authors: Author[] = []
	topicItem: TopicItem | null = null
	metaDescription: string = ''
	knowledgeSummary: KnowledgeSummary = {
		solid: 0,
		needsConsolidation: 0,
		needsLearning: 0,
		notLearned: 0,
	}
	gridItems: GridTopicItem[] = []
	isChildOfPersonalWiki: boolean = false
	textIsHidden: boolean = false
}

export interface KnowledgeSummary {
	solid: number
	needsConsolidation: number
	needsLearning: number
	notLearned: number
}

export interface FooterTopics {
	rootWiki: Topic
	mainTopics: Topic[]
	memoWiki: Topic
	memoTopics: Topic[]
	helpTopics: Topic[]
	popularTopics: Topic[]
	documentation: Topic
}

export interface TinyTopicModel {
	id: number
	name: string
	imgUrl: string
}

export const useTopicStore = defineStore('topicStore', {
	state: () => {
		return {
			id: 0,
			name: '',
			initialName: '',
			imgUrl: '',
			imgId: 0,
			questionCount: 0,
			directQuestionCount: 0,
			content: '',
			initialContent: '',
			contentHasChanged: false,
			parentTopicCount: 0,
			parents: [] as TinyTopicModel[],
			childTopicCount: 0,
			directVisibleChildTopicCount: 0,
			views: 0,
			commentCount: 0,
			visibility: null as Visibility | null,
			authorIds: [] as number[],
			isWiki: false,
			currentUserIsCreator: false,
			canBeDeleted: false,
			authors: [] as Author[],
			searchTopicItem: null as null | TopicItem,
			knowledgeSummary: {} as KnowledgeSummary,
			gridItems: [] as GridTopicItem[],
			isChildOfPersonalWiki: false,
			textIsHidden: false,
		}
	},
	actions: {
		setTopic(topic: Topic) {
			if (topic != null) {
				this.id = topic.id
				this.name = topic.name
				this.initialName = topic.name
				this.imgUrl = topic.imageUrl
				this.imgId = topic.imageId
				this.content = topic.content
				this.initialContent = topic.content

				this.parentTopicCount = topic.parentTopicCount
				this.parents = topic.parents
				this.childTopicCount = topic.childTopicCount
				this.directVisibleChildTopicCount = topic.directVisibleChildTopicCount

				this.views = topic.views
				this.commentCount = topic.commentCount
				this.visibility = topic.visibility

				this.authorIds = topic.authorIds
				this.isWiki = topic.isWiki
				this.currentUserIsCreator = topic.currentUserIsCreator
				this.canBeDeleted = topic.canBeDeleted

				this.questionCount = topic.questionCount
				this.directQuestionCount = topic.directQuestionCount

				this.authors = topic.authors
				this.searchTopicItem = topic.topicItem
				this.knowledgeSummary = topic.knowledgeSummary
				this.gridItems = topic.gridItems
				this.isChildOfPersonalWiki = topic.isChildOfPersonalWiki
				this.textIsHidden = topic.textIsHidden
			}
		},
		async saveTopic() {
			const userStore = useUserStore()
			const snackbarStore = useSnackbarStore()

			if (!userStore.isLoggedIn) {
				userStore.openLoginModal()
				return
			}

			const json = {
				id: this.id,
				name: this.name,
				saveName: this.name != this.initialName,
				content: this.content,
				saveContent: this.content != this.initialContent
			}
			const result = await $fetch<FetchResult<boolean>>('/apiVue/TopicStore/SaveTopic', {
				method: 'POST', body: json, mode: 'cors', credentials: 'include',
				onResponseError(context) {
					const { $logger } = useNuxtApp()
					$logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
				}
			})
			if (result.success == true) {
				const data: SnackbarData = {
                    type: 'success',
                    text: messages.success.category.saved
                }
                snackbarStore.showSnackbar(data)
				this.contentHasChanged = false
			}
			else if (result.success == false) {
				const alertStore = useAlertStore()
				alertStore.openAlert(AlertType.Error, { text: messages.getByCompositeKey(result.messageKey) })
			}
		},
		resetContent() {
			this.name = this.initialName
			this.content = this.initialContent
			this.contentHasChanged = false
		},
		isOwnerOrAdmin() {
			const userStore = useUserStore()
			return userStore.isAdmin || this.currentUserIsCreator
		},

		async refreshTopicImage() {
			this.imgUrl = await $fetch<string>(`/apiVue/TopicStore/GetTopicImageUrl/${this.id}`, {
				method: 'GET', mode: 'cors', credentials: 'include',
				onResponseError(context) {
					const { $logger } = useNuxtApp()
					$logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, req: context.request }])
				}
			})
		},
		async reloadKnowledgeSummary() {
			this.knowledgeSummary = await $fetch<KnowledgeSummary>(`/apiVue/TopicStore/GetUpdatedKnowledgeSummary/${this.id}`, {
				method: 'GET', mode: 'cors', credentials: 'include',
				onResponseError(context) {
					const { $logger } = useNuxtApp()
					$logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, req: context.request }])
				}
			})
		},
		async reloadGridItems() {
			const result = await $fetch<GridTopicItem[]>(`/apiVue/TopicStore/GetGridTopicItems/${this.id}`, {
				method: 'GET', 
				mode: 'cors', 
				credentials: 'include',
				onResponseError(context) {
					const { $logger } = useNuxtApp()
					$logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
				}
			})

			if (result)
				this.gridItems = result
		},
		async hideOrShowText() {
			if ((!!this.content && this.content.length > 0) || this.contentHasChanged)
				return

			const data = {
				hideText: !this.textIsHidden,
				topicId: this.id
			}
			const result = await $fetch<boolean>(`/apiVue/TopicStore/HideOrShowText/`, {
				body: data,
				method: 'POST', 
				mode: 'cors', 
				credentials: 'include',
				onResponseError(context) {
					const { $logger } = useNuxtApp()
					$logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, req: context.request }])
				}
			})

			this.textIsHidden = result
		}

	},
	getters: {
		getTopicName(): string {
			return this.name
		},
	},
})