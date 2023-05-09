// import { createLogger, format, transports } from 'winston'
// import { SeqTransport } from '@datalust/winston-seq'

export default defineNuxtPlugin(() => {
    // const logger = createLogger({
    //     level: 'info',
    //     format: format.combine(
    //         format.timestamp({
    //             format: 'YYYY-MM-DD HH:mm:ss'
    //         }),
    //         format.errors({ stack: true }),
    //         format.splat(),
    //         format.json()
    //     ),
    //     defaultMeta: { service: 'nuxt-app' },
    //     transports: [
    //         new SeqTransport({
    //             serverUrl: "https://localhost:5341",
    //             apiKey: "",
    //             onError: (e => { console.error(e) }),
    //         })
    //     ],
    // });

    // if (process.env.NODE_ENV !== 'production') {
    //     logger.add(
    //         new transports.Console({
    //             format: format.combine(format.colorize(), format.simple()),
    //         })
    //     );
    // }
    return {
        // provide: {
        //     logger: logger
        // }
    }
})