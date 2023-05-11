// logger.ts
import pino, { LoggerOptions, Level, LogFn } from 'pino'

export class CustomPino {
    private logger: pino.Logger
    private seqApiKey: string
    private seqServerUrl: string

    constructor(seqApiKey: string, seqServerUrl: string) {
        const options: LoggerOptions = {
            level: 'info',
            serializers: pino.stdSerializers,
        }

        this.logger = pino(options)
        this.seqApiKey = seqApiKey
        this.seqServerUrl = seqServerUrl
    }

    private levelToLabel(level: Level): string {
        return pino.levels.labels[parseInt(level as unknown as string, 10)]
    }

    private async sendToSeq(level: Level, args: unknown[]): Promise<void> {
        const timestamp = new Date().toISOString()
        const message = args[0] as string
        const additionalData = args[1] as Record<string, unknown>

        const log = {
            Level: this.levelToLabel(level),
            MessageTemplate: message,
            Timestamp: timestamp,
            ...additionalData,
        }

        this.logger[level](log)

        try {
            await $fetch(`${this.seqServerUrl}/api/events/raw`, {
                method: 'POST',
                headers: {
                    'X-Seq-ApiKey': this.seqApiKey,
                },
                body: { Events: [log] },
            })
        } catch (error) {
            console.error('Error sending log to Seq:', error)
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