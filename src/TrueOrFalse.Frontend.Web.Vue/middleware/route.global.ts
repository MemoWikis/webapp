export default defineNuxtRouteMiddleware((to) => {

    const split = to.fullPath.split('/')
    if (split[1] == 'Registrieren') {
        navigateTo('/user/register/',{ replace: true })
        history.pushState(null, 'Registrieren', `/Registrieren`);  
    }
  })