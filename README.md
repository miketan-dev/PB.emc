# Enhanced Mech Customization - E.M.C.

![Mod Version](https://img.shields.io/badge/Mod%20Version-v2.1.1-blue)
![Game Version](https://img.shields.io/badge/Phantom%20Brigade-%3E%3D%20v2.0-green)
![Framework](https://img.shields.io/badge/Framework-.NET%20v4.7.2-purple)
![Language](https://img.shields.io/badge/Language-C%23%207.3-informational)

| Metadata                 | Details                                                    |
|:-------------------------|:-----------------------------------------------------------|
| **Release Date**         | 2025/12/14                                                 |
| **Update Date**          | TBD                                                        |
| **Mod Version**          | `v2.1.1`                                                   |
| **Repository**           | [GitHub Repository](https://github.com/miketan-dev/PB.emc) |
| **Programming Language** | C# 7.3 (.NET Framework v4.7.2)                             |
| **Minimum Game Version** | >= v2.0                                                    |

---

## CREDITS

- Harmony Framework for the patching
- Phantom Brigade Modding System
- Brace Yourself Games for the awesome game!

---

## MOD STATUS

- **Steam Workshop:** 🟡  
  [Steam Workshop Link - TBA](#)
- **Nexus Mod:** 🟡  
  [Nexus Mod Link - TBA](#)

---

## INSTALLATION (EPIC GAME VERSION)

To install the mod:

1. Extract the mod folder into the following directory:
   <br>```[Drive]:\Users\[yourUser]\AppData\Local\PhantomBrigade\Mods```
   <br><br>
2. Launch the game; the mod will be automatically detected and activated.

> ⚠️ **[DISCLAIMER]** ⚠️
> <br>While the mod has been fully tested by covering most of the use cases, make sure to back up your save file before
> applying the mod to avoid any unintended (and negative) effects.
> <br><br>I will not be held responsible for any misuse of this mod or the damage can cause to
> saves corruption.
> <br>The above code project is made public to adhere
> with [Brace Yourself Games' guidelines](https://braceyourselfgames.com/mod-policy/)
> mostly to certify the present Library Code **DOES NOT CONTAIN** any malware and/or trojan in every form.
> <br><br>You are free to use my mod as a dependency to other mods as long as you give
> credits to me, as this mod is also covered under **BSD-3 Licence**.

---

## Mod intro

**Enhanced Mech Customization (E.M.C.)** is a library mod for *Phantom Brigade* that unlocks advanced hardpoint
customization, enabling dynamic and granular configuration of hardpoints at runtime through the so-called **"hardpoint
candidates"**.

In-game, based on vanilla parts, their hardpoints (such as arms, legs, torso, etc.) spawn already "fused" and
"non-editable", blocking access to the sub-pieces of said sub-parts (head, thigs, lower/upper arm, etc.) which are
invisible to the user; this was seemingly unnecessary during game-design phase.

For this reason, hiding a big potential, **E.M.C.** intercepts equipment generation and unlocks specific hardpoints at
runtime, making them editable in Customization screen and preventing subsystems from being permanently fused to the
part.

The true power of this mod lies in its flexibility: candidate hardpoints for unlocking are not hardcoded, but completely
customizable through a simple text-based YAML configuration file generated only once during mod loading.

---

## Key Features

* **Dynamic Unfuse:** Prevents native fusion of subsystems during part generation (crafting or drops), leaving
  hardpoints empty and ready to accept new modules.
* **Universal Editor:** Makes chosen hardpoints visible and editable within the inventory and Workshop interface.
* **AI Safe (No AI Break):** Designed to unlock editing features exclusively for the player's UI. Enemies generated on
  the battlefield continue to spawn normally with their original, intact, and functional equipment.
* **YAML Configuration:** Add or remove hardpoints you want to make editable simply by editing a text file.

---

## YAML Caching System (configuration)

Inside the mod folder, a configuration file will be generated upon game launch at the following path:):

```path
emc_cache/candidate_hardpoints.yaml
```

which contains a pre-loaded list of hardpoints, as shown in the following code:

```yaml
# [Enhanced Customization Mod - v2.0] 
# [Candidate Hardpoints Utility] 
# © .Miketan - https://github.com/miketan-dev 
#
# ============================================================================================ 
# This configuration file is composed in two sections: 
# 1. candidateHardpoints -> affects normal hardpoint definition to make it editable 
# 2. candidateHardpointsTargeted -> affects hardpoints generation state in part presets 
#
# Add or remove the desired hardpoints to enable or disable them, according to your preference. 
# ============================================================================================ 
data:
  candidateHardpoints:
  - external_arm_lower
  - external_arm_upper
  - external_bottom_left_lower
  - external_bottom_right_lower
  - external_bottom_left_upper
  - external_bottom_right_upper
  - external_top_head
  - external_top_pelvis
  candidateHardpointsTargeted:
  - external_arm_lower
  - external_arm_upper
  - external_bottom_left_lower
  - external_bottom_right_lower
  - external_bottom_left_upper
  - external_bottom_right_upper
  - external_top_head
  - external_top_pelvis
```

<br>The caching system will grant the user total control of which hardpoint can be enabled to be unfused or not
config-wise.<br>
While the logic revolves around unlocking body parts in the first place, the user have the total freedom to add/remove
as many hardpoints as he/she wants.
<br><bR>A notoriously good example can be the development of custom hardpoint mods that can enable this mod to be
interoperable with them. Althought a huge potential, said use case may work but NOT OFFICIALLY GUARANTEED/TESTED (in
case, report to me in case of bug or unexpected behavior).