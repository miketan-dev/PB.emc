def update_readme():
content = """# 🛠️ PB.emc (Equipment Modification Core)
Una mod per **Phantom Brigade** che sblocca la personalizzazione avanzata degli hardpoint, permettendoti di configurare dinamicamente quali moduli e sottosistemi possono essere modificati.

---

## 📖 Descrizione

Nel gioco base, molti hardpoint delle parti (braccia, gambe, torso) nascono "fusi" (fused) e non editabili, bloccando la creatività del giocatore nella costruzione del mech perfetto.
**PB.emc** intercetta la generazione degli equipaggiamenti e sblocca specifici hardpoint, rendendoli editabili nel Workshop e impedendo che i sottosistemi vengano fusi permanentemente al pezzo.

Il vero potere di questa mod risiede nella sua **flessibilità**: gli hardpoint candidati allo sblocco non sono decisi a priori dal codice, ma sono **completamente personalizzabili dall'utente** tramite un semplice file di configurazione testuale.

---

## ✨ Funzionalità Principali

*   **Sblocco Dinamico (Unfuse):** Impedisce la fusione nativa dei sottosistemi durante la generazione del pezzo (crafting o drop), lasciando gli hardpoint vuoti e pronti ad accogliere nuovi moduli.
*   **Editor Universale:** Rende visibili e modificabili gli hardpoint scelti all'interno dell'interfaccia dell'inventario e del Workshop.
*   **Sicuro per l'IA (No AI Break):** La mod è progettata per sbloccare le funzionalità di editing solo per l'interfaccia utente del giocatore. I nemici generati sul campo di battaglia continueranno a spawnare regolarmente con i loro equipaggiamenti intatti e funzionanti, senza bug.
*   **Configurazione via YAML:** Aggiungi o rimuovi gli hardpoint che vuoi rendere editabili semplicemente modificando un file testuale.

---

## ⚙️ Come usare la configurazione (YAML)

All'interno della cartella della mod, troverai un file di configurazione situato in:
`emc_cache/candidate_hardpoints.yaml`

Puoi aprire questo file con un qualsiasi editor di testo (come Blocco Note, VS Code o Notepad++). Il file si presenta così:

```yaml
data:
  candidateHardpoints:
  - external_arm_lower
  - external_arm_upper
  - external_bottom_left_lower
  # ... aggiungi altri qui
  
  candidateHardpointsTargeted:
  - external_arm_lower
  - external_arm_upper
  - external_bottom_left_lower
  # ... aggiungi altri qui