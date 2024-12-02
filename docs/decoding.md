# Decoding .rfglocatext files
Localization string files are located within the `misc.vpp_pc` file in the game root's `data` folder. To extract them, use the [RfgUtil](https://github.com/moneyl/RfgUtil) tool to unpack `.vpp_pc` files.

To convert a `.rfglocatext` file to XML, drag-and-drop the file onto the program. Optionally, run the following within a command line: 

`rfg-localization.exe <input>`

An XML file will be generated with contents similar to the snippet below:
```xml
<root>
    <Entry>
      <Identifier />
      <Hash>1473498024</Hash>
      <String>THE EDF ARE HOLDING ANOTHER PERSON IN THEIR HEADQUARTERS. I NEED TO GET HIM OUT.</String>
    </Entry>
    <Entry>
      <Identifier />
      <Hash>3470432786</Hash>
      <String>HE'S NOT DOING SO HOT...</String>
    </Entry>

    [...]
</root>
```

For further information on modifying strings, continue to the [Editing](editing.md) page.

## Specifying output file (optional)
Use the `--output` option (or its alias `-o`) to specify the file's output location when decoding/encoding. Run the following within a command line:

`rfg-localization.exe <input> -o <output>`

## Extracting string identifiers (optional)
String identifiers can be extracted to simplify locating specific strings by their identifiers in XTBL files.

By default, the `Identifier` fields in the decoded file are empty because identifiers must be scraped from XTBL files within `.vpp_pc` archives.

To extract string identifiers during decoding, use the `--xtbldir` option (or its alias `-x`):

`rfg-localization.exe <input> -x <directory>`

`<directory>` should point to a folder containing unpacked `.vpp_pc` files. For the most comprehensive results, ensure that `dlcp(01-03)_misc.vpp_pc` files are also unpacked. If RfgUtil is being used, the tool automatically unpacks these files into an `Unpack` folder, which can be used as the directory for scraping. The process is recursive and will search through all directories.

With identifiers extracted, the decoded file will look like this:
```xml
<root>
  <Entry>
    <Identifier>D01DAN_ZZZ_MIS_SPE_DLC_MIS03_DDAN_CLR_01</Identifier>
    <Hash>1473498024</Hash>
    <String>THE EDF ARE HOLDING ANOTHER PERSON IN THEIR HEADQUARTERS. I NEED TO GET HIM OUT.</String>
  </Entry>
  <Entry>
    <Identifier>D01DAN_ZZZ_MIS_SPE_DLC_MIS03_DDAN_CLR_02</Identifier>
    <Hash>3470432786</Hash>
    <String>HE'S NOT DOING SO HOT...</String>
  </Entry>

  [...]
</root>
```

### Additional information
String identifiers are in most XTBL files, although many are hardcoded and cannot be scraped (e.g. game dialogue and menu text). In-game, these identifiers are converted into hash values, which get matched to their corresponding localization strings to display text in-game.

For example, the file `guerrilla_handbook.xtbl` uses identifiers:
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
String identifiers always follow an uppercase naming convention with underscores replacing spaces. In the above example, only the `<Title>` and `<Description>` fields contain string identifiers. Fields like `<Category>` may look similar but are not identifiers.
