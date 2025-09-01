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
        try {
            const result = await $api<AddSkillResult>(`/apiVue/UserSkill/Add/${pageId}`, {
                method: 'POST',
                mode: 'cors',
                credentials: 'include',
            })
            
            return result
        } catch (error) {
            console.error('Error adding skill:', error)
            return {
                success: false,
                errorMessageKey: 'error.addSkill.failed',
                addedSkill: null
            }
        }
    }

    const removeSkill = async (userId: number, pageId: number): Promise<RemoveSkillResult> => {
        try {
            const result = await $api<RemoveSkillResult>(`/apiVue/UserSkill/Remove/${pageId}`, {
                method: 'POST',
                mode: 'cors',
                credentials: 'include',
            })
            
            return result
        } catch (error) {
            console.error('Error removing skill:', error)
            return {
                success: false,
                errorMessageKey: 'error.removeSkill.failed'
            }
        }
    }

    const checkSkill = async (userId: number, pageId: number): Promise<boolean> => {
        try {
            const result = await $api<boolean>(`/apiVue/UserSkill/Check/`, {
                method: 'POST',
                mode: 'cors',
                credentials: 'include',
                body: {
                    userId: userId,
                    pageId: pageId
                }
            })
            
            return result
        } catch (error) {
            console.error('Error checking skill:', error)
            return false
        }
    }

    return {
        addSkill,
        removeSkill,
        checkSkill
    }
}
