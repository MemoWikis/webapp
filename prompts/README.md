# Translation Guidelines & Prompts

This directory contains shared prompts and guidelines for consistent German translations in the memoWikis platform.

## Files Overview

### Core Guidelines
- **`translation-glossary.md`** - Comprehensive terminology reference
- **`translation-instructions.md`** - Copilot shared prompts for consistent translations
- **`translation-qa.md`** - Quality assurance prompts for translation review

### UI Text Guidelines
- **`ui-text-de.md`** - Original UI text improvement prompts
- **`ui-text-de-updated.md`** - Enhanced version with reference system

## Quick Start

### For New Translations
1. Reference `#translation-glossary` for consistent terminology
2. Use prompts from `#translation-instructions` 
3. Apply guidelines from `#translation-qa` for review

### Common Issues to Fix
Based on your current translations, focus on:

**Inconsistent terminology:**
```json
// ❌ Problem - mixed usage
"childPageCount": "Eingeschlossene Unterseiten",
"directVisibleChildPageLabel": "Direkt verknüpfte Unterseiten", 
"subpageViewsLabel": "Unterseiten"
```

**Correct distinction:**
- **Unterseiten** = direct child pages
- **Eingeschlossene Seiten/Unterseiten** = all pages in hierarchy
- Use context to determine which is appropriate

## Usage Examples

### Terminology Check Prompt
```
Review this German translation against #translation-glossary:

"childPageCount": "Eingeschlossene Unterseiten",
"subpageViewsLabel": "Unterseiten"

Are these terms used consistently?
```

### Bulk Translation Prompt  
```
Translate these UI labels using #translation-instructions and #translation-glossary:

- "Included pages"
- "Direct subpages" 
- "Parent page views"
```

### Consistency Review Prompt
```
Using #translation-qa prompts, review this analytics section for terminology consistency:

[paste your analytics translations]
```

## Integration

### In Development
- Reference these guides when adding new translations
- Use hashtag system (`#translation-glossary`) in prompts
- Run QA checks before committing translation updates

### In Documentation
- Link to specific guidelines when documenting translation decisions
- Update glossary when new terms are established
- Use shared prompts for consistent AI-assisted translations

## Contributing

When updating translations:
1. Check existing terminology in glossary
2. Update glossary if adding new concepts
3. Ensure consistency across related translations
4. Use QA prompts to validate changes
