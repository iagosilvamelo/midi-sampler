using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json.Serialization;

namespace MidiSampler.Models;

public partial class PadMapping : ObservableObject
{
    [ObservableProperty]
    [JsonPropertyName("note")]
    private int note;

    [ObservableProperty]
    [JsonPropertyName("audio")]
    private string audioPath = string.Empty;

    [JsonIgnore]
    [ObservableProperty]
    private bool isLearning = false;
}

public class AudioDevice
{
    public int Index { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public override string ToString() => Name;
}

public class MidiMessage
{
    public byte Status { get; set; }
    public byte Data1 { get; set; }
    public byte Data2 { get; set; }
}
