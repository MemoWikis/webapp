import { CustomPino } from "~~/logs/logger"

export default defineNuxtPlugin(() => {
    return {
        provide: {
            logger: new CustomPino()
        }
    }
})