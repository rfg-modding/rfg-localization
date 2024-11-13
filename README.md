# Localization Editor
A tool to convert .rfglocatext (localization string) files to and from JSON. Created for [Red Faction: Guerrilla Re-Mars-tered](https://www.redfactionwiki.com/wiki/Red_Faction:_Guerrilla).

## Usage
Localization string files can be found within the `misc.vpp_pc` file in your game root's `data` folder. Use [RfgUtil](https://github.com/Moneyl/RfgUtil) to unpack vpp_pc files.
* To convert .rfglocatext to .json, run `rfg-localization.exe decode <input_path>`.
* To convert .json back to .rfglocatext, run `rfg-localization.exe encode <input_path>`.
