export interface AddSkillRequest {
    pageId: number
}

export interface AddSkillResult {
    success: boolean
    errorMessageKey: string
    addedSkill: any
}

export interface RemoveSkillRequest {
    pageId: number
}

export interface RemoveSkillResult {
    success: boolean
    errorMessageKey: string
}

export interface CheckSkillRequest {
    pageId: number
}

export interface CheckSkillResult {
    isSkill: boolean
}

export const useUserSkills = () => {
    const addSkill = async (userId: number, pageId: number): Promise<AddSkillResult> => {
        try {
            const result = await $api<AddSkillResult>(`/apiVue/UserSkill/Add/${userId}`, {
                method: 'POST',
                mode: 'cors',
                credentials: 'include',
                body: {
                    pageId: pageId
                }
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
            const result = await $api<RemoveSkillResult>(`/apiVue/UserSkill/Remove/${userId}`, {
                method: 'POST',
                mode: 'cors',
                credentials: 'include',
                body: {
                    pageId: pageId
                }
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

    const checkSkill = async (userId: number, pageId: number): Promise<CheckSkillResult> => {
        try {
            const result = await $api<CheckSkillResult>(`/apiVue/UserSkill/Check/${userId}`, {
                method: 'POST',
                mode: 'cors',
                credentials: 'include',
                body: {
                    pageId: pageId
                }
            })
            
            return result
        } catch (error) {
            console.error('Error checking skill:', error)
            return {
                isSkill: false
            }
        }
    }

    return {
        addSkill,
        removeSkill,
        checkSkill
    }
}
