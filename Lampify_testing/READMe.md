Project Testing & Security 2024-2025 2e jaarsvak
  - De applicatie (en het testen ervan) 
  - Unit testen – TDD 
  - Integratie testen
  - Acceptatie testen

----------------------------------------------------------------------------------
    
* C# voor de backend-programmering.
* xUnit voor unit testing van individuele methoden.
* Test-Driven Development (TDD) waarbij tests eerst worden geschreven voordat de code wordt ontwikkeld.
* Behavior-Driven Development (BDD) met Gherkin voor het schrijven van testscenario's.
* Integratie- en acceptatietests om te controleren of de verschillende systeemonderdelen goed samenwerken.
* Agile methodologie voor het beheer van het project.


----------------------------------------------------------------------------------

# Lampify  Slimme Verlichting op Basis van Je Mood

Lampify is een innovatieve oplossing voor slimme verlichting, ontworpen om naadloos aan te passen aan jouw stemming en omgeving. De kernfunctionaliteiten zijn:

    Het in- en uitschakelen van de lamp.
    Aanpassen van helderheid en kleur.
    Overschakelen naar een veilige modus bij meerdere fouten.
    Automatische aanpassing op basis van omgevingslichtsterkte en stemming (Mood

    ## Interfaces
### ILightSensorApi

    Methoden: GetLightIntensity(): Retourneert de lichtsterkte (in lux).

### ILamp

    Methoden:
        TurnOn(): Schakelt de lamp in.
        TurnOff(): Schakelt de lamp uit.
        SetBrightness(int brightness): Stelt de helderheid in (0-100).
        SetColor(string color): Stelt de kleur in.
    Eigenschappen:
        IsOn { get; }: Geeft de aan/uit-status van de lamp.
        Brightness: Huidige helderheid.

## Klassen
### Lamp

    Verantwoordelijkheden: Houdt de status van de lamp bij, zoals IsOn, Brightness en Color.
    Methoden: TurnOn(), TurnOff(), SetBrightness(int brightness), SetColor(string color).
    Eigenschappen:
        Brightness: Regelt de helderheid van de lamp.
# 
### LampController

    Verantwoordelijkheden: Beheert de lamp en implementeert de ILamp-interface.
    Functionaliteiten:
        ApplySettings(int brightness, string color): Past de gewenste instellingen toe.
        AdjustLighting: Past de lamp automatisch aan op basis van stemming en omgevingslicht.
        EnterSafeMode: Schakelt naar een veilige modus bij foutieve werking.
        ToggleLamp: Wisselt tussen aan- en uitgeschakelde status.

### LightSensorApi

    Verantwoordelijkheden: Verzamelt omgevingslichtsterkte via een API.
    Methoden: GetLightIntensity(): Retourneert de lichtsterkte.

### LampStub

    Doel: Een testversie van de ILamp interface, ontworpen voor unit tests. Hiermee wordt de fysieke lamp gesimuleerd.

### Enum Mood
Vooraf ingestelde stemmingen:

    Cozy: Helderheid 50, kleur rood. Ideaal voor een gezellige sfeer.
    Angry: Helderheid 100, kleur rood. Voor intense emoties.
    Bright: Helderheid 100, kleur wit. Voor een heldere, productieve omgeving.
    Dark: De lamp wordt uitgeschakeld voor volledige duisternis.

## Extra Functionaliteit
### Automatische Lichtaanpassing

De lamp past zich automatisch aan op basis van:

    Omgevingslichtsterkte: Bij weinig licht schakelt de lamp zichzelf in.
    Mood: Biedt een persoonlijke verlichtingservaring op basis van je stemming.

Veilige Modus

Bij meerdere fouten (zoals communicatieproblemen met de lichtsensor) schakelt het systeem over naar een veilige modus waarin de lamp wordt uitgeschakeld en verdere actie wordt gelogd.

## Teststrategie

Om de functionaliteit te valideren, worden verschillende testsoorten ingezet:

    Unit Tests:
        Verifieer correcte helderheid en kleurinstellingen op basis van Mood.
        Test de toggles tussen aan/uit.
    Integration Tests:
        Controleer de samenwerking tussen LampController en LightSensorApi.
    Acceptance Tests:
        Verifieer eindgebruikersfunctionaliteit, zoals automatische aanpassingen bij een donkere omgeving.

## Testen met TDD  unit tests

Testdoelen (ZOMBIES)
Z: Zorg dat de lamp aan gaat als het donker is (minder dan 500 lux).

    Test: AdjustLighting_TurnsOnLamp_WhenDarkEnvironment
    Controleert of de lamp wordt ingeschakeld wanneer de lichtsterkte onder de 500 lux ligt.

O: Ontdek of de lamp de juiste kleur en helderheid krijgt op basis van de mood.

    Test: AdjustLighting_SetsCozyMood_WhenDarkEnvironment
    Controleert of de lamp de helderheid op 50 en de kleur op rood instelt wanneer de mood 'Cozy' is in een donkere omgeving.
    Test: AdjustLighting_SetsAngryMood_WhenDarkEnvironment
    Controleert of de lamp de helderheid op 100 en de kleur op rood instelt wanneer de mood 'Angry' is in een donkere omgeving.
    Test: AdjustLighting_SetsMultipleMoods_Sequentially
    Controleert of verschillende moods (zoals 'Cozy' en 'Bright') achtereenvolgens correct worden toegepast.

M: Maak zeker dat de lamp uitgaat als de mood 'Dark' is.

    Test: Niet geïmplementeerd in de huidige code
    De test AdjustLighting_TurnsOffLamp_WhenDarkMood ontbreekt in de code en wordt daarom uit de beschrijving verwijderd. Overweeg deze test toe te voegen als dit relevant is.

B: Bevestig dat de lamp in veilige modus gaat na meerdere fouten.

    Test: ApplySettings_ThrowsException_WhenBrightnessOutOfRange
    Controleert of een uitzondering wordt gegooid wanneer een ongeldige helderheid wordt ingesteld (buiten de 0-100 range), en of de lamp in veilige modus gaat bij herhaalde fouten.

I: Identificeer dat de juiste foutmeldingen worden weergegeven bij het instellen van ongeldige waarden.

    Test: AdjustLighting_ThrowsException_WhenInvalidMood
    Controleert of een uitzondering wordt gegooid bij het doorgeven van een ongeldige mood aan de AdjustLighting-methode.

E: Evalueer de status van de lamp na elke actie.

    Test: AdjustLighting_HandlesError_WhenLightSensorFails
    Controleert of de lampstatus correct wordt afgehandeld wanneer de lichtsensor faalt, inclusief het afvangen van de foutmelding.


## Integration tests:

In dit project zijn integration tests ontwikkeld om de samenwerking tussen verschillende componenten zoals LampController, ILamp, en ILightSensorApi te valideren. 

1. LampTurnsOnInDarkEnvironment

Deze test controleert of de lamp automatisch inschakelt in een donkere omgeving:

    Arrange: Een mock-lichtsensor geeft een lichtintensiteit van 300 lux (donkere omgeving) terug. De lamp is aanvankelijk uitgeschakeld.
    Act: De methode AdjustLighting wordt aangeroepen met de mood Cozy.
    Assert: Verifieert dat de lamp wordt ingeschakeld.

2. LampTurnsOffWhenMoodIsDark

Deze test valideert of de lamp wordt uitgeschakeld wanneer de mood Dark wordt geselecteerd:

    Arrange: De lamp is aanvankelijk ingeschakeld.
    Act: De AdjustLighting-methode wordt aangeroepen met de mood Dark.
    Assert: Controleert dat de lamp wordt uitgeschakeld.

3. LampRemainsInPreviousStateWhenSensorFails

Deze test controleert of de lamp zijn huidige staat behoudt als de lichtsensor faalt:

    Arrange: De lamp is aanvankelijk ingeschakeld. De lichtsensor werkt eerst correct, maar gooit vervolgens een uitzondering.
    Act: AdjustLighting wordt twee keer aangeroepen, waarbij de tweede aanroep resulteert in een sensorfout.
    Assert: Verifieert dat de lamp ingeschakeld blijft ondanks de sensorfout.

4. LampEntersSafeModeAfterMultipleFailures

Deze test controleert of de lamp in een veilige modus schakelt na meerdere opeenvolgende fouten van de lichtsensor:

    Arrange: De lichtsensor gooit drie opeenvolgende uitzonderingen, gevolgd door een geldige invoer.
    Act: AdjustLighting wordt vier keer aangeroepen.
    Assert: Verifieert dat de lamp wordt uitgeschakeld na de derde fout, wat wijst op het activeren van de veilige modus.

5. LampResetsAfterValidInputFollowingSafeMode

Deze test valideert dat de lamp zichzelf correct reset na het ontvangen van geldige invoer, nadat hij in de veilige modus is geschakeld:

    Arrange: Drie opeenvolgende fouten van de lichtsensor activeren de veilige modus. Daarna retourneert de lichtsensor een geldige waarde.
    Act: AdjustLighting wordt meerdere keren aangeroepen, waarbij de veilige modus wordt geactiveerd en daarna een herstelpoging plaatsvindt.
    Assert: Verifieert dat de lamp correct uit blijft na het betreden van de veilige modus en geldige invoer.


## Acceptance tests: 

Als gebruiker wil ik de lamp kunnen bedienen en aanpassen op basis van stemming en lichtintensiteit.

Scenario: Toggling the lamp

    Given: De lamp is uitgeschakeld.
    When: De gebruiker schakelt de lamp in.
    Then: De lamp moet worden ingeschakeld.

Scenario: Applying valid settings

    Given: De lamp is ingeschakeld.
    When: De gebruiker past instellingen toe met helderheid 80 en kleur "Blue".
    Then: De lamp moet helderheid 80 en kleur "Blue" hebben.

Scenario: Applying invalid settings

    Given: De lamp is ingeschakeld.
    When: De gebruiker probeert instellingen toe te passen met een ongeldige helderheid van -10.
    Then: Het systeem moet een ArgumentOutOfRangeException genereren.

Scenario: Adjusting lighting to Cozy mood

    Given: De lamp is ingeschakeld.
    And: De lichtintensiteit is lager dan 500.
    When: De gebruiker stelt de stemming in op Cozy.
    Then: De lamp moet helderheid 50 en kleur "Red" hebben.

Scenario: Adjusting lighting to Angry mood

    Given: De lamp is ingeschakeld.
    And: De lichtintensiteit is lager dan 500.
    When: De gebruiker stelt de stemming in op Angry.
    Then: De lamp moet helderheid 100 en kleur "Red" hebben.

Scenario: Adjusting lighting to Bright mood

    Given: De lamp is ingeschakeld.
    And: De lichtintensiteit is lager dan 500.
    When: De gebruiker stelt de stemming in op Bright.
    Then: De lamp moet helderheid 100 en kleur "White" hebben.

Scenario: Adjusting lighting to Dark mood

    Given: De lamp is ingeschakeld.
    When: De gebruiker stelt de stemming in op Dark.
    Then: De lamp moet worden uitgeschakeld.

Scenario: Entering Safe Mode

    Given: De lamp is ingeschakeld.
    And: De gebruiker past drie keer ongeldige instellingen toe.
    When: De gebruiker past instellingen toe met een helderheid van 101.
    Then: De lamp moet worden uitgeschakeld en de veilige modus moet worden geactiveerd.

 foutscenario's: 
 Scenario: Apply settings with invalid brightness

    Given: De lamp is ingeschakeld.
    When: De gebruiker probeert een ongeldige helderheid van 150 toe te passen.
    Then: Het systeem moet een ArgumentOutOfRangeException genereren.

Scenario: Apply settings when lamp is off

    Given: De lamp is uitgeschakeld.
    When: De gebruiker probeert instellingen toe te passen.
    Then: Het systeem moet een InvalidOperationException genereren.

Scenario: Toggle lamp when in safe mode

    Given: De lamp bevindt zich in de veilige modus.
    When: De gebruiker probeert de lamp in- of uit te schakelen.
    Then: De lamp moet uitgeschakeld blijven.

Scenario: When the lamp is toggled while in safe mode

    Given: De lamp bevindt zich in de veilige modus.
    When: De gebruiker schakelt de lamp om terwijl deze in de veilige modus is.
    Then: De lamp moet uitgeschakeld blijven.

Scenario: When the mood is set to an invalid value

    Given: De lamp is ingeschakeld.
    When: De stemming wordt ingesteld op een ongeldige waarde zoals "NonExistingMood".
    Then: De lamp moet in zijn huidige toestand blijven.
