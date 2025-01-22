using McMaster.Extensions.CommandLineUtils;
using RFGM.Formats.Localization;
using System.ComponentModel.DataAnnotations;

namespace rfg_localization
{
    internal class Program
    {
        [Argument(0, Name = "input", Description = "Path to .rfglocatext or .xml file")]
        [Required]
        public string Input { get; } = "";

        [Option("-o|--output", Description = "Path to output file")]
        public string Output { get; } = "";

        [Option("-x|--xtbldir", Description = "Path to directory of unpacked misc and/or dlcp##_misc .vpp_pc files")]
        public string XtblDir { get; } = "";

        private void OnExecute()
        {
            string fileExtension = Path.GetExtension(Input);

            //if (fileExtension == ".rfglocatext")
            //{
            //    Console.WriteLine(".rfglocatext detected, decoding '{0}'", Input);
            //    DecodeFile(Input, Output, XtblDir);
            //}
            //else if (fileExtension == ".xml")
            //{
            //    Console.WriteLine(".xml detected, encoding '{0}'", Input);
            //    EncodeFile(Input, Output);
            //}
            //else
            //{
            //    throw new ArgumentException($"Unrecognized file extension '{fileExtension}'");
            //}

            switch (fileExtension)
            {
                case ".rfglocatext":
                    Console.WriteLine(".rfglocatext detected, decoding '{0}'", Input);
                    DecodeFile(Input, Output, XtblDir);
                    break;
                case ".xml":
                    Console.WriteLine(".xml detected, encoding '{0}'", Input);
                    EncodeFile(Input, Output);
                    break;
                default:
                    throw new ArgumentException($"Unrecognized file extension '{fileExtension}'");
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("rfg-localization 0.3.0\nLocalization tool for Red Faction: Guerrilla Re-Mars-tered.\n");
            CommandLineApplication.Execute<Program>(args);
        }

        static void DecodeFile(string inputPath, string outputPath, string xtblPath)
        {
            if (!File.Exists(inputPath))
            {
                throw new FileNotFoundException("File not found.", inputPath);
            }

            outputPath = string.IsNullOrEmpty(outputPath) ? Path.ChangeExtension(inputPath, ".xml") : outputPath;

            try
            {
                var localizationFile = new LocalizationFile();

                using var readStream = File.OpenRead(inputPath);
                localizationFile.Read(readStream, xtblPath);

                using var saveStream = File.Create(outputPath);
                localizationFile.WriteToXml(saveStream);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while decoding the file: {ex.Message}", ex);
            }
        }

        static void EncodeFile(string xmlPath, string outputPath)
        {
            if (!File.Exists(xmlPath))
            {
                throw new FileNotFoundException("File not found.", xmlPath);
            }

            outputPath = string.IsNullOrEmpty(outputPath) ? Path.ChangeExtension(xmlPath, ".rfglocatext") : outputPath;

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
                LocalizationFile localizationFile = new();

                using var readStream = File.OpenRead(xmlPath);
                localizationFile.ReadFromXml(readStream);

                using var saveStream = File.Create(outputPath);
                localizationFile.Write(saveStream);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while encoding the file: {ex.Message}", ex);
            }
        }
    }
}