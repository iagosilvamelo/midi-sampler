using System;
using System.Collections.Generic;
using System.Diagnostics;
using NAudio.Midi;
using MidiSampler.Models;
using System.Linq;

namespace MidiSampler.Services;

public class MidiService
{
    public event EventHandler<Models.MidiMessage>? MidiMessageReceived;
    public event EventHandler<string>? LogMessage;
    
    private List<MidiInCapabilities> _midiInputCapabilities = new();
    private MidiIn? _openedInput;

    public List<string> GetAvailableMidiInputs()
    {
        try
        {
            _midiInputCapabilities.Clear();
            
            int inputCount = MidiIn.NumberOfDevices;
            Debug.WriteLine($"🎹 {inputCount} dispositivos MIDI encontrados");
            
            var result = new List<string>();
            for (int i = 0; i < inputCount; i++)
            {
                var caps = MidiIn.DeviceInfo(i);
                _midiInputCapabilities.Add(caps);
                var name = $"[{i}] {caps.ProductName}";
                result.Add(name);
                Debug.WriteLine($"   {name}");
            }
            
            return result;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ Erro ao listar MIDI inputs: {ex.Message}");
            return new List<string> { "❌ Erro ao listar MIDI" };
        }
    }

    public void OpenMidiInput(int deviceIndex)
    {
        try
        {
            CloseMidiInput(); // Fechar anterior

            if (deviceIndex < 0 || deviceIndex >= MidiIn.NumberOfDevices)
            {
                Debug.WriteLine($"⚠️ Índice de dispositivo MIDI inválido: {deviceIndex}");
                return;
            }

            try
            {
                var midiIn = new MidiIn(deviceIndex);
                midiIn.ErrorReceived += (s, e) => LogMessage?.Invoke(this, $"MIDI Error: {e.RawMessage}");
                midiIn.MessageReceived += (sender, args) =>
                {
                    OnMidiMessageReceived(args);
                };
                midiIn.Start();
                
                _openedInput = midiIn;
                Debug.WriteLine($"✓ Aberto: {_midiInputCapabilities[deviceIndex].ProductName}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"⚠️ Erro ao abrir MIDI input {deviceIndex}: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ Erro ao abrir MIDI inputs: {ex.Message}");
        }
    }

    public void CloseMidiInput()
    {
        if (_openedInput != null)
        {
            try
            {
                _openedInput.Stop();
                _openedInput.Dispose();
            }
            catch { }
            _openedInput = null;
            Debug.WriteLine("✅ MIDI input fechado");
        }
    }
    
    private void OnMidiMessageReceived(MidiInMessageEventArgs args)
    {
        LogMessage?.Invoke(this, $"Debug(MidiService): Raw={args.RawMessage:X}");
        
        int status = (int)(args.RawMessage & 0xFF);
        
        // Filter for Control Change (0xB0-0xBF) OR Note-On (0x90-0x9F)
        if ((status & 0xF0) == 0xB0 || (status & 0xF0) == 0x90)
        {
            int data1 = (int)((args.RawMessage >> 8) & 0xFF); // Note Number or CC Number
            int data2 = (int)((args.RawMessage >> 16) & 0xFF); // Velocity or CC Value
            
            // Only fire if value/velocity > 0
            if (data2 > 0)
            {
                var message = new Models.MidiMessage { Status = (byte)status, Data1 = (byte)data1, Data2 = (byte)data2 };
                MidiMessageReceived?.Invoke(this, message);
            }
            else
            {
                LogMessage?.Invoke(this, $"Debug(MidiService): Ignored (Value/Vel=0)");
            }
        }
        else
        {
            LogMessage?.Invoke(this, $"Debug(MidiService): Not a NoteOn or CC message (Status={status:X})");
        }
    }

    public void CloseAllMidiInputs()
    {
        CloseMidiInput();
    }
}