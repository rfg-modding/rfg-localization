using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LocalizationFile3
{
    public class LocalizationEntry
    {
        [JsonInclude]
        public uint KeyHash { get; private set; }

        [JsonIgnore]
        public uint Offset { get; internal set; }

        [JsonIgnore]
        public uint Length { get; internal set; }

        [JsonInclude]
        public string Text { get; private set; } = string.Empty;

        public LocalizationEntry() { }

        public LocalizationEntry(BinaryReader reader)
        {
            KeyHash = reader.ReadUInt32();
            Offset = reader.ReadUInt32();
            Length = reader.ReadUInt32();
        }

        public void LoadString(BinaryReader reader)
        {
            reader.BaseStream.Seek(Offset, SeekOrigin.Begin);
            byte[] stringBytes = reader.ReadBytes((int)Length);
            Text = Encoding.Unicode.GetString(stringBytes).TrimEnd('\0');
        }
    }

    public class LocalizationFile
    {
        private const uint ExpectedSignature = 2823585651;
        private const uint ExpectedVersion = 3;

        // Header
        public uint Signature { get; private set; }
        public uint Version { get; private set; }
        public uint NumStrings { get; private set; }

        // Data
        public List<LocalizationEntry> Entries { get; private set; } = new List<LocalizationEntry>();

        public void Read(BinaryReader reader)
        {
            ValidateHeader(reader);

            // Read entry metadata
            for (int i = 0; i < NumStrings; i++)
            {
                var entry = new LocalizationEntry(reader);
                Entries.Add(entry);
            }

            // Load entry strings
            foreach (var entry in Entries)
            {
                entry.LoadString(reader);
            }
        }

        // Write data back to .rfglocatext using list of LocalizationEntry3
        public static void Write(string filePath, List<LocalizationEntry> entries)
        {
            using (var reader = new BinaryWriter(File.Open(filePath, FileMode.Create)))
            {
                var localizationFile = new LocalizationFile
                {
                    Signature = ExpectedSignature,
                    Version = ExpectedVersion,
                    NumStrings = (uint)entries.Count,
                    Entries = entries
                };

                // Write header
                reader.Write(localizationFile.Signature);
                reader.Write(localizationFile.Version);
                reader.Write(localizationFile.NumStrings);

                // Calculate entry offsets and lengths
                uint offset = (uint)(12 + entries.Count * 12);
                foreach (var entry in entries)
                {
                    entry.Offset = offset;
                    entry.Length = (uint)Encoding.Unicode.GetByteCount(entry.Text + "\0");
                    offset += entry.Length;
                }

                // Write entry metadata
                foreach (var entry in entries)
                {
                    reader.Write(entry.KeyHash);
                    reader.Write(entry.Offset);
                    reader.Write(entry.Length);
                }

                // Write entry text
                foreach (var entry in entries)
                {
                    var textBytes = Encoding.Unicode.GetBytes(entry.Text + "\0");
                    reader.Write(textBytes);
                }
            }
        }


        private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        public void ConvertToJson(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("File path cannot be null or empty", nameof(filePath));
            }

            if (Entries == null)
            {
                throw new InvalidOperationException("Entries cannot be null");
            }

            string json = JsonSerializer.Serialize(Entries, _jsonSerializerOptions);

            try
            {
                File.WriteAllText(filePath, json);
            }
            catch (IOException ex)
            {
                throw new IOException($"Failed to write JSON to file: {filePath}", ex);
            }
        }

        public static void ConvertFromJson(string jsonFilePath, string outputFilePath)
        {
            var json = File.ReadAllText(jsonFilePath);
            var localizationEntries = JsonSerializer.Deserialize<List<LocalizationEntry>>(json);
            if (localizationEntries == null || localizationEntries.Count == 0)
            {
                throw new InvalidOperationException("The JSON file is empty or could not be deserialized.");
            }
            Write(outputFilePath, localizationEntries);
        }

        private void ValidateHeader(BinaryReader reader)
        {
            Signature = reader.ReadUInt32();
            Version = reader.ReadUInt32();
            NumStrings = reader.ReadUInt32();

            if (Signature != ExpectedSignature)
                throw new FormatException($"Invalid file signature. Expected {ExpectedSignature}, but found {Signature}");

            if (Version != ExpectedVersion)
                throw new FormatException($"Invalid file version. Expected {ExpectedVersion}, but found {Version}");
        }
    }
}