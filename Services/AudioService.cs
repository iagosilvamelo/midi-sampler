using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using MidiSampler.Models;

namespace MidiSampler.Services;

public class AudioService
{
    private IWavePlayer? _wavePlayer;
    private AudioFileReader? _audioFileReader;
    private string? _selectedDeviceId;

    public List<Models.AudioDevice> GetAudioDevices()
    {
        var devices = new List<Models.AudioDevice>();
        try
        {
            var enumerator = new MMDeviceEnumerator();
            var renderDevices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
            
            Debug.WriteLine($"🔊 {renderDevices.Count} dispositivos de áudio (WASAPI) encontrados");

            foreach (var device in renderDevices)
            {
                devices.Add(new Models.AudioDevice { Id = device.ID, Name = device.FriendlyName });
                Debug.WriteLine($"   [{device.ID}] {device.FriendlyName}");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ Erro ao listar dispositivos WASAPI: {ex.Message}");
        }
        
        return devices;
    }

    public void SetAudioDevice(string deviceId, string deviceName)
    {
        Debug.WriteLine($"🔊 Configurando dispositivo: [{deviceId}] {deviceName}");
        _selectedDeviceId = deviceId;
    }

    public void PlayAudio(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                Debug.WriteLine($"❌ Arquivo não encontrado: {filePath}");
                return;
            }

            Stop();
            Dispose();

            _audioFileReader = new AudioFileReader(filePath);

            if (string.IsNullOrEmpty(_selectedDeviceId))
            {
                // Fallback to default device if none selected
                _wavePlayer = new WasapiOut();
            }
            else
            {
                var enumerator = new MMDeviceEnumerator();
                var device = enumerator.GetDevice(_selectedDeviceId);
                _wavePlayer = new WasapiOut(device, AudioClientShareMode.Shared, false, 300);
            }
            
            Debug.WriteLine($"✓ Usando dispositivo {_selectedDeviceId}");

            _wavePlayer.Init(_audioFileReader);
            
            Debug.WriteLine($"▶️ Reproduzindo: {Path.GetFileName(filePath)}");
            _wavePlayer.Play();
            
            _wavePlayer.PlaybackStopped += (s, e) =>
            {
                Debug.WriteLine("✓ Reprodução finalizada");
                if (e.Exception != null)
                {
                    Debug.WriteLine($"  ❌ Erro na reprodução: {e.Exception.Message}");
                }
            };
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ Erro ao reproduzir áudio: {ex.Message}");
        }
    }

    public void Stop()
    {
        _wavePlayer?.Stop();
    }

    public void Dispose()
    {
        _wavePlayer?.Dispose();
        _wavePlayer = null;
        _audioFileReader?.Dispose();
        _audioFileReader = null;
    }
}