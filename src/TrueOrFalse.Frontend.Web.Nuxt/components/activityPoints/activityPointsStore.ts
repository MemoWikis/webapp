import { defineStore } from "pinia"
import { useUserStore } from "../user/userStore"

export enum Activity {
    CorrectAnswer,
    WrongAnswer,
    ShowedSolution,
    CountAsCorrect
}

export const useActivityPointsStore = defineStore('activityPointsStore', () => {
    const level = ref(0)
    const points = ref(0)
    const activityPointsPercentageOfNextLevel = ref(0)
    const activityPointsTillNextLevel = ref(0)
    const levelUp = ref(false)

    async function addPoints(type: Activity) {
        let amount
        let typeString

        switch (type) {
            case Activity.CorrectAnswer:
                amount = 15
                typeString = "RightAnswer"
                break
            case Activity.WrongAnswer:
                amount = 1
                typeString = "WrongAnswer"
                break
            case Activity.ShowedSolution:
                amount = 3
                typeString = "ShowedSolution"
                break
            case Activity.CountAsCorrect:
                amount = 12
                typeString = "CountAsCorrect"
                break
        }

        const data = {
            activityTypeString: typeString,
            points: amount
        }

        const result = await $fetch<any>('/apiVue/ActivityPointsStore/Add', {
            method: 'POST',
            mode: 'cors',
            body: data
        })
        if (result != null) {
            setData(result)
        }
    }
    const showLevelPopUp = ref(false)
    function setData(data: { points: number, level: number, levelUp: boolean, activityPointsTillNextLevel: number, activityPointsPercentageOfNextLevel?: number }) {
        points.value = data.points
        level.value = data.level
        levelUp.value = data.levelUp
        activityPointsTillNextLevel.value = data.activityPointsTillNextLevel
        const userStore = useUserStore()
        if (userStore.isLoggedIn && data.activityPointsPercentageOfNextLevel)
            activityPointsPercentageOfNextLevel.value = data.activityPointsPercentageOfNextLevel

        if (levelUp.value)
            showLevelPopUp.value = true
    }

    return { level, points, addPoints, activityPointsTillNextLevel, setData, showLevelPopUp }
})
