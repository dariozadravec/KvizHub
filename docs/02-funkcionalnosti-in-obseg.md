# KvizHub: funkcionalnosti in obseg

## 1. Uporabniške vloge

| Vloga | Kaj lahko počne | Česa ne more |
|---|---|---|
| **Kvizmaster** | Ustvari in objavi dogodek kviza s sklopi. Določi, ali dogodek uporablja joker. Doda lokal. Zapre rezervacije. Označi prisotnost ekip in vnaša točke po sklopih. Objavi lestvico. Vidi povratne informacije za svoje dogodke. | Rezervirati miz za ekipe. Spreminjati točk po objavi lestvice. Upravljati uporabnikov. Videti povratnih informacij drugih kvizmastrov. |
| **Vodja ekipe** | Išče objavljene dogodke. Ustvari ekipo. Rezervira mizo in jo prekliče. Vidi lestvico končanih dogodkov. Odda oceno dogodka. | Ustvarjati ali urejati dogodkov. Vnašati točk ali označevati prisotnosti. Videti rezervacij drugih ekip. Oceniti dogodka, ki se ga ekipa ni udeležila. Oddati več kot ene ocene na dogodek. |
| **Administrator** | Upravlja uporabnike. Moderira lokale (urejanje, odstranjevanje, združevanje podvojenih). | Sodelovati pri točkovanju. Oddajati ocen. Ustvarjati dogodkov. |

## 2. Predvidene funkcionalnosti

Skupaj je predvidenih 14 funkcionalnosti, od tega 7 za prvo delujočo različico (**MVP**).

| # | Funkcionalnost | Opis | Vloga | MVP |
|---|---|---|---|---|
| 1 | Registracija, prijava in vloge | Račun z vlogo kvizmaster ali vodja ekipe, dostop do funkcij glede na vlogo. | vse | MVP |
| 2 | Ustvarjanje dogodka kviza | Termin, lokal, kapaciteta (število mest), sklopi (tema, največ točk) in nastavitev, ali dogodek uporablja joker. Dogodek prehaja med stanji: osnutek, objavljen, rezervacije zaprte, v teku, zaključen, ocenjevanje odprto, arhiviran. | kvizmaster | MVP |
| 3 | Iskalnik dogodkov | Prikaz objavljenih dogodkov s številom prostih mest. Filtri po kraju, datumu in organizatorju. | vodja ekipe | MVP |
| 4 | Rezervacija mize | Vodja ekipe rezervira mizo za svojo ekipo. Rezervacija porabi toliko mest, kolikor je članov ekipe. Če ni dovolj prostih mest, rezervacija ni mogoča. Vodja jo lahko prekliče, dokler so rezervacije odprte. | vodja ekipe | MVP |
| 5 | Vnos točk po sklopih in prisotnosti | Kvizmaster po vsakem sklopu vnese točke ekip ter zabeleži morebitne jokerje. Ob vnosu označi prisotnost ekip. Ob izenačenju, ki se razreši na dogodku, ekipi, ki je zmagala, vnese dodatno točko. Do objave lestvice lahko kvizmaster točke popravlja, po objavi pa so dokončne. | kvizmaster | MVP |
| 6 | Lestvica dogodka | Samodejni seštevek točk z upoštevanjem jokerja in dodatnih točk. Izenačeni ekipi imata isto mesto, dokler kvizmaster ne vnese dodatne točke. Po objavi je lestvica vidna vsem udeležencem. | sistem | MVP |
| 7 | Ocena dogodka | Ocena sklopov (1–5 in komentar) in celotnega večera. Možna samo za prisotno ekipo, samo enkrat in v časovnem oknu po zaključku (npr. 7 dni). | vodja ekipe | MVP |
| 8 | Dodajanje lokala s predlogi | Pri vnosu lokala sistem predlaga podobne obstoječe lokale in z normaliziranim naslovom prepreči točne duplikate. | kvizmaster | |
| 9 | Upravljanje ekip in članov | Urejanje ekipe, dodajanje in odstranjevanje članov. | vodja ekipe | |
| 10 | Čakalna lista | Ekipa se pri polnem dogodku uvrsti na čakalno listo in se ob odjavi druge ekipe samodejno premakne na potrjeno mesto. | sistem | |
| 11 | Odjava z rokom | Po izteku roka za odjavo se odjava šteje kot izostanek. | vodja ekipe | |
| 12 | Pregled povratnih informacij | Povprečne ocene po sklopih, temah in skozi čas za pripravo naslednjega dogodka. | kvizmaster | |
| 13 | Moderacija in združevanje lokalov | Administrator ureja lokale in združi podvojene (dogodki se prestavijo na pravi lokal). | administrator | |
| 14 | Upravljanje uporabnikov | Blokiranje in odblokiranje uporabnikov. | administrator | |

### Opombe k MVP

- **Ekipa in lokal v MVP:** za prvo različico je ekipa ustvarjena ob prvi rezervaciji (ime in vodja), lokal pa se pri ustvarjanju dogodka vnese z osnovnimi podatki (ime, naslov, kapaciteta). Napredno upravljanje ekip (9) in predlogi podobnih lokalov (8) sta kasnejši funkcionalnosti.
- **Čakalna lista (10) ni v MVP.** Pri polnem dogodku vodja ekipe rezervacije preprosto ne more oddati.
- **Izenačenja sistem ne razrešuje.** Rešijo se na samem dogodku, kvizmaster pa nato vnese dodatno točko.

## 3. Kasnejše nadgradnje

- **Mobilna aplikacija** (kot vmesni korak PWA, namestljiva spletna aplikacija),
- **Sezonska lestvica ekip** (zahteva dogovor o enotnem kriteriju točkovanja, saj kvizi med seboj niso vedno primerljivi),
- **Več uporabnikov z dostopom do ekipe** (npr. namestnik vodje), ker vodja ni vedno prisoten ali ne prijavlja vedno sam,
- **Kazenske točke ali omejitve za ekipe z izostanki**,
- **Ponavljajoči se dogodki** (npr. vsak torek),
- **Obvestila po e-pošti** (potrditev rezervacije, odprtje ocenjevanja),
- **Zunanja zbirka lokacij** (OpenStreetMap) namesto prostega vnosa lokalov,
- **Nastavljiva pravila jokerja** (npr. različni množitelji),
- **Statistika ekip skozi čas**,
- **Plačilo kotizacij ali rezervacij**.

## 4. Out of scope

- vprašanja, odgovori in baza vprašanj v sistemu (kviz poteka na papir),
- digitalna oddaja odgovorov in kviz prek telefonov med dogodkom,
- družabne funkcije (klepet, sledenje drugim uporabnikom, javni profili).
