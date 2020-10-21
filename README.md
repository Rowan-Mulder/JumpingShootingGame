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

- [x] **Schieten implementeren**
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

- [ ] **Look-script nakijken**
* *Kijken is gebonden aan framerate. Met een lagere framerate (op laptop) stottert het.*
  - ~~Gebruik hierbij Time.deltaTime~~
    - Time.deltatime is geïmplementeerd, maar werkt nog niet.

- [ ] **Health toevoegen**

- [ ] **Basis AI maken**
- Hoort de speler en ziet de speler binnen een bepaalde radius.
  - Gebruik raycasts om te zien of de speler achter een muur zit.
    - Zo ja, reageert het niet op de speler.
    - Zo niet, loopt het richting de speler.
  - Bij een aantal luide geluidstriggers worden signalen gestuurd vanuit de audiosource, om alle enemies binnen een radius in te lichten van de locatie van het audiosource. Hier maakt een muur niet uit.

- [ ] **Animaties maken**
* Zoek op hoe meerdere animaties onder 1 rig kunnen worden opgeslagen in **Blender 2.8+**
* Idle
* IdleCrouch
* Jump
* Land
* ~~WalkForwards~~
* WalkBackwards
* WalkLeft
* WalkRight
* RunForwards
* RunBackwards
* RunLeft
* RunRight
- ShootGun
  - *Een aparte animatie voor iedere gun?*

<br><br><br>
---

## TO-DO - lage-prioriteit
- [ ] **Movement-script rennen verbeteren**
  - Transition van walking naar rennen moet smoother

- [ ] **Movement-script sliding toevoegen**
  - Sliden van ramps, bouw veel snelheid op.

- [ ] **Decals limiteren tot een x-aantal**

<br><br><br>
---

## TO-DO - optioneel
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

- [ ] **Details toevoegen aan de playermodel, op zijn minst in het gezicht.**

<br><br><br>
---

*Tijdens het werken aan scripts noteer ik hierbinnen alle ondervindingen/problemen/oplossingen in de comments.*
