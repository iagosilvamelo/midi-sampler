#!/bin/bash
# Script para testar o MIDI Sampler

echo "🎹 MIDI Sampler - Script de Teste"
echo "===================================="
echo ""
echo "✓ Compilação..."
dotnet build

echo ""
echo "✓ Build Release..."
dotnet publish -c Release -o ./publish

echo ""
echo "✓ Tamanho do executável:"
ls -lh ./publish/MidiSampler.exe

echo ""
echo "✓ Estrutura de arquivos:"
find ./publish -name "*.dll" -o -name "*.exe" | head -10

echo ""
echo "✅ Teste concluído!"
echo ""
echo "📝 Próximos passos:"
echo "1. Execute: ./publish/MidiSampler.exe"
echo "2. Conecte um controlador MIDI"
echo "3. Clique em 'Editar Pads' e selecione áudios"
echo "4. Toque notas no controlador para reproduzir"
