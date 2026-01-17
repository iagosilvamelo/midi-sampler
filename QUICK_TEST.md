# 🎹 Guia Rápido de Teste - MIDI Sampler

## 🚀 Como Testar Sem Hardware MIDI

### Opção 1: Usar Voicemeeter MIDI (Recomendado)

1. **Instale Voicemeeter Banana**
   - Download: https://vb-audio.com/Voicemeeter/banana.htm
   - Instale com suporte a MIDI

2. **Execute o MIDI Sampler**
   ```bash
   cd publish
   ./MidiSampler.exe
   ```

3. **Selecione Dispositivo de Áudio**
   - Dropdown: "Voicemeeter Banana"

4. **Teste com Software MIDI**
   - Use MIDI Monitor ou DAW qualquer
   - Envie notas MIDI para o Voicemeeter
   - Áudio deve tocar no dispositivo selecionado

### Opção 2: Teste Manual com Arquivo JSON

1. **Crie arquivo mappings.json**
   ```json
   [
     {
       "note": 60,
       "audio": "C:\\Users\\YourUser\\Music\\sample.mp3"
     }
   ]
   ```

2. **Coloque arquivos de áudio**
   ```
   C:\Users\YourUser\Music\sample.mp3
   ```

3. **Execute o app**
   ```bash
   ./MidiSampler.exe
   ```

4. **Verifique Console/Debug**
   - Visual Studio Debug Output
   - Veja mensagens de dispositivos MIDI

## 🧪 Checklist de Teste

### Inicialização
- [ ] App inicia sem erro
- [ ] UI carrega corretamente
- [ ] Lista de dispositivos não está vazia
- [ ] Status bar mostra "Aplicação pronta"

### Audio Device
- [ ] Dropdown mostra vários dispositivos
- [ ] Pode selecionar cada um
- [ ] Status muda quando seleciona

### PadEditor
- [ ] Botão "Editar Pads" abre janela
- [ ] Grid 4x4 com 16 botões
- [ ] Click abre file dialog
- [ ] Seleciona arquivo com sucesso

### Mapeamento
- [ ] Arquivo salvo em mappings.json
- [ ] Pode remover mapeamento
- [ ] Lista atualiza na main window
- [ ] Carrega ao reiniciar

### MIDI (Se tiver hardware)
- [ ] Detect porta MIDI
- [ ] Recebe nota
- [ ] Status mostra "Tocando: [arquivo]"
- [ ] Áudio sai do dispositivo selecionado

## 📝 Variáveis de Ambiente (Debug)

Se quiser logs mais detalhados:
```bash
# Abra Visual Studio Code
# F5 para debug
# Ver Debug Output (Ctrl+Shift+Y)
```

## 🐛 Troubleshooting

### App não inicia
- Verifique se .NET 8 está instalado
- Execute: `dotnet --version`

### Nenhum dispositivo de áudio aparece
- Pode estar com audio devices desabilitados
- Verifique Windows Sound Settings

### MIDI não funciona
- Verifique Device Manager > MIDI Controllers
- Use Voicemeeter para teste

### Arquivo de áudio não toca
- Verifique caminho (use caminhos absolutos)
- Confirme formato suportado (MP3, WAV, OGG, FLAC)
- Teste com arquivo diferente

## 📊 Debug Output

No Visual Studio, você verá logs como:

```
🚀 Inicializando aplicação...
🎹 2 dispositivos MIDI encontrados
   [0] USB MIDI Controller
   [1] Voicemeeter Banana
🔊 3 dispositivos de áudio encontrados
   [0] Speakers
   [1] Voicemeeter Banana
✓ 2 mapeamentos carregados
✅ Inicialização concluída

[Ao tocar nota MIDI]
🎵 MIDI recebido: Status=0x90, Note=60, Velocity=100
✓ Nota 60 encontrada! Tocando: C:\sample.mp3
▶️ Reproduzindo: sample.mp3
```

## 🎯 Teste Completo (5 minutos)

1. Instale Voicemeeter Banana (2 min)
2. Copie sample.mp3 para alguma pasta (1 min)
3. Execute MIDI Sampler (30s)
4. Edite pads e selecione arquivo (1 min)
5. Use software MIDI ou controlador (30s)
6. Verifique se áudio toca em Voicemeeter ✅

---

**Resultado esperado**: 
- Áudio toca no dispositivo selecionado
- Sem lag ou delay excessivo
- Múltiplas notas tocam corretamente
- Mapeamentos salvam e carregam

**Sucesso** = ✅ MIDI Sampler está funcionando!
