

import { UseFetchOptions } from '#app'
import { AlertType, messages, useAlertStore } from '~/components/alert/alertStore'

export function useMyApi<T = void> (url: string, options?: UseFetchOptions<T>) {

    if (options && options.onResponseError)
        return useFetch(url, {
            key: url,
            ...options,
        })
    else {
        return useFetch(url, {
            key: url,
            onResponseError: ({response, request, options}) => {
                handleResponseError(response, request, options)
            },
            ...options,
        })
    }
}

export function useMyLazyApi<T = void> (url: string, options?: UseFetchOptions<T>) {

    if (options && options.onResponseError)
        return useLazyFetch(url, {
            key: url,
            ...options,
        })
    else {
        return useLazyFetch(url, {
            key: url,
            onResponseError: ({response, request, options}) => {
                handleResponseError(response, request, options)
            },
            ...options,
        })
    }
}

export const $api = $fetch.create({
    onResponseError: ({response, request, options}) => {
        handleResponseError(response, request, options)
    },
})

function handleResponseError(response:any, request:any, options:any) {
    const { $logger } = useNuxtApp()
    $logger.error('FetchError', [{response, request, options}])
    if (import.meta.client) {
        const alertStore = useAlertStore()
        alertStore.openAlert(AlertType.Error, { text: messages.error.default })
    }
}   