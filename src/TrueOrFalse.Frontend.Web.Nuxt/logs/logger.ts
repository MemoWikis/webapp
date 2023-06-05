import pino, { Level, LogFn } from 'pino'
interface Property {
    [key: string]: any;
}
export class CustomPino {

    private levelToLabel(level: Level): string {
        return pino.levels.labels[parseInt(level as unknown as string, 10)]
    }

    private async sendToSeq(level: Level, args: unknown[] = []): Promise<void> {

        const timestamp = new Date().toISOString()
        const message = args[0] as string
        const additionalData = args[1] as Property[]

        let properties = {}
        if (additionalData != undefined)
            properties = Object.assign(properties, ...additionalData)

        const log = {
            Level: this.levelToLabel(level),
            MessageTemplate: message,
            Timestamp: timestamp,
            Properties: properties
        }

        let url = '/seqlog'
        let apiKey = ''
        if (process.server) {
            url = process.env.NUXT_SEQ_RAW_URL ? process.env.NUXT_SEQ_RAW_URL : 'http://localhost:5341/api/events/raw'
            if (process.env.NUXT_SEQ_SERVER_API_KEY)
                apiKey = process.env.NUXT_SEQ_SERVER_API_KEY
        } else {
            const config = useRuntimeConfig()
            apiKey = config.public.seqClientApiKey
        }

        const loggingContent = {
            method: 'POST',
            headers: {
                'X-Seq-ApiKey': apiKey,
            },
            body: { Events: [log] },
        }

        try {
            await $fetch(url, {
                method: 'POST',
                headers: {
                    'X-Seq-ApiKey': apiKey,
                },
                body: { Events: [log] },
            })
        } catch (error) {
            console.error('Error sending log to Seq:', error)
            console.log(loggingContent)
            console.log("Log: ", log)
            console.log({ ...log })
        }
    }

    log: LogFn = (level: any, ...args: any[]) => {
        this.sendToSeq(level, args)
    }

    info: LogFn = (...args: any[]) => {
        this.sendToSeq('info', args)
    }

    error: LogFn = (...args: any[]) => {
        this.sendToSeq('error', args)
    }

    warn: LogFn = (...args: any[]) => {
        this.sendToSeq('warn', args)
    }

    debug: LogFn = (...args: any[]) => {
        this.sendToSeq('debug', args)
    }

    fatal: LogFn = (...args: any[]) => {
        this.sendToSeq('fatal', args)
    }

    trace: LogFn = (...args: any[]) => {
        this.sendToSeq('trace', args)
    }
}
