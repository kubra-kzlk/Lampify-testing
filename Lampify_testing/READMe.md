# Lampify Solution - Structuur en Logica

## Overzicht
Deze oplossing is ontworpen om een lamp te beheren met de volgende functionaliteiten:
- Aan/uit zetten van de lamp.
- Aanpassen van helderheid en kleur.
- Overschakelen naar een veilige modus bij meerdere fouten.
- Automatische aanpassingen gebaseerd op lichtsterkte en mood.
    
## Relaties:
- LampController implementeert ILampApi: Dit betekent dat de controller toegang biedt tot de lampstatus, helderheid en kleur via een gestandaardiseerde interface.
- LampController gebruikt Lamp: De controller bestuurt een Lamp-object voor alle lampfunctionaliteiten zoals aan- of uitzetten en instellingen toepassen.
- LampController gebruikt ILightSensorApi: De controller maakt gebruik van de ILightSensorApi interface om de huidige lichtsterkte op te halen

## Interfaces
### ILightSensorApi
- **Methoden:** `GetLightIntensity()`
### ILamp
- **Methoden:** `TurnOn()`, `TurnOff()`, `SetBrightness(int brightness)`, `SetColor(string color)`, `IsOn { get; }`

## Klassen
### Lamp
- **Verantwoordelijkheden:** Houdt de status van de lamp bij, zoals `IsOn`, `Brightness` en `Color`.
- **Methoden:** `TurnOn`, `TurnOff`, `SetBrightness`, `SetColor`.

### LampController
- **Verantwoordelijkheden:** Beheert de lamp en implementeert de `ILamp`-interface.
- **Functionaliteiten:**
  - Aanpassingen via `ApplySettings(int brightness, string color)`.
  - Mood-gebaseerde instellingen via `AdjustLighting`
  - Foutenlogica en veilige modus via `EnterSafeMode`
  - Lamp aan/uit zetten via `ToggleLamp`

 ## LightSensorApi
 - **Verantwoordelijkheden:** Haalt de lichtsterkte op via een externe API.
 - **Methoden:**  `GetLightIntensity()`

 ## Enum
### Mood: 
  - Cozy: Voor een gezellige sfeer: rood
  - Angry: Voor een boze sfeer: fel rood licht
  - Bright: Voor een heldere sfeer: helder wit licht 
  - Dark: Zet de lamp uit als het donker is


## Testen met TDD 
Testdoelen (ZOMBIES)

Z: Zorg dat de lamp aan gaat als het donker is (minder dan 500 lux).

    Test: AdjustLighting_TurnsOnLamp_WhenDarkEnvironment - Deze test controleert of de lamp wordt ingeschakeld wanneer de lichtsterkte onder de 500 lux ligt

O: Ontdek of de lamp de juiste kleur en helderheid krijgt op basis van de mood.

    Test: AdjustLighting_SetsCozyMood_WhenDarkEnvironment - Deze test controleert of de lamp de helderheid op 50 en de kleur op rood instelt wanneer de mood 'Cozy' is in een donkere omgeving.
    Test: AdjustLighting_SetsAngryMood_WhenDarkEnvironment - Deze test controleert of de lamp de helderheid op 100 en de kleur op rood instelt wanneer de mood 'Angry' is in een donkere omgeving.

M: Maak zeker dat de lamp uitgaat als het donker is en de mood 'Dark' is.

    Test: AdjustLighting_TurnsOffLamp_WhenDarkMood - Deze test controleert of de lamp wordt uitgeschakeld wanneer de mood 'Dark' is en de omgeving donker is.

B: Bevestig dat de lamp in veilige modus gaat na meerdere fouten.
    
    Test: ApplySettings_ThrowsException_WhenBrightnessOutOfRange - Deze test controleert of een uitzondering wordt gegooid wanneer een ongeldige helderheid wordt ingesteld, en of de lamp in veilige modus gaat na meerdere fouten.

I: Identificeer dat de juiste foutmeldingen worden weergegeven bij het instellen van ongeldige waarden.

    Test: AdjustLighting_ThrowsException_WhenInvalidMood - Deze test controleert of een uitzondering wordt gegooid wanneer een ongeldige mood wordt doorgegeven aan de AdjustLighting-methode.
    Test: ApplySettings_ThrowsException_WhenColorIsNull - Deze test controleert of een uitzondering wordt gegooid wanneer een ongeldige kleur (null) wordt doorgegeven aan de ApplySettings-methode.

E: Evalueer de status van de lamp na elke actie
   
    Test: AdjustLighting_HandlesError_WhenLightSensorFails - Deze test controleert of de status van de lamp correct wordt geëvalueerd en of de juiste foutafhandeling plaatsvindt wanneer de lichtsensor faalt.