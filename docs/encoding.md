# Encoding .rfglocatext files
To convert a JSON file back into the `.rfglocatext` format, run the following within a command line:

`rfg-localization.exe encode <input-path>`

The `.rfglocatext` will be generated in the same directory as the JSON file. If a `.rfglocatext` file with the same name already exists, it will be renamed to a `.rfglocatext.bak` file to preserve the original version.

It is recommended to use [SyncFaction](https://rfg-modding.github.io/SyncFaction) to repack modified `.rfglocatext` files into the game, rather than manually repacking a `.vpp_pc` file.