# TODO

## Extra Refactoring
- [ ] Extract World data and graphics
- [ ] Decouple parts of the GUI?
- [ ] Decouple parts of the GameMaster
- [ ] Create Unit Tests
- [X] Serialize Abilities into files
- [X] Decouple Ability Database from specific implementation
- [X] Redesign of Iventory/Items (remove dependecy on Type string checking)
- [X] Decouple PlayerUnitEntityController from ItemActions
- [ ] More formally define GUIMaster's responsiblility
- [ ] Reduce unnecessary dependencies on GameMaster

## Better Visuals?
- [ ] Improve Ability indicators
- [ ] Improve automatic unit panel/info update
- [ ] Improve tooltips
- [ ] Drag windows
- [ ] Fix resizing

## Turn One to Ten
- [X] City vision
- [X] Level up
- [X] Display Level and EXP
- [ ] Unit Fortify and Resting
- [X] Equipment
  - [X] Unit Inventory
  - [X] Utility Items
  - [X] Weapons & Armor
  - [X] Visual
  - [X] Unequip ItemAction
- [X] Loot
- [ ] Enemy target cities
- [ ] Classes
- [ ] Skill Unlocking
- [ ] Redesign of Population mechanic
- [ ] Basic Construction
  - [ ] Construction Slots

## Refactor Cities/Construction
- [ ] Move Project data to ScriptableObject and a interfaced singleton
- [X] Refactor ProjectSelectionManager
- [X] Split Project details into two (controller for selection and data for game logic)
- [ ] General refactoring/extracting of construction code
- [ ] Refactor MonoBehaviours to be more decoupled
- [ ] Get rid of unavailable projects

## Turn Ten to Twenty-Five
- [ ] Crafting
  - [ ] Forging
  - [ ] Enchanting
  - [ ] Refining
  - [ ] Alchemy
- [ ] NPC Clans

## Other
- [ ] Faction identification
- [ ] Natural Regen
- [ ] Status Effects
- [ ] More Abilities
- [ ] Make Unity editor work during playtime