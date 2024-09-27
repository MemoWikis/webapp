import { defineStore } from 'pinia'
import { useUserStore } from '../user/userStore'
import { Visibility } from '../shared/visibilityEnum'
import { Author } from '../author/author'
import { TopicItem } from '../search/searchHelper'
import { GridTopicItem } from './content/grid/item/gridTopicItem'
import { AlertType, messages, useAlertStore } from '../alert/alertStore'
import { useSnackbarStore, SnackbarData } from '../snackBar/snackBarStore'
import { ErrorCode } from '../error/errorCodeEnum'
import { nanoid } from 'nanoid'

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
	messageKey: string | null = null
	errorCode: ErrorCode | null = null
	viewsLast30DaysAggregatedTopic: ViewSummary[] | null = null
	viewsLast30DaysTopic: ViewSummary[] | null = null
	viewsLast30DaysAggregatedQuestions: ViewSummary[] | null = null
	viewsLast30DaysQuestions: ViewSummary[] | null = null
}
export interface ViewSummary{
	count: number
	date: string
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

interface GetTopicAnalyticsResponse {
	viewsPast90DaysAggregatedTopics: ViewSummary[]
	viewsPast90DaysTopic: ViewSummary[]
	viewsPast90DaysAggregatedQuestions: ViewSummary[]
	viewsPast90DaysDirectQuestions: ViewSummary[]
}

export enum FeedItemType {
    Create = 0,
    Update = 1,
    Delete = 2,
    Published = 3,
    Privatized = 4,
    Renamed = 5,
    Text = 6,
    Relations = 7,
    Image = 8,
    Restore = 9,
    Moved = 10,
}

export interface FeedItem {
	date: string
	type: FeedItemType
	categoryChangeId: number
	topicId: number
	visibility: Visibility
}

interface GetFeedResponse{
	feedItems: FeedItem[]
	maxCount: number
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
			uploadedImagesInContent: [] as string[],
			uploadedImagesMarkedForDeletion: [] as string[],
			uploadTrackingArray: [] as string[],
			viewsPast90DaysAggregatedTopics: [] as ViewSummary[],
			viewsPast90DaysTopic: [] as ViewSummary[],
			viewsPast90DaysAggregatedQuestions: [] as ViewSummary[],
			viewsPast90DaysDirectQuestions: [] as ViewSummary[],
			analyticsLoaded: false
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
				this.uploadedImagesInContent = []
				this.uploadedImagesMarkedForDeletion = []

				this.analyticsLoaded = false
				this.viewsPast90DaysAggregatedTopics = []
				this.viewsPast90DaysTopic = []
				this.viewsPast90DaysAggregatedQuestions = []
				this.viewsPast90DaysDirectQuestions = []
			}
		},
		async saveTopic() {
			const userStore = useUserStore()
			const snackbarStore = useSnackbarStore()

			if (!userStore.isLoggedIn) {
				userStore.openLoginModal()
				return
			}

			
			await this.waitUntilAllUploadsComplete()
					
			const data = {
				id: this.id,
				name: this.name,
				saveName: this.name != this.initialName,
				content: this.content,
				saveContent: this.content != this.initialContent && this.contentHasChanged
			}

			const result = await $api<FetchResult<boolean>>('/apiVue/TopicStore/SaveTopic', {
				method: 'POST', 
				body: data, 
				mode: 'cors', 
				credentials: 'include',
				onResponseError(context) {
					const { $logger } = useNuxtApp()
					$logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
				}
			})

			if (result.success == true && this.visibility != Visibility.Owner) {
				const data: SnackbarData = {
                    type: 'success',
                    text: messages.success.category.saved
                }
                snackbarStore.showSnackbar(data)
				this.contentHasChanged = false
				this.initialName = this.name
				this.initialContent = this.content
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
			this.uploadedImagesInContent = []
			this.uploadedImagesMarkedForDeletion = []
		},
		isOwnerOrAdmin() {
			const userStore = useUserStore()
			return userStore.isAdmin || this.currentUserIsCreator
		},

		async refreshTopicImage() {
			this.imgUrl = await $api<string>(`/apiVue/TopicStore/GetTopicImageUrl/${this.id}`, {
				method: 'GET', mode: 'cors', credentials: 'include',
				onResponseError(context) {
					const { $logger } = useNuxtApp()
					$logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, req: context.request }])
				}
			})
		},
		async reloadKnowledgeSummary() {
			this.knowledgeSummary = await $api<KnowledgeSummary>(`/apiVue/TopicStore/GetUpdatedKnowledgeSummary/${this.id}`, {
				method: 'GET', mode: 'cors', credentials: 'include',
				onResponseError(context) {
					const { $logger } = useNuxtApp()
					$logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, req: context.request }])
				}
			})
		},
		async reloadGridItems() {
			const result = await $api<GridTopicItem[]>(`/apiVue/TopicStore/GetGridTopicItems/${this.id}`, {
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
			const result = await $api<boolean>(`/apiVue/TopicStore/HideOrShowText/`, {
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
		},
		async uploadContentImage(file: File): Promise<string> {
			const uploadId = nanoid(5)
			this.uploadTrackingArray.push(uploadId)
			
			const data = new FormData()
			data.append('file', file)
			data.append('topicId', this.id.toString())

			const result = await $api<string>('/apiVue/TopicStore/UploadContentImage', {
				body: data,
				method: 'POST',
				mode: 'cors',
				credentials: 'include',
			})

			this.uploadTrackingArray = this.uploadTrackingArray.filter(id => id !== uploadId)
			
			return result
		},
		addImageUrlToDeleteList(url: string) {
			if (!this.uploadedImagesMarkedForDeletion.includes(url))
				this.uploadedImagesMarkedForDeletion.push(url)
		},
		refreshDeleteImageList() {
			const imagesToKeep = this.uploadedImagesInContent
			this.uploadedImagesMarkedForDeletion = this.uploadedImagesMarkedForDeletion.filter(url => imagesToKeep.includes(url))
		},
		async deleteTopicContentImages() {
			if (this.uploadedImagesMarkedForDeletion.length == 0)
				return

			const data = {
				topicId: this.id,
				imageUrls: this.uploadedImagesMarkedForDeletion
			}
			await $api<void>('/apiVue/TopicStore/DeleteContentImages', {
				body: data,
				method: 'POST',
				mode: 'cors',
				credentials: 'include',
			})
			this.uploadedImagesMarkedForDeletion = []
		},
		async waitUntilAllUploadsComplete() {
			while (this.uploadTrackingArray.length > 0) {
				await new Promise(resolve => setTimeout(resolve, 100))
			}
		},
		async getAnalyticsData() {
			
			const data = await $api<GetTopicAnalyticsResponse>(`/apiVue/TopicStore/GetTopicAnalytics/${this.id}`, {
				method: 'GET',
				mode: 'cors',
				credentials: 'include',
				onResponseError(context) {
					const { $logger } = useNuxtApp()
					$logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, req: context.request }])
				}
			})

			if (data) {
				this.viewsPast90DaysAggregatedTopics = data.viewsPast90DaysAggregatedTopics
				this.viewsPast90DaysTopic = data.viewsPast90DaysTopic
				this.viewsPast90DaysAggregatedQuestions = data.viewsPast90DaysAggregatedQuestions
				this.viewsPast90DaysDirectQuestions = data.viewsPast90DaysDirectQuestions

				this.analyticsLoaded = true
			}
		},
		async getFeed() {
			const data = {
				topicId: this.id,
				page: 1,
				pageSize: 100
			}

			const result = await $api<GetFeedResponse>(`/apiVue/TopicStore/GetFeed/`, {
				method: 'POST',
				mode: 'cors',
				credentials: 'include',
				body: data,
				onResponseError(context) {
					const { $logger } = useNuxtApp()
					$logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, req: context.request }])
				}
			})

			return result
		},
		async getFeedWithDescendants() {
			const data = {
				topicId: this.id,
				page: 1,
				pageSize: 100
			}

			const result = await $api<GetFeedResponse>(`/apiVue/TopicStore/GetFeedWithDescendants/`, {
				method: 'POST',
				mode: 'cors',
				credentials: 'include',
				body: data,
				onResponseError(context) {
					const { $logger } = useNuxtApp()
					$logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, req: context.request }])
				}
			})

			return result
		}

	},
	getters: {
		getTopicName(): string {
			return this.name
		},
	},
})