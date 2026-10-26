---
# Status: predlagano | sprejeto | zavrnjeno | opuščeno | nadomeščeno z [ADR-NNN](NNN-naslov.md)
status: "{predlagano | sprejeto | zavrnjeno | opuščeno | nadomeščeno z ADR-NNN}"
# Datum zadnje spremembe odločitve (YYYY-MM-DD)
date: {YYYY-MM-DD}
# Kdo je odločitev sprejel
decision-makers: {seznam oseb}
# Neobvezno: ADR, ki ga ta odločitev nadomešča
supersedes: "{ADR-NNN | /}"
---

<!--
  Predloga temelji na minimalni predlogi MADR 4.0
  (https://github.com/adr/madr/blob/develop/template/adr-template-minimal.md),
  dopolnjeni z metapodatki iz polne predloge in prevedeni v slovenščino.

  Pravila uporabe:
  - Datoteke so oštevilčene zaporedno: 001-kratek-naslov.md, 002-...
  - Sprejetega ADR-ja ne spreminjamo. Če se odločitev spremeni, ustvarimo nov ADR,
    v njem izpolnimo "supersedes", v starem pa status nastavimo na "nadomeščeno z ADR-NNN".
  - ADR opisuje, ZAKAJ je bila odločitev sprejeta, ne le katera tehnologija je izbrana.
-->

# {Kratek naslov, ki povzame rešeni problem in izbrano rešitev}

## Kontekst in opis problema

{Opišite kontekst in problem, npr. v dveh do treh stavkih ali kot kratko zgodbo.
Problem lahko zapišete tudi kot vprašanje in dodate povezave na povezane dokumente ali naloge.}

## Obravnavane možnosti

* {naslov možnosti 1}
* {naslov možnosti 2}
* {naslov možnosti 3}
* … <!-- število možnosti je poljubno -->

## Odločitev

Izbrana možnost: "{naslov možnosti 1}", ker {utemeljitev, npr. edina izpolni ključno zahtevo | razreši {problem} | se na podlagi primerjave izkaže kot najboljša}.

### Posledice

* Dobro, ker {pozitivna posledica, npr. izboljšanje ene ali več želenih lastnosti}
* Slabo, ker {negativna posledica, npr. poslabšanje ene ali več želenih lastnosti}
* … <!-- število posledic je poljubno -->
