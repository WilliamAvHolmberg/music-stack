DATAMODELL:

1. Songs
```
- Id: unique identifier
- Title: låttitel
- Artist: artist/band
- FirstLine: första raden (för "Första Raden"-rundan)
- Year: utgivningsår
- Difficulty: 1-3
- Category: enum (Pop, Rock, Schlager, etc)
- Language: enum (Swedish, English)
```

2. GameSession
```
- Id: unique identifier
- CreatedAt: timestamp
- Status: enum (Created, InProgress, Finished)
- CurrentRoundNumber: int
- CurrentTeamIndex: int
```

3. Team
```
- Id: unique identifier
- GameSessionId: foreign key
- Name: string
- Score: int
- Color: string (för visuell identifiering)
```

4. Round
```
- Id: unique identifier
- GameSessionId: foreign key
- Type: enum (GuessTheMelody, FirstLine, ArtistQuiz)
- Status: enum (NotStarted, InProgress, Completed)
- CurrentSongId: foreign key (nullable)
- TimeLeft: int (sekunder)
```

SPELFLÖDE:

1. Startsida
```
- Knapp: "Starta nytt spel"
- Knapp: "Spelregler"
- (Optional) Highscore-lista från tidigare spel
```

2. Spelkonfiguration
```
- Ange antal lag (2-4)
- För varje lag:
  * Namn
  * Välja färg
- Välj antal rundor (3-5)
- Välj svårighetsgrad (Easy, Medium, Hard)
```

3. Spelledarvy
```
- Översikt:
  * Aktuell runda
  * Poängställning
  * Timer när relevant
- Kontroller:
  * Nästa runda
  * Dela ut poäng
  * Pausa/Återuppta timer
  * Visa/Dölj ledtrådar
```

4. Publikvy (huvudskärm)
```
- Alltid synligt:
  * Poängställning
  * Aktuell runda
  * Timer när relevant
- Runspecifikt innehåll:
  * Första raden-text
  * Artistquiz-ledtrådar
  * "Gissa Melodin"-indikator
```

RUNDSPECIFIKATIONER:

1. Gissa Melodin
```
Spelledarvy:
- Visa: Låttitel + Artist
- Kontroller: Start/Stopp timer
- Poängknappar: +3 för rätt gissning

Publikvy:
- Visa: "Runda 1: Gissa Melodin"
- Timer: 30 sekunder
- Lagindikator: Visar vilket lag som gissar
```

2. Första Raden
```
Spelledarvy:
- Visa: Hela låttexten (första 4 rader)
- Kontroller: Visa nästa rad
- Poängknappar: +2 per korrekt rad

Publikvy:
- Visa: Aktuell textrad
- Indikator: Vilket lag som ska fortsätta
```

3. Artistquiz
```
Spelledarvy:
- Visa: Artist + alla ledtrådar
- Kontroller: Visa nästa ledtråd
- Timer: 30 sekunder
- Poängknappar: +3/+2/+1 beroende på antal ledtrådar

Publikvy:
- Visa: Aktuella ledtrådar
- Timer
- Poängskala (3,2,1)
```

TEKNISKA KRAV:

1. State Management
```
- Speltillstånd ska vara realtid
- Alla lag ska se samma information samtidigt
- Automatisk uppdatering av poäng/timer
```

2. Error Handling
```
- Hantera tappade anslutningar
- Möjlighet att återuppta spel
- Validering av poängutdelning
```

3. Prestanda
```
- Snabb laddning av nya rundor
- Minimal latency för poänguppdateringar
- Optimerad för mobil/tablet för spelledare
```

PRIORITERINGSORDNING FÖR IMPLEMENTATION:

1. Fas 1 - Core
```
- Grundläggande datamodell
- Skapa spel
- Lägga till lag
- Basal poänghantering
```

2. Fas 2 - Rundor
```
- Implementera Gissa Melodin
- Implementera Första Raden
- Implementera Artistquiz
- Timer-funktionalitet
```

3. Fas 3 - Polish
```
- Förbättrad UI/UX
- Animationer
- Ljudeffekter
- Statistik/highscore
```

Vill du att jag utvecklar någon del ytterligare eller ska vi börja med implementationen av någon specifik del?