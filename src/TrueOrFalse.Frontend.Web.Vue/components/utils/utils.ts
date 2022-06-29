import { useUtilsStore } from '../utils/utilsStore'
const utilsStore = useUtilsStore()

export class Utils {

    static showSpinner() {
        utilsStore.showSpinner()
    }

    static hideSpinner() {
        utilsStore.hideSpinner()
    }
}