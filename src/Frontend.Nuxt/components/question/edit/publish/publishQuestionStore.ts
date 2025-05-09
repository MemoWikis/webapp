import { defineStore } from "pinia";
import {
    useSnackbarStore,
    SnackbarData,
} from "~/components/snackBar/snackBarStore";

export const usePublishQuestionStore = defineStore(
    "publishQuestionStore",
    () => {
        const showModal = ref(false);
        const errorMsg = ref("");
        const showErrorMsg = ref(false);
        const questionId = ref<number>(0);
        const text = ref("");

        const openModal = async (id: number) => {
            questionId.value = id;
            text.value = "";
            errorMsg.value = "";
            showErrorMsg.value = false;

            interface Result {
                success: boolean;
                messageKey?: string;
                text: string;
            }
            const result = await $api<Result>(
                `/apiVue/PublishQuestionStore/GetShortText/${id}`,
                {
                    method: "GET",
                    mode: "cors",
                    credentials: "include",
                }
            );
            if (result && result.success && result.text) {
                text.value = result.text;
                showModal.value = true;
            } else if (result?.messageKey) {
                const snackbarStore = useSnackbarStore();
                const nuxtApp = useNuxtApp();
                const { $i18n } = nuxtApp;

                const data: SnackbarData = {
                    type: "error",
                    text: { message: $i18n.t(result.messageKey) },
                };
                snackbarStore.showSnackbar(data);
            }
        };

        const closeModal = () => {
            showModal.value = false;
        };

        const confirmPublish = async () => {
            interface Result {
                success: boolean;
                messageKey?: string;
            }
            const result = await $api<Result>(
                `/apiVue/PublishQuestionStore/PublishQuestion/${questionId.value}`,
                {
                    method: "POST",
                    mode: "cors",
                    credentials: "include",
                }
            );
            const nuxtApp = useNuxtApp();
            const { $i18n } = nuxtApp;

            if (result && result.success) {
                const snackbarStore = useSnackbarStore();
                const data: SnackbarData = {
                    type: "success",
                    text: {
                        message: $i18n.t("success.question.published", {
                            name: text.value,
                        }),
                    },
                };
                snackbarStore.showSnackbar(data);
                closeModal();
                return questionId.value;
            } else if (result && !result.success && result.messageKey) {
                errorMsg.value = $i18n.t(result.messageKey);
                showErrorMsg.value = true;
                return null;
            }
        };

        return {
            showModal,
            errorMsg,
            showErrorMsg,
            questionId,
            text,
            openModal,
            closeModal,
            confirmPublish,
        };
    }
);
