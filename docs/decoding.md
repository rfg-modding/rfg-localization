# Decoding .rfglocatext files
Localization string files are located within the `misc.vpp_pc` file in the game root's `data` folder. To extract them, use the [RfgUtil](https://github.com/moneyl/RfgUtil) tool to unpack `.vpp_pc` files.

To convert a `.rfglocatext` file to JSON format, run the following within a command line: 

`rfg-localization.exe decode <input-path>`

This will generate a JSON file in the same directory as the input file. Its contents should look similar to this:
```json
[
  {
    "Identifier": "",
    "Hash": 1473498024,
    "String": "THE EDF ARE HOLDING ANOTHER PERSON IN THEIR HEADQUARTERS. I NEED TO GET HIM OUT."
  },
  {
    "Identifier": "",
    "Hash": 3470432786,
    "String": "HE'S NOT DOING SO HOT..."
  },

  [...]
]
```

For further information on modifying strings, continue to the [Editing](editing.md) page.

## Extracting string identifiers (optional)
String identifiers can be extracted to simplify locating specific strings by their identifiers in XTBL files.

By default, the `Identifier` fields in the decoded JSON are empty because identifiers need to be automatically scraped from XTBL files within the game's `.vpp_pc` archives.

To extract string identifiers during decoding, use the `--load-xtbl` option (or its aliases `-x` and `--xtbl`):

`rfg-localization.exe decode <input-path> --load-xtbl <directory-path>`

`<directory-path>` should point to a folder containing unpacked `.vpp_pc` files. For the most comprehensive results, ensure that `dlcp(01-03)_misc.vpp_pc` files are also unpacked. If you are using RfgUtil, the tool automatically unpacks these files into an `Unpack` folder, which can be used as the directory for scraping. The process is recursive and will search through all unpacked `.vpp_pc` files.

With string identifiers extracted, the decoded JSON file will look like this:
```json
[
  {
    "Identifier": "D01DAN_ZZZ_MIS_SPE_DLC_MIS03_DDAN_CLR_01",
    "Hash": 1473498024,
    "String": "THE EDF ARE HOLDING ANOTHER PERSON IN THEIR HEADQUARTERS. I NEED TO GET HIM OUT."
  },
  {
    "Identifier": "D01DAN_ZZZ_MIS_SPE_DLC_MIS03_DDAN_CLR_02",
    "Hash": 3470432786,
    "String": "HE'S NOT DOING SO HOT..."
  },

  [...]
]
```

### Additional information
String identifiers are present in most XTBL files, although some are hardcoded and cannot be scraped, such as certain dialogue and menu text. In-game, these identifiers are converted into hash values, which are then matched to their corresponding localization strings to display text in-game.

For example, the file `guerrilla_handbook.xtbl` contains many string identifiers:
```xml
<Handbook_Entry>
		<Name>Ammo Boxes</Name>
		<Title>MENU_GH_AMMO_BOX</Title>
		<Description>MENU_GH_AMMO_BOX_DESC</Description>
		<Unlocked>True</Unlocked>
		<_Editor>
			<Category>MENU_COMBAT</Category>
			</_Editor>
		<Image>
			<Filename>ui_handbook_ammo.tga</Filename>
			</Image>
		<Info>ui_hud_icon_ammo_box</Info>
</Handbook_Entry>
```
String identifiers always follow an uppercase naming convention with underscores replacing spaces. In the above example, only the `<Title>` and `<Description>` fields contain string identifiers. Fields like `<Category>` might look similar but are not string identifiers.
