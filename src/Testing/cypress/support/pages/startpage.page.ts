import BasePage from './base.page';

export class StartPageClass extends BasePage {
    constructor() {
        super();
        this.path = '/';
    }
}

export const startPage = new StartPageClass();
