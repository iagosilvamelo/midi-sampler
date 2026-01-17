using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using MidiSampler.Models;

namespace MidiSampler.Services;

public class StorageService
{
    private const string MappingsFileName = "mappings.json";
    
    public List<PadMapping> LoadMappings()
    {
        try
        {
            if (File.Exists(MappingsFileName))
            {
                string json = File.ReadAllText(MappingsFileName);
                var mappings = JsonSerializer.Deserialize<List<PadMapping>>(json) ?? new();
                Debug.WriteLine($"✓ {mappings.Count} mapeamentos carregados");
                return mappings;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"⚠️ Erro ao carregar mapeamentos: {ex.Message}");
        }
        
        return new();
    }

    public void SaveMappings(List<PadMapping> mappings)
    {
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(mappings, options);
            File.WriteAllText(MappingsFileName, json);
            Debug.WriteLine($"✓ {mappings.Count} mapeamentos salvos");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ Erro ao salvar mapeamentos: {ex.Message}");
        }
    }
}
