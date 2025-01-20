

import { AlertType, messages, useAlertStore } from '~/components/alert/alertStore'
import { $fetch, FetchResponse, FetchOptions } from 'ofetch'

export const $api = $fetch.create({
    onResponseError: ({response, request, options}) => {
        handleResponseError(response, request, options)
    },
})

function handleResponseError(response:FetchResponse<any> & FetchResponse<ResponseType>, request?:RequestInfo, options?:FetchOptions) {
    const { $logger } = useNuxtApp()
    $logger.error('Default Fetch Error', [{response, request, options}])
    if (import.meta.client) {
        const alertStore = useAlertStore()
        alertStore.openAlert(AlertType.Error, { text: null, customHtml:  messages.error.api.body, customDetails: response._data}, "Seite neu laden", true, messages.error.api.title, 'reloadPage', 'Zurück')

        alertStore.$onAction(({ name, after }) => {
            if (name === 'closeAlert') {

                after((result) => {
                    if (result.cancelled === false && result.id == 'reloadPage')
                        window.location.reload()
                })
            }
        })
    }
}   