import {startPage} from '@/pages/startpage.page';
import {loginPage} from '@/pages/login.page';


describe('New learning', () => {
    beforeEach(() => {
        console.log(Cypress.env());
        startPage.visit();
        loginPage.doLoginAsTestUser();
    });

    it('learning ', function () {
        // eslint-disable-next-line cypress/no-unnecessary-waiting
        cy.get('#BreadcrumbLogoSmall')
            .click()
            .get('#LearningFooterBtn')
            .click()


        cy.get('#AnswerAndSolutionCol').then((learning) => {

            console.log(learning)
            if (learning.find('#btnFlipCard').length > 0) {
                console.log('btnFlipCard is visible');
            }

            if (learning.find('#txtAnswer').length > 0) {
                console.log('#txtAnswer is visible');
            }
            if (learning.find('#AnswerInputSection > .checkbox > label > input').length > 0) {
                console.log('multiple choice is visible');
            }
        });
    });
});