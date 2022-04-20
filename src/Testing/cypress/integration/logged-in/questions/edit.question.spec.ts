import {startPage} from '@/pages/startpage.page';
import {loginPage} from '@/pages/login.page';

describe('New Question', () => {
    beforeEach(() => {
        startPage.visit();
        console.log(startPage.topicName);
        loginPage.doLoginAsTestUser();
    });

    it('New Question', () => {
        // eslint-disable-next-line cypress/no-unnecessary-waiting
        cy.get('#BreadcrumbLogoSmall')
            .click()
            .should('be.visible')

            .get('a > b')
            .click()

            .get('.margin-top-4 > .dropdown-toggle')
            .click()

            .get('.margin-top-4 > .dropdown-menu > :nth-child(1) > a')
            .click()

            .get('#QuestionInputField')
            .clear()
            .type('Wie lautet die Basisklasse für Threads?')

            .get(':nth-child(2) > :nth-child(3) > .ProseMirror')
            .clear()
            .type('using System.Threading;')
            .get('#EditQuestionModal > .modal-dialog > .modal-content > .modal-footer > .btn-primary')
            .click()
            .get('#SuccessModal > .modal-dialog > .modal-content > .modal-footer > .btn')
            .click();
    });
});