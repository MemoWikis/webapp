export default class BasePage {
    path = '/';

    get url() {
        return `${Cypress.env('URL')}${this.path}`;
    }

    visit(link = '') {
        if (link != '') {
            link = `${Cypress.env('URL')}${link}`;
        }
        cy.visit(link || this.url);
        return this;
    }

    waitUntilRedirectedTo() {
        cy.waitUntil(
            () => {
                return cy.url().then($url => {
                    const actualPath = new URL($url).pathname;
                    return new RegExp(`^${this.path}$`).test(actualPath);
                });
            },
            {interval: 1000}
        );
    }

    get actualDateString() {
        const today = new Date();
        const day = String(today.getDate()).padStart(2, '0');

        const month = String(today.getMonth() + 1).padStart(2, '0');
        const year = today.getFullYear();
        const hour = today.getHours();
        const minute = (today.getMinutes() < 10 ? '0' : '') + today.getMinutes();

        return year + '-' + month + '-' + day + '_' + hour + minute;
    }

    get userName() {
        return `${Cypress.env('userName')}`;
    }

    get password() {
        return `${Cypress.env('password')}`;
    }

    get topicName() {
        return `${Cypress.env('topicName')}`;
    }

    get whatIsNew() {
        return `${Cypress.env('whatIsNew')}`;
    }

    get newQuestion() {
        return `${Cypress.env('newQuestion')}`;
    }

    get newAnswer() {
        return `${Cypress.env('newAnswer')}`;
    }
}
