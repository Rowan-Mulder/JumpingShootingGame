# JumpingShootingGame
*Tijdelijke titel. Ik wacht nog met de titel tot de basis (MVP) rond is.*
![Screenshot vanuit Unity](https://raw.githubusercontent.com/Rowan-Mulder/JumpingShootingGame/master/Github%20bestanden/Screenshots/Screenshot1.png)

<br><br><br>
---

**Om dit project uit te breiden zijn er een aantal stappen nodig:**
* Sommige stappen hebben een [hoge-prioriteit](https://github.com/Rowan-Mulder/JumpingShootingGame/blob/master/README.md#to-do---hoge-prioriteit).
* Sommige stappen hebben een [lage-prioriteit](https://github.com/Rowan-Mulder/JumpingShootingGame/blob/master/README.md#to-do---lage-prioriteit).
* Sommige stappen zijn [optioneel](https://github.com/Rowan-Mulder/JumpingShootingGame/blob/master/README.md#to-do---optioneel).

<br><br><br>
---

## TO-DO - hoge-prioriteit
- [x] ~~**Unity opzetten met een developer/test-zone**~~
* ~~Vlak veld~~
* ~~Objecten om op/af te springen~~
* ~~Trappen voor testen van traplopen~~
* ~~Steile blokken om op te lopen~~
* ~~Wanden om tegenaan te springen~~

- [x] ~~**Basic look script maken**~~
* ~~Om je heen kunnen kijken~~
* ~~Niet je nek verder kunnen draaien dan realistisch~~

- [x] ~~**Basis movement-script maken**~~
* ~~Bewegen~~
* ~~Rennen~~
* ~~Walljumping~~

- [x] ~~**Movement-script uitbreiden met walljumping**~~
* ~~Reflecteer de velocity tegen normals~~

- [x] ~~**Gun model maken**~~
* ~~Pistol object maken~~
  ~~- Blender~~
    - ~~Model maken~~
  ~~- Unity~~
    - ~~Materials albedo/metallic maken~~
    - ~~Binden aan player-hand armature en maak (uiteindelijk na testen) ontzichtbaar, totdat deze daadwerkelijk in gebruik is of opgepakt wordt.~~

- [x] ~~**Schieten implementeren**~~
* ~~Particles activeren van het wapen bij het schieten~~
* ~~Decals plaatsen in de vorm van een pijl (om normal vector te controleren)~~
* ~~Maak een decal texture~~
* ~~Decal pijl model vervangen met billboard met decal texture~~

- [x] **Gerigged character model maken (voor speler + enemy)**
* ~~Modelen~~
* ~~Armarture~~
* ~~Rigging~~
  - ~~Weight painting optimaliseren~~
* ~~Materials~~

- [x] **Layermask toevoegen voor waar kogels op zullen landen**
* ~~Maak een nieuwe layer~~
  - ~~Zorg dat kogels alleen zullen stoppen zodra zij deze layer tegen komen (of het afstandslimiet)~~
    - ~~Kijk of het werkt (en dat jij, of de enemies, niet meer jouw eigen globale playermodel kan schieten)~~

- [ ] **AnimationController maken**
* ~~Alle animation calls zelf beheren met een script binnen PlayerMove.cs~~
  - Gebruik een Enumerator/Dictionary (hash array) voor alle constante animatie namen en call de animatie met behulp hiervan.

- [ ] **Health script maken**
* ~~Houd bij hoeveel health het object heeft~~
* Het object wordt verwijderd bij 0hp of doet speciale handelingen bij uitzonderingen als enemies en players.

- [ ] **Basis AI gebruiken/maken**
- Gebruik eerst de Unity AI om te kijken of dit voldoende zal zijn.
- Hoort de speler en ziet de speler binnen een bepaalde radius.
  - Gebruik raycasts om te zien of de speler achter een muur zit.
    - Zo ja, reageert het niet op de speler.
    - Zo niet, loopt het richting de speler.
  - Bij een aantal luide geluidstriggers worden signalen gestuurd vanuit de audiosource, om alle enemies binnen een radius in te lichten van de locatie van het audiosource. Hier maakt een muur niet uit.

- [ ] **Animaties maken**
* ~~Zoek op hoe meerdere animaties onder 1 rig kunnen worden opgeslagen in **Blender 2.8+**~~
* ~~Idle~~
* IdleCrouch
* Jump
* Falling
* Land
* DeathFromFalling
* DeathFromBullet
* ~~WalkForward~~
* WalkBackward
* WalkLeft
* WalkRight
* ~~RunForward~~
* RunBackward
* RunLeft
* RunRight
- ShootGun
  - *Een aparte animatie voor iedere gun?*

<br><br><br>
---

## TO-DO - lage-prioriteit

- [ ] **Verschillende guns toevoegen**
* Assault rifle
* Sniper rifle
* Rocket launcher

- [ ] **Grenades toevoegen**
* HE Grenade
* Smoke bomb
* Decoy

- [ ] **Melee weapons maken**
* Knife
* Baseball bat
* Katana
* Rubber chicken

<br><br><br>
---

## TO-DO - optioneel

- [ ] **Movement-script sliding toevoegen**
  - Sliden van ramps, zou hierbij snelheid kunnen opbouwen.

- [ ] **Decals limiteren tot een x-aantal**

- [ ] **Details toevoegen aan de playermodel, op zijn minst in het gezicht.**

- [ ] **Movement-script verbeteren**
  - Movement animaties laten bepalen door 'average move direction' (Vector2(x,y))
  - Transition van walking naar rennen kan smoother

- [x] **Look-script nakijken**
* *Rondkijken was gebonden aan Time.deltaTime, wat niet nodig is omdat deltaTime al is berekend voor mouse input. Met een lagere framerate (op laptop) stotterde het.*
  - ~~Gebruik hierbij Time.deltaTime~~
    - ~~Time.deltaTime is geïmplementeerd, maar werkt nog niet als verwacht (Oplossing: Time.deltaTime verwijderd).~~

<br><br><br>
---

*Tijdens het werken aan scripts noteer ik hierbinnen alle ondervindingen/problemen/oplossingen in de comments.*
