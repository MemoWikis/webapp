import pino, { Level, LogFn } from 'pino'
interface Property {
    [key: string]: any;
}
export class CustomPino {
    private seqApiKey: string
    private seqServerUrl: string

    constructor(apiKey: string, seqServerUrl: string) {
        this.seqApiKey = apiKey
        this.seqServerUrl = seqServerUrl
    }

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

        const loggingContent = {
            method: 'POST',
            headers: {
                'X-Seq-ApiKey': this.seqApiKey,
            },
            body: { Events: [log] },
        }

        try {
            // await $fetch(`${this.seqServerUrl}/api/events/raw`, {
            await $fetch(`/seqlog`, {
                method: 'POST',
                headers: {
                    'X-Seq-ApiKey': this.seqApiKey,
                },
                body: { Events: [log] },
            })
        } catch (error) {
            console.error('Error sending log to Seq:', error)
            console.log(loggingContent)
            console.log("Log: ", log)
            console.log({ ...log });
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
