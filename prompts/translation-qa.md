# Translation Quality Assurance

## Copilot Prompts for Translation Review

### Quick Consistency Check Prompt

```
Review the following German translations for memoWikis platform. 

Check against #translation-glossary and #translation-instructions:

1. Terminology consistency (especially Seite/Unterseite/eingeschlossen)
2. UI text appropriateness (length, clarity, tone)
3. "Du" form usage consistency
4. Action language (imperative forms)

Highlight any inconsistencies and suggest improvements.

[Paste translation content here]
```

### Comprehensive Translation Review Prompt

```
Perform a comprehensive review of these German UI translations:

Reference guides:
- #translation-glossary for terminology standards
- #translation-instructions for style guidelines

Review criteria:
1. **Terminology Consistency**: Check all page/content hierarchy terms
2. **User Address**: Verify consistent "Du" usage
3. **UI Suitability**: Ensure text fits interface context
4. **Clarity**: Confirm meaning is clear and unambiguous
5. **Tone**: Maintain friendly, encouraging platform voice

Provide:
- List of inconsistencies found
- Specific correction suggestions
- Overall consistency score (1-10)

[Paste translation content here]
```

### Bulk Translation Prompt

```
Translate the following UI text for German memoWikis platform.

Guidelines:
- Follow #translation-glossary for consistent terminology
- Apply #translation-instructions style rules
- Use "Du" form consistently
- Keep text concise and action-oriented
- Maintain friendly, encouraging tone

Context: [Web UI / Mobile / Analytics / etc.]

[Paste English source text here]
```

### Specific Issue Resolution Prompt

```
Fix terminology inconsistencies in German translations:

Focus on:
- Unterseite vs eingeschlossene Seiten usage
- Parent/child page relationship terms
- Analytics label consistency

Reference: #translation-glossary

[Paste problematic translations here]
```

## Usage Instructions

1. **Copy appropriate prompt** based on your needs
2. **Add your translation content** where indicated
3. **Reference the guides** by including the hashtags
4. **Run the prompt** with your AI assistant

## Integration with Development Workflow

### Pre-commit Hook (Optional)
Consider adding translation consistency checks to your development process by referencing these prompts in code review guidelines.

### Documentation Updates
When adding new translations:
1. Check against existing glossary
2. Update glossary if new terms are needed
3. Ensure consistency across related translations
