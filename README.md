# Open Invoice Manager

Ein Rechnungsprogramm das ich als Azubi-Projekt entwickle. Damit kann man Kunden verwalten und Rechnungen erstellen und als PDF speichern.

## Was das Programm kann

### Kundenverwaltung
- Kunden anlegen, bearbeiten und löschen
- Kundendaten speichern (Name, Adresse, Email, Telefon)
- Alle Kunden in einer Übersicht sehen
- Nach Kunden suchen

### Rechnungen
- Neue Rechnungen erstellen
- Rechnungsnummer wird automatisch vergeben
- Kunde aus der Liste auswählen
- Mehrere Positionen hinzufügen
- Zwischensumme, MwSt. und Gesamtbetrag wird berechnet
- Zahlungsziel festlegen

### Artikel / Dienstleistungen
- Artikel anlegen mit Preis und Beschreibung
- Bei Rechnungserstellung schnell auswählen

### PDF Export
- Rechnung als PDF speichern
- Mit Firmendaten und Logo
- Kann dann per Mail verschickt werden

### Rechnungsübersicht
- Alle Rechnungen anzeigen
- Status: Offen, Bezahlt, Überfällig
- Filtern und suchen

### Einstellungen
- Firmendaten hinterlegen
- Bankdaten
- MwSt. Satz einstellen

---

## Technologie

| Was | Womit |
|-----|-------|
| Programmiersprache | C# (.NET 8) |
| UI Framework | WPF (Windows Presentation Foundation) |
| Architektur | Model-View-Presenter (MVP) |
| Datenbank | SQLite |
| PDF Erstellung | PdfSharp |
| ORM | kein ORM, direkte SQL Abfragen mit SQLite |

## Projektstruktur (MVP)

```
OpenInvoiceManager/
│
├── Models/               # Datenklassen z.B. Customer, Invoice, Article
├── Views/                # WPF Windows und UserControls (.xaml)
├── Presenters/           # Logik zwischen View und Model
├── Database/             # SQLite Datenbankzugriff
├── Services/             # PDF Export usw.
└── Resources/            # Icons, Bilder
```

### Wie MVP hier funktioniert

- **Model** – einfache Klassen die Daten halten, z.B. `Customer.cs`
- **View** – das WPF Fenster, macht nur UI-Sachen, keine Logik
- **Presenter** – bekommt Events von der View, holt Daten aus dem Model und sagt der View was sie anzeigen soll

Beispiel:
```
CustomerView (WPF Window)
    --> klick auf "Speichern"
CustomerPresenter
    --> validiert Eingabe
    --> ruft CustomerRepository.Save() auf
    --> sagt View "Kunde wurde gespeichert"
```

---

## Installation / Starten

### Voraussetzungen
- Windows 10 oder neuer
- .NET 8 SDK ([download hier](https://dotnet.microsoft.com/download))
- Visual Studio 2022 (Community reicht)

### Schritte

1. Repository klonen
```bash
git clone https://github.com/deinname/openinvoicemanager.git
```

2. In Visual Studio öffnen (`OpenInvoiceManager.sln`)

3. NuGet Pakete werden automatisch geladen, sonst:
```bash
dotnet restore
```

4. Starten mit F5 oder:
```bash
dotnet run
```

Die SQLite Datenbank wird beim ersten Start automatisch angelegt.

---

## NuGet Pakete die ich benutze

- `Microsoft.Data.Sqlite` – für die SQLite Datenbank
- `PdfSharp` – für PDF Erstellung

---

## Bekannte Probleme / TODO

- [ ] PDF Layout noch nicht fertig
- [ ] Fehlerbehandlung muss noch verbessert werden
- [ ] Tests fehlen noch komplett
- [ ] Suche funktioniert noch nicht überall

---

## Lizenz

MIT License – kann jeder benutzen

## Hinweis

Das ist ein Lernprojekt. Der Code ist nicht perfekt aber er funktioniert. Verbesserungsvorschläge gerne als Issue oder Pull Request.
