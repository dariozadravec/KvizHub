---
status: "sprejeto"
date: 2026-10-24
decision-makers: Dario Zadravec
supersedes: "/"
---

# Brez realnočasovne komunikacije med odjemalcem in strežnikom

## Kontekst in opis problema

Kviz poteka v živo na papir in brez telefonov. Kvizmaster po vsakem sklopu ročno pregleda papirje in vnese točke, lestvico pa objavi po zaključku dogodka. Rezervacije potekajo vnaprej, kapaciteta pa se preveri v transakciji ob vsaki rezervaciji ([ADR-002](002-modularni-monolit.md)).

Ali mora sistem spremembe (točke, lestvico, proste mize) uporabnikom potiskati v realnem času?

## Obravnavane možnosti

* Samo klasični zahtevki REST (zahtevek in odgovor)
* Realnočasovna komunikacija s SignalR (WebSockets) za lestvico in proste mize
* Periodično poizvedovanje (polling) iz odjemalca

## Odločitev

Izbrana možnost: "Samo klasični zahtevki REST", ker v procesu ni primera, ki bi zahteval sprotno osveževanje:

* med kvizom udeleženci telefonov ne uporabljajo,
* lestvica je objavljena šele po zaključku dogodka,
* pravilnost rezervacij zagotavlja transakcija na strežniku in ne prikaz na odjemalcu,
* SignalR bi dodal stalne povezave, dodatno stanje na strežniku in zahtevnejše testiranje, periodično poizvedovanje pa nepotrebne zahtevke.

### Posledice

* Dobro, ker backend ostane brez stanja povezav, kar poenostavi gostovanje, skaliranje in testiranje.
* Dobro, ker je rešitev skladna s konceptom kviza brez telefonov.
* Slabo, ker uporabnik spremembe (npr. število prostih mest) vidi šele ob osvežitvi strani, zato lahko rezervacija ob hkratnem zahtevku druge ekipe ne uspe; odjemalec mora takrat prikazati jasno sporočilo.
* Slabo, ker bi zahteva po živi lestvici (npr. na zaslonu v lokalu) zahtevala nov ADR in dodelavo.
