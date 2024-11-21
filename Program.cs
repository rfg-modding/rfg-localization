using System.CommandLine;
using RFGM.Formats.Localization;

namespace rfg_localization
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var rootCommand = new RootCommand("Localization file tool for Red Faction: Guerrilla Re-Mars-tered");

            // Decode
            var decodeCommand = new Command("decode", "Write a .rfglocatext file into JSON.")
            {
                new Argument<string>("input-path", "Path to a .rfglocatext file"),
                new Option<string>(["-x", "--xtbl", "--load-xtbl"],
                    () => string.Empty,
                    "Used for scraping identifiers. Requires path to unpacked misc and/or dlcp##_misc .vpp_pc files.")
                    { IsRequired = false }
            };
            decodeCommand.SetHandler(DecodeFile, (Argument<string>)decodeCommand.Arguments[0], (Option<string>)decodeCommand.Options[0]);

            // Encode
            var encodeCommand = new Command("encode", "Write a JSON to a .rfglocatext file.")
            {
                new Argument<string>("input-path", "Path to a JSON file"),
            };
            encodeCommand.SetHandler(EncodeFile, (Argument<string>)encodeCommand.Arguments[0]);

            rootCommand.AddCommand(decodeCommand);
            rootCommand.AddCommand(encodeCommand);

            await rootCommand.InvokeAsync(args);
        }

        static void DecodeFile(string inputPath, string xtblPath)
        {
            if (!File.Exists(inputPath))
            {
                throw new FileNotFoundException("File not found.", inputPath);
            }

            try
            {
                using var stream = File.OpenRead(inputPath);
                using var reader = new BinaryReader(stream);
                var localizationFile = new LocalizationFile();
                localizationFile.Read(reader, xtblPath);
                localizationFile.ConvertToJson(Path.ChangeExtension(inputPath, ".json"));
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while decoding the file: {ex.Message}", ex);
            }
        }

        static void EncodeFile(string inputPath)
        {
            if (!File.Exists(inputPath))
            {
                throw new FileNotFoundException("File not found.", inputPath);
            }

            string outputPath = Path.ChangeExtension(inputPath, ".rfglocatext");
            string backupPath = Path.ChangeExtension(outputPath, ".rfglocatext.bak");

            if (File.Exists(outputPath))
            {
                if (File.Exists(backupPath))
                {
                    File.Delete(backupPath);
                }
                File.Move(outputPath, backupPath);
            }

            try
            {
                LocalizationFile.ConvertFromJson(inputPath, outputPath);
            }
            catch (Exception ex) 
            {
                throw new Exception($"An error occurred while encoding the file: {ex.Message}", ex);
            }
        }
    }
}