// logger.ts
import pino, { LoggerOptions, Level, LogFn } from 'pino';

class CustomPino {
    private logger: pino.Logger;
    private seqApiKey: string;

    constructor(options: LoggerOptions, seqApiKey: string) {
        this.logger = pino(options);
        this.seqApiKey = seqApiKey;
    }

    private async sendToSeq(level: Level, args: unknown[]): Promise<void> {
        const timestamp = new Date().toISOString();
        const message = args[0] as string;
        const additionalData = args[1] as Record<string, unknown>;

        const log = {
            Level: pino.levels.labels[level as unknown as number],
            MessageTemplate: message,
            Timestamp: timestamp,
            ...additionalData,
        };

        this.logger[level](log);

        try {
            await fetch('http://localhost:5341/api/events/raw', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'X-Seq-ApiKey': this.seqApiKey,
                },
                body: JSON.stringify({ Events: [log] }),
            });
        } catch (error) {
            console.error('Error sending log to Seq:', error);
        }
    }

    log: LogFn = (level: any, ...args: any[]) => {
        this.sendToSeq(level, args);
    };

    info: LogFn = (...args: any[]) => {
        this.sendToSeq('info', args);
    };

    error: LogFn = (...args: any[]) => {
        this.sendToSeq('error', args);
    };

    warn: LogFn = (...args: any[]) => {
        this.sendToSeq('warn', args);
    };

    debug: LogFn = (...args: any[]) => {
        this.sendToSeq('debug', args);
    };

    fatal: LogFn = (...args: any[]) => {
        this.sendToSeq('fatal', args);
    };

    trace: LogFn = (...args: any[]) => {
        this.sendToSeq('trace', args);
    };
}

const loggerOptions: LoggerOptions = {
    level: 'info',
    serializers: pino.stdSerializers,
};

const seqApiKey = 'qvEaDJ3A5QFJGMeJOAbH';
const logger = new CustomPino(loggerOptions, seqApiKey);


export default defineNuxtPlugin(() => {
    return {
        provide: {
            logger: logger
        }
    }
});