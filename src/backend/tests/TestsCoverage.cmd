@echo off
cd..
setlocal
echo === Limpando relatórios antigos ===
if exist tests\coverage-report rmdir /s /q tests\coverage-report

echo === Limpando testes antigos ===
set "testesCaminho=.\TestResults"
for /d /r %testesCaminho% %%d in (TestResults) do (
    if exist "%%d" (
        echo Excluindo pasta: %%d
        rd /s /q "%%d"
    )
)

echo === Executando testes Conteudo.IntegrationTests ===
dotnet test tests\Conteudo.IntegrationTests --collect:"XPlat Code Coverage"

echo === Executando testes Conteudo.UnitTests ===
dotnet test tests\Conteudo.UnitTests --collect:"XPlat Code Coverage"







echo === Gerando Relatório de Cobertura ===
reportgenerator -reports:tests\**\coverage.cobertura.xml -targetdir:tests\coverage-report -reporttypes:Html

echo === Abrindo o Relatório ===
if exist tests\coverage-report\index.html (
    start tests\coverage-report\index.html
) else (
    echo !!! ERRO: Relatório HTML não foi gerado !!!
)

endlocal

