# Bilder: KI-Optionen einbauen

## Status: Open

## Beschreibung

KI-basierte Bildgenerierung als Option anbieten und Bilder in den Tree aufnehmen.

## Anforderungen

- KI-Option für Bildgenerierung einbauen
- 2–5 Style-Optionen anbieten (z. B. Icon-Styles: Gemini, Banana, etc.)
- Token-Budget klären (eigenes Budget? Gemini? → teuer)
- Limitierung einführen:
  - **Free-User:** max. 3 Bilder pro Monat
  - **Reguläre User:** max. 10 Bilder pro Monat
- Bilder anschließend in den Page-Tree aufnehmen

## Offene Fragen

- Welcher Bildgenerierungs-Provider? (Kosten vs. Qualität)
- Eigenes Token-Budget oder shared?
- Priorität: erst später implementieren?

## Aufgaben

1. Provider evaluieren und Kosten kalkulieren
2. Backend: Bildgenerierung-Endpunkt + Limitierung implementieren
3. Frontend: Style-Auswahl-UI bauen
4. Bilder in den Tree integrieren
