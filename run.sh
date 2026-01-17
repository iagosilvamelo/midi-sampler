#!/bin/bash
# Build and run MIDI Sampler C# Edition

echo "🚀 Restaurando dependências..."
dotnet restore

echo "🔨 Compilando projeto..."
dotnet build --configuration Release

echo "✅ Build concluído!"
echo "Executando: dotnet run"
dotnet run --configuration Release
