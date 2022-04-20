import {startPage} from '@/pages/startpage.page';
import {loginPage} from '@/pages/login.page';

describe('Edit topic', () => {
    beforeEach(() => {
        startPage.visit();
        loginPage.doLoginAsTestUser();
        console.log(startPage.topicName);
    });
    it('Edit topic', () => {

        //Press topic button
        // eslint-disable-next-line cypress/no-unnecessary-waiting
        cy.get('#BreadcrumbLogoSmall')
            .click()
            .should('be.visible')

            .get('div.topic-name > a')
            .eq(0)
            .click()

            //Editor open
            .get('#InlineEdit')
            .click()
            .type(startPage.whatIsNew, {delay: 200, force: true})

            //Click save button
            .get('div.btn-label')
            .eq(0)
            .click()
    });
});
