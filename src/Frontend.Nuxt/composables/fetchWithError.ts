import { AlertType, useAlertStore } from "~/components/alert/alertStore"
import { $fetch, FetchResponse, FetchOptions } from "ofetch"

export const $api = $fetch.create({
    onResponseError: ({ response, request, options }) => {
        handleResponseError(response, request, options)
    },
})

function handleResponseError(
    response: FetchResponse<any> & FetchResponse<ResponseType>,
    request?: RequestInfo,
    options?: FetchOptions
) {
    const { $logger } = useNuxtApp()
    $logger.error("Default Fetch Error", [{ response, request, options }])
    if (import.meta.client) {
        const alertStore = useAlertStore()
        const { $i18n } = useNuxtApp()
        alertStore.openAlert(
            AlertType.Error,
            {
                text: null,
                texts: [
                    $i18n.t("error.api.body.title"),
                    $i18n.t("error.api.body.suggestion"),
                ],
                customDetails: response._data,
            },
            $i18n.t("label.reloadPage"),
            true,
            $i18n.t("error.api.title"),
            "reloadPage",
            $i18n.t("label.back")
        )

        alertStore.$onAction(({ name, after }) => {
            if (name === "closeAlert") {
                after((result) => {
                    if (result.cancelled === false && result.id == "reloadPage")
                        window.location.reload()
                })
            }
        })
    }
}
