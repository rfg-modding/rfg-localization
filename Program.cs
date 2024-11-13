using LocalizationFile3;
using System.CommandLine;

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
                new Argument<string>("input_path", "Path to a .rfglocatext file")
            };
            decodeCommand.SetHandler(DecodeFile, (Argument<string>)decodeCommand.Arguments[0]);

            // Encode
            var encodeCommand = new Command("encode", "Write a JSON to a .rfglocatext file.")
            {
                new Argument<string>("input_path", "Path to a JSON file"),
            };
            encodeCommand.SetHandler(EncodeFile, (Argument<string>)encodeCommand.Arguments[0]);

            rootCommand.AddCommand(decodeCommand);
            rootCommand.AddCommand(encodeCommand);

            await rootCommand.InvokeAsync(args);
        }

        static void DecodeFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found.", filePath);
            }

            try
            {
                using (var stream = File.OpenRead(filePath))
                using (var reader = new BinaryReader(stream))
                {
                    var localizationFile = new LocalizationFile();
                    localizationFile.Read(reader);
                    localizationFile.ConvertToJson(Path.ChangeExtension(filePath, ".json"));

                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while decoding the file: {ex.Message}", ex);
            }
        }
        
        static void EncodeFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found.", filePath);
            }

            string outputFilePath = Path.ChangeExtension(filePath, ".rfglocatext");

            if (File.Exists(outputFilePath))
            {
                File.Move(outputFilePath, Path.ChangeExtension(outputFilePath, ".rfglocatext.bak"));
            }

            try
            {
                LocalizationFile.ConvertFromJson(filePath, outputFilePath);
            }
            catch (Exception ex) 
            {
                throw new Exception($"An error occurred while encoding the file: {ex.Message}", ex);
            }
        }
    }
}