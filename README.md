# MIDI Sampler - C# / .NET WPF

Uma aplicação desktop em C# com WPF para mapeamento e reprodução de áudio via MIDI com suporte completo a seleção de dispositivos de áudio no Windows.

## 🎯 Recursos

✅ **MIDI Input Real** - Suporte completo a múltiplas portas MIDI  
✅ **Audio Device Selection** - Selecione o dispositivo de áudio desejado (Voicemeeter, etc)  
✅ **NAudio Integration** - Reprodução com suporte nativo a dispositivos Windows  
✅ **Persistent Mappings** - Salva mapeamento de notas em JSON  
✅ **WPF UI** - Interface moderna e responsiva  

## 📋 Requisitos

- .NET 8.0 SDK (ou superior)
- Windows 7+ (com suporte WASAPI)
- Visual Studio 2022 ou VS Code

## 🚀 Instalação

### 1. Instalar .NET 8 SDK
```powershell
# Download e instale de: https://dotnet.microsoft.com/download/dotnet/8.0
# Ou via chocolatey:
choco install dotnet-sdk-8.0
```

### 2. Clonar e preparar
```powershell
cd d:\Projetos\Code\midi-sampler-csharp
dotnet restore
```

### 3. Compilar
```powershell
dotnet build
```

### 4. Executar
```powershell
dotnet run
```

Ou execute o .exe gerado em `bin\Debug\net8.0-windows\MidiSampler.exe`

## 💾 Dependências NuGet (instaladas automaticamente)

- **Melanchall.DryWetMidi** 11.2.0 - MIDI input
- **NAudio** 2.2.1 - Audio playback com device selection
- **CommunityToolkit.MVVM** 8.2.1 - Architecture pattern

## 🎮 Como Usar

1. **Iniciar a aplicação** - Todas as portas MIDI são abertas automaticamente
2. **Selecionar dispositivo de áudio** - Dropdown em "Dispositivo de Áudio"
   - Selecione "Voicemeeter Banana" ou outro dispositivo desejado
3. **Mapear notas MIDI**
   - Pressione uma nota no seu controller MIDI
   - Clique "Selecionar Arquivo" para escolher o áudio
4. **Reproduzir**
   - Pressione uma nota mapeada no controller
   - O áudio será reproduzido no dispositivo selecionado

## 📁 Estrutura do Projeto

```
midi-sampler-csharp/
├── Models/
│   └── DataModels.cs          # Classes de dados
├── Services/
│   ├── MidiService.cs         # Gerenciamento MIDI
│   ├── AudioService.cs        # Reprodução com device selection
│   └── StorageService.cs      # Persistência JSON
├── ViewModels/
│   └── MainViewModel.cs       # MVVM ViewModel
├── App.xaml                   # Configuração app
├── App.xaml.cs
├── MainWindow.xaml            # Interface WPF
├── MainWindow.xaml.cs
├── Program.cs                 # Entry point
├── MidiSampler.csproj         # Project file
└── mappings.json              # Mapeamentos (criado automaticamente)
```

## 🔧 Configurações

### Mudar dispositivo de áudio padrão

No código (`Services/AudioService.cs`), o dispositivo é configurado via:
```csharp
_waveOutDevice.DeviceNumber = _selectedDeviceIndex;
```

Isso usa a API nativa do Windows (WASAPI) para seleção real de device.

### Adicionar suporte a mais formatos

Edite `Services/AudioService.cs` e aumente suporte a codecs:
```csharp
// Atualmente suporta: MP3, WAV, FLAC, OGG
// Para ALAC, M4A, etc, adicione NuGet packages
```

## 🐛 Troubleshooting

### "Nenhum dispositivo MIDI encontrado"
- Conecte seu controlador MIDI
- Restart a aplicação
- Verifique em Configurações > Sons do Windows

### Áudio não reproduz em Voicemeeter
1. Abra Voicemeeter Banana
2. Configure entrada (A1) para "Voicemeeter Aux Input"
3. Selecione "VB-Audio Virtual Cable" ou "Voicemeeter Banana" no dropdown
4. Teste reprodução

### "Cannot find NAudio"
```powershell
dotnet restore
dotnet clean
dotnet build
```

## 📝 Notas de Desenvolvimento

- A aplicação usa **DryWetMIDI** para input (mais robusto que play-sound)
- **NAudio** é a melhor biblioteca C# para audio com device selection real
- MVVM pattern para fácil extensão e testes
- Persistência em JSON simples (pode ser migrado para SQL se necessário)

## 🎓 Próximas Melhorias Possíveis

- Banco de pads com múltiplas configurações
- Visualizador de MIDI
- Recording de sequências
- Efeitos de áudio (volume, fade-in/out)
- Suporte a VST plugins
- Dark mode UI
- Profiles salvos por projeto

## 📄 Licença

Desenvolvido para uso pessoal - adaptável para suas necessidades.

---

**Esta é uma reescrita completa em C# WPF com suporte real a dispositivos de áudio do Windows!** 🎉
