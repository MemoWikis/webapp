import { defineStore } from 'pinia'
import { useUserStore } from '../user/userStore'
import { Visibility } from '../shared/visibilityEnum'
import { Author } from '../author/author'
import { TopicItem } from '../search/searchHelper'
import { GridTopicItem } from './content/grid/item/gridTopicItem'
import { AlertType, messages, useAlertStore } from '../alert/alertStore'

export class Topic {
	CanAccess: boolean = false
	Id: number = 0
	Name: string = ''
	ImageUrl: string = ''
	ImageId: number = 0
	Content: string = ''
	ParentTopicCount: number = 0
	Parents: TinyTopicModel[] = []
	ChildTopicCount: number = 0
	DirectVisibleChildTopicCount: number = 0
	Views: number = 0
	CommentCount: number = 0
	Visibility: Visibility = Visibility.Owner
	AuthorIds: number[] = []
	IsWiki: boolean = false
	CurrentUserIsCreator: boolean = false
	CanBeDeleted: boolean = false
	QuestionCount: number = 0
	DirectQuestionCount: number = 0
	Authors: Author[] = []
	TopicItem: TopicItem | null = null
	MetaDescription: string = ''
	KnowledgeSummary: KnowledgeSummary = {
		solid: 0,
		needsConsolidation: 0,
		needsLearning: 0,
		notLearned: 0,
	}
	gridItems: GridTopicItem[] = []
	isChildOfPersonalWiki: boolean = false
}

export interface KnowledgeSummary {
	solid: number
	needsConsolidation: number
	needsLearning: number
	notLearned: number
}

export interface FooterTopics {
	RootWiki: Topic
	MainTopics: Topic[]
	MemoWiki: Topic
	MemoTopics: Topic[]
	HelpTopics: Topic[]
	PopularTopics: Topic[]
	Documentation: Topic
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
			isChildOfPersonalWiki: false
		}
	},
	actions: {
		setTopic(topic: Topic) {
			if (topic != null) {
				this.id = topic.Id
				this.name = topic.Name
				this.initialName = topic.Name
				this.imgUrl = topic.ImageUrl
				this.imgId = topic.ImageId
				this.content = topic.Content
				this.initialContent = topic.Content

				this.parentTopicCount = topic.ParentTopicCount
				this.parents = topic.Parents
				this.childTopicCount = topic.ChildTopicCount
				this.directVisibleChildTopicCount = topic.DirectVisibleChildTopicCount

				this.views = topic.Views
				this.commentCount = topic.CommentCount
				this.visibility = topic.Visibility

				this.authorIds = topic.AuthorIds
				this.isWiki = topic.IsWiki
				this.currentUserIsCreator = topic.CurrentUserIsCreator
				this.canBeDeleted = topic.CanBeDeleted

				this.questionCount = topic.QuestionCount
				this.directQuestionCount = topic.DirectQuestionCount

				this.authors = topic.Authors
				this.searchTopicItem = topic.TopicItem
				this.knowledgeSummary = topic.KnowledgeSummary
				this.gridItems = topic.gridItems
				this.isChildOfPersonalWiki = topic.isChildOfPersonalWiki
			}
		},
		async saveTopic() {
			const userStore = useUserStore()
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
			if (result.success == true)
				this.contentHasChanged = false
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
		}

	},
	getters: {
		getTopicName(): string {
			return this.name
		},
	},
})