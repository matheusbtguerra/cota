# Cota — Design System

Visual identity and UI guidelines for Cota. Product language is Portuguese (target users are Porto Alegre residents); code and docs are in English.

## 1. Principles

1. **Readable in 5 seconds.** The current risk must be understandable at a glance, by anyone, on a phone, possibly in a stressful situation.
2. **Calm by default, loud when it matters.** The interface stays sober (blues, neutrals) so that status colors carry real weight when they appear.
3. **Never color alone.** Every status is conveyed by color + text (+ icon where possible). Accessibility is a requirement, not a nice-to-have.
4. **Every number has a timestamp and a source.** Trust is the product.

## 2. Color palette

### Brand

| Token | Hex | Usage |
|-------|-----|-------|
| `--cota-blue` | `#0F5C8C` | Primary: buttons, links, active states, app icon |
| `--cota-deep` | `#0B2E44` | Header bar, strong headings |
| `--cota-light` | `#8FBFDD` | Water fill in charts/gauge, soft backgrounds |
| `--cota-mist` | `#F5F8FA` | Page background |

### Status (the core semantic scale)

| Status | Hex | Threshold | Text on fill |
|--------|-----|-----------|--------------|
| Normal | `#1D9E75` | below 2.0 m | `#E1F5EE` |
| Atenção | `#E3A008` | 2.0 – 2.5 m | `#412402` |
| Alerta | `#D85A30` | 2.5 – 3.0 m | `#FAECE7` |
| Inundação | `#B42323` | 3.0 m and above | `#FCEBEB` |

Rules:
- Status changes trigger alerts; readings alone never do.
- Four distinct hues (green / amber / burnt orange / red) — Alerta is orange, not a second red, so all four remain distinguishable.
- Status is always rendered as a pill with the status name inside, never as a bare colored dot in primary UI (small dots allowed in dense lists, always beside text).

### Neutrals

| Token | Hex | Usage |
|-------|-----|-------|
| `--text-strong` | `#22313D` | Primary text |
| `--text-soft` | `#5E7180` | Secondary text, captions |
| `--surface` | `#FFFFFF` | Cards |
| `--border` | `#E2E8ED` | Hairline borders (0.5–1px) |

### Map layers

| Layer | Style |
|-------|-------|
| Risco muito alto | `#B42323` at 40% opacity |
| Risco alto | `#D85A30` at 35% opacity |
| Risco moderado | `#E3A008` at 35% opacity |
| Abrigo (shelter) | `#0F6E56` pin, 2px white stroke |
| Ponto de doação | `#534AB7` pin, 2px white stroke |

Pins always carry a white stroke so they stay visible over any layer.

## 3. Typography

- **Font:** [Inter](https://rsms.me/inter/) (free, variable). Fallback: system-ui, sans-serif.
- **Tabular numerals** (`font-variant-numeric: tabular-nums`) on the level display and any live-updating number, so digits don't shift as values change.
- Scale: 40px (hero level number) / 20px (h1) / 16px (h2) / 14px (body) / 12px (captions, legends). Weights: 400 and 500 only.

## 4. Key screens

### 4.1 Home / hero — "a régua"
- Vertical water column (SVG) with the live level; water rises/falls with data.
- Reference lines on the ruler: 2.5 m alerta (orange), 3.0 m inundação (red, solid), 5.35 m May 2024 record (red, dashed) — the historic mark gives emotional context.
- Beside the gauge: level number (40px, tabular), trend in cm/h with direction icon, station name + "updated X min ago", distance to flood threshold in plain words ("Faltam 2,67 m para a cota de inundação"), rain forecast summary.
- Status pill in the header, always visible.

### 4.2 Map
- Filter chips: Zonas de risco / Abrigos / Pontos de doação (toggleable layers).
- Legend always visible below the map.
- Current status pill repeated in this screen's header.

### 4.3 Minhas regiões (subscriptions)
- List of regions, each with: status dot + name + current status text + subscribe toggle.
- Copy: "Você recebe alerta quando o status de uma região assinada mudar."
- Saving preferences = joining/leaving SignalR groups server-side.

### 4.4 Alert (push/in-app notification)
- Format: "{Região} mudou para {Status}" + one action line: level, trend, nearest shelter.
- Emergency disclaimer visible in-app: "Em emergência, ligue 199 (Defesa Civil). Este app complementa e não substitui os canais oficiais."

## 5. Voice and copy

- Portuguese, plain language, no jargon ("cota de inundação" is the only technical term, and the product teaches it).
- Sentence case, short sentences, verb-first buttons ("Salvar preferências", "Ver abrigos").
- Numbers use Brazilian formatting (comma decimal: "2,70 m").
- Never alarmist adjectives; the data speaks. "Nível em 2,70 m e subindo 4 cm/h" — not "PERIGO!".
