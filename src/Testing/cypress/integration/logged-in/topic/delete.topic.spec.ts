import {startPage} from '@/pages/startpage.page';
import {loginPage} from '@/pages/login.page';

describe('Edit topic', () => {
    beforeEach(() => {
        startPage.visit();
        loginPage.doLoginAsTestUser();
        console.log(startPage.topicName);
    });
    it('Delete topic', () => {
        //Press topic button
        // eslint-disable-next-line cypress/no-unnecessary-waiting
        cy.get('#BreadcrumbLogoSmall')
            .click()
            .should('be.visible')

            .get('div.topic-name > a')
            .eq(0)
            .click()

            //Delete topic
            .get('div.DropdownButton')
            .click()
            .contains(' Thema löschen ')
            .click()
            .get('.btn-danger')
            .click()
    });
});