using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using NAudio.Wave;
using MidiSampler.Models;

namespace MidiSampler.Services;

public class AudioService
{
    private WaveOutEvent? _waveOutDevice;
    private AudioFileReader? _audioFileReader;
    private int _selectedDeviceIndex = 0;
    private string _selectedDeviceName = "Default";

    public List<Models.AudioDevice> GetAudioDevices()
    {
        var devices = new List<Models.AudioDevice>();
        
        try
        {
            int deviceCount = WaveOut.DeviceCount;
            Debug.WriteLine($"🔊 {deviceCount} dispositivos de áudio encontrados");
            
            for (int i = 0; i < deviceCount; i++)
            {
                var caps = WaveOut.GetCapabilities(i);
                var device = new Models.AudioDevice { Index = i, Name = caps.ProductName };
                devices.Add(device);
                Debug.WriteLine($"   [{i}] {caps.ProductName}");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ Erro ao listar dispositivos: {ex.Message}");
        }
        
        return devices;
    }

    public void SetAudioDevice(int deviceIndex, string deviceName)
    {
        Debug.WriteLine($"🔊 Configurando dispositivo: [{deviceIndex}] {deviceName}");
        _selectedDeviceIndex = deviceIndex;
        _selectedDeviceName = deviceName;
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

            // Parar reprodução anterior
            if (_waveOutDevice?.PlaybackState == PlaybackState.Playing)
            {
                _waveOutDevice.Stop();
            }

            _waveOutDevice?.Dispose();
            _audioFileReader?.Dispose();

            // Criar novo player
            _waveOutDevice = new WaveOutEvent { DeviceNumber = _selectedDeviceIndex };
            
            Debug.WriteLine($"✓ Usando dispositivo {_selectedDeviceIndex}: {_selectedDeviceName}");

            // Criar reader para o arquivo
            _audioFileReader = new AudioFileReader(filePath);
            _waveOutDevice.Init(_audioFileReader);
            
            Debug.WriteLine($"▶️ Reproduzindo: {Path.GetFileName(filePath)}");
            _waveOutDevice.Play();
            
            // Event para quando terminar
            _waveOutDevice.PlaybackStopped += (s, e) =>
            {
                Debug.WriteLine("✓ Reprodução finalizada");
            };
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ Erro ao reproduzir áudio: {ex.Message}");
        }
    }

    public void Stop()
    {
        if (_waveOutDevice?.PlaybackState == PlaybackState.Playing)
        {
            _waveOutDevice.Stop();
        }
    }

    public void Dispose()
    {
        _waveOutDevice?.Dispose();
        _audioFileReader?.Dispose();
    }
}
