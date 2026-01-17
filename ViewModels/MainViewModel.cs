using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MidiSampler.Models;
using MidiSampler.Services;
using Microsoft.Win32;

namespace MidiSampler.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly MidiService _midiService = new();
    private readonly AudioService _audioService = new();
    private readonly StorageService _storageService = new();

    [ObservableProperty]
    private ObservableCollection<string> midiInputs = new();

    [ObservableProperty]
    private string? selectedMidiInput;

    [ObservableProperty]
    private ObservableCollection<PadMapping> padMappings = new();

    [ObservableProperty]
    private ObservableCollection<AudioDevice> audioDevices = new();

    [ObservableProperty]
    private AudioDevice? selectedAudioDevice;

    [ObservableProperty]
    private string statusMessage = "Inicializando...";

    [ObservableProperty]
    private int lastNoteNumber = -1;
    
    public MainViewModel()
    {
        Initialize();
    }

    private void Initialize()
    {
        try
        {
            Debug.WriteLine("🚀 Inicializando aplicação...");

            // Carregar mapeamentos
            var mappings = _storageService.LoadMappings();
            foreach (var mapping in mappings)
            {
                PadMappings.Add(mapping);
            }
            StatusMessage = $"✓ {mappings.Count} mapeamentos carregados";

            // Listar MIDI inputs
            RefreshMidiInputs();

            _midiService.MidiMessageReceived += OnMidiMessageReceived;

            // Listar dispositivos de áudio
            var audioDeviceList = _audioService.GetAudioDevices();
            foreach (var device in audioDeviceList)
            {
                AudioDevices.Add(device);
            }

            if (AudioDevices.Count > 0)
            {
                SelectedAudioDevice = AudioDevices[0];
            }

            StatusMessage = "✓ Aplicação pronta";
            Debug.WriteLine("✅ Inicialização concluída");
        }
        catch (Exception ex)
        {
            StatusMessage = $"❌ Erro: {ex.Message}";
            Debug.WriteLine($"❌ {ex}");
        }
    }

    [RelayCommand]
    private void RefreshMidiInputs()
    {
        MidiInputs.Clear();
        var midiInputList = _midiService.GetAvailableMidiInputs();
        foreach (var input in midiInputList)
        {
            MidiInputs.Add(input);
        }

        if (MidiInputs.Count > 0)
        {
            SelectedMidiInput = MidiInputs[0];
        }
        StatusMessage = $"✓ Lista de dispositivos MIDI atualizada ({MidiInputs.Count} encontrados).";
    }

    partial void OnSelectedMidiInputChanged(string? value)
    {
        if (value != null)
        {
            try
            {
                // Extrair o índice do nome "[0] Nome do dispositivo"
                var match = Regex.Match(value, @"\[(\d+)\]");
                if (match.Success && int.TryParse(match.Groups[1].Value, out int deviceIndex))
                {
                    _midiService.OpenMidiInput(deviceIndex);
                    StatusMessage = $"✓ Dispositivo MIDI selecionado: {value}";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"❌ Erro ao selecionar MIDI: {ex.Message}";
            }
        }
    }

    partial void OnSelectedAudioDeviceChanged(AudioDevice? value)
    {
        if (value != null)
        {
            _audioService.SetAudioDevice(value.Index, value.Name);
            Debug.WriteLine($"🔊 Dispositivo selecionado: {value.Name}");
            StatusMessage = $"Dispositivo: {value.Name}";
        }
    }

    private void OnMidiMessageReceived(object? sender, MidiMessage message)
    {
        var learningPad = PadMappings.FirstOrDefault(p => p.IsLearning);
        if (learningPad != null)
        {
            // Rodar na thread da UI para evitar problemas de cross-threading
            App.Current.Dispatcher.Invoke(() =>
            {
                learningPad.Note = message.Data1;
                learningPad.IsLearning = false;
                
                _storageService.SaveMappings(new(PadMappings));
                StatusMessage = $"✓ Nota {message.Data1} atribuída ao pad.";
            });
            return;
        }

        // Procurar no mapeamento se existe audio para esta nota
        foreach (var mapping in PadMappings)
        {
            if (mapping.Note == message.Data1)
            {
                LastNoteNumber = message.Data1;
                Debug.WriteLine($"✓ Nota {message.Data1} encontrada! Tocando: {mapping.AudioPath}");
                _audioService.PlayAudio(mapping.AudioPath);
                StatusMessage = $"▶️ Tocando: {System.IO.Path.GetFileName(mapping.AudioPath)} (Nota {message.Data1})";
                break;
            }
        }
    }
    
    [RelayCommand]
    public void StartLearningMidiNote(PadMapping padMapping)
    {
        // Garantir que apenas um pad esteja aprendendo por vez
        foreach (var pad in PadMappings)
        {
            pad.IsLearning = false;
        }
        
        padMapping.IsLearning = true;
        StatusMessage = "Aguardando entrada MIDI... Pressione uma tecla no seu dispositivo.";
    }
    
    [RelayCommand]
    public void AddPadMapping()
    {
        var newPad = new PadMapping { Note = -1, AudioPath = "Nenhum áudio selecionado" };
        PadMappings.Add(newPad);
        _storageService.SaveMappings(new(PadMappings));
        StatusMessage = "✓ Novo pad adicionado.";
    }

    [RelayCommand]
    public void SelectAudioFileForPad(PadMapping padMapping)
    {
        var dialog = new Microsoft.Win32.OpenFileDialog
        {
            Filter = "Audio Files (*.mp3;*.wav;*.flac;*.ogg)|*.mp3;*.wav;*.flac;*.ogg|All Files (*.*)|*.*",
            Title = $"Selecionar arquivo para nota {padMapping.Note}"
        };

        if (dialog.ShowDialog() == true)
        {
            padMapping.AudioPath = dialog.FileName;
            _storageService.SaveMappings(new(PadMappings));
            StatusMessage = $"✓ Nota {padMapping.Note} mapeada para {System.IO.Path.GetFileName(dialog.FileName)}";
        }
    }

    [RelayCommand]
    public void RemovePadMapping(int noteNumber)
    {
        var mapping = PadMappings.FirstOrDefault(p => p.Note == noteNumber);
        if (mapping != null)
        {
            PadMappings.Remove(mapping);
            _storageService.SaveMappings(new(PadMappings));
            StatusMessage = $"✓ Mapeamento da nota {noteNumber} removido";
        }
    }

    public void Cleanup()
    {
        _midiService.CloseAllMidiInputs();
        _audioService.Stop();
        _audioService.Dispose();
    }
}