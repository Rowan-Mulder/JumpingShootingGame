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

- [ ] ~~**Globale Player Shadows**
  - ~~Globale shadows werken lokaal niet omdat het globale object lokaal niet gerenderd mag worden.~~
    - ~~De oplossing was om een apart model te hebben, wat specifiek lokaal zichtbaar is, maar volledig transparant is.~~
      - Het zou beter zijn als dit model ook de globale schaduw berekend, dan gebeurt er geen dubbel werk. Zet hiervoor schaduw uit van het globale object en verander de layer naar general voor het schaduw object.

- [x] **Basis AI gebruiken/maken**
- ~~Gebruik eerst de Unity AI om te kijken of dit voldoende zal zijn. (het is voldoende voor de basis)~~
- Hoort de speler en ziet de speler binnen een bepaalde radius.
  - Gebruikt raycasts vanuit PlayerShoot.cs om te zien of de speler achter een muur zit.
    - Zo ja, reageert het niet of minder snel op de speler. (of is afhankelijk van de afstand tussen de AI en de speler, als deze info op te halen zou zijn. Achter een paal mag de AI wel reageren, maar in een huis niet.)
    - Zo niet, loopt het richting de speler.
  - Bij een aantal luide geluidstriggers worden signalen gestuurd vanuit de audiosource, om alle enemies binnen een radius in te lichten van de locatie van het audiosource. Hier maakt een muur niet uit.

- [ ] **Animaties maken**
* ~~Zoek op hoe meerdere animaties onder 1 rig kunnen worden opgeslagen in **Blender 2.8+**~~
* Misschien is het beter om alleen RunForward toe te staan. Stel alle run animaties voorlopig uit.
* ~~Idle~~
* Jump
* Falling
* Land
* ~~WalkForward~~
* ~~WalkBackward~~
* WalkLeft
* WalkRight
* ~~RunForward~~
* ~~RunBackward~~
* RunLeft
* RunRight

- [ ] **Health script maken**
* ~~Houd bij hoeveel health het object heeft~~
* Het object wordt verwijderd bij 0hp of doet speciale handelingen bij uitzonderingen als enemies en players.

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
* Katana
* Guitar
* Rubber chicken


- [ ] **Meer animaties maken**
* DeathFromFalling
* DeathFromBullet

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
