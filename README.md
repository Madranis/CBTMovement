# CBT Movement Core

CBT Movement Core is a fork of the CBT Movement mod for HBS's BATTLETECH game.  It is an attempt to bring Classic Battletech Tabletop movement rules flavor into HBS's BATTLETECH game.  Features include:

- Sprinting no longer ends the turn
- Any movement now incurs a +1 ToHit Penalty
- Sprinting incurs an addtional +1 ToHit Penalty
- Jumping incures an additional +2 ToHit Penalty
- ToHit modifiers are allowed to go below your base to hit chance, making something easier to hit if you stack you modifiers right

The way movement currently works in the game is that ToHitSelfWalk modifiers are applied whenever you make any movement.  So Sprinting, for example, will have a +1 for movement and an additional +1 for sprinting, bringing it in line with the original Tabletop rules of +2.  The same applies to the Jump ToHit Modifiers.

Unlike the original CBT Movement, CBT Movement Core does not change evasion behavior.  This allows the ToHit options and Sprinting no longer ending a turn features of CBT Movement to be used with either the base game's evasion rules or used with more powerful evasion mods like Semi-Permanent Evasion.

Additionally, CBT Movement Core does not force missions into "interleaved" mode at all times like the original CBT Movement.  The interleaved mode is the normal combat mode when enemies are detected.  Forcing interleaved mode meant losing the faster movement behavior when no enemies were nearby.  Forcing interleaved mode is also known to break completion of the Locura story mission.  There is an option to force interleaved behavior back on should someone want it. (It is off by default.)


## Installation

Install [BTML](https://github.com/Mpstark/BattleTechModLoader) and [ModTek](https://github.com/Mpstark/ModTek). Extract files to `BATTLETECH\Mods\CBTMovement\`.

## Configuration

`mod.json` contains movement ToHit modifier for Jumping.  Walk and Sprint movement ToHit modifiers are found in the StreamingAssests\data\constants\CombatGameConstants.json file
