---
status: "sprejeto"
date: 2026-10-24
decision-makers: Dario Zadravec
supersedes: "/"
---

# Modularni monolit namesto mikrostoritev

## Kontekst in opis problema

Backend mora pokriti uporabnike, lokale, dogodke s sklopi, rezervacije, točke, lestvice in ocene. Najbolj občutljiva operacija je rezervacija: preverjanje prostih mest in zapis rezervacije morata potekati v eni transakciji, sicer lahko dve hkratni rezervaciji presežeta kapaciteto. Projekt razvija en razvijalec v okviru enega semestra.

Kako naj bo backend razdeljen: kot ena aplikacija ali kot več samostojnih storitev?

## Obravnavane možnosti

* Modularni monolit: ena aplikacija s plastmi (krmilniki, storitve, dostop do podatkov)
* Mikrostoritve (npr. ločene storitve za uporabnike, dogodke in rezervacije)
* Monolit brez notranjih plasti (logika neposredno v krmilnikih)

## Odločitev

Izbrana možnost: "Modularni monolit", ker:

* rezervacijo in preverjanje kapacitete v eni aplikaciji in eni bazi zagotovi navadna transakcija, pri mikrostoritvah pa bi za isto potrebovali porazdeljene transakcije ali vzorce, kot je saga,
* bi mikrostoritve za obseg projekta in enega razvijalca povečale zahtevnost zagona, razhroščevanja in vzdrževanja,
* bi se pri monolitu brez plasti poslovna pravila pomešala s potmi API-ja in bi jih bilo težko testirati.

### Posledice

* Dobro, ker je rezervacija varna pri hkratnih zahtevkih z eno transakcijo v bazi.
* Dobro, ker se aplikacija zažene in namesti kot celota z enim ukazom, razhroščevanje pa poteka v enem procesu.
* Dobro, ker je plast storitev mogoče testirati ločeno od krmilnikov in baze.
* Dobro, ker jasne meje med moduli dopuščajo, da se posamezen del kasneje izloči v samostojno storitev, če bi bilo to potrebno.
* Slabo, ker se aplikacija skalira samo kot celota.
* Slabo, ker ločenost plasti in modulov ni vsiljena s strani okolja, ampak jo mora razvijalec vzdrževati sam.
* Slabo, ker napaka v enem delu lahko ustavi celotno aplikacijo.
