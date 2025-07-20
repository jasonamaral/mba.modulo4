# Diagrama de Banco de Dados - Microsserviços Independentes

## Visão Geral da Arquitetura de Dados

Cada microsserviço possui seu próprio banco de dados isolado, seguindo o padrão **Database per Service**. A sincronização entre bancos acontece através de eventos assíncronos via RabbitMQ.

## Diagrama Geral dos Bancos de Dados

```mermaid
graph TB
    subgraph "Auth API - Porto 5001"
        AuthAPI[Auth.API]
        AuthDB[(AuthDB<br/>SQL Server)]
        AuthAPI --- AuthDB
    end
    
    subgraph "Conteudo API - Porto 5002"
        ConteudoAPI[Conteudo.API]
        ConteudoDB[(ConteudoDB<br/>SQL Server)]
        ConteudoAPI --- ConteudoDB
    end
    
    subgraph "Alunos API - Porto 5003"
        AlunosAPI[Alunos.API]
        AlunosDB[(AlunosDB<br/>SQL Server)]
        AlunosAPI --- AlunosDB
    end
    
    subgraph "Pagamentos API - Porto 5004"
        PagamentosAPI[Pagamentos.API]
        PagamentosDB[(PagamentosDB<br/>SQL Server)]
        PagamentosAPI --- PagamentosDB
    end
    
    subgraph "BFF API - Porto 5000"
        BFFAPI[BFF.API]
        Redis[(Redis Cache<br/>Distributed)]
        BFFAPI -.-> Redis
    end
    
    subgraph "Mensageria"
        RabbitMQ[RabbitMQ<br/>Message Broker]
    end
    
    %% Comunicação via Eventos
    AuthAPI -->|UserRegistered| RabbitMQ
    PagamentosAPI -->|PagamentoConfirmado| RabbitMQ
    AlunosAPI -->|MatriculaRealizada| RabbitMQ
    AlunosAPI -->|CertificadoGerado| RabbitMQ
    
    RabbitMQ -->|UserRegistered| AlunosAPI
    RabbitMQ -->|PagamentoConfirmado| AlunosAPI
    RabbitMQ -->|MatriculaRealizada| ConteudoAPI
    RabbitMQ -->|CertificadoGerado| ConteudoAPI
    
    %% Comunicação HTTP (BFF)
    BFFAPI -->|HTTP/JWT| AuthAPI
    BFFAPI -->|HTTP/JWT| ConteudoAPI
    BFFAPI -->|HTTP/JWT| AlunosAPI
    BFFAPI -->|HTTP/JWT| PagamentosAPI
    
    %% Styling
    classDef api fill:#e8f5e8
    classDef db fill:#fff3e0
    classDef cache fill:#f3e5f5
    classDef message fill:#fce4ec
    
    class AuthAPI,ConteudoAPI,AlunosAPI,PagamentosAPI,BFFAPI api
    class AuthDB,ConteudoDB,AlunosDB,PagamentosDB db
    class Redis cache
    class RabbitMQ message
```

## AuthDB - Banco de Dados de Autenticação

```mermaid
erDiagram
    Users {
        uniqueidentifier Id PK
        nvarchar_256 Email UK
        nvarchar_256 UserName UK
        nvarchar_max PasswordHash
        bit EmailConfirmed
        nvarchar_256 SecurityStamp
        nvarchar_256 ConcurrencyStamp
        nvarchar_max PhoneNumber
        bit PhoneNumberConfirmed
        bit TwoFactorEnabled
        datetimeoffset LockoutEnd
        bit LockoutEnabled
        int AccessFailedCount
        datetime2 CreatedAt
        datetime2 UpdatedAt
        bit IsActive
    }
    
    Roles {
        uniqueidentifier Id PK
        nvarchar_256 Name UK
        nvarchar_256 NormalizedName UK
        nvarchar_max ConcurrencyStamp
        nvarchar_500 Description
        datetime2 CreatedAt
        bit IsActive
    }
    
    UserRoles {
        uniqueidentifier UserId PK, FK
        uniqueidentifier RoleId PK, FK
        datetime2 AssignedAt
        uniqueidentifier AssignedBy FK
    }
    
    UserClaims {
        int Id PK
        uniqueidentifier UserId FK
        nvarchar_max ClaimType
        nvarchar_max ClaimValue
        datetime2 CreatedAt
    }
    
    UserLogins {
        nvarchar_128 LoginProvider PK
        nvarchar_128 ProviderKey PK
        nvarchar_max ProviderDisplayName
        uniqueidentifier UserId FK
        datetime2 CreatedAt
    }
    
    UserTokens {
        uniqueidentifier UserId PK, FK
        nvarchar_128 LoginProvider PK
        nvarchar_128 Name PK
        nvarchar_max Value
        datetime2 CreatedAt
        datetime2 ExpiresAt
    }
    
    RefreshTokens {
        uniqueidentifier Id PK
        uniqueidentifier UserId FK
        nvarchar_max Token UK
        datetime2 ExpiresAt
        datetime2 CreatedAt
        bit IsRevoked
        nvarchar_500 RevokedReason
    }
    
    AuditLogs {
        uniqueidentifier Id PK
        uniqueidentifier UserId FK
        nvarchar_100 Action
        nvarchar_500 Description
        nvarchar_45 IpAddress
        nvarchar_500 UserAgent
        datetime2 CreatedAt
        nvarchar_max AdditionalData
    }
    
    %% Relacionamentos
    Users ||--o{ UserRoles : "has"
    Roles ||--o{ UserRoles : "assigned to"
    Users ||--o{ UserClaims : "has"
    Users ||--o{ UserLogins : "has"
    Users ||--o{ UserTokens : "has"
    Users ||--o{ RefreshTokens : "has"
    Users ||--o{ AuditLogs : "performed"
```

## ConteudoDB - Banco de Dados de Conteúdo

```mermaid
erDiagram
    Cursos {
        uniqueidentifier Id PK
        nvarchar_200 Nome
        nvarchar_max Descricao
        decimal_10_2 Preco
        int CargaHoraria
        nvarchar_100 Categoria
        nvarchar_50 Nivel
        nvarchar_max ImagemUrl
        bit IsAtivo
        datetime2 CreatedAt
        datetime2 UpdatedAt
        uniqueidentifier CreatedBy
        uniqueidentifier UpdatedBy
    }
    
    Aulas {
        uniqueidentifier Id PK
        uniqueidentifier CursoId FK
        nvarchar_200 Titulo
        nvarchar_max Descricao
        int Ordem
        int DuracaoMinutos
        nvarchar_50 TipoConteudo
        nvarchar_max VideoUrl
        nvarchar_max MaterialUrl
        bit IsGratuita
        bit IsAtiva
        datetime2 CreatedAt
        datetime2 UpdatedAt
    }
    
    Materiais {
        uniqueidentifier Id PK
        uniqueidentifier AulaId FK
        nvarchar_200 Nome
        nvarchar_max Descricao
        nvarchar_50 TipoMaterial
        nvarchar_max Url
        bigint TamanhoBytes
        bit IsObrigatorio
        datetime2 CreatedAt
    }
    
    ConteudoProgramatico {
        uniqueidentifier Id PK
        uniqueidentifier CursoId FK
        nvarchar_200 Topico
        nvarchar_max Descricao
        int Ordem
        int CargaHoraria
        datetime2 CreatedAt
    }
    
    Categorias {
        uniqueidentifier Id PK
        nvarchar_100 Nome UK
        nvarchar_max Descricao
        nvarchar_100 Cor
        nvarchar_max IconeUrl
        bit IsAtiva
        datetime2 CreatedAt
    }
    
    CursoAcessos {
        uniqueidentifier Id PK
        uniqueidentifier CursoId FK
        uniqueidentifier UserId
        datetime2 DataAcesso
        nvarchar_45 IpAddress
        nvarchar_500 UserAgent
        int TempoPermanencia
    }
    
    %% Relacionamentos
    Cursos ||--o{ Aulas : "contains"
    Aulas ||--o{ Materiais : "has"
    Cursos ||--o{ ConteudoProgramatico : "has"
    Categorias ||--o{ Cursos : "categorizes"
    Cursos ||--o{ CursoAcessos : "accessed by"
```

## AlunosDB - Banco de Dados de Alunos

```mermaid
erDiagram
    Alunos {
        uniqueidentifier Id PK
        uniqueidentifier UserId UK
        nvarchar_200 Nome
        nvarchar_256 Email
        nvarchar_15 Telefone
        date DataNascimento
        nvarchar_1 Genero
        nvarchar_100 Cidade
        nvarchar_50 Estado
        nvarchar_10 CEP
        datetime2 CreatedAt
        datetime2 UpdatedAt
        bit IsAtivo
    }
    
    Matriculas {
        uniqueidentifier Id PK
        uniqueidentifier AlunoId FK
        uniqueidentifier CursoId
        nvarchar_50 Status
        decimal_10_2 ValorPago
        datetime2 DataMatricula
        datetime2 DataVencimento
        datetime2 DataCancelamento
        nvarchar_500 MotivoCancelamento
        datetime2 CreatedAt
        datetime2 UpdatedAt
    }
    
    Progresso {
        uniqueidentifier Id PK
        uniqueidentifier MatriculaId FK
        uniqueidentifier AulaId
        nvarchar_50 Status
        decimal_5_2 PercentualAssistido
        int TempoAssistido
        datetime2 DataInicio
        datetime2 DataConclusao
        datetime2 UltimoAcesso
        datetime2 CreatedAt
        datetime2 UpdatedAt
    }
    
    Certificados {
        uniqueidentifier Id PK
        uniqueidentifier MatriculaId FK
        uniqueidentifier AlunoId FK
        uniqueidentifier CursoId
        nvarchar_100 CodigoCertificado UK
        datetime2 DataEmissao
        datetime2 DataValidade
        nvarchar_max UrlCertificado
        nvarchar_50 Status
        nvarchar_max HashValidacao
        datetime2 CreatedAt
    }
    
    HistoricoAluno {
        uniqueidentifier Id PK
        uniqueidentifier AlunoId FK
        nvarchar_100 Acao
        nvarchar_500 Descricao
        nvarchar_max DetalhesJson
        datetime2 CreatedAt
    }
    
    Notas {
        uniqueidentifier Id PK
        uniqueidentifier MatriculaId FK
        uniqueidentifier AulaId
        decimal_4_2 NotaObtida
        decimal_4_2 NotaMaxima
        datetime2 DataAvaliacao
        nvarchar_max Observacoes
        datetime2 CreatedAt
    }
    
    AlunoPreferencias {
        uniqueidentifier Id PK
        uniqueidentifier AlunoId FK
        nvarchar_100 Chave
        nvarchar_max Valor
        datetime2 CreatedAt
        datetime2 UpdatedAt
    }
    
    %% Relacionamentos
    Alunos ||--o{ Matriculas : "has"
    Matriculas ||--o{ Progresso : "tracks"
    Matriculas ||--o{ Certificados : "generates"
    Matriculas ||--o{ Notas : "receives"
    Alunos ||--o{ HistoricoAluno : "has"
    Alunos ||--o{ AlunoPreferencias : "has"
```

## PagamentosDB - Banco de Dados de Pagamentos

```mermaid
erDiagram
    Pagamentos {
        uniqueidentifier Id PK
        uniqueidentifier MatriculaId UK
        uniqueidentifier AlunoId
        uniqueidentifier CursoId
        decimal_10_2 Valor
        nvarchar_50 Status
        nvarchar_50 MetodoPagamento
        nvarchar_100 TransacaoId
        nvarchar_100 GatewayPagamento
        datetime2 DataPagamento
        datetime2 DataVencimento
        datetime2 CreatedAt
        datetime2 UpdatedAt
    }
    
    Transacoes {
        uniqueidentifier Id PK
        uniqueidentifier PagamentoId FK
        nvarchar_100 TipoTransacao
        decimal_10_2 Valor
        nvarchar_50 Status
        nvarchar_100 ReferenciaTid
        nvarchar_100 AutorizacaoId
        nvarchar_max ResponseJson
        datetime2 DataTransacao
        datetime2 CreatedAt
    }
    
    StatusPagamento {
        uniqueidentifier Id PK
        uniqueidentifier PagamentoId FK
        nvarchar_50 StatusAnterior
        nvarchar_50 StatusAtual
        nvarchar_500 Motivo
        nvarchar_max DetalhesJson
        datetime2 DataMudanca
        datetime2 CreatedAt
    }
    
    Webhooks {
        uniqueidentifier Id PK
        uniqueidentifier PagamentoId FK
        nvarchar_100 Origem
        nvarchar_50 Evento
        nvarchar_max Payload
        nvarchar_50 Status
        datetime2 DataRecebimento
        datetime2 DataProcessamento
        int TentativasProcessamento
        nvarchar_max ErroProcessamento
    }
    
    Reembolsos {
        uniqueidentifier Id PK
        uniqueidentifier PagamentoId FK
        decimal_10_2 Valor
        nvarchar_50 Status
        nvarchar_500 Motivo
        nvarchar_100 ReembolsoId
        datetime2 DataSolicitacao
        datetime2 DataProcessamento
        datetime2 CreatedAt
    }
    
    LogsFinanceiros {
        uniqueidentifier Id PK
        uniqueidentifier PagamentoId FK
        nvarchar_100 Acao
        nvarchar_500 Descricao
        nvarchar_max DetalhesJson
        datetime2 CreatedAt
    }
    
    %% Relacionamentos
    Pagamentos ||--o{ Transacoes : "has"
    Pagamentos ||--o{ StatusPagamento : "tracks"
    Pagamentos ||--o{ Webhooks : "receives"
    Pagamentos ||--o{ Reembolsos : "can have"
    Pagamentos ||--o{ LogsFinanceiros : "logs"
```

## Cache Distribuído (Redis) - BFF API

```mermaid
graph TB
    subgraph "Redis Cache - BFF API"
        Cache[Redis Cache]
        
        subgraph "Cached Data"
            UserSessions[User Sessions<br/>jwt:user:{userId}]
            ApiResponses[API Responses<br/>api:response:{hash}]
            UserProfiles[User Profiles<br/>user:profile:{userId}]
            CursoDetails[Curso Details<br/>curso:details:{cursoId}]
            DashboardData[Dashboard Data<br/>dashboard:{userId}]
        end
        
        Cache --> UserSessions
        Cache --> ApiResponses
        Cache --> UserProfiles
        Cache --> CursoDetails
        Cache --> DashboardData
    end
    
    subgraph "TTL Configuration"
        TTL1[User Sessions: 30 min]
        TTL2[API Responses: 5 min]
        TTL3[User Profiles: 1 hour]
        TTL4[Curso Details: 15 min]
        TTL5[Dashboard Data: 10 min]
    end
```

## Fluxo de Dados Entre Bancos via Eventos

```mermaid
sequenceDiagram
    participant A as Aluno
    participant BFF as BFF API
    participant Auth as Auth API
    participant AuthDB as AuthDB
    participant Pag as Pagamentos API
    participant PagDB as PagamentosDB
    participant MQ as RabbitMQ
    participant Alunos as Alunos API
    participant AlunosDB as AlunosDB
    participant Cont as Conteudo API
    participant ContDB as ConteudoDB

    Note over A,ContDB: Fluxo: Matrícula com Pagamento
    
    A->>BFF: Solicita matrícula
    BFF->>Auth: Valida usuário
    Auth->>AuthDB: Consulta usuário
    AuthDB-->>Auth: Dados do usuário
    Auth-->>BFF: Usuário validado
    
    BFF->>Alunos: Cria matrícula pendente
    Alunos->>AlunosDB: INSERT Matricula (Status: Pendente)
    AlunosDB-->>Alunos: Matrícula criada
    Alunos-->>BFF: Matrícula pendente
    
    BFF->>Pag: Processa pagamento
    Pag->>PagDB: INSERT Pagamento
    PagDB-->>Pag: Pagamento criado
    Pag->>MQ: Publish: PagamentoConfirmado
    
    MQ->>Alunos: Consume: PagamentoConfirmado
    Alunos->>AlunosDB: UPDATE Matricula (Status: Ativa)
    AlunosDB-->>Alunos: Matrícula ativada
    Alunos->>MQ: Publish: MatriculaRealizada
    
    MQ->>Cont: Consume: MatriculaRealizada
    Cont->>ContDB: INSERT CursoAcesso
    ContDB-->>Cont: Acesso registrado
    
    BFF-->>A: Matrícula confirmada
```

## Sincronização de Dados de Referência

```mermaid
graph TB
    subgraph "Dados de Referência Sincronizados"
        subgraph "Auth API"
            AuthUser[User ID: abc123<br/>Email: aluno@teste.com<br/>Role: Aluno]
        end
        
        subgraph "Alunos API"
            AlunoProfile[Aluno ID: def456<br/>User ID: abc123<br/>Nome: João Silva]
        end
        
        subgraph "Conteudo API"
            CursoAccess[Curso ID: ghi789<br/>User ID: abc123<br/>Data Acesso: 2024-01-15]
        end
        
        subgraph "Pagamentos API"
            Payment[Pagamento ID: jkl012<br/>User ID: abc123<br/>Valor: R$ 299,99]
        end
        
        AuthUser -.->|UserRegistered Event| AlunoProfile
        Payment -.->|PagamentoConfirmado Event| AlunoProfile
        AlunoProfile -.->|MatriculaRealizada Event| CursoAccess
    end
    
    subgraph "Eventos de Sincronização"
        E1[UserRegistered<br/>- UserId<br/>- Email<br/>- UserType]
        E2[PagamentoConfirmado<br/>- MatriculaId<br/>- UserId<br/>- ValorPago]
        E3[MatriculaRealizada<br/>- AlunoId<br/>- CursoId<br/>- DataMatricula]
        E4[CertificadoGerado<br/>- AlunoId<br/>- CursoId<br/>- CertificadoId]
    end
```

## Considerações de Consistência

### Consistência Eventual
- **Princípio**: Dados podem ficar temporariamente inconsistentes
- **Solução**: Eventos assíncronos garantem sincronização eventual
- **Monitoramento**: Logs de eventos para rastreamento

### Transações Distribuídas
- **Problema**: Não há ACID entre bancos diferentes
- **Solução**: Saga Pattern para transações distribuídas
- **Compensação**: Eventos de rollback quando necessário

### Dados Duplicados
- **Princípio**: Duplicação intencional para independência
- **Exemplos**: UserId em todos os bancos
- **Sincronização**: Via eventos assíncronos

## Backup e Recuperação

```mermaid
graph TB
    subgraph "Estratégia de Backup"
        subgraph "Bancos Independentes"
            AuthBackup[AuthDB Backup<br/>Daily: 2AM]
            ConteudoBackup[ConteudoDB Backup<br/>Daily: 2:30AM]
            AlunosBackup[AlunosDB Backup<br/>Daily: 3AM]
            PagamentosBackup[PagamentosDB Backup<br/>Daily: 3:30AM]
        end
        
        subgraph "Point-in-Time Recovery"
            PITR[Transaction Log Backup<br/>Every 15 minutes]
        end
        
        subgraph "Disaster Recovery"
            DR[Geo-Replication<br/>Secondary Region]
        end
    end
```

## Monitoramento de Performance

```mermaid
graph TB
    subgraph "Métricas por Banco"
        subgraph "AuthDB Metrics"
            AuthConn[Connection Pool: 100]
            AuthQuery[Avg Query Time: 50ms]
            AuthTrans[Transactions/sec: 200]
        end
        
        subgraph "ConteudoDB Metrics"
            ContConn[Connection Pool: 50]
            ContQuery[Avg Query Time: 30ms]
            ContTrans[Transactions/sec: 100]
        end
        
        subgraph "AlunosDB Metrics"
            AlunosConn[Connection Pool: 80]
            AlunosQuery[Avg Query Time: 40ms]
            AlunosTrans[Transactions/sec: 150]
        end
        
        subgraph "PagamentosDB Metrics"
            PagConn[Connection Pool: 60]
            PagQuery[Avg Query Time: 60ms]
            PagTrans[Transactions/sec: 80]
        end
    end
    
    subgraph "Alertas"
        Alert1[Connection Pool > 80%]
        Alert2[Query Time > 100ms]
        Alert3[Deadlocks Detected]
        Alert4[Disk Space < 20%]
    end
```

---

Esta arquitetura de banco de dados garante:

✅ **Independência Completa**: Cada microsserviço tem seu próprio banco  
✅ **Escalabilidade Individual**: Pode escalar bancos independentemente  
✅ **Falhas Isoladas**: Falha em um banco não afeta outros  
✅ **Flexibilidade Tecnológica**: Cada banco pode usar tecnologia diferente  
✅ **Consistência Eventual**: Dados sincronizados via eventos  
✅ **Transações Distribuídas**: Saga pattern para fluxos complexos  

A estrutura segue as melhores práticas de microsserviços com **Database per Service Pattern**! 