# KvizHub: zasnova rešitve

## 1. Ime rešitve

**KvizHub** je spletna platforma za organizacijo pub kvizov, ki potekajo v živo na papir. Podpira objavo dogodka, iskanje dogodkov, rezervacijo miz, vnos točk, lestvice in zbiranje povratnih informacij.

## 2. Namen in problem

Pub kvizi so druženje večje skupine ljudi ob mizah, navadno v barih in gostilnah, brez uporabe mobilnih naprav. Odgovore ekipe zapišejo na papir, kvizmaster jih ročno pregleda in točkuje. Okoli tega osrednjega dogodka pa je veliko neurejenega dela, ki se danes opravlja po družbenih omrežjih, SMS-ih, elektronski pošti in v Excelu:

- **Iskanje dogodkov in rezervacije miz:** igralci težko izvedo, kje in kdaj je kviz ter ali so še prosta mesta. Ekipe se prijavljajo prek sporočil, kvizmaster pa težko spremlja zasedenost lokala.
- **Točkovanje in lestvice:** ročno seštevanje točk po sklopih (z morebitnim jokerjem) vodi do napak, lestvica pa je pogosto objavljena z zamudo ali sploh ne.
- **Povratne informacije:** kvizmaster redko izve, katere teme in sklopi so ekipam ustrezali, zato se na naslednji kviz ne more optimalno ciljno pripraviti.
- **Pregled nad lokali:** isti lokal gosti kvize različnih organizatorjev, vendar ni skupnega mesta, kjer bi bili ti podatki urejeni.

KvizHub ta okoliški proces digitalizira, sam kviz pa namenoma ostane analogen. Vprašanja in odgovori se v platformi ne shranjujejo. Platforma ne sestavlja vsebine kviza, ampak ureja organizacijo **dogodka kviza** (termin, lokal, kapaciteta, sklopi, pravila točkovanja). Papir in druženje brez telefonov sta bistvo takih večerov.

## 3. Ciljni uporabniki

| Uporabnik | Situacija | Kaj rešitev prinese |
|---|---|---|
| **Kvizmaster** (organizator) | Redno pripravlja kvize v enem ali več lokalih. | Objava dogodka, pregled rezervacij, hiter vnos točk po sklopih, samodejna lestvica, povratne informacije po sklopih in temah. |
| **Vodja ekipe** | Ekipo (npr. 4–6 prijateljev ali sodelavcev) prijavi na kviz in jo zastopa. | Iskanje dogodkov s prostimi mesti, enostavna rezervacija mize, vpogled v lestvico, možnost ocene dogodka. |
| **Administrator** | Skrbi za urejenost platforme. | Moderacija lokalov (urejanje, odstranjevanje podvojenih) in upravljanje uporabnikov. |



## 4. Kratek opis delovanja

Tipičen potek enega dogodka izgleda takole:

1. **Priprava.** Kvizmaster izbere lokal (ali ga doda, če ga ni na seznamu) in ustvari dogodek kviza: datum in uro, lokal, kapaciteto (število mest) ter sklope. Vsak sklop ima ime, temo in največje število točk. Pri dogodku kvizmaster določi tudi, ali kviz uporablja **joker**, saj ga ne vključujejo vsi kvizi. Joker je pravilo, po katerem ekipa v enem izbranem sklopu podvoji svoje točke. Dogodek je v stanju *osnutek*, dokler ga kvizmaster ne objavi.
2. **Iskanje in rezervacija.** Po objavi vodje ekip dogodek najdejo z iskalnikom (prikazani so samo objavljeni dogodki, filtriranje je po kraju, datumu in organizatorju). Mizo rezervirajo za svojo ekipo in navedejo število članov, rezervacija pa porabi toliko mest, kolikor je članov. Če ni dovolj prostih mest, rezervacija ni mogoča. Vodja ekipe lahko rezervacijo prekliče, dokler so rezervacije odprte. Kvizmaster rezervacije kasneje zapre.
3. **Izvedba v živo.** Kviz poteka na papir. Če dogodek uporablja joker, ekipa na papirju izbere sklop, v katerem ga uporabi. Po vsakem sklopu kvizmaster pregleda papirje in v sistem vnese točke posamezne ekipe (ter jokerja, če je v uporabi). Kvizmaster označi tudi, katere ekipe so se dogodka zares udeležile (prisotne) in katere ne (izostanek).
4. **Lestvica.** Sistem sešteje točke z upoštevanjem jokerja ter izračuna lestvico. Izenačenje se reši na samem dogodku (npr. z dodatnim vprašanjem), sistem pa ga ne razrešuje: kvizmaster ekipi, ki je zmagala, vnese dodatno točko. Po zaključku dogodka kvizmaster lestvico objavi, ekipe pa jo lahko vidijo.
5. **Povratne informacije.** Po zaključku dogodka se odpre okno (do 7 dni), v katerem lahko **vodja prisotne ekipe** enkrat oceni posamezne sklope (ocena 1–5 in komentar) ter celoten večer. Kvizmaster dobi pregled povprečnih ocen po sklopih in temah, da lahko naslednji kviz pripravi bolje.
6. **Arhiv.** Ko se okno za povratne informacije izteče, je dogodek arhiviran in podatki ostanejo za statistiko.
