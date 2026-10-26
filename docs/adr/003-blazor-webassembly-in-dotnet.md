---
status: "sprejeto"
date: 2026-10-24
decision-makers: Dario Zadravec
supersedes: "/"
---

# Blazor WebAssembly in ASP.NET Core za celoten sklad v .NET

## Kontekst in opis problema

Po odločitvi za ločen odjemalec in REST API ([ADR-001](001-locen-odjemalec-in-rest-api.md)) je treba izbrati tehnologijo za SPA odjemalec in platformo za backend. Razvijalec ima največ izkušenj z ogrodjem .NET. Odjemalec in backend si izmenjujeta veliko enakih podatkovnih struktur (dogodki, rezervacije, točke), med nadgradnjami pa je predvidena tudi mobilna aplikacija.

Katero tehnologijo uporabiti za frontend in backend?

## Obravnavane možnosti

* Blazor WebAssembly (C#) in ASP.NET Core Web API (.NET 10)
* React ali Angular (TypeScript) in ASP.NET Core Web API
* React (TypeScript) in Node.js (Express ali NestJS)

## Odločitev

Izbrana možnost: "Blazor WebAssembly in ASP.NET Core Web API", ker:

* je celoten sklad v enem jeziku (C#) in na eni platformi, v kateri ima razvijalec največ izkušenj,
* si odjemalec in backend delita projekt z razredi za prenos podatkov (DTO), zato se pogodba API-ja ne podvaja v dveh jezikih,
* je znanje Blazor komponent kasneje mogoče uporabiti v mobilni aplikaciji (MAUI Blazor Hybrid),
* bi React ali Angular pomenila dva jezika in dve verigi orodij.

### Posledice

* Dobro, ker sprememba DTO-ja takoj pokaže napake pri prevajanju v obeh delih.
* Dobro, ker se uporablja ena veriga orodij (.NET SDK, Visual Studio) in en način testiranja.
* Dobro, ker se komponente in znanje lahko prenesejo v mobilno aplikacijo MAUI Blazor Hybrid.
* Slabo, ker je prvi prenos aplikacije Blazor WebAssembly večji in prvi zagon počasnejši kot pri aplikacijah v JavaScriptu, kar je opazno na telefonih.
* Slabo, ker je ekosistem gotovih komponent manjši kot pri Reactu ali Angularju.
