export class Site {
    static redirectToRegistration() { location.href = "/Registrieren"; }
    static reloadPage() { window.location.reload() };

    static loadValidPage(link: string = window.location.pathname) {
        const isInvalid = link == '/Registrieren' || link == '/Ausloggen' || link == '/Fehler/500' || link == '/Fehler/404';
        if (isInvalid)
            location.href = "/";
        else this.reloadPage();
    }
}