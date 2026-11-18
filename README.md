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
├── frontend/   # Blazor WebAssembly odjemalec
├── backend/    # ASP.NET Core Web API
├── shared/     # skupne datoteke za backend in frontend
├── tests/      # testi (xUnit)
└── docs/       # dokumentacija, wireframi in ADR
```

## Lokalni zagon

### Zahteve

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)

### Koraki

Vsi ukazi se izvedejo v korenu repozitorija.

1. Klonirajte repozitorij:

   ```bash
   git clone https://github.com/dariozadravec/KvizHub.git
   cd KvizHub
   ```

2. Ustvarite datoteko `.env` iz vzorca in v njej nastavite geslo za bazo (`POSTGRES_PASSWORD`):

   Geslo nastavite pred prvim zagonom baze (korak 3), ker ga PostgreSQL prebere samo ob ustvarjanju baze.

   ```bash
   cp .env.example .env
   ```

   Na Windows v ukaznem pozivu: `copy .env.example .env`.

3. Zaženite PostgreSQL:

   ```bash
   docker compose up -d
   ```

4. Nastavite povezovalni niz do baze in podatke administratorskega računa v uporabniških skrivnostih. Geslo za bazo mora biti enako kot v `.env`, geslo administratorja pa mora imeti vsaj 8 znakov, vsaj eno črko in eno številko:

   ```bash
   dotnet user-secrets set "ConnectionStrings:KvizHub" "Host=localhost;Port=5432;Database=kvizhub;Username=kvizhub;Password=<geslo iz .env>" --project backend/KvizHub.Api
   dotnet user-secrets set "Admin:Email" "admin@kvizhub.si" --project backend/KvizHub.Api
   dotnet user-secrets set "Admin:Password" "<geslo administratorja>" --project backend/KvizHub.Api
   dotnet user-secrets set "Admin:DisplayName" "Administrator" --project backend/KvizHub.Api
   ```

5. Obnovite orodja (`dotnet-ef`) in ustvarite shemo baze:

   ```bash
   dotnet restore
   dotnet tool restore
   dotnet ef database update --project backend/KvizHub.Api
   ```

6. Zaupajte razvojnemu certifikatu HTTPS (samo ob prvem zagonu na računalniku):

   ```bash
   dotnet dev-certs https --trust
   ```

7. Zaženite API, ki streže tudi odjemalca:

   ```bash
   dotnet run --project backend/KvizHub.Api --launch-profile https
   ```

   Ob prvem zagonu se ustvari administratorski račun iz koraka 4.
   
### Naslovi

| Naslov | Namen |
|---|---|
| https://localhost:7268 | aplikacija |
| https://localhost:7268/swagger | Swagger UI za ročno testiranje API-ja (samo v razvojnem okolju) |
| https://localhost:7268/health | preverjanje povezave z bazo |

Aplikacijo je treba zagnati prek HTTPS (profil `https`), ker se piškotek seje pošilja samo prek HTTPS.

### Testi

## Razvojni proces

Razvoj poteka po [GitHub Flow](https://docs.github.com/en/get-started/using-github/github-flow):

1. Iz veje `main` ustvarimo novo vejo z opisnim imenom (npr. `feature/rezervacija-mize`, `docs/adr-007`).
2. Na veji delamo manjše commite in jo potisnemo na GitHub.
3. Odpremo pull request v `main`.
4. Po pregledu spremembe združimo v `main` in vejo izbrišemo.

Veja `main` je zaščitena: spremembe so mogoče samo prek pull requestov, prisilno potiskanje in brisanje veje sta onemogočena, pravila veljajo tudi za administratorja.
