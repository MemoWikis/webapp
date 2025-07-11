# Common code style guide for all languages

- Please always write code comments in English.
- Always spell out variable names; no abbreviations.
- After if statements and loops, never use single line statements.

# C#

- Do not use namespaces in C#.
- I prefer Verify() for Tests
- Use _testHarness.ApiCall("apiVue/{controller}/{action}") to do apicalls, do not init/resolve controllers in tests

# Unit Tests

- For API calls use Testharness.ApiCall(..)
# Glossar

- `page`: a page
- `childpage`: a page that is a child of page or wiki
- `subpage`: subpage is a synonym for childpage and only used in user interface
- `wiki`: is a special kind of a page
- `orphaned page`: a page that does not have a parent page or wiki

# Frontend and Translations
- refer to the .copilotinstructions file in `../src/Frontend.Nuxt` for frontend specific instructions

