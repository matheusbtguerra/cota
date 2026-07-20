# Cota — Project Scope

**Cota** is a real-time flood monitoring and alert platform for Porto Alegre, focused on the communities most affected by the Guaíba river floods. The name comes from *cota de inundação* (flood threshold) — the number the whole city learned to watch in May 2024.

## 1. Problem

Porto Alegre's riverside and island communities (Arquipélago, Humaitá, Sarandi, Zona Sul) are the first and hardest hit when the Guaíba rises — as seen in the May 2024 flood (5.35m, historic record). Official data exists (ANA/SGB telemetry, Civil Defense bulletins, INMET forecasts) but is scattered across multiple government portals, hard to interpret, and not delivered proactively to the people who need it. Existing monitoring sites show the river level but do not answer the resident's real questions: **"Is my region at risk? Where do I go? How do I get warned in time?"**

## 2. Target users

- **Primary:** residents of flood-prone regions of Porto Alegre, especially peripheral and riverside communities.
- **Secondary:** volunteers and donors looking for shelters and donation points during emergencies; anyone in the metro region tracking flood risk.

## 3. Product goals

1. Translate raw hydrological data into a clear risk status anyone can read in 5 seconds.
2. Deliver alerts only to people who subscribed to affected regions (no noise).
3. Map risk zones, shelters, and donation points in one place.
4. (Portfolio goal) Demonstrate a production-grade Vue 3 + .NET 8 architecture with real-time push via SignalR.

## 4. MVP scope (v1 — in)

| # | Feature | Description |
|---|---------|-------------|
| 1 | River level panel | Current Guaíba level, trend (cm/h, rising/falling), distance to alert (2.5m) and flood (3.0m) thresholds, last-updated timestamp |
| 2 | Level history chart | Last 7/30 days, with threshold lines and 2024 record for context |
| 3 | Rain forecast | Accumulated rain forecast for POA and river headwaters (next 7 days) via Open-Meteo |
| 4 | Risk status | Simple computed status: Normal / Attention / Alert / Flood, derived from level + trend |
| 5 | Interactive map | Risk zones (GeoJSON), official shelters, donation points |
| 6 | Accounts | Email + password registration, JWT auth |
| 7 | Region subscription | User subscribes to one or more regions (Ilhas, Centro/4º Distrito, Zona Sul, Sarandi/Norte...) |
| 8 | Real-time alerts | SignalR push: status changes are sent only to subscribers of the affected regions |

## 5. Out of scope (v1)

- Native mobile app (web responsive / PWA only)
- SMS or WhatsApp notifications (future: costs money, needs approval)
- Crowdsourced flooding reports from users (great v2 feature, needs moderation)
- Coverage beyond Porto Alegre metro
- Historical flood analytics / ML predictions
- Admin panel (seed data via scripts/migrations)

## 6. Data sources

| Source | Data | Access |
|--------|------|--------|
| ANA/SGB telemetry (HidroWeb) | River level, 15-min updates | Public REST API |
| Open-Meteo | Rain forecast | Public REST API, no key |
| INMET | Official weather alerts | Public API/RSS |
| SGB risk mapping / City Hall | Risk zone polygons, shelters | GeoJSON (manual curation into seed data) |

Ingestion strategy: a .NET `BackgroundService` polls sources on schedule (15 min for level), persists snapshots to PostgreSQL, and broadcasts changes via SignalR. The frontend never calls government APIs directly.

## 7. Non-functional requirements

- **Reliability of data:** always show data timestamp and source; degrade gracefully ("data unavailable") when upstream APIs fail — never show stale data as current.
- **Responsibility:** clear disclaimer that official emergency guidance comes from Defesa Civil (199); the app complements, never replaces, official channels.
- **Performance:** panel loads under 2s on a mid-range phone on 4G; mobile-first UI.
- **Accessibility:** readable color contrast, status conveyed by text + color (not color alone).
- **Cost:** run on free tiers (Vercel + Railway/Render + managed Postgres free tier).

## 8. Tech stack

- **Frontend:** Vue 3 (Composition API) + TypeScript + Pinia + Vue Router, Chart.js, MapLibre GL (open-source, no token) or Leaflet
- **Backend:** .NET 8 Web API, EF Core, PostgreSQL, SignalR, JWT auth
- **Infra:** Vercel (front), Railway (API + DB), GitHub Actions (CI: build + tests)

## 9. Success criteria

- A resident can open the site and understand the current risk in under 5 seconds.
- A logged-in user subscribed to "Ilhas" receives a push when — and only when — the Ilhas status changes.
- Deployed and publicly accessible; README in English with architecture diagram.
- At least one real person outside the project uses it during a rain event.

## 10. Risks & mitigations

| Risk | Mitigation |
|------|------------|
| Government API instability or format changes | Ingestion isolated behind an interface; store raw payloads; alerts on ingestion failure |
| Risk-zone GeoJSON hard to obtain | Start with simplified hand-drawn polygons based on 2024 flood maps; refine later |
| Scope creep | Anything not in section 4 goes to the v2 backlog |
| Alarm fatigue / false alarms | Conservative status thresholds; alerts only on status *change*, not every reading |
