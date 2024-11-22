# Editing .rfglocatext files
After [decoding](decoding.md) an `.rfglocatext` file, it becomes an editable JSON file.

## Modifying existing strings
To modify an existing localization string, update the `String` field in the JSON file with the desired text.

**Note:** When editing the JSON file, certain characters require escape sequences to be correctly formatted, for example:
- Double quotes: `\"`
- Newlines: `\\n`
- White-space characters: `\\s`

### Markup syntax
Various markup codes can be used to modify text appearance or insert elements into strings. Each markup syntax code affects only the text written after it.

Additionally, format tags allow for customizing specific sections of a string. Wrap the desired section with `[format][/format]` to apply any formatting within it. For example: `"[format][color:#ff8010]This is orange-colored text.[/format] And this isn't colored text."`

Below is a list of known supported markup codes:

- `[color:#HEX_VALUE]` - Changes the color of the text or element.

  - Accepts RGB or RGBA color codes.
  - Example: `"[color:#ff8010]This is orange-colored text."`

- `[image:IMAGE_NAME]` - Replaces the text placeholder with an icon.
  
  - Example: `"[image:UI_HUD_GUERILLA_LOGO_WHITE] GUERRILLA REINFORCEMENTS"`

- `[image_tint:IMAGE_NAME]` - Similar to `[image]`, but affected by color codes.

- `[scale:NUM]` - Adjusts the size of the text or element. 

  - The default value is `1.0`.
  - Example: `"[scale:0.9][image:UI_CTRL_360_DPAD_D]"`

- `[button:BUTTON_NAME]` - Displays the key for the specified button name.

  - Example: `"PRESS [button:ZOOM] TO ENTER FINE AIM MODE WITH RANGED WEAPONS."`

## Adding new strings
To add a new localization entry, use the following format:

```json
  {
    "Identifier": "",
    "String": ""
  },
```

### Best practices

- Add new entries at the end of the JSON file to separate them from the base game entries.
- Do not include the `Hash` field for new entries, as it will be calculated automatically when the file is encoded back into `.rfglocatext` format.
- Write the new identifier into an XTBL file before encoding the JSON. 

### Important notes

- Ensure that each new identifier follows the correct naming convention.
- For more details, refer to the [Decoding](decoding.md) page.