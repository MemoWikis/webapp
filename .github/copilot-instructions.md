# Common code style guide for all languages

- Please always write code comments in English.
- Always spell out variable names; no abbreviations.
- After if statements and loops, never use single line statements.

# C#

- Do not use namespaces in C#.
- I prefer Verify() for Tests
- Use _testHarness.ApiCall("apiVue/{controller}/{action}") to do apicalls, do not init/resolve controllers in tests
