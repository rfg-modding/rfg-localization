# Editing .rfglocatext files
After [decoding](decoding.md) an `.rfglocatext` file, it becomes an editable XML file.

## Modifying existing entries
To modify the string of an entry, update the respective `String` field with the desired text. 

If desired, the Identifier of an existing entry can be updated and its Hash value will be re-calculated. It isn't recommended to modify the Identifier of a base game entry.

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

```xml
  <Entry>
    <Identifier></Identifier>
    <String></String>
  </Entry>
```

### Best practices

- Add new entries at the end of the file to separate them from the base game entries.
- Do not include the `Hash` field for new entries, as it will be calculated automatically when the file is re-encoded.
- Ensure that each identifier follows the standard naming convention.

For more details, refer to the [Decoding](decoding.md) page.