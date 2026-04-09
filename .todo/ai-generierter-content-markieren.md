# AI-generierter Content muss markiert sein

## Status: Open

## Beschreibung

Content, der von einer KI generiert wurde, muss als solcher gekennzeichnet werden.

## Anforderungen

- In der Datenbank ein Feld/Marking erstellen
- Verschiedene Abstufungen (Level/Modi):
  - **AI-Generated**: Vollständig von KI generiert
  - **AI-Based**: Auf KI basierend (z. B. von KI erstellt, aber vom User bearbeitet)
  - **AI-was-part**: KI war am Erstellungsprozess beteiligt

## Aufgaben

1. Datenbankschema erweitern (Spalte/Enum für AI-Level)
2. Migration-Step erstellen
3. Backend: Modell und Persistenz anpassen
4. Frontend: Markierung im UI anzeigen
