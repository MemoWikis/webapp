import { useAlertStore } from "./alertStore";
const alertStore = useAlertStore()

export type AlertMsg = {
    text: string,
    reload?: boolean,
    customHtml?: string,
    customBtn?: string,
}

export enum AlertType {
    Default,
    Success,
    Error
}

export class Alerts {
    static showError(msg: AlertMsg): void {
        alertStore.openAlert(AlertType.Error, msg)
    }
    static showSuccess(msg: AlertMsg): void {
        alertStore.openAlert(AlertType.Success, msg)
    }
}