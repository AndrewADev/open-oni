/// <reference types="cypress" />

context('Newspapers', () => {
  it('can visit the Newspapers page', () => {
    cy.visit('');

    cy.contains('Newspapers').click();

    // Bit of coupling here - to DOM and data
    // TODO: bad - probably too much coupling
    cy.get('.search_results_body').contains('Publication Info');

    cy.get('.search_results_body').contains('Nordstern'); // TODO: coupled to data
  });
});
