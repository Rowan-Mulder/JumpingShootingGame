# Wild West Woodworker
*Deze game zit nog erg vroeg in zijn ontwikkelingsfase, de titel hiervan wordt duidelijk in het game design document.*
![Screenshot vanuit Unity](https://raw.githubusercontent.com/Rowan-Mulder/WildWestWoodworker/master/Assets/Screenshots/2021-01-19.png)

<br><br><br>
---

**Om dit project uit te breiden zijn er een aantal stappen nodig:**
* Sommige stappen hebben een [hoge-prioriteit](https://github.com/Rowan-Mulder/WildWestWoodworker/blob/master/README.md#to-do---hoge-prioriteit).
* Sommige stappen hebben een [lage-prioriteit](https://github.com/Rowan-Mulder/WildWestWoodworker/blob/master/README.md#to-do---lage-prioriteit).
* Sommige stappen zijn [optioneel](https://github.com/Rowan-Mulder/WildWestWoodworker/blob/master/README.md#to-do---optioneel).

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

- [x] ~~**Gerigged character model maken (voor speler + enemy)**~~
* ~~Modelen~~
* ~~Armarture~~
* ~~Rigging~~
  - ~~Weight painting optimaliseren~~
* ~~Materials~~

- [x] ~~**Layermask toevoegen voor waar kogels op zullen landen**~~
* ~~Maak een nieuwe layer~~
  - ~~Zorg dat kogels alleen zullen stoppen zodra zij deze layer tegen komen (of het afstandslimiet)~~
    - ~~Kijk of het werkt (en dat jij, of de enemies, niet meer jouw eigen globale playermodel kan schieten)~~

- [x] ~~**Kijk naar LateUpdate()**~~
* *LateUpdate() wordt later uitgevoerd dan Update()*
  - ~~Stop van PlayerLook.cs de globale beweging van de nek en het arm binnen LateUpdate()~~
    - ~~Kijk of dit het probleem globaal oplost waarbij de animatie prioriteit geeft over de nek/arm bewegingen. (dit loste het probleem op)~~

- [x] ~~**AnimationController maken**~~
* ~~Alle animation calls zelf beheren met een script binnen PlayerMove.cs~~
  - ~~Gebruik een Enumerator/Dictionary (hash array) voor alle constante animatie namen en call de animatie met behulp hiervan.~~
  - ~~Kijk voor animation blending (Besloten dit niet te doen. Uiteindelijk misschien de niet-scripted methode gebruiken, maar dit zal veel tijd kosten)~~

- [x] ~~**Crouching**~~
  - ~~Bij crouching (LEFT_SHIFT), maak de Character Controller height (direct) lager.~~
  - ~~Bij uncrouching (Loslaten LEFT_SHIFT), doe collision checks of je genoeg ruimte hebt om op te staan.~~
    - ~~Blijf ieder frame kijken of je genoeg ruimte hebt om op te staan tot je genoeg ruimte hebt. Zet de Character Controller height hierna weer (direct) terug.~~

- [x] **Globale Player Shadows**
  - ~~Globale shadows werken lokaal niet omdat het globale object lokaal niet gerenderd mag worden.~~
    - ~~De oplossing was om een apart model te hebben, wat specifiek lokaal zichtbaar is, maar volledig transparant is.~~

- [ ] **Basis AI gebruiken/maken**
- ~~Gebruik eerst de Unity AI om te kijken of dit voldoende zal zijn. (het is voldoende voor de basis)~~
- Moeten niet spawnen in het zicht.
- ~~Hoort de speler en ziet de speler binnen een bepaalde radius.~~
  - ~~Gebruikt raycasts om te zien of de speler niet achter een muur zit.~~
  - ~~Op momenten waar geluid getriggered kan worden, wordt er gekeken of de enemy dichtbij genoeg is om dit te horen.~~
    - *Beter zou zijn als hiervoor een class aangemaakt wordt die dit afhandeld, dan copy-pasten van deze logica waar nodig.*
  - ~~Momenteel ziet de enemy de speler nog niet bij crouchen, dit moet onderzocht worden.~~
    - Enemy kan speler niet goed zien vanwege crouch-animaties. Kijk of dit beter kan.

- [ ] **Animaties maken**
* ~~Idle~~
* Jump
* Falling
* Land
* ~~CrouchForward~~
* ~~CrouchBackward~~
* CrouchLeft
* CrouchRight
* ~~WalkForward~~
* ~~WalkBackward~~
* ~~WalkLeft~~
* ~~WalkRight~~
* ~~RunForward~~

- [ ] **Health script maken**
* ~~Houd bij hoeveel health het object heeft~~
* Het object wordt verwijderd bij 0hp of doet speciale handelingen bij uitzonderingen als enemies en players.

- [ ] **Game wereld maken**
- *Kijk hiervoor naar het gamedesign document*
- Bepaal hier de spawnpoints van enemies
- Breid WorldController.cs uit voor wat variatie in het spel

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
* Decoy (steen ter afleiding)

- [ ] **Melee weapons maken**
* Knife
* Baseball bat
* Axe
* Katana
* Guitar
* Rubber chicken

- [ ] **Meer animaties maken**
* JumpSprinted (heavy kickoff, stretched legs up to landing, non-looping, meant to blend into RunFallLand animation)
* SprintFallLand (heavy impact, meant to blend into RunForwards animation)
* LongFallLand (very heavy impact, stops movement for animation duration)
* DeathFromFalling (pancake)
* DeathFromBullet
* DeathFromVoid (flailing arms around)

<br><br><br>
---

## TO-DO - optioneel

- [ ] **Movement-script sliding toevoegen**
  - Sliden van ramps, zou hierbij snelheid kunnen opbouwen.

- [ ] **Decals limiteren tot een x-aantal**
  - Kijk of je decals binnen een empty-object kan plaatsen, en hiervan het aantal kan bijhouden.

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
