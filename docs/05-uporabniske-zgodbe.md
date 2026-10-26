# KvizHub: uporabniške zgodbe

Dokument pretvori funkcionalnosti iz `02-funkcionalnosti-in-obseg.md` v uporabniške zgodbe s kriteriji sprejemljivosti (acceptance criteria). Funkcionalnosti, ki bi bile kot ena zgodba prevelike (1, 2, 4 in 5), so razdeljene na več manjših zgodb, da je vsaka samostojna, ocenljiva in preverljiva. Imena stanj in polj v kriterijih se ujemajo s podatkovnim modelom v `03-podatkovni-model.md`.

## Pregled zgodb

| Oznaka | Zgodba | Funkcionalnost | Vloga | MVP |
|---|---|---|---|---|
| US-01 | Registracija z vlogo | 1 | neprijavljen uporabnik | MVP |
| US-02 | Prijava in dostop glede na vlogo | 1 | vse | MVP |
| US-03 | Ustvarjanje dogodka kviza | 2 | kvizmaster | MVP |
| US-04 | Objava in vodenje stanj dogodka | 2 | kvizmaster | MVP |
| US-05 | Iskanje dogodkov | 3 | vodja ekipe | MVP |
| US-06 | Rezervacija mize | 4 | vodja ekipe | MVP |
| US-07 | Preklic rezervacije | 4 | vodja ekipe | MVP |
| US-08 | Označevanje prisotnosti | 5 | kvizmaster | MVP |
| US-09 | Vnos točk po sklopih | 5 | kvizmaster | MVP |
| US-10 | Dodatna točka ob izenačenju | 5 | kvizmaster | MVP |
| US-11 | Izračun in objava lestvice | 6 | kvizmaster | MVP |
| US-12 | Ocena dogodka | 7 | vodja ekipe | MVP |
| US-13 | Predlogi podobnih lokalov | 8 | kvizmaster | |
| US-14 | Upravljanje ekipe in članov | 9 | vodja ekipe | |
| US-15 | Čakalna lista | 10 | vodja ekipe | |
| US-16 | Odjava z rokom | 11 | vodja ekipe | |
| US-17 | Pregled povratnih informacij | 12 | kvizmaster | |
| US-19 | Urejanje in združevanje lokalov | 13 | administrator | |
| US-20 | Upravljanje uporabnikov | 14 | administrator | |

## MVP

### US-01: Registracija z vlogo

- **Funkcionalnost:** 1. Registracija, prijava in vloge
- **Vloga:** neprijavljen uporabnik (bodoči kvizmaster ali vodja ekipe)

**Zgodba:** Kot nov uporabnik želim ustvariti račun z vlogo kvizmaster ali vodja ekipe, da lahko uporabljam funkcije, ki jih potrebujem.

**Kriteriji sprejemljivosti:**

1. Obrazec zahteva e-pošto, prikazno ime, geslo in vlogo. Na izbiro sta samo vlogi kvizmaster in vodja ekipe, vloge administrator ni mogoče izbrati.
2. Če račun s tem e-naslovom že obstaja (ne glede na velike in male črke), registracija ne uspe in prikaže se sporočilo „Račun s tem e-naslovom že obstaja.“
3. Geslo mora imeti vsaj 8 znakov ter vsaj eno črko in eno številko. Ob neustreznem geslu se izpiše, katero pravilo ni izpolnjeno.
4. Geslo je v bazi shranjeno samo kot zgoščena vrednost (`password_hash`), nikoli v izvirni obliki.
5. Po uspešni registraciji je uporabnik prijavljen in preusmerjen na začetno stran svoje vloge (kvizmaster: „Moji dogodki“, vodja ekipe: iskalnik dogodkov).

### US-02: Prijava in dostop glede na vlogo

- **Funkcionalnost:** 1. Registracija, prijava in vloge
- **Vloga:** vse (kvizmaster, vodja ekipe, administrator)

**Zgodba:** Kot registriran uporabnik se želim prijaviti z e-pošto in geslom, da vidim in uporabljam samo funkcije svoje vloge.

**Kriteriji sprejemljivosti:**

1. Ob napačni e-pošti ali geslu se prikaže enotno sporočilo „Napačen e-naslov ali geslo.“, ki ne izda, kateri podatek je napačen.
2. Po uspešni prijavi backend nastavi piškotek seje. Odjava piškotek izbriše in uporabnika preusmeri na prijavno stran.
3. Meni prikazuje samo povezave do funkcij prijavljene vloge (npr. vodja ekipe ne vidi povezave „Nov dogodek“, kvizmaster ne vidi gumba „Rezerviraj“).
4. API zahtevek brez prijave vrne 401, zahtevek, ki za vlogo uporabnika ni dovoljen, pa vrne 403, ne glede na to, kaj prikazuje vmesnik.

### US-03: Ustvarjanje dogodka kviza

- **Funkcionalnost:** 2. Ustvarjanje dogodka kviza
- **Vloga:** kvizmaster

**Zgodba:** Kot kvizmaster želim ustvariti dogodek kviza z lokalom, terminom, kapaciteto in sklopi, da ga lahko v miru pripravim pred objavo.

**Kriteriji sprejemljivosti:**

1. Obvezni podatki so naslov, datum in ura začetka, lokal, kapaciteta (celo število, večje od 0) in vsaj en sklop. Brez njih se dogodek ne shrani, manjkajoča polja pa so označena.
2. Lokal kvizmaster izbere s seznama obstoječih ali doda novega z imenom, ulico, poštno številko, krajem in privzeto kapaciteto. Ob izbiri lokala se kapaciteta dogodka predizpolni s privzeto kapaciteto lokala in jo je mogoče spremeniti.
3. Vsak sklop ima ime, temo in največje število točk (celo število, večje od 0). Sklopi so oštevilčeni po vrstnem redu (`position`), številka je znotraj dogodka enolična.
4. Kvizmaster označi, ali dogodek uporablja joker. Privzeto joker ni vključen (`joker_enabled = false`).
5. Shranjen dogodek je v stanju `draft`, ni viden v iskalniku, kvizmaster pa ga lahko v tem stanju poljubno ureja.

### US-04: Objava in vodenje stanj dogodka

- **Funkcionalnost:** 2. Ustvarjanje dogodka kviza
- **Vloga:** kvizmaster

**Zgodba:** Kot kvizmaster želim dogodek objaviti in ga voditi skozi stanja do zaključka, da ekipe vedo, kdaj se lahko prijavijo, in da je jasno, v kateri fazi je večer.

**Kriteriji sprejemljivosti:**

1. Kvizmaster lahko dogodek premika samo naprej po zaporedju `draft` → `published` → `registrations_closed` → `in_progress` → `finished`. Preskok stanja ali vrnitev v prejšnje stanje ni mogoča.
2. Dogodek je mogoče objaviti samo, če ima vsaj en sklop in je začetek v prihodnosti.
3. Po objavi dogodka sklopov in nastavitve jokerja ni več mogoče spreminjati.
4. Dogodek je mogoče zaključiti (`finished`) šele, ko je pri vseh rezervacijah v stanju `confirmed` označena prisotnost (`attended` ali `no_show`).
5. Kvizmaster lahko spreminja samo svoje dogodke. Poskus spremembe tujega dogodka API zavrne.

### US-05: Iskanje dogodkov

- **Funkcionalnost:** 3. Iskalnik dogodkov
- **Vloga:** vodja ekipe

**Zgodba:** Kot vodja ekipe želim poiskati objavljene kvize po kraju, datumu in organizatorju, da hitro najdem večer, na katerega se lahko prijavimo.

**Kriteriji sprejemljivosti:**

1. Iskalnik prikaže samo dogodke v stanju `published`. Najprej so prikazani dogodki z vsaj enim prostim mestom (kapaciteta minus vsota `team_size` rezervacij v stanju confirmed), polni dogodki pa so na koncu seznama.
2. Filter po kraju vrne dogodke v lokalih, katerih kraj se začne z vnesenim besedilom, ne glede na velike in male črke (npr. „murska“ najde „Murska Sobota“).
3. Filter po datumu sprejme obdobje od–do in vrne dogodke z začetkom v tem obdobju. Organizatorja vodja izbere s seznama kvizmastrov.
4. Vsak zadetek prikaže naslov, lokal s krajem, datum in uro, organizatorja, število prostih mest in ali dogodek uporablja joker.
5. Če ni zadetkov, se prikaže sporočilo „Ni kvizov, ki ustrezajo izbranim filtrom.“ in gumb za ponastavitev filtrov.

### US-06: Rezervacija mize

- **Funkcionalnost:** 4. Rezervacija mize
- **Vloga:** vodja ekipe

**Zgodba:** Kot vodja ekipe želim rezervirati mizo za svojo ekipo in navesti število članov, da imamo zagotovljeno mesto na kvizu.

**Kriteriji sprejemljivosti:**

1. Rezervacija je mogoča samo za dogodek v stanju `published`. Vodja izbere eno od ekip, v katerih je član z računom, ali vnese ime nove ekipe, ki se ustvari skupaj z njim kot članom.
2. Število članov (`team_size`) mora biti od 1 do 6 in ne sme presegati prostih mest. Če mest ni dovolj, rezervacija ne uspe in prikaže se, koliko mest je še prostih.
3. Preverjanje prostih mest in shranjevanje rezervacije potekata v eni transakciji, zato hkratne rezervacije skupaj nikoli ne presežejo kapacitete.
4. Ekipa ima na dogodku največ eno rezervacijo. Ponovna rezervacija že potrjene ekipe ni mogoča, po preklicu pa se uporabi obstoječa vrstica, ki znova dobi stanje `confirmed`.
5. Uspešna rezervacija je v stanju `confirmed`, prijavljeni uporabnik je zapisan kot vodja (`leader_id`). Rezervacijo vidita vodja (na strani „Moje rezervacije“) in kvizmaster (na seznamu rezervacij dogodka), drugi vodje ekip je ne vidijo.

### US-07: Preklic rezervacije

- **Funkcionalnost:** 4. Rezervacija mize
- **Vloga:** vodja ekipe

**Zgodba:** Kot vodja ekipe želim preklicati rezervacijo, dokler so rezervacije odprte, da sprostim mesta, če ekipa ne more priti.

**Kriteriji sprejemljivosti:**

1. Preklic je mogoč samo, dokler je dogodek v stanju `published`. Po zaprtju rezervacij gumb za preklic ni prikazan, API pa preklic zavrne.
2. Rezervacijo lahko prekliče samo njen vodja (`leader_id`).
3. Pred preklicem sistem zahteva potrditev. Po potrditvi je rezervacija v stanju `cancelled`.
4. Mesta preklicane rezervacije se takoj štejejo kot prosta, dogodek pa se posodobi v iskalniku, če je bil prej poln.

### US-08: Označevanje prisotnosti

- **Funkcionalnost:** 5. Vnos točk po sklopih in prisotnosti
- **Vloga:** kvizmaster

**Zgodba:** Kot kvizmaster želim na dogodku označiti, katere ekipe so prišle, da so točke in ocene vezane samo na ekipe, ki so bile zares prisotne.

**Kriteriji sprejemljivosti:**

1. Prisotnost je mogoče označevati, ko je dogodek v stanju `registrations_closed` ali `in_progress`.
2. Seznam prikaže vse nepreklicane rezervacije dogodka z imenom ekipe in številom mest. Preklicane rezervacije niso prikazane.
3. Vsako rezervacijo kvizmaster označi kot `attended` ali `no_show`. Oznako lahko spremeni, dokler dogodek ni zaključen.
4. Za rezervacijo, ki ni v stanju `attended`, ni mogoče vnesti točk.

### US-09: Vnos točk po sklopih

- **Funkcionalnost:** 5. Vnos točk po sklopih in prisotnosti
- **Vloga:** kvizmaster

**Zgodba:** Kot kvizmaster želim po vsakem sklopu vnesti točke ekip in sklop, v katerem je ekipa uporabila joker, da sistem namesto mene sešteje rezultat.

**Kriteriji sprejemljivosti:**

1. Točke je mogoče vnašati in popravljati, ko je dogodek v stanju `in_progress` ali `finished` in lestvica še ni objavljena. Po objavi lestvice so točke dokončne in jih ni mogoče več spreminjati.
2. Zaslon za vnos prikaže en sklop z vsemi prisotnimi ekipami. Sklopi, za katere so vnesene točke vseh ekip, so označeni kot vneseni.
3. Vnos je število od 0 do največjega števila točk sklopa v korakih po 0,5 (npr. 7 ali 7,5). Vrednost zunaj razpona ali z drugačnim decimalnim delom (npr. 7,3) se ne shrani, polje pa se označi z napako.
4. Za vsako ekipo in sklop obstaja največ en vnos. Ponoven vnos prepiše obstoječo vrednost.
5. Če dogodek uporablja joker, lahko kvizmaster ekipi označi joker v največ enem sklopu. Če dogodek jokerja ne uporablja, možnost ni prikazana.

### US-10: Dodatna točka ob izenačenju

- **Funkcionalnost:** 5. Vnos točk po sklopih in prisotnosti
- **Vloga:** kvizmaster

**Zgodba:** Kot kvizmaster želim ekipi, ki je zmagala v razrešitvi izenačenja na dogodku, dodati dodatno točko, da lestvica pokaže dejanski vrstni red.

**Kriteriji sprejemljivosti:**

1. Dodatno točko je mogoče dodati samo prisotni ekipi, ki ima v tistem trenutku enak skupni rezultat kot vsaj ena druga ekipa.
2. Vsako dodajanje poveča `tiebreak_points` ekipe za 1. Dokler lestvica ni objavljena, je dodatno točko mogoče odstraniti.
3. Dodatne točke so na lestvici prikazane ločeno od točk sklopov (npr. „+1“) in so vključene v skupni rezultat.
4. Vnos je mogoč samo, ko je dogodek v stanju `in_progress` ali `finished` in lestvica še ni objavljena.

### US-11: Izračun in objava lestvice

- **Funkcionalnost:** 6. Lestvica dogodka
- **Vloga:** kvizmaster

**Zgodba:** Kot kvizmaster želim, da sistem samodejno izračuna lestvico, ki jo nato objavim, da ekipe vidijo rezultate takoj po kvizu brez ročnega seštevanja.

**Kriteriji sprejemljivosti:**

1. Skupni rezultat ekipe je vsota točk vseh sklopov, pri čemer se točke sklopa z jokerjem podvojijo, in `tiebreak_points`. Primer: sklopi 8, 6 (joker) in 9 ter 1 dodatna točka dajo 8 + 12 + 9 + 1 = 30.
2. Lestvica vključuje samo rezervacije v stanju `attended` in je razvrščena po skupnem rezultatu padajoče. Ekipe z enakim rezultatom imajo isto mesto, naslednja ekipa pa dobi mesto glede na število ekip pred njo (npr. 1, 2, 2, 4).
3. Lestvice ni mogoče objaviti, dokler dogodek ni v stanju `finished` in dokler niso vnesene točke vseh prisotnih ekip v vseh sklopih. Sistem prikaže, kateri vnosi manjkajo.
4. Pred objavo lestvico vidi samo kvizmaster. Po objavi jo vidijo vsi vodje ekip z rezervacijo na dogodku, s točkami po sklopih in označenim jokerjem.
5. Ob objavi lestvice dogodek preide v stanje `feedback_open`, `feedback_closes_at` pa se nastavi na 7 dni po objavi.

### US-12: Ocena dogodka

- **Funkcionalnost:** 7. Ocena dogodka
- **Vloga:** vodja ekipe

**Zgodba:** Kot vodja prisotne ekipe želim oceniti posamezne sklope in celoten večer, da kvizmaster ve, kaj je ekipam ustrezalo.

**Kriteriji sprejemljivosti:**

1. Ocena je na voljo samo vodji rezervacije (`leader_id`) v stanju `attended`, ko je dogodek v stanju `feedback_open` in `feedback_closes_at` še ni minil. Drugim uporabnikom gumb ni prikazan, API pa oddajo zavrne.
2. Ocena večera (1–5) je obvezna, ocene sklopov (1–5) in komentarji so neobvezni. Ocena zunaj razpona 1–5 se ne shrani.
3. Na rezervacijo je mogoče oddati samo eno oceno. Po oddaji obrazec ni več na voljo, vodja pa vidi svojo oddano oceno.
4. Ko `feedback_closes_at` mine, oddaja ni več mogoča, sistem pa dogodek prestavi v stanje `archived`.
5. Oddane ocene druge ekipe ne vidijo, kvizmaster pa jih vidi samo za svoje dogodke.

## Kasnejše funkcionalnosti

### US-13: Predlogi podobnih lokalov

- **Funkcionalnost:** 8. Dodajanje lokala s predlogi
- **Vloga:** kvizmaster

**Zgodba:** Kot kvizmaster želim pri dodajanju lokala videti podobne obstoječe lokale, da ne ustvarim dvojnika lokala, ki ga je dodal že kdo drug.

**Kriteriji sprejemljivosti:**

1. Ko kvizmaster vnese vsaj 3 znake imena ali ulice, sistem prikaže do 5 obstoječih lokalov, katerih ime ali naslov vsebuje vneseno besedilo.
2. Ob shranjevanju sistem iz ulice, poštne številke in kraja izračuna `normalized_address` (male črke, brez šumnikov in ločil, zaporedni presledki združeni). Primer: „Šolska ulica 3“ in „solska ulica, 3“ dasta enak normaliziran naslov.
3. Če lokal z enakim normaliziranim naslovom že obstaja, se nov lokal ne ustvari, kvizmaster pa dobi ponudbo, da izbere obstoječega.
4. Izbira predlaganega lokala ga poveže z dogodkom, ne da bi se ustvaril nov zapis lokala.

### US-14: Upravljanje ekipe in članov

- **Funkcionalnost:** 9. Upravljanje ekip in članov
- **Vloga:** vodja ekipe

**Zgodba:** Kot vodja ekipe želim urejati ime ekipe in seznam njenih članov, da so podatki o ekipi ob prijavah na kvize ažurni.

**Kriteriji sprejemljivosti:**

1. Ekipo lahko ureja vsak njen član z uporabniškim računom. Drugi uporabniki ekipe ne vidijo.
2. Člana vodja doda z imenom. Če vnese tudi e-pošto registriranega uporabnika, se član poveže z njegovim računom (`user_id`).
3. Člana, ki je vodja rezervacije za še nezaključen dogodek, ni mogoče odstraniti. Sistem prikaže, za kateri dogodek gre.
4. V ekipi mora ostati vsaj en član z uporabniškim računom. Odstranitev zadnjega takega člana ni mogoča.
5. Pri novi rezervaciji je število članov predizpolnjeno s številom članov ekipe in ga je mogoče spremeniti.

### US-15: Čakalna lista

- **Funkcionalnost:** 10. Čakalna lista
- **Vloga:** vodja ekipe

**Zgodba:** Kot vodja ekipe želim ekipo uvrstiti na čakalno listo polnega dogodka, da dobimo mesto, če se kakšna ekipa odjavi.

**Kriteriji sprejemljivosti:**

1. Če objavljen dogodek nima dovolj prostih mest za vneseno število članov, se namesto rezervacije ponudi uvrstitev na čakalno listo.
2. Čakalna lista je urejena po času uvrstitve, vodja pa vidi svoje mesto na njej (npr. „2. na čakalni listi“).
3. Ko se mesta sprostijo, sistem samodejno potrdi prvo ekipo na čakalni listi, za katero je dovolj prostih mest. Ekipe pred njo, za katere mest ni dovolj, ohranijo svoje mesto na listi.
4. Ekipa se lahko s čakalne liste umakne, dokler so rezervacije odprte. Ob zaprtju rezervacij se vse uvrstitve na čakalni listi samodejno prekličejo.

### US-16: Odjava z rokom

- **Funkcionalnost:** 11. Odjava z rokom
- **Vloga:** vodja ekipe

**Zgodba:** Kot vodja ekipe želim vedeti, do kdaj se lahko odjavim brez posledic, da kvizmastru pravočasno sporočim, če ne pridemo.

**Kriteriji sprejemljivosti:**

1. Kvizmaster pri dogodku določi rok za prijavo in odjavo, ki mora biti pred začetkom. Ob roku sistem dogodek samodejno prestavi v stanje `registrations_closed`. Rok je prikazan na strani dogodka in v podrobnostih rezervacije.
2. Odjava pred rokom rezervacijo prestavi v stanje `cancelled`.
3. Odjava po roku in pred začetkom dogodka se avtomatsko šteje kot izostanek. Rezervacija nato preide v stanje `no_show`.
4. V primeru odjave pred rokom se mesta rezervacije sprostijo.
5. Kvizmaster na seznamu rezervacij vidi, katere ekipe so se odjavile pred ali po roku.

### US-17: Pregled povratnih informacij

- **Funkcionalnost:** 12. Pregled povratnih informacij
- **Vloga:** kvizmaster

**Zgodba:** Kot kvizmaster želim videti povprečne ocene po sklopih, temah in skozi čas, da naslednji kviz bolje prilagodim ekipam.

**Kriteriji sprejemljivosti:**

1. Za posamezen dogodek pregled prikaže povprečno oceno večera, povprečno oceno vsakega sklopa (na eno decimalko natančno) in število oddanih ocen.
2. Pregled po temah združi ocene vseh sklopov z enako temo (ne glede na velike in male črke) iz kvizmastrovih dogodkov in jih razvrsti od najbolje do najslabše ocenjene.
3. Pregled skozi čas prikaže povprečno oceno večera za vsak dogodek v izbranem obdobju, razvrščeno po datumu.
4. Komentarji so prikazani ob ocenah, brez podatka o ekipi, ki jih je oddala.
5. Kvizmaster vidi samo povratne informacije za svoje dogodke.

### US-18: Urejanje in združevanje lokalov

- **Funkcionalnost:** 13. Moderacija in združevanje lokalov
- **Vloga:** administrator

**Zgodba:** Kot administrator želim urejati lokale in združevati podvojene, da ima vsak lokal en zapis z vsemi svojimi dogodki.

**Kriteriji sprejemljivosti:**

1. Administrator lahko ureja ime, naslov in privzeto kapaciteto lokala. Ob spremembi naslova se ponovno izračuna `normalized_address`, ki mora ostati enoličen.
2. Pri združevanju administrator izbere podvojeni in ciljni lokal. Pred potrditvijo sistem prikaže, koliko dogodkov se bo spremenilo.
3. Po združitvi so vsi dogodki podvojenega lokala povezani s ciljnim lokalom, podvojeni lokal pa je izbrisan.
4. Lokal brez dogodkov je mogoče izbrisati, lokala z dogodki pa ne, dokler ga administrator ne združi z drugim.
5. Urejanje, združevanje in brisanje lokalov so dostopna samo administratorju.

### US-19: Upravljanje uporabnikov

- **Funkcionalnost:** 14. Upravljanje uporabnikov
- **Vloga:** administrator

**Zgodba:** Kot administrator želim blokirati uporabnike, da preprečim zlorabo platforme.

**Kriteriji sprejemljivosti:**

1. Seznam uporabnikov prikazuje e-pošto, prikazno ime, vlogo in blokado ter omogoča iskanje po e-pošti ali imenu.
2. Blokiran uporabnik (`is_blocked = true`) se ne more prijaviti in vidi sporočilo „Račun je blokiran.“ Njegova obstoječa seja se konča ob naslednjem zahtevku.
3. Administrator lahko blokado odstrani, uporabnik pa se nato lahko znova prijavi.
4. Administrator ne more blokirati sebe.
