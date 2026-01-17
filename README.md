# MIDI Sampler - C# / .NET WPF

Uma aplicação desktop moderna em C# com WPF para mapeamento e reprodução de áudio via MIDI, com suporte flexível a dispositivos e roteamento de áudio.

## 🎯 Recursos

✅ **Seleção de Dispositivo MIDI** - Escolha e troque de dispositivo MIDI em tempo real com um botão para atualizar a lista.  
✅ **Suporte a Note-On e Control Change (CC)** - Compatível com teclados, baterias eletrônicas e controladores que enviam mensagens CC.  
✅ **Seleção de Dispositivo de Áudio (WASAPI)** - Utiliza a API moderna do Windows (WASAPI) para listar e selecionar dispositivos de saída, garantindo compatibilidade com interfaces de áudio e roteadores virtuais como Voicemeeter.  
✅ **Edição Direta na Interface** - Adicione, configure e remova pads diretamente na tela principal.  
✅ **Mapeamento Persistente** - Salva todos os seus mapeamentos de pads em um arquivo `mappings.json`.  
✅ **UI com WPF** - Interface de usuário limpa e intuitiva construída com WPF.  

## 📋 Requisitos

- .NET 8.0 SDK (ou superior)
- Windows 7+ (com suporte a WASAPI)

## 🚀 Como Executar

1. **Clone o repositório**
2. **Abra um terminal** na pasta do projeto.
3. **Execute o comando:**
   ```powershell
   dotnet run
   ```
Opcionalmente, compile com `dotnet build` e execute o `.exe` gerado em `bin\Debug\net8.0-windows\MidiSampler.exe`.

## 🎮 Como Usar

A aplicação agora centraliza todas as operações na tela principal:

1. **Selecione seus Dispositivos**
   - **Áudio:** No dropdown "Dispositivo de Áudio", escolha para onde o som deve ser enviado (ex: seus fones de ouvido, ou uma entrada virtual do Voicemeeter).
   - **MIDI:** No dropdown "Entrada MIDI", escolha seu controlador. Se você conectou o dispositivo depois de abrir o app, clique no botão **🔄** para atualizar a lista.

2. **Adicione e Configure Pads**
   - Clique no botão **➕ Adicionar Pad**. Uma nova linha aparecerá na lista.
   - **Para mapear o áudio:** Clique em **Selecionar Áudio** na nova linha e escolha um arquivo de som (`.mp3`, `.wav`, etc.).
   - **Para mapear o MIDI:** Clique em **Aprender MIDI**. O botão mudará para "Ouvindo...". Pressione a tecla ou botão desejado no seu controlador MIDI. A nota/CC será capturada automaticamente.

3. **Reproduza!**
   - Com os pads configurados, pressione as teclas/botões correspondentes no seu dispositivo MIDI para tocar os sons.

4. **Remover um Pad**
   - Clique no botão **Remover** na linha do pad que deseja apagar.

## 🎤 Integração com Roteamento Virtual (Voicemeeter, etc.)

Para enviar o áudio do `MidiSampler` para outra aplicação (como Discord, OBS, etc.), você precisa de um roteador de áudio virtual.

**Exemplo com Voicemeeter Banana:**

1. **No MidiSampler:**
   - No dropdown "Dispositivo de Áudio", selecione uma das entradas virtuais do Voicemeeter, como `Voicemeeter Aux Input (VB-Audio...`.

2. **No Voicemeeter:**
   - O áudio do `MidiSampler` aparecerá no canal "AUX Input".
   - Nesse canal, você pode processar o áudio e roteá-lo para onde precisar. Por exemplo, para enviar o som para outros apps, ative o botão **B1** ou **B2**. O dispositivo de gravação `Voicemeeter Output (B1)` ou `Voicemeeter Aux Output (B2)` funcionará como um microfone em outros aplicativos, transmitindo os sons do sampler.

## 📁 Estrutura do Projeto

```
midi-sampler-csharp/
├── Models/
│   └── DataModels.cs          # Classes de dados (PadMapping, etc.)
├── Services/
│   ├── MidiService.cs         # Gerenciamento de entrada MIDI
│   ├── AudioService.cs        # Reprodução de áudio com WASAPI
│   └── StorageService.cs      # Persistência em JSON
├── ViewModels/
│   └── MainViewModel.cs       # View-Model principal (lógica da UI)
├── App.xaml                   # Configuração da aplicação
├── MainWindow.xaml            # Interface principal da UI
├── ...
└── mappings.json              # Mapeamentos (criado automaticamente)
```

## 💾 Dependências NuGet

- **NAudio** (incluindo `NAudio.Midi`): Biblioteca principal para toda a manipulação de áudio e MIDI.
- **CommunityToolkit.MVVM**: Usada para implementar o padrão de arquitetura MVVM.

## 🐛 Troubleshooting

### Nenhum dispositivo MIDI é listado
- Verifique se seu controlador está conectado.
- Clique no botão **🔄** para atualizar a lista de dispositivos MIDI.
- Se for um controlador Bluetooth, verifique se o software conector (ex: Sinco Connector) está em execução.

### O áudio não toca
- Verifique se um dispositivo de áudio válido está selecionado no `MidiSampler`.
- Certifique-se de que o volume do dispositivo de saída não está no mudo.
- Verifique se o arquivo de áudio mapeado ainda existe no caminho original.

--- 

**Aplicação reconstruída para ser mais flexível, moderna e fácil de usar diretamente na tela principal.** 🎉

```