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

            .get('#LearningTab')
            .click()

            .get('.margin-top-4 > .dropdown-toggle')
            .click()

            .get('.margin-top-4 > .dropdown-menu > :nth-child(4) > a')
            .should('be.visible')
            .click()
            .get('#modalDeleteQuestion > .modal-dialog > .modal-content > .cardModalContent > .modalFooter > .btn-danger')
            .should('be.visible')
            .click()
            .get('#SuccessModal > .modal-dialog > .modal-content > .modal-footer > .btn')
            .should('be.visible')
            .click();
    });
});
