import { defineStore } from "pinia"

export interface Comment {
    id: number
    creatorName: string
    creationDate: string
    creationDateNiceText: string
    imageUrl: string
    creatorUrl: string
    creatorId: number
    title: string
    text: string
    shouldBeImproved: boolean
    shouldBeDeleted: boolean
    isSettled: boolean
    shouldReasons: string[]
    answers: Comment[]
    settledAnswersCount: number
    showSettledAnswers: boolean
}

export const useCommentsStore = defineStore('commentsStore', () => {

    const show = ref<boolean>(false)

    const questionId = ref<number>(0)
    const comments = ref<Comment[]>([])
    const settledComments = ref<Comment[]>([])

    async function openModal(id: number) {
        questionId.value = id
        comments.value = await $fetch<Comment[]>(`/apiVue/Comments/Get/${id}`)
        settledComments.value = await $fetch<Comment[]>(`/apiVue/Comments/GetSettledComments/${id}`)

        show.value = true
    }

    return { show, questionId, comments, settledComments, openModal }
})