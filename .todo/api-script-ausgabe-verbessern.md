# API-Script: Ausgabe beim Hochfahren verbessern

## Status: Open

## Beschreibung

Das `api.ps1`-Skript gibt während des Startens nur "Waiting for Health Check" aus, was nicht hilfreich ist. Stattdessen soll der .NET-Output (wie im API-Log) direkt in der Konsole sichtbar sein.

## Anforderungen

- Während des Hochfahrens den dotnet-Output live in der Konsole anzeigen
- Fehlermeldungen sichtbar machen, falls etwas beim Start schiefgeht
- Inhalt von `.api.log.out` / `.api.log.err` streamen oder direkt anzeigen

## Aufgaben

1. `api.ps1` analysieren (aktuelle Start-Logik)
2. Output-Streaming während der Health-Check-Wartezeit einbauen
3. Bei Fehlschlag die relevanten Log-Zeilen ausgeben
