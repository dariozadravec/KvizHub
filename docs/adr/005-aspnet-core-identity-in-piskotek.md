---
status: "sprejeto"
date: 2026-10-24
decision-makers: Dario Zadravec
supersedes: "/"
---

# ASP.NET Core Identity in seja v piškotku za avtentikacijo

## Kontekst in opis problema

Uporabniki se registrirajo z e-pošto in geslom kot kvizmaster ali vodja ekipe, administrator se ustvari ob zagonu. Backend mora pri vsakem zahtevku preveriti vlogo in lastništvo, administratorju pa omogočiti blokiranje uporabnikov. Gesla morajo biti varno shranjena. Odjemalec Blazor WebAssembly streže isti ASP.NET Core strežnik, zato sta odjemalec in API na istem izvoru.

Kako izvesti registracijo, prijavo, vloge in sejo, ne da bi pisali lastno varnostno kodo?

## Obravnavane možnosti

* ASP.NET Core Identity in seja v piškotku
* ASP.NET Core Identity in žeton JWT, ki ga hrani odjemalec
* Zunanji ponudnik identitete (npr. Auth0, Microsoft Entra ID, Keycloak)
* Lastna implementacija registracije, zgoščevanja gesel in sej

## Odločitev

Izbrana možnost: "ASP.NET Core Identity in seja v piškotku", ker:

* zgoščevanje gesel, prijavo, zaklepanje računa in vloge rešuje preverjena knjižnica, ki je del platforme, lastna implementacija pa bi pomenila največje varnostno tveganje,
* piškotek deluje brez dodatnih nastavitev, ker sta odjemalec in API na istem izvoru,
* odjemalcu ni treba hraniti in osveževati žetonov, kot bi bilo potrebno pri JWT,
* bi zunanji ponudnik za ta obseg dodal odvisnost od zunanje storitve in dodatno nastavljanje.

### Posledice

* Dobro, ker gesla, prijava in vloge temeljijo na preverjeni in vzdrževani kodi.
* Dobro, ker piškotek HttpOnly ni dostopen JavaScriptu, zato ga ni mogoče prebrati z napadom XSS, odjemalec pa ne hrani nobenega žetona.
* Dobro, ker blokiranje uporabnika (`is_blocked`) in preverjanje vlog izvaja backend na enem mestu.
* Slabo, ker Identity doda tabeli `app_user` nekaj tehničnih stolpcev in zahteva preslikavo na obstoječi podatkovni model.
* Slabo, ker je pri piškotkih treba poskrbeti za zaščito pred CSRF (nastavitev SameSite in protiponarejevalni žetoni).
* Slabo, ker bi mobilna aplikacija ali odjemalec na drugem izvoru zahteval žetone; takrat bo potreben nov ADR.
