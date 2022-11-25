import { isNumber } from "underscore";

export default defineNuxtRouteMiddleware((to) => {

    const split = to.fullPath.split('/')
    if (split[1] == 'Registrieren') {
        navigateTo('/user/register/',{ replace: true })
        history.pushState(null, 'Registrieren', `/Registrieren`);  
    } 
    else if (split[1] == 'Nachrichten') {
        navigateTo('/user/messages/',{ replace: true })
        history.pushState(null, 'Nachrichten', `/Nachrichten`);  
    } else if (split[1] == 'Frage' && isNumber(parseInt(split[3]))) {
        navigateTo({path: `/question/${split[2]}/${split[3]}`})
    }
  })