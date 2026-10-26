---
status: "sprejeto"
date: 2026-10-24
decision-makers: Dario Zadravec
supersedes: "/"
---

# PostgreSQL kot podatkovna baza, lokalno zagnan z Docker Compose

## Kontekst in opis problema

Podatki KvizHuba so močno povezani: uporabniki, ekipe, lokali, dogodki, sklopi, rezervacije, točke in ocene (glej `03-podatkovni-model.md`). Kapaciteta dogodka mora ostati pravilna tudi pri hkratnih rezervacijah, kar zahteva transakcije. Baza mora imeti dobro podporo v Entity Framework Core, shema pa naj bo del kode (migracije). Razvojno okolje naj bo enako pri vsakem zagonu in na vsakem računalniku, brez ročne namestitve strežnika baze.

Katero podatkovno bazo uporabiti in kako jo zagnati pri lokalnem razvoju?

## Obravnavane možnosti

### Podatkovna baza

* PostgreSQL
* MySQL
* Microsoft SQL Server
* Dokumentna baza (MongoDB)

### Lokalni zagon baze

* Vsebnik prek Docker Compose
* Ročna namestitev strežnika baze na razvijalčev računalnik

## Odločitev

Izbrana možnost: "PostgreSQL, zagnan prek Docker Compose", ker:

* relacijski model s tujimi ključi in omejitvami ustreza močno povezanim podatkom, dokumentna baza pa bi zahtevala ročno skrb za konsistentnost povezav,
* transakcije omogočajo varno preverjanje kapacitete pri rezervaciji,
* ponudnik Npgsql za EF Core je dobro podprt, PostgreSQL pa je brezplačen in brez licenčnih omejitev (za razliko od SQL Serverja v produkciji),
* MySQL bi bil enako smiselna izbira, saj prav tako podpira relacijski model in transakcije, vendar nima časovnega tipa s časovnim pasom. PostgreSQL ga ima (`timestamptz`) in ga podatkovni model že uporablja (npr. `starts_at`, `feedback_closes_at`), zato so termini dogodkov in rok za oddajo ocen shranjeni nedvoumno, tudi ob prehodu na poletni čas,
* Docker Compose zagotovi isto različico baze pri vseh razvijalcih in zagon z enim ukazom (`docker compose up -d`), brez ročne namestitve in nastavljanja.

Shemo ustvarjajo migracije EF Core, ki so shranjene v repozitoriju skupaj s kodo.

### Posledice

* Dobro, ker so integriteta podatkov in kapaciteta dogodka zagotovljene na ravni baze.
* Dobro, ker je razvojno okolje ponovljivo: vsak klon repozitorija dobi enako bazo z istimi migracijami.
* Dobro, ker je spremembe sheme mogoče pregledati v pull requestu kot del kode.
* Slabo, ker mora biti na razvojnem računalniku nameščen Docker (na Windows Docker Desktop z WSL 2), kar porabi dodatne vire.
