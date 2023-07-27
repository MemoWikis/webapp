import { CustomPino } from "~~/logs/logger"
enum LifecycleHooks {
    BEFORE_CREATE = <any>"bc",
    CREATED = <any>"c",
    BEFORE_MOUNT = <any>"bm",
    MOUNTED = <any>"m",
    BEFORE_UPDATE = <any>"bu",
    UPDATED = <any>"u",
    BEFORE_UNMOUNT = <any>"bum",
    UNMOUNTED = <any>"um",
    DEACTIVATED = <any>"da",
    ACTIVATED = <any>"a",
    RENDER_TRIGGERED = <any>"rtg",
    RENDER_TRACKED = <any>"rtc",
    ERROR_CAPTURED = <any>"ec",
    SERVER_PREFETCH = <any>"sp"
}

function getKeyFromValue(value: string): string | undefined {
    const keys = Object.keys(LifecycleHooks).filter(k => LifecycleHooks[k as any] === value);
    return keys.length > 0 ? keys[0] : undefined;
}

export default defineNuxtPlugin((nuxtApp) => {
    nuxtApp.vueApp.config.errorHandler = (error, context, info) => {
        const logger = new CustomPino()
        if (error instanceof SyntaxError || error instanceof TypeError) {
            const errorObject = {
                name: error.name,
                message: error.message,
                stack: error.stack,
            }

            logger.error(`NUXT ERROR`, [{ error: errorObject, route: context?.$route.path, file: context?.$options.__file, lifeCycleHook: getKeyFromValue(info) }])
        } else
            logger.error(`NUXT ERROR`, [{ error: 'An unknown error occured', route: context?.$route.path, file: context?.$options.__file, lifeCycleHook: getKeyFromValue(info) }])
    }
})