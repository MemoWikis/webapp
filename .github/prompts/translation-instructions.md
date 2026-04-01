# Translation Instructions - Copilot Shared Prompts

## Core Translation Guidelines

### Language Standards
You are translating for the German memoWikis platform. Follow these principles:

1. **Consistent Terminology**: Always reference `#translation-glossary` for established terms
2. **User Address**: Use consistent "Du" (informal) addressing throughout
3. **Tone**: Keep language friendly, encouraging, and clear
4. **Brevity**: Prefer concise, action-oriented text suitable for web UI

### Key Translation Rules

#### 1. Page Structure Terminology
```
✅ CORRECT:
- Unterseite = subpage (direct child page)
- eingeschlossene Seiten = included pages (all pages in hierarchy)
- Übergeordnete Seite = parent page

❌ AVOID:
- Using "eingeschlossene Seiten" when you mean "Unterseiten"
- Inconsistent terminology for the same concept
```

#### 2. UI Text Best Practices
- **Actions**: Use imperative form ("Erstellen", "Bearbeiten", "Hinzufügen")
- **Labels**: Be descriptive but concise
- **Error Messages**: Be helpful and encouraging
- **Success Messages**: Be positive and motivating

#### 3. Context-Aware Translation

**Analytics/Statistics Context:**
- Use precise terminology distinguishing between direct and included content
- "Direkt verknüpfte" vs "Eingeschlossene" indicates scope level

**Navigation Context:**
- Maintain clear hierarchy language
- Use established parent/child terminology

### Translation Workflow

1. **Check Glossary**: Reference existing terminology in `#translation-glossary`
2. **Maintain Consistency**: Ensure new translations align with existing patterns
3. **Context Awareness**: Consider where the text appears (button, label, description, etc.)
4. **User Experience**: Prioritize clarity and usability

### Validation Checklist

Before finalizing translations:
- [ ] Terminology matches glossary
- [ ] Tone is consistent with platform voice
- [ ] Text length is appropriate for UI element
- [ ] Related terms use consistent patterns
- [ ] No ambiguous or confusing language

### Examples

**Good Translation Patterns:**
```json
"createChildPage": "Unterseite erstellen",
"linkChildPage": "Unterseite verknüpfen",
"childPageCount": "Eingeschlossene Unterseiten",
"directVisibleChildPageLabel": "Direkt verknüpfte Unterseiten"
```

**Error to Fix:**
```json
// ❌ Inconsistent
"someLabel": "eingeschlossene Seiten", 
"anotherLabel": "Unterseiten" // when referring to same concept

// ✅ Consistent  
"directChildPages": "Direkt verknüpfte Unterseiten",
"allIncludedPages": "Eingeschlossene Seiten"
```

## Usage

Reference this guide with `#translation-instructions` in your prompts when working on German translations for memoWikis.
