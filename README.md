# KvizHub

KvizHub je spletna platforma za organizacijo pub kvizov, ki potekajo v živo na papir. Kvizmastrom omogoča objavo dogodkov s sklopi in kapaciteto, vnos točk in samodejno lestvico, vodjem ekip pa iskanje dogodkov, rezervacijo miz in oceno sklopov po kvizu. Sam kviz namenoma ostane analogen: vprašanja in odgovori se v sistemu ne shranjujejo.

Projekt nastaja pri predmetu Razvoj informacijskih rešitev in storitev, ki je del magistrskega študijskega programa Informatika in podatkovne tehnologije na Fakulteti za elektrotehniko, računalništvo in informatiko Univerze v Mariboru.

## Uporabniki

| Vloga | Glavne funkcionalnosti |
|---|---|
| Kvizmaster | Lokali, dogodki s sklopi in jokerjem, pregled rezervacij, vnos točk, objava lestvice, pregled ocen |
| Vodja ekipe | Iskanje dogodkov, rezervacija mize, lestvica, ocena sklopov in večera |
| Administrator | Moderacija lokalov, upravljanje in blokiranje uporabnikov |

## Tehnologije

| Del | Tehnologija |
|---|---|
| Frontend | Blazor WebAssembly (C#, .NET 10) |
| Backend | ASP.NET Core Web API (C#, .NET 10) |
| Dostop do baze | Entity Framework Core (Npgsql) |
| Podatkovna baza | PostgreSQL |
| Avtentikacija | ASP.NET Core Identity |
| Testi in dokumentacija API-ja | xUnit, OpenAPI |

## Dokumentacija

Vsa projektna dokumentacija je v direktoriju [`docs/`](docs/):

1. [Zasnova rešitve](docs/01-zasnova-resitve.md)
2. [Funkcionalnosti in obseg](docs/02-funkcionalnosti-in-obseg.md)
3. [Podatkovni model](docs/03-podatkovni-model.md)
4. [Arhitektura in tehnološki sklad](docs/04-arhitektura-in-tehnoloski-sklad.md)
5. [Uporabniške zgodbe](docs/05-uporabniske-zgodbe.md)
6. [Wireframes](docs/06-wireframes.md)

### Arhitekturne odločitve (ADR)

Odločitve so zapisane po predlogi [MADR](https://adr.github.io/madr/) v [`docs/adr/`](docs/adr/) ([predloga](docs/adr/adr-template.md)).

| ADR | Odločitev | Status |
|---|---|---|
| [001](docs/adr/001-locen-odjemalec-in-rest-api.md) | Ločen odjemalec in REST API, poslovna logika samo v backendu | sprejeto |
| [002](docs/adr/002-modularni-monolit.md) | Modularni monolit namesto mikrostoritev | sprejeto |
| [003](docs/adr/003-blazor-webassembly-in-dotnet.md) | Blazor WebAssembly in ASP.NET Core | sprejeto |
| [004](docs/adr/004-postgresql-in-docker-compose.md) | PostgreSQL, lokalno zagnan z Docker Compose | sprejeto |
| [005](docs/adr/005-aspnet-core-identity-in-piskotek.md) | ASP.NET Core Identity in seja v piškotku | sprejeto |
| [006](docs/adr/006-brez-realnocasovne-komunikacije.md) | Brez realnočasovne komunikacije | sprejeto |

## Struktura repozitorija

```
├── frontend/   # Blazor WebAssembly odjemalec (v pripravi)
├── backend/    # ASP.NET Core Web API (v pripravi)
└── docs/       # dokumentacija, wireframi in ADR
```

## Razvojni proces

Razvoj poteka po [GitHub Flow](https://docs.github.com/en/get-started/using-github/github-flow):

1. Iz veje `main` ustvarimo novo vejo z opisnim imenom (npr. `feature/rezervacija-mize`, `docs/adr-0007`).
2. Na veji delamo manjše commite in jo potisnemo na GitHub.
3. Odpremo pull request v `main`.
4. Po pregledu spremembe združimo v `main` in vejo izbrišemo.

Veja `main` je zaščitena: spremembe so mogoče samo prek pull requestov, prisilno potiskanje in brisanje veje sta onemogočena, pravila veljajo tudi za administratorja.
