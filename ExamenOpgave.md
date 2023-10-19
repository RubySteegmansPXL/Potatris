# Examenopgave Unity

Voor dit examen maken jullie Tetris na. Het spel voldoet aan onderstaande minimumvereisten:

* Je voorziet voldoende polish: je geeft aandacht aan geluid, visuele effecten en 'game feel'.
* Het spel is voorzien van een titelscherm.
* Het spel is voorzien van een tutorial die een nieuwe speler leert hoe je tetris moet spelen.
* Het spel ondersteunt minstens 2 talen. Daarvoor voorzie je een localization framework (zie hieronder).
* De speler kan zichzelf kort identificeren, voor of na een spel, zodat de score netjes wordt opgeslagen.
* Je voorziet minstens 3 sterren aan uitbreiding (zie hieronder).

## Localization Framework
Ons fictief spelletje Tetris moet verschillende talen ondersteunen. Om het het vertaalbureau gemakkelijk te maken willen we hun een excel, google sheet, of dergelijke geven. Je mag zelf nadenken over een goede oplossing die toepasselijk is voor personen met weinig technische kennis. Geef bijvoorbeeld geen .json file aan een vertaalbureau!
Het document dat ingevuld wordt door het vertaalbureau moet worden ingelezen door het spel (dat moet niet in zijn ruwe vorm gebeuren at runtime)

## Uitbreidingen
Je kiest uit onderstaande lijst minstens voor 3 sterren aan uitbreidingen:

* De lijn staat in het midden en de blokken komen alternerend van boven en beneden. (1 ster)
* Je voorziet een lijst van 10 challenges die de speler kan halen. De lijst van challenges is raadpleegbaar via het titelscherm. Het behalen van challenges moet persistent zijn. (1 ster)
* Je voorziet minstens 3 modifiers die de speler kan aan- of uitzetten. Bijvoorbeeld: de volgende blok is steeds onzichtbaar, geen lange blokken meer, een blok kan meer 2x gedraaid worden, etc. (1 ster)
* Het spel is volledig speelbaar op Android (2 sterren)
* De verschillende blokken in het spel zijn data-assets (scriptable objects) in je Unity project. Je voorziet een interne tool in Unity zodat de designer extra blokken kan aanmaken of bestaande blokken kan wijzigen. (2 sterren) 
* Het spel kan gespeeld worden met 2 spelers. Elke keer als een speler een lijn vormt, krijgt de andere speler een nadelig effect (bijvoorbeeld een zeer bizar blokje, distortie op het scherm, tijdelijke verhoging van de snelheid). (2 sterren)

## Indienen
Je deelt je repository met sam.agten@pxl.be. Deze opgave zet je mee in de repository. Je deelt ook een build van het spel (.exe of .apk).

## Beoordeling
Je wordt beoordeeld op:

* Het voldoen aan de minimumvereisten
* De graad van polish
* De codekwaliteit en architectuur van het localization framework.
