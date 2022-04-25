import {startPage} from '@/pages/startpage.page';

describe('Check seo best practice', () => {
    beforeEach(() => {
        startPage.visit();
    });

    it('Only is one H1 on the page', () => {
        cy.get('h1').should('have.length', 2);
    });
});
