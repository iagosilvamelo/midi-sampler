# Script para testar o MIDI Sampler
Write-Host "🎹 MIDI Sampler - Script de Teste" -ForegroundColor Cyan
Write-Host "====================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "✓ Compilação..." -ForegroundColor Green
dotnet build

Write-Host ""
Write-Host "✓ Build Release..." -ForegroundColor Green
dotnet publish -c Release -o ./publish

Write-Host ""
Write-Host "✓ Tamanho do executável:" -ForegroundColor Green
$exePath = "./publish/MidiSampler.exe"
if (Test-Path $exePath) {
    $size = (Get-Item $exePath).Length / 1MB
    Write-Host "   MidiSampler.exe: $([Math]::Round($size, 2)) MB" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "✓ Estrutura de arquivos:" -ForegroundColor Green
Get-ChildItem -Path "./publish" -Filter "*.dll" -Recurse | Select-Object -First 5 | ForEach-Object { Write-Host "   $($_.Name)" }

Write-Host ""
Write-Host "✅ Build concluído!" -ForegroundColor Green
Write-Host ""
Write-Host "📝 Próximos passos:" -ForegroundColor Cyan
Write-Host "1. Execute: ./publish/MidiSampler.exe" -ForegroundColor Yellow
Write-Host "2. Conecte um controlador MIDI" -ForegroundColor Yellow
Write-Host "3. Clique em 'Editar Pads' e selecione áudios" -ForegroundColor Yellow
Write-Host "4. Toque notas no controlador para reproduzir" -ForegroundColor Yellow
