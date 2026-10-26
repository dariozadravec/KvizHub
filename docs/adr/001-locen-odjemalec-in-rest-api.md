---
status: "sprejeto"
date: 2026-10-24
decision-makers: Dario Zadravec
supersedes: "/"
---

# Ločen odjemalec in REST API, poslovna logika samo v backendu

## Kontekst in opis problema

KvizHub uporabljajo tri vloge (kvizmaster, vodja ekipe, administrator), vsi prek brskalnika, vodje ekip večinoma s telefona. Sistem ima več poslovnih pravil, ki morajo veljati ne glede na to, kateri del vmesnika jih sproži: prehodi stanj dogodka, kapaciteta pri rezervaciji, pravilo ocene (samo prisotna ekipa, enkrat, v 7-dnevnem oknu) in izračun lestvice z jokerjem. Med nadgradnjami sta predvideni tudi mobilna aplikacija in PWA.

Odločiti je treba, kako se uporabniški vmesnik in strežniški del delita delo ter kje se izvajajo poslovna pravila.

## Obravnavane možnosti

* Enostranska aplikacija (SPA) v brskalniku in ločen REST API, poslovna logika samo v backendu
* Strežniško generirane strani (klasična večstranska aplikacija, npr. MVC)
* Interaktivni vmesnik, ki se izvaja na strežniku in z brskalnikom komunicira prek stalne povezave (npr. Blazor Server, Laravel Livewire)

## Odločitev

Izbrana možnost: "SPA in ločen REST API, poslovna logika samo v backendu", ker:

* ustvari jasno mejo med prikazom in poslovno logiko, ki jo je mogoče testirati neodvisno od vmesnika,
* isti API lahko brez sprememb uporabi tudi kasnejša mobilna aplikacija ali PWA, česar strežniško generirane strani ne omogočajo,
* vmesnik na strežniku bi zahteval stalno povezavo za vsakega uporabnika, kar je na mobilnih omrežjih v lokalih nezanesljivo.

Pravila se izvajajo samo v storitvah backenda, ker je odjemalca v brskalniku mogoče obiti (npr. z neposrednim klicem API-ja). Odjemalec pravila le prikaže (npr. skrije gumb za oceno), nikoli ne odloča o njih.

### Posledice

* Dobro, ker so pravila na enem mestu in jih je mogoče pokriti z enotskimi testi brez vmesnika.
* Dobro, ker se frontend in backend razvijata ločeno (direktorija `frontend/` in `backend/`) in ju je mogoče spreminjati neodvisno, dokler se ohrani pogodba API-ja.
* Dobro, ker je pot do mobilne aplikacije ali PWA odprta brez ponovnega pisanja poslovne logike.
* Slabo, ker je treba vzdrževati pogodbo API-ja (poti, DTO-ji) in jo usklajevati med obema deloma.
* Slabo, ker je kode več kot pri strežniško generiranih straneh (krmilniki, DTO-ji, klici iz odjemalca).
* Slabo, ker je prvi prenos aplikacije v brskalnik večji kot pri klasičnih straneh.
