@echo off
cd..
setlocal
echo === Limpando relat�rios antigos ===
if exist .\coverage-report rmdir /s /q .\coverage-report

echo === Limpando testes antigos ===
set "testesCaminho=.\TestResults"
for /d /r %testesCaminho% %%d in (TestResults) do (
    if exist "%%d" (
        echo Excluindo pasta: %%d
        rd /s /q "%%d"
    )
)

echo === Executando testes Conteudo.UnitTests ===
dotnet test Conteudo.UnitTests\Conteudo.UnitTests --collect:"XPlat Code Coverage"

echo === Gerando Relat�rio de Cobertura ===
reportgenerator -reports:tests\**\coverage.cobertura.xml -targetdir:tests\coverage-report -reporttypes:Html

echo === Abrindo o Relat�rio ===
if exist tests\coverage-report\index.html (
    start tests\coverage-report\index.html
) else (
    echo !!! ERRO: Relat�rio HTML n�o foi gerado !!!
)

endlocal

