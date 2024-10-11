import { defineStore } from "pinia"

export interface CommentModel {
    id: number
    creatorName: string
    creatorEncodedName: string
    creationDate: string
    creationDateNiceText: string
    creatorImgUrl: string
    creatorId: number
    title: string
    text: string
    shouldBeImproved: boolean
    shouldBeDeleted: boolean
    isSettled: boolean
    shouldReasons: string[]
    answers: CommentModel[]
    settledAnswersCount: number
    showSettledAnswers: boolean
}

export const useCommentsStore = defineStore('commentsStore', () => {

    const show = ref<boolean>(false)

    const questionId = ref<number>(0)
    const unsettledComments = ref<CommentModel[]>([])
    const settledComments = ref<CommentModel[]>([])

    async function openModal(id: number) {
        questionId.value = id
        if (await loadComments()) {
            show.value = true
        }
    }

    async function loadFirst(id: number) { 
        questionId.value = id
        await loadComments()
    }

    async function loadComments() {
        interface Result {
            settledComments: CommentModel[]
            unsettledComments: CommentModel[]
        }
        const result = await $api<Result>(`/apiVue/CommentsStore/GetAllComments/${questionId.value}`, {
            mode: 'cors',
            credentials: 'include'
        })
        if (result) {
            settledComments.value = result.settledComments
            unsettledComments.value = result.unsettledComments
            return true
        }
        return false
    }

    async function loadComment(id: number) {
        const result = await $api<CommentModel>(`/apiVue/CommentsStore/GetComment/${id}`, {
            mode: 'cors',
            credentials: 'include'
        })
        return result
    }

    return { show, questionId, unsettledComments, settledComments, openModal, loadComments, loadFirst, loadComment }
})