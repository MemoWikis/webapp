import { PageData } from "./missionControl/pageData"

export interface AddSkillResult {
    success: boolean
    errorMessageKey: string
    addedSkill?: PageData | null 
}

export interface RemoveSkillResult {
    success: boolean
    errorMessageKey: string
}

export interface CheckSkillRequest {
    pageId: number
}

export const useUserSkills = () => {
    const addSkill = async (pageId: number): Promise<AddSkillResult> => {
            const result = await $api<AddSkillResult>(`/apiVue/UserSkill/Add/${pageId}`, {
                method: 'POST',
                mode: 'cors',
                credentials: 'include',
            })
            
            return result
    }

    const removeSkill = async (pageId: number): Promise<RemoveSkillResult> => {
            const result = await $api<RemoveSkillResult>(`/apiVue/UserSkill/Remove/${pageId}`, {
                method: 'POST',
                mode: 'cors',
                credentials: 'include',
            })
            
            return result
    }

    const checkSkill = async (pageId: number): Promise<boolean> => {
            const result = await $api<boolean>(`/apiVue/UserSkill/Check/${pageId}`, {
                method: 'GET',
                mode: 'cors',
                credentials: 'include'
            })
            
            return result
    }

    return {
        addSkill,
        removeSkill,
        checkSkill
    }
}
