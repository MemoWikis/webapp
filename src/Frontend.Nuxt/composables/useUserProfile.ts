export interface UpdateAboutMeRequest {
    aboutMeText: string
}

export interface UpdateAboutMeResult {
    success: boolean
    errorMessageKey?: string
}

export const useUserProfile = () => {
    const updateAboutMe = async (userId: number, aboutMeText: string): Promise<UpdateAboutMeResult> => {
        try {
            const result = await $api<UpdateAboutMeResult>(`/apiVue/UserProfile/UpdateAboutMe/${userId}`, {
                method: 'POST',
                mode: 'cors',
                credentials: 'include',
                body: {
                    aboutMeText: aboutMeText
                }
            })
            
            return result
        } catch (error) {
            console.error('Error updating about me text:', error)
            return {
                success: false,
                errorMessageKey: 'error.user.updateFailed'
            }
        }
    }

    return {
        updateAboutMe
    }
}
