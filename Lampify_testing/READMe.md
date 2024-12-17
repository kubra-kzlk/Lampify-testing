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
- **Methoden:** `GetLampStatus()`,` GetBrightness()`, ` GetColor()`

## Klassen
### Lamp
- **Verantwoordelijkheden:** Houdt de status van de lamp bij, zoals `IsOn`, `Brightness` en `Color`.
- **Methoden:** `TurnOn`, `TurnOff`, `SetBrightness`, `SetColor`.

### LampController
- **Verantwoordelijkheden:** Beheert de lamp en implementeert de `ILamp`-interface.
- **Functionaliteiten:**
  - Aanpassingen via `ApplySettings`.
  - Mood-gebaseerde instellingen via `AdjustLighting`
  - Foutenlogica en veilige modus via `EnterSafeMode`
  - Lamp aan/uit zetten via `ToggleLamp`

 ## LightSensorApi
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
O: Ontdek of de lamp de juiste kleur en helderheid krijgt op basis van de mood.
M: Maak zeker dat de lamp uitgaat als het donker is en de mood 'Dark' is.
B: Bevestig dat de lamp in veilige modus gaat na meerdere fouten.
I: Identificeer dat de juiste foutmeldingen worden weergegeven bij het instellen van ongeldige waarden.
E: Evalueer de status van de lamp na elke actie
