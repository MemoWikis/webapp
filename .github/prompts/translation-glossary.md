# Translation Glossary - German (DE)

## Reference Guide for Consistent Translation

This glossary ensures consistent terminology across all German translations in the memoWikis platform.

## Core Terminology

### Pages & Structure
| German | Consistent Translation | Notes |
|--------|----------------------|-------|
| Seite | page | Never use "Webseite" |
| Unterseite | subpage | **NOT** "eingeschlossene Seite" |
| Übergeordnete Seite | parent page | |
| Hauptseite | main page | |
| eingeschlossen | included | Used for questions/content within pages |
| direkt verknüpft | directly linked | |

### Content & Learning
| German | Consistent Translation | Notes |
|--------|----------------------|-------|
| Frage | question | |
| Karteikarte | flashcard | |
| Wissen | knowledge | |
| Lernen | learning | |
| Wissensstand | knowledge status | |
| Lernaktivität | learning activity | |
| Wunschwissen | wishknowledge | |

### User Interface
| German | Consistent Translation | Notes |
|--------|----------------------|-------|
| Aufrufe | views | For page/question views |
| Seitenaufrufe | page views | |
| Fragenaufrufe | question views | |
| Nutzer | user | Use "Nutzer", not "Benutzer" |

### Actions & Operations
| German | Consistent Translation | Notes |
|--------|----------------------|-------|
| erstellen | create | |
| bearbeiten | edit | |
| verknüpfen | link | |
| hinzufügen | add | |
| entfernen | remove | |
| löschen | delete | |
| veröffentlichen | publish | |
| privat stellen | set to private | |

## Problematic Terms to Avoid

❌ **Inconsistent usage to fix:**
- "eingeschlossene Seiten" when referring to subpages → use "Unterseiten"
- Mixed usage of "Unterseiten" vs "eingeschlossene Seiten" for the same concept

✅ **Correct usage:**
- **Unterseiten** = direct child pages/subpages
- **eingeschlossene Seiten/Fragen** = all content included within a page hierarchy (including subpages and their content)

## Context-Specific Guidelines

### Analytics Section
- **directVisibleChildPageLabel**: "Direkt verknüpfte Unterseiten" 
- **childPageCount**: "Eingeschlossene Unterseiten" (when counting all nested levels)
- **subpageViewsLabel**: "Unterseiten" (short form in UI labels)

### Page Relations
- Parent-child relationships always use "Übergeordnet/Untergeordnet"
- Direct links use "direkt verknüpft"
- Inclusion hierarchy uses "eingeschlossen"

## References
Use `#translation-glossary` to reference this guide in prompts and documentation.
