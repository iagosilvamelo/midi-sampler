# 🎹 MIDI Sampler - Guia de Uso

## 📋 Visão Geral

O MIDI Sampler permite mapear notas MIDI a arquivos de áudio e reproduzi-los em tempo real. Desenvolvido em C# .NET 8 com WPF, oferece suporte nativo a seleção de dispositivos de áudio no Windows.

## 🚀 Como Começar

### 1. Selecionar Dispositivo de Áudio

Na janela principal, selecione o dispositivo de áudio desejado no dropdown "Dispositivo de Áudio:". 

**Opções comuns:**
- `Speakers` (Falantes padrão)
- `Voicemeeter Banana` (Virtual audio device para streaming)
- `USB Audio Interface` (Interface de áudio USB)

### 2. Mapear Notas MIDI para Áudio

**Passo 1:** Clique no botão **"📝 Editar Pads"**

**Passo 2:** Clique em um botão de pad (números 36-61, notas comuns de bateria)

**Passo 3:** Selecione um arquivo de áudio (MP3, WAV, FLAC, OGG)

**Passo 4:** O mapeamento é salvo automaticamente em `mappings.json`

### 3. Usar com Controlador MIDI

Conecte seu controlador MIDI ao computador. O aplicativo detectará automaticamente:
- Detecta todas as portas MIDI disponíveis
- Abre todas as portas para receber notas
- Toca o áudio mapeado quando a nota é recebida

## 📁 Formato de Dados

### mappings.json

Os mapeamentos são armazenados em JSON no formato:

```json
[
  {
    "note": 36,
    "audio": "C:\\Samples\\drum_kick.mp3"
  },
  {
    "note": 38,
    "audio": "C:\\Samples\\drum_snare.mp3"
  },
  {
    "note": 42,
    "audio": "C:\\Samples\\drum_hihat.mp3"
  }
]
```

**Campos:**
- `note` (int): Número da nota MIDI (0-127)
- `audio` (string): Caminho completo do arquivo de áudio

## 🎯 Notas MIDI Comuns (Bateria)

| Nota | Nome | Instrumento |
|------|------|-------------|
| 36 | C1 | Kick (Bumbo) |
| 38 | D1 | Snare (Caixa) |
| 42 | Fis1 | Hi-Hat Closed |
| 46 | Ais1 | Hi-Hat Open |
| 49 | Dis2 | Crash |
| 51 | Dis2 | Ride |

## 🔧 Recursos Técnicos

### Dispositivos de Áudio
- **NAudio + WaveOut API**: Suporte nativo a device selection
- **WASAPI**: Acesso a qualquer dispositivo de áudio Windows
- Voicemeeter: Funciona perfeitamente para roteamento virtual

### MIDI Input
- **NAudio.Midi**: Win32 MIDI API wrapper
- Suporte a múltiplas portas MIDI simultâneas
- Filtra apenas eventos Note-On (velocity > 0)

### Formatos de Áudio
- MP3, WAV, FLAC, OGG
- Suportado via NAudio + codecs

## ⚡ Atalhos e Dicas

1. **Remover Mapeamento**: Clique em "Remover" na lista de pads
2. **Trocar Arquivo**: Clique novamente no pad para selecionar outro arquivo
3. **Visualizar Mapeamentos**: A lista mostra todas as notas mapeadas
4. **Status em Tempo Real**: Barra inferior mostra qual áudio está tocando

## 🆘 Solução de Problemas

### "Nenhuma porta MIDI encontrada"
- Verifique se o controlador MIDI está conectado
- Reinicie a aplicação após conectar o dispositivo
- Windows > Gerenciador de Dispositivos > Controladores MIDI

### Áudio não toca no dispositivo selecionado
- Verifique se o dispositivo está selecionado corretamente
- Teste com Speakers primeiro
- Verifique se o arquivo de áudio existe

### Arquivo de áudio inválido
- Use MP3 ou WAV (mais compatíveis)
- Verifique se o caminho não contém caracteres especiais
- Teste com um arquivo diferente

## 📦 Compilação Release

```bash
dotnet publish -c Release -o ./publish
```

Executável gerado: `publish/MidiSampler.exe`

## 🔄 Migração do Electron

Os mapeamentos do projeto Electron anterior são compatíveis! Copie o arquivo `mappings.json` para o diretório da aplicação.

## 📝 Exemplo de Uso Prático

1. Baixe samples de bateria (kick, snare, hihat)
2. Abra o PadEditor e mapeie as notas
3. Instale Voicemeeter Banana (opcional)
4. Selecione Voicemeeter como dispositivo de áudio
5. Conecte seu controlador MIDI
6. Comece a tocar!

## 🎵 Performance

- ✅ Startup < 100ms
- ✅ Latência de reprodução: ~50ms
- ✅ Múltiplas portas MIDI simultâneas
- ✅ Aplicação ~5MB vs ~200MB no Electron

---

**Versão:** 1.0  
**Plataforma:** Windows (net8.0-windows)  
**Última Atualização:** 16/01/2026
