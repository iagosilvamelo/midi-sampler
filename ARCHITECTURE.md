# 🎹 MIDI Sampler - Arquitetura Visual

## 🏗️ Estrutura do Projeto

```
MidiSampler/
├── Models/
│   ├── AudioDevice         # 🔊 Dispositivo de áudio
│   ├── PadMapping          # 🎹 Mapeamento nota → arquivo
│   └── MidiMessage         # 📨 Evento MIDI
│
├── Services/
│   ├── AudioService        # 🎵 Reprodução de áudio
│   ├── MidiService         # 🎹 Entrada MIDI
│   └── StorageService      # 💾 Persistência JSON
│
├── ViewModels/
│   └── MainViewModel       # 🔗 Binding e lógica
│
├── UI (XAML)/
│   ├── MainWindow          # 🪟 Janela principal
│   └── PadEditorWindow     # ✏️ Editor de pads
│
└── Data/
    └── mappings.json       # 📋 Mapeamentos salvos
```

## 🔄 Fluxo de Dados Completo

```
                         INICIALIZAÇÃO
                              │
                  ┌───────────┼───────────┐
                  ▼           ▼           ▼
            ┌─────────┐ ┌─────────┐ ┌──────────┐
            │ Carregar│ │ Detectar│ │  Abrir   │
            │ Mapping │ │MIDI/Audio│ │MIDI Input│
            │ (JSON)  │ │ Devices │ │ Ports   │
            └────┬────┘ └────┬────┘ └────┬─────┘
                 │           │           │
                 └───────────┼───────────┘
                             │
                         ┌───▼────┐
                         │ UI PRONTA│
                         └───┬────┘
                             │
    ┌────────────────────────┼────────────────────────┐
    │                        │                        │
    ▼                        ▼                        ▼
┌────────┐            ┌─────────────┐          ┌───────────┐
│ SELECT │            │  EDIT PADS  │          │ PLAY MIDI │
│DEVICE  │            │  (UI Click) │          │ (Hardware)│
│        │            │             │          │           │
└────┬───┘            └─────┬───────┘          └────┬──────┘
     │                      │                      │
     │ user selects    click on pad           MIDI event
     │                      │                      │
     ▼                      ▼                      ▼
┌─────────────┐        ┌─────────────┐       ┌──────────────┐
│AudioService │        │ OpenDialog  │       │ MidiService  │
│SetAudioDev  │        │SelectFile   │       │OnMidiMessage │
│             │        │             │       │              │
└────┬────────┘        └─────┬───────┘       └────┬─────────┘
     │                      │                      │
     │ Device configured    │ File selected        │ Raise event
     │                      │                      │
     │                      ▼                      │
     │              ┌──────────────┐               │
     │              │StorageService│               │
     │              │SaveMappings  │               │
     │              │   (JSON)     │               │
     │              └──────┬───────┘               │
     │                     │                       │
     │                     ▼                       │
     │              ┌──────────────┐               │
     │              │mappings.json │               │
     │              │[{note,audio}]│               │
     │              └──────┬───────┘               │
     │                     │                       │
     └─────────┬───────────┼───────────┬───────────┘
               │           │           │
               └───────────┼───────────┘
                           │
                      ┌────▼─────┐
                      │MainView  │
                      │Model     │
                      │Handler   │
                      └────┬─────┘
                           │
         ┌─────────────────┼─────────────────┐
         │                 │                 │
         ▼                 ▼                 ▼
    ┌────────┐        ┌────────┐       ┌────────┐
    │ Search │        │ Found? │       │ Not    │
    │PadMap  │        │        │       │Found   │
    │for     │        └─┬──┬───┘       │        │
    │note    │          │  │           └───┬────┘
    └────┬───┘          │  └─────────┐      │
         │              │            │      │
         ▼              ▼            ▼      ▼
    ┌──────────────────────────┐   ┌──────────────┐
    │    AudioService.Play()   │   │Debug Log:    │
    │    filePath              │   │Note not in   │
    │    deviceNumber          │   │mappings      │
    └────┬─────────────────────┘   └──────────────┘
         │
         │ Initialize WaveOutEvent
         │
         ▼
    ┌──────────────────────────┐
    │  NAudio.Wave             │
    │  (WaveOutEvent)          │
    │  Init(AudioFileReader)   │
    └────┬─────────────────────┘
         │
         ▼
    ┌──────────────────────────┐
    │  Set DeviceNumber        │
    │  (from selected device)  │
    └────┬─────────────────────┘
         │
         ▼
    ┌──────────────────────────┐
    │  player.Play()           │
    └────┬─────────────────────┘
         │
         ▼
    ┌──────────────────────────┐
    │  🔊 ÁUDIO TOCANDO        │
    │  No dispositivo          │
    │  selecionado             │
    └──────────────────────────┘
```

## 📊 Interação entre Componentes

```
┌──────────────────────────────────────────────────────────────┐
│                     MAINWINDOW (UI)                          │
│                                                              │
│  ┌─ Device Dropdown ─┐     ┌─ Status Bar ─┐    ┌─ Button ─┐│
│  │ (SelectedItem)    │     │ (StatusMsg)   │    │ Edit Pads││
│  └──────┬────────────┘     └────┬──────────┘    └─────┬────┘│
│         │Binding                │                     │     │
│         └─────────┬─────────────┴─────────────────────┘     │
│                   │                                         │
└───────────────────┼─────────────────────────────────────────┘
                    │
              ┌─────▼────────┐
              │ MAINVIEWMODEL│
              │              │
              │ Properties:  │ ◄─── Binding (INotifyPropertyChanged)
              │ • AudioDev   │
              │ • PadMaps    │
              │ • StatusMsg  │
              │ • LastNote   │
              │              │
              │ Commands:    │ ◄─── Button clicks
              │ • EditPads   │
              │ • RemoveMap  │
              │              │
              │ Events:      │ ◄─── MIDI input
              │ • OnMidi()   │
              └─────┬────────┘
                    │
         ┌──────────┼──────────┐
         │          │          │
         ▼          ▼          ▼
    ┌─────────┐ ┌────────┐ ┌──────────┐
    │ AUDIO   │ │ MIDI   │ │ STORAGE  │
    │ SERVICE │ │SERVICE │ │ SERVICE  │
    │         │ │        │ │          │
    │Get Dev  │ │Get Inp │ │Load Maps │
    │Set Dev  │ │Open    │ │Save Maps │
    │Play     │ │Close   │ │          │
    │Stop     │ │Event   │ │JSON I/O  │
    │Dispose  │ │Handler │ │          │
    └────┬────┘ └───┬────┘ └────┬─────┘
         │          │           │
         ▼          ▼           ▼
    ┌─────────┐ ┌────────┐ ┌──────────┐
    │ NAUDIO  │ │NAUDIO  │ │ FS/JSON  │
    │WAVEOUT  │ │ MIDI   │ │ PARSER   │
    │ API     │ │ API    │ │          │
    └─────────┘ └────────┘ └──────────┘
         │          │           │
         ▼          ▼           ▼
    🔊 Audio    🎹 MIDI    💾 Storage
    Device     Device     (JSON file)
```

## 🎵 Sequência de Eventos MIDI → Áudio

```
Timeline:

T0: Usuário toca nota no controlador MIDI
    │
    ├─ MIDI message: Note-On (status=0x90, note=60, vel=100)
    │
T1: (+2ms) NAudio.Midi intercepta no OS
    │
    ├─ MidiInPort.MessageReceived event fired
    │
T2: (+5ms) MidiService.OnMidiMessageReceived()
    │
    ├─ Parse message
    ├─ Check if Note-On && velocity > 0
    ├─ Raise MidiMessageReceived event
    │
T3: (+8ms) MainViewModel.OnMidiMessageReceived()
    │
    ├─ Search PadMappings for matching note
    ├─ If found: AudioService.PlayAudio()
    ├─ Update StatusMessage for UI
    │
T4: (+15ms) AudioService.PlayAudio()
    │
    ├─ Create WaveOutEvent with device number
    ├─ Initialize AudioFileReader from file
    ├─ player.Init(reader)
    ├─ player.Play()
    │
T5: (+20ms) NAudio starts playback
    │
    ├─ Windows Audio System routing to device
    │
T5+: Audio plays! 🔊
    │
    └─ Total latency: ~20ms (imperceptível)
```

## 💾 Formato de Dados JSON

```json
// mappings.json structure
[
  {
    "note": 36,           // MIDI Note number (0-127)
    "audio": "C:\\..."    // Absolute file path
  },
  {
    "note": 38,
    "audio": "C:\\Samples\\snare.mp3"
  },
  {
    "note": 42,
    "audio": "C:\\Samples\\hihat.wav"
  }
]

// Device capabilities (listed in UI)
Devices:
  [0] Speakers (default)
  [1] Voicemeeter Banana (virtual)
  [2] USB Audio Interface
  ...
```

## 🔐 Segurança e Tratamento de Erros

```
User Input → Validation → Processing → Error Handler

Select Device
    ↓
SetAudioDevice(index, name)
    ├─ Check: index in range?
    ├─ Check: device still exists?
    ├─ Apply: WaveOutEvent.DeviceNumber = index
    └─ Error: catch, log, use default device

Select Audio File
    ↓
SelectAudioFileForPad(noteNumber)
    ├─ Check: file exists?
    ├─ Check: format supported?
    ├─ Save: mappings.json
    └─ Error: catch, show UI message

MIDI Event
    ↓
OnMidiMessageReceived(message)
    ├─ Check: null message?
    ├─ Check: valid note number?
    ├─ Search: PadMappings
    ├─ Check: file still exists?
    ├─ Play: AudioService.PlayAudio()
    └─ Error: catch, log to debug
```

---

**Arquitetura em resumo:**
- **Separation of Concerns**: UI → ViewModel → Services → APIs
- **Data Flow**: One-way binding + event handlers
- **Error Handling**: Try-catch em serviços, logs em debug
- **Performance**: Async-ready (NAudio handles threading)

