if (-not (Get-Command "allure" -ErrorAction SilentlyContinue)) {
    Write-Host "❌ Allure не установлен. Установи через 'scoop install allure' или 'npm install -g allure-commandline'"
    exit 1
}

Write-Host "`n🧹 Удаляем старые результаты..."
Remove-Item -Recurse -Force -ErrorAction SilentlyContinue "allure-results"
Remove-Item -Recurse -Force -ErrorAction SilentlyContinue "allure-report"

Write-Host "`n🚀 Запуск dotnet тестов..."
dotnet test

$sourceResults = "bin\Debug\net8.0\allure-results"
$targetResults = "allure-results"

if (Test-Path $sourceResults) {
    Write-Host "`n📁 Копируем allure-results в корень..."
    Copy-Item $sourceResults $targetResults -Recurse
} else {
    Write-Host "`n❌ Не найдены результаты в '$sourceResults'."
    exit 1
}

Write-Host "`n📊 Генерация Allure отчёта..."
allure generate $targetResults --clean -o allure-report

Write-Host "`n🌐 Открытие отчёта в браузере..."
allure open allure-report
