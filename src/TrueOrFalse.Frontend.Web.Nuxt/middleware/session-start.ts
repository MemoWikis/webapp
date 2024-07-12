
export default defineNuxtRouteMiddleware(async (to, from) => {
    if (import.meta.client) return
    const sessionHasStarted = useState('sessionHasStarted', () => false)
    
    if (sessionHasStarted.value) return

    sessionHasStarted.value = true

    const headers = useRequestHeaders(['cookie']) as HeadersInit
    const { $config, $urlHelper } = useNuxtApp()
    interface SessionStartResult {
        success: boolean
        loginGuid?: string
        expiryDate?: string
        alreadyLoggedIn?: boolean
    }

    function getSessionCookieValue(setCookieHeader: string) {
	if (!setCookieHeader) return null;

        // Split the header by semicolon and filter out the relevant cookie
        const cookieParts = setCookieHeader.split(';');
        for (const part of cookieParts) {
            const [name, value] = part.split('=').map(part => part.trim());
            if (name === '.AspNetCore.Session') {
                return value;
            }
        }
        return null 
    }

    const sessionStartResult = await $api<SessionStartResult>('/apiVue/App/SessionStart', {
        method: 'POST',
        credentials: 'include',
        mode: 'no-cors',
        body: {
            sessionStartGuid: $config.sessionStartGuid
        },
        onRequest({ options }) {
            options.headers = headers
            options.baseURL = $config.public.serverBase
        },
        onResponse(response) {
            const str = response.response.headers.get('set-cookie')
            if (str) {
                const sessionCookieValue = getSessionCookieValue(str)
                if (sessionCookieValue) {

                    useCookie('.AspNetCore.Session', {
                        sameSite: 'lax',
                        secure: $config.public.environment != 'development',
                        httpOnly: true
                    }).value = sessionCookieValue

                    refreshCookie('.AspNetCore.Session')
                }
            }

        },
        onResponseError(context) {
            throw createError({ statusMessage: context.error?.message })
        }
    })
    if (sessionStartResult?.success) {

        const loginGuid = sessionStartResult.loginGuid
        const expiryDate = sessionStartResult.expiryDate

        if (loginGuid && expiryDate) {
            useCookie('persistentLogin', {
                expires: new Date(expiryDate),
                sameSite: 'lax',
                secure: $config.public.environment != 'development',
                httpOnly: true
            }).value = loginGuid

        refreshCookie('persistentLogin')                
        }
    }

    return
})
