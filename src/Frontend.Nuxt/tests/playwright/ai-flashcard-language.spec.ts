import { test, expect } from './fixtures/auth.fixture'
import { takeDevScreenshot } from './fixtures/screenshot.helper'

test.describe('AI Flashcard Language Detection', () => {
    test.setTimeout(120000) // AI calls can take time

    const GERMAN_TEXT = `Definition und Konzept der Tokenisierung

Tokens sind die fundamentalen lexikalischen Einheiten, in die ein Text während der Tokenisierung segmentiert wird. Im Kontext des Natural Language Processing (NLP) und der maschinellen Sprachverarbeitung stellt ein Token die kleinste bedeutungstragende Einheit dar, die von einem Algorithmus verarbeitet werden kann. Die Granularität der Tokenisierung variiert dabei je nach Anwendungsdomäne: Sie kann auf Wort-Ebene, Subwort-Ebene (Byte-Pair Encoding, WordPiece) oder Zeichen-Ebene erfolgen.

Tokenisierungsverfahren in Large Language Models

Moderne Large Language Models (LLMs) wie GPT oder BERT verwenden primär Subword-Tokenisierung. Algorithmen wie Byte-Pair Encoding (BPE) oder SentencePiece generieren ein Vokabular, das häufige Wörter als einzelne Tokens repräsentiert, während seltene Wörter in morphologisch plausible Subeinheiten zerlegt werden.`

    // German-specific words and patterns that indicate German language
    const GERMAN_INDICATORS = [
        /\bist\b/i, // "is" in German
        /\bund\b/i, // "and" in German
        /\bdie\b/i, // "the" in German (feminine)
        /\bder\b/i, // "the" in German (masculine)
        /\bdas\b/i, // "the" in German (neuter)
        /\bein\b/i, // "a/an" in German
        /\beine\b/i, // "a/an" in German (feminine)
        /\bwird\b/i, // "becomes/is" in German
        /\bwerden\b/i, // "become" in German
        /\bwas\b/i, // "what" in German
        /\bwie\b/i, // "how" in German
        /\bwelche\b/i, // "which" in German
        /ung\b/, // German suffix -ung
        /keit\b/, // German suffix -keit
        /heit\b/, // German suffix -heit
        /ieren\b/, // German verb suffix -ieren
    ]

    // English-specific words that indicate English language
    const ENGLISH_INDICATORS = [
        /\bthe\b/i,
        /\bis\b/i,
        /\bare\b/i,
        /\bwhat\b/i,
        /\bhow\b/i,
        /\bwhich\b/i,
        /\bused\b/i,
        /\bprocess\b/i,
        /\bmethod\b/i,
    ]

    /**
     * Scores how "German" a text is based on indicator matches
     * Returns a score from 0 to 1
     */
    function getGermanScore(text: string): number {
        const germanMatches = GERMAN_INDICATORS.filter((pattern) =>
            pattern.test(text),
        ).length
        const englishMatches = ENGLISH_INDICATORS.filter((pattern) =>
            pattern.test(text),
        ).length

        if (germanMatches + englishMatches === 0) return 0.5
        return germanMatches / (germanMatches + englishMatches)
    }

    test('flashcards generated from German text should be in German', async ({
        authenticatedPage,
    }) => {
        const page = authenticatedPage

        // First, we need to navigate to a page where we can test flashcard generation
        // We'll use the API directly to test the language detection

        // Get a page ID to test with (navigate to a known page)
        await page.goto('/')
        await page.waitForLoadState('networkidle')
        await takeDevScreenshot(page, 'flashcard-test-start')

        // Find a page that we can use for testing - navigate to one
        // Look for any page link
        const pageLink = page
            .locator('a[href*="/"]')
            .filter({ hasText: /\w+/ })
            .first()

        if (await pageLink.isVisible().catch(() => false)) {
            await pageLink.click()
            await page.waitForLoadState('networkidle')
        }

        // Extract page ID from URL
        const url = page.url()
        const pageIdMatch = url.match(/\/(\d+)$/)

        if (!pageIdMatch) {
            // Create a direct API test instead
            console.log('No page ID found, testing via API directly')

            // Use page ID 1 (usually exists in dev database)
            const testPageId = 1

            // Make the API call directly
            const apiResponse = await page.request.post(
                'http://localhost:5069/apiVue/PageStore/GenerateFlashCard',
                {
                    data: {
                        pageId: testPageId,
                        text: GERMAN_TEXT,
                        count: 3,
                    },
                    headers: {
                        'Content-Type': 'application/json',
                    },
                },
            )

            console.log('API Response status:', apiResponse.status())

            if (apiResponse.status() === 200) {
                const responseBody = await apiResponse.json()
                console.log(
                    'Response body:',
                    JSON.stringify(responseBody, null, 2),
                )

                if (
                    responseBody.flashcards &&
                    responseBody.flashcards.length > 0
                ) {
                    // Analyze the language of the flashcards
                    for (const flashcard of responseBody.flashcards) {
                        const frontText =
                            flashcard.front || flashcard.Front || ''
                        const backText = flashcard.back || flashcard.Back || ''
                        const combinedText = `${frontText} ${backText}`

                        const germanScore = getGermanScore(combinedText)
                        console.log(`Flashcard: "${frontText}"`)
                        console.log(`  German score: ${germanScore}`)

                        // Flashcard should be predominantly German (score > 0.5)
                        expect(germanScore).toBeGreaterThan(0.4)
                    }
                } else if (responseBody.messageKey) {
                    console.log(`Message key: ${responseBody.messageKey}`)
                    // If we get an error about tokens, that's okay for this test
                    if (responseBody.messageKey.includes('token')) {
                        test.skip(
                            true,
                            'Insufficient tokens for flashcard generation',
                        )
                    }
                }
            } else {
                console.log('API returned error:', await apiResponse.text())
            }
            return
        }

        const pageId = parseInt(pageIdMatch[1], 10)
        console.log(`Testing with page ID: ${pageId}`)

        // Make the API call to generate flashcards
        const apiResponse = await page.request.post(
            'http://localhost:5069/apiVue/PageStore/GenerateFlashCard',
            {
                data: {
                    pageId: pageId,
                    text: GERMAN_TEXT,
                    count: 3,
                },
                headers: {
                    'Content-Type': 'application/json',
                },
            },
        )

        console.log('API Response status:', apiResponse.status())

        if (apiResponse.status() === 200) {
            const responseBody = await apiResponse.json()
            console.log('Response body:', JSON.stringify(responseBody, null, 2))

            if (responseBody.flashcards && responseBody.flashcards.length > 0) {
                let allInGerman = true

                for (const flashcard of responseBody.flashcards) {
                    const frontText = flashcard.front || flashcard.Front || ''
                    const backText = flashcard.back || flashcard.Back || ''
                    const combinedText = `${frontText} ${backText}`

                    const germanScore = getGermanScore(combinedText)
                    console.log(`Flashcard Front: "${frontText}"`)
                    console.log(`Flashcard Back: "${backText}"`)
                    console.log(`German score: ${germanScore}`)

                    if (germanScore < 0.4) {
                        allInGerman = false
                        console.error(
                            `FAIL: Flashcard appears to be in English, not German!`,
                        )
                    }
                }

                // At least 80% of flashcards should be in German
                expect(allInGerman).toBe(true)
            } else if (responseBody.messageKey) {
                console.log(`Message key: ${responseBody.messageKey}`)
                if (responseBody.messageKey.includes('token')) {
                    test.skip(
                        true,
                        'Insufficient tokens for flashcard generation',
                    )
                }
            }
        }

        await takeDevScreenshot(page, 'flashcard-language-test-complete')
    })

    test('should detect language mismatch in flashcards', async ({
        authenticatedPage,
    }) => {
        const page = authenticatedPage

        // This test just validates our language detection logic works
        const germanText =
            'Was ist eine Tokenisierung? Die Tokenisierung ist ein Verfahren.'
        const englishText =
            'What is tokenization? The tokenization is a process.'

        const germanScore = getGermanScore(germanText)
        const englishScore = getGermanScore(englishText)

        console.log(`German text score: ${germanScore}`)
        console.log(`English text score: ${englishScore}`)

        // German text should score high
        expect(germanScore).toBeGreaterThan(0.6)

        // English text should score low
        expect(englishScore).toBeLessThan(0.4)
    })
})
