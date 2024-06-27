import { CustomPino } from "~/logs/logger"

export default defineEventHandler(async (event) => {

    const urlObj = getRequestURL(event)
    const pathName = urlObj.pathname
    const log = new CustomPino()

    if (pathName.startsWith('/Fragen')) {

        const splitArray = pathName.split('/')
        const filteredArray = splitArray.filter(element => element !== '')

        // only redirect if there were unused additions to the landingpage path
        if (filteredArray.length > 3) {
            log.debug('Redirecting to correct question page', [{ pathName: pathName }])

            const resultArray = filteredArray.slice(1, 3)
            if (!isNaN(parseInt(resultArray[1])) ) {
                await sendRedirect(event, `/Fragen/${resultArray[0]}/${resultArray[1]}`, 301)
            }
        }
    } else if (pathName.endsWith('/Wissensnetz')) {
        const splitArray = pathName.split('/')
        const filteredArray = splitArray.filter(element => element !== '')

        if (filteredArray.length === 3) {
            log.debug('Redirecting to correct Topic page', [{ pathName: pathName }])

            if (!isNaN(parseInt(filteredArray[1])) ) {
                await sendRedirect(event, `${filteredArray[0]}/${filteredArray[1]}/Analytics}`, 301)
            }
        }
    }
    return
})