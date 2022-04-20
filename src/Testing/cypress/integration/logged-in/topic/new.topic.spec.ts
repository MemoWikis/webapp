import {startPage} from '@/pages/startpage.page';
import {loginPage} from '@/pages/login.page';

const topicNameRandom = startPage.topicName + (Math.floor(Math.random() * 10000) + 1);
console.log(topicNameRandom);
describe('new topic', () => {
    beforeEach(() => {
        startPage.visit();
        //cy.get('.stageOverlayCloseButton').click();
        console.log(startPage.topicName);
        loginPage.doLoginAsTestUser();
    });

    it('new topic', () => {
        // eslint-disable-next-line cypress/no-unnecessary-waiting
        cy.get('#BreadcrumbLogoSmall')
            .click()
            .should('be.visible')

            .get('#AddToCurrentCategoryCard > div.col-xs-9.addCategoryLabelContainer > :nth-child(1)')
            .click()
            .get('.form-group > input')
            .type(topicNameRandom, {force: true, delay: 100})
            .should('have.value', topicNameRandom)

            .get('#AddNewCategoryBtn')
            .click()
    });
});
