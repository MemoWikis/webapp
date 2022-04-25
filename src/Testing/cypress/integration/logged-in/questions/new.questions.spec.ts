import {startPage} from '@/pages/startpage.page';
import {loginPage} from '@/pages/login.page';

describe('New Question', () => {
    beforeEach(() => {
        startPage.visit();
        loginPage.doLoginAsTestUser();
    });

    it('New Text Question', () => {
        // eslint-disable-next-line cypress/no-unnecessary-waiting
        cy.get('#BreadcrumbLogoSmall')
            .click({force: true})
            .should('be.visible')

            .get('div.topic-name > a')
            .eq(0)
            .should('be.visible')
            .click({force: true})

            .get('#LearningTab > a')
            .click()

            //New Text Tropic
            .get('#AddQuestionFormContainer > .btn-container > .btn-link')
            .click()
            .get('select')

            .select('Text')

            .get('#QuestionInputField')
            .click()
            .type(startPage.newQuestion)

            .get('.col-sm-12 > textarea')
            .click()
            .type(startPage.newAnswer)

            .get('#EditQuestionModal > .modal-dialog > .modal-content > .modal-footer > .btn-primary')
            .click()

            .get('#SuccessModal > .modal-dialog > .modal-content > .modal-footer > .btn')
            .click();
    });
});

describe.only('New Multiple Choice Question', () => {
    beforeEach(() => {
        startPage.visit();
        loginPage.doLoginAsTestUser();
    });
    it('New Multiple Choice Question', () => {
        // eslint-disable-next-line cypress/no-unnecessary-waiting
        cy.get('#BreadcrumbLogoSmall')
            .click()
            .should('be.visible')

            .get('#LearningTab > a')
            .click()

            .get('#AddQuestionFormContainer > .btn-container > .btn-link')
            .click()
            .get('select')

            .select('MultipleChoice')
            .get('#QuestionInputField')
            .type('Was war der letzte Tag der Woche?')

            .get('#SolutionInput-0')
            .type('Wochenende')

            //Answer true
            // .get('.input-group-addon.toggle-correctness.btn.is-correct.grey-bg.active')
            // .click()
            .get(':nth-child(3) > .d-flex > .btn')
            .click()
            //Answer false
            .get('#SolutionInput-1')
            .type('Montag')

            .get('#EditQuestionModal > .modal-dialog > .modal-content > .modal-footer > .btn-primary')
            .click()

            .get('#SuccessModal > .modal-dialog > .modal-content > .modal-footer > .btn')
            .should('be.visible')
            .click();
    });
});

describe('New assignment list Question', () => {
    beforeEach(() => {
        startPage.visit();
        loginPage.doLoginAsTestUser();
    });
    it('assignment list', () => {
        // eslint-disable-next-line cypress/no-unnecessary-waiting
        cy.get('#BreadcrumbLogoSmall')
            .click()
            .should('be.visible')

            .get('#LearningTab > a')
            .click()

            .get('#AddQuestionFormContainer > .btn-container > .btn-link')
            .click()
            .get('select')

            .select('Zuordnung (Liste)')
            .get('#QuestionInputField')
            .type('Was war der letzte Tag der Woche?')

            .get('#left-0')
            .type('Wochenende')

            .get('#EditQuestionModal > .modal-dialog > .modal-content > .modal-footer > .btn-primary')
            .click()

            .get('#SuccessModal > .modal-dialog > .modal-content > .modal-footer > .btn')
            .should('be.visible')
            .click();
    });
});

describe('New index card', () => {
    beforeEach(() => {
        startPage.visit();
        console.log(startPage.topicName);
        loginPage.doLoginAsTestUser();
    });
    it('New index card', () => {
        // eslint-disable-next-line cypress/no-unnecessary-waiting
        cy.get('#BreadcrumbLogoSmall')
            .click()
            .should('be.visible')

            .get('#LearningTab > a')
            .click()

            .get('#AddQuestionFormContainer > .btn-container > .btn-link')
            .click()
            .get('select')

            .select('Karteikarte')
            .get('#QuestionInputField')
            .type('Was war der letzte Tag der Woche?')

            .get('.ProseMirror')
            .type('Wochenende')

            .get('#EditQuestionModal > .modal-dialog > .modal-content > .modal-footer > .btn-primary')
            .click()

            .get('#SuccessModal > .modal-dialog > .modal-content > .modal-footer > .btn')
            .should('be.visible')
            .click();
    });
});