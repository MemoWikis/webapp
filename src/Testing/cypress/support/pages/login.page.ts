import BasePage from './base.page';

export class LoginPage extends BasePage {
  constructor() {
    super();
    this.path = '/';
  }

  doLoginAsTestUser() {
    //cy.get('.stageOverlayCloseButton').click();
    cy.contains(' Anmelden ').should('be.visible').click({force: true});
    this.inputUsername.type(`${Cypress.env('userName')}`);
    this.inputPassword.type(`${Cypress.env('password')}`);
    cy.get('.row > .col-xs-12 > .btn').click();
    cy.request('/');

  }

  get inputUsername() {
    return cy.get('input[name=login]');
  }

  get inputPassword() {
    return cy.get('input[name=password]');
  }
}

export const loginPage = new LoginPage();
