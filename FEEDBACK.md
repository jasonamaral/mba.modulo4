## Funcionalidade 30%

Avalie se o projeto atende a todos os requisitos funcionais definidos.
* Será revisado na avalição final.


## Qualidade do Código 20%

Considere clareza, organização e uso de padrões de codificação.
* Será revisado na avalição final.


## Eficiência e Desempenho 20%

Avalie o desempenho e a eficiência das soluções implementadas.
* Será revisado na avalição final.


## Inovação e Diferenciais 10%

Considere a criatividade e inovação na solução proposta.
* Será revisado na avalição final.


## Documentação e Organização 10%

Verifique a qualidade e completude da documentação, incluindo README.md.

- Antes de submeterem para avaliação final, atualizem o README.md e repitam o processo de execução do projeto, para garantir que todas as instruções estão corretas e atualizadas.
- Não é necessário separar testes em Unit e Integration, mas separe por contexto.
- Mantenha a estrutura de pastas conforme o padrão sugerido: 
  - `<Solução>/<Contexto>/<Projeto>`
  - Namespace: `MBA.Modulo4.<Contexto>.<Projeto>`. Ex: `MBA.Modulo4.Alunos.Api`
  - Arquivo de projeto: `./src/MBA.Modulo4/Alunos/Api.csproj` ou `./src/MBA.Modulo4/Alunos/Alunos.Api.csproj` 
  - Pastas da Solução: `MBA.Modulo4/Alunos/Api`

```bash
├── MBA.Modulo4.sln # Arquivo de solução na raiz do repositório
├── src/
│   ├── Alunos/ # Nome do Contexto
│   │   ├── Api/Api.csproj # Projeto de API
│   │   ├── Application/Application.csproj # Projeto de Aplicação
│   │   ├── Domain/Domain.csproj # Projeto de Domínio
.   .   .
│   ├── Conteudo/ # Nome do Contexto
│   │   ├── Api/Api.csproj
.   .   .
├── tests/ # Pasta de Testes, sob ./src
│   ├── Core.Tests.csproj # Projeto de Testes Comuns
│   ├── Alunos.Tests.csproj # Projeto de Testes do Contexto Alunos
│   ├── Conteudo.Tests.csproj # Projeto de Testes do Contexto Conteudo
.   .
```

E na "Solution Explorer" do Visual Studio:
```
MBA.Modulo4
├── Alunos 
│   ├── Api
│   ├── Application
│   ├── Domain
│   ├── Data
.   .
├── Conteudo
│   ├── Api
.   .
├── Tests
│   ├── Alunos
│   ├── Conteudo
.   .
```

## Resolução de Feedbacks 10%

Avalie a resolução dos problemas apontados na primeira avaliação de frontend
* Será revisado na avalição final.


## Notas

| Critério                     | Peso | Nota | Nota Ponderada |
|------------------------------|------|-----:|---------------:|
| Funcionalidade               | 30%  |    5 |            1.5 |
| Qualidade do Código          | 20%  |    7 |            1.4 |
| Eficiência e Desempenho      | 20%  |    9 |            1.8 |
| Inovação e Diferenciais      | 10%  |    8 |            0.8 |
| Documentação e Organização   | 10%  |    5 |            0.5 |
| Resolução de Feedbacks       | 10%  |    5 |            0.5 |
| **Total**                    |      |      |        **6.5** |

