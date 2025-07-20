# Plataforma Educacional Distribuída - Guia de Desenvolvimento

## Visão Geral do Projeto

Este guia apresenta o passo-a-passo para desenvolver uma plataforma educacional baseada em microsserviços REST, evoluindo de uma aplicação monolítica para um ecossistema distribuído com comunicação resiliente, mensageria e segurança JWT.

## Estrutura de Pastas do Projeto

```
mba.modulo4/
├── docs/
│   ├── Projeto-Quarto-Modulo-Mba-DevXpert.md
│   ├── desenvolvimento-passo-a-passo.md
│   ├── arquitetura-microsservicos.md
│   └── README.md
├── backend/
│   ├── auth-api/                          # Microsserviço independente
│   │   ├── src/
│   │   │   ├── Auth.API/
│   │   │   ├── Auth.Domain/
│   │   │   ├── Auth.Infrastructure/
│   │   │   └── Auth.Application/
│   │   ├── tests/
│   │   ├── docker/
│   │   │   ├── Dockerfile
│   │   │   └── docker-compose.yml
│   │   ├── Auth.API.sln
│   │   └── README.md
│   ├── conteudo-api/                      # Microsserviço independente
│   │   ├── src/
│   │   │   ├── Conteudo.API/
│   │   │   ├── Conteudo.Domain/
│   │   │   ├── Conteudo.Infrastructure/
│   │   │   └── Conteudo.Application/
│   │   ├── tests/
│   │   ├── docker/
│   │   │   ├── Dockerfile
│   │   │   └── docker-compose.yml
│   │   ├── Conteudo.API.sln
│   │   └── README.md
│   ├── alunos-api/                        # Microsserviço independente
│   │   ├── src/
│   │   │   ├── Alunos.API/
│   │   │   ├── Alunos.Domain/
│   │   │   ├── Alunos.Infrastructure/
│   │   │   └── Alunos.Application/
│   │   ├── tests/
│   │   ├── docker/
│   │   │   ├── Dockerfile
│   │   │   └── docker-compose.yml
│   │   ├── Alunos.API.sln
│   │   └── README.md
│   ├── pagamentos-api/                    # Microsserviço independente
│   │   ├── src/
│   │   │   ├── Pagamentos.API/
│   │   │   ├── Pagamentos.Domain/
│   │   │   ├── Pagamentos.Infrastructure/
│   │   │   └── Pagamentos.Application/
│   │   ├── tests/
│   │   ├── docker/
│   │   │   ├── Dockerfile
│   │   │   └── docker-compose.yml
│   │   ├── Pagamentos.API.sln
│   │   └── README.md
│   ├── bff-api/                           # Microsserviço independente
│   │   ├── src/
│   │   │   ├── BFF.API/
│   │   │   ├── BFF.Domain/
│   │   │   ├── BFF.Infrastructure/
│   │   │   └── BFF.Application/
│   │   ├── tests/
│   │   ├── docker/
│   │   │   ├── Dockerfile
│   │   │   └── docker-compose.yml
│   │   ├── BFF.API.sln
│   │   └── README.md
│   └── docker-compose.yml                 # Apenas para desenvolvimento local
├── frontend/
│   ├── src/
│   │   ├── app/
│   │   ├── environments/
│   │   └── assets/
│   ├── package.json
│   └── README.md
└── infrastructure/
    ├── docker/
    │   ├── rabbitmq/
    │   ├── sqlserver/
    │   └── redis/
    └── scripts/
        ├── start-all.sh
        └── stop-all.sh
```

## Fase 1: Preparação e Configuração Inicial

### 1.1 Configuração do Ambiente

**Objetivo**: Configurar o ambiente de desenvolvimento e infraestrutura básica

**Atividades**:
- [ ] Configurar ambiente .NET 8
- [ ] Instalar Docker e Docker Compose
- [ ] Configurar RabbitMQ via Docker
- [ ] Configurar SQL Server e SQLite
- [ ] Configurar Angular 18
- [ ] Criar estrutura de pastas do projeto

### 1.2 Configuração da Infraestrutura

**Objetivo**: Preparar a infraestrutura base (externa aos microsserviços)

**Atividades**:
- [ ] Configurar RabbitMQ via Docker
- [ ] Configurar SQL Server via Docker
- [ ] Configurar Redis via Docker
- [ ] Criar scripts de inicialização da infraestrutura
- [ ] Configurar Swagger para documentação das APIs
- [ ] Preparar estrutura de pastas para cada microsserviço

## Princípios de Independência dos Microsserviços

### ✅ Características de Independência Implementadas:

1. **Codebase Independente**: Cada microsserviço tem sua própria solution (.sln)
2. **Deployment Independente**: Cada serviço pode ser implantado separadamente
3. **Banco de Dados Próprio**: Database per service pattern
4. **Containerização Individual**: Dockerfile e docker-compose próprios
5. **Testes Independentes**: Suíte de testes para cada serviço
6. **Versionamento Independente**: Cada serviço pode evoluir em seu próprio ritmo
7. **Equipes Independentes**: Cada serviço pode ser desenvolvido por equipes diferentes
8. **Tecnologia Flexível**: Liberdade para escolher stack tecnológico por serviço

### ❌ Anti-Patterns Evitados:

1. **Shared Libraries**: Removidos projetos compartilhados
2. **Shared Database**: Cada serviço tem seu próprio banco
3. **Synchronous Communication**: Uso de mensageria assíncrona
4. **Centralized Configuration**: Configuração por serviço
5. **Monolith Decomposition**: Evitado serviços muito acoplados

## Fase 2: Desenvolvimento dos Microsserviços

### 2.1 Auth API (Contexto de Autenticação)

**Objetivo**: Desenvolver o serviço de autenticação centralizada

**Atividades**:
- [ ] Criar solution independente Auth.API.sln
- [ ] Implementar Clean Architecture (API, Domain, Application, Infrastructure)
- [ ] Configurar ASP.NET Core Identity
- [ ] Implementar JWT Token generation
- [ ] Implementar endpoints de registro/login
- [ ] Implementar refresh token
- [ ] Configurar banco de dados próprio (AuthDB)
- [ ] Implementar validação de credenciais
- [ ] Criar middleware de autenticação JWT
- [ ] Implementar publisher de eventos (UserRegistered, UserLoggedIn)
- [ ] Configurar Dockerfile e docker-compose próprios
- [ ] Implementar logs de auditoria
- [ ] Testes unitários e de integração

**Endpoints**:
- `POST /api/auth/register` - Cadastro de usuários
- `POST /api/auth/login` - Login e geração de JWT
- `POST /api/auth/refresh` - Renovação de token
- `POST /api/auth/logout` - Logout

### 2.2 Conteúdo API (Contexto de Cursos e Aulas)

**Objetivo**: Gerenciar cursos, aulas e conteúdo programático

**Atividades**:
- [ ] Criar projeto Conteudo.API
- [ ] Implementar modelos de Curso, Aula e Conteúdo
- [ ] Configurar Entity Framework Core
- [ ] Implementar repositórios
- [ ] Criar endpoints CRUD para cursos
- [ ] Criar endpoints CRUD para aulas
- [ ] Implementar consulta de conteúdo programático
- [ ] Configurar autorização baseada em roles
- [ ] Implementar validações de negócio
- [ ] Testes unitários e de integração

**Endpoints**:
- `GET /api/cursos` - Listar cursos
- `POST /api/cursos` - Criar curso (Admin)
- `GET /api/cursos/{id}` - Obter curso específico
- `PUT /api/cursos/{id}` - Atualizar curso (Admin)
- `DELETE /api/cursos/{id}` - Remover curso (Admin)
- `GET /api/cursos/{id}/aulas` - Listar aulas do curso
- `POST /api/cursos/{id}/aulas` - Criar aula (Admin)

### 2.3 Alunos API (Contexto de Matrículas e Progresso)

**Objetivo**: Gerenciar matrículas, progresso e certificados

**Atividades**:
- [ ] Criar projeto Alunos.API
- [ ] Implementar modelos de Matrícula, Progresso e Certificado
- [ ] Configurar Entity Framework Core
- [ ] Implementar sistema de matrículas
- [ ] Implementar tracking de progresso
- [ ] Implementar geração de certificados
- [ ] Configurar consumer de eventos de pagamento
- [ ] Implementar histórico do aluno
- [ ] Configurar comunicação HTTP com outras APIs
- [ ] Testes unitários e de integração

**Endpoints**:
- `POST /api/matriculas` - Realizar matrícula
- `GET /api/matriculas/aluno/{id}` - Obter matrículas do aluno
- `PUT /api/progresso/{aulaId}` - Atualizar progresso da aula
- `GET /api/progresso/curso/{cursoId}` - Obter progresso do curso
- `POST /api/certificados/{cursoId}` - Gerar certificado
- `GET /api/certificados/aluno/{id}` - Obter certificados do aluno

### 2.4 Pagamentos API (Contexto de Pagamentos)

**Objetivo**: Processar pagamentos e emitir eventos

**Atividades**:
- [ ] Criar projeto Pagamentos.API
- [ ] Implementar modelos de Pagamento e Transação
- [ ] Configurar Entity Framework Core
- [ ] Implementar processamento de pagamentos
- [ ] Implementar consulta de status
- [ ] Configurar publisher de eventos
- [ ] Implementar webhook de confirmação
- [ ] Configurar retry e circuit breaker (Polly)
- [ ] Implementar logs de transações
- [ ] Testes unitários e de integração

**Endpoints**:
- `POST /api/pagamentos` - Processar pagamento
- `GET /api/pagamentos/{id}` - Consultar status do pagamento
- `GET /api/pagamentos/aluno/{id}` - Histórico de pagamentos do aluno
- `POST /api/pagamentos/{id}/webhook` - Webhook de confirmação

### 2.5 BFF API (Backend for Frontend)

**Objetivo**: Orquestrar chamadas entre microsserviços para o frontend

**Atividades**:
- [ ] Criar projeto BFF.API
- [ ] Implementar cliente HTTP para todas as APIs
- [ ] Configurar HttpClientFactory
- [ ] Implementar orquestração de fluxos complexos
- [ ] Implementar agregação de dados
- [ ] Configurar cache distribuído
- [ ] Implementar circuit breaker para resiliência
- [ ] Configurar CORS para frontend
- [ ] Implementar rate limiting
- [ ] Testes de integração

**Endpoints**:
- `GET /api/dashboard/aluno` - Dashboard do aluno
- `GET /api/dashboard/admin` - Dashboard do administrador
- `POST /api/matricula-completa` - Fluxo completo de matrícula
- `GET /api/curso-detalhado/{id}` - Curso com progresso do aluno

## Comunicação Entre Microsserviços Independentes

### Estratégias para Evitar Acoplamento:

#### 1. **Contratos de API**
- Cada microsserviço define seu próprio contrato de API
- Versionamento independente de APIs
- Documentação OpenAPI (Swagger) por serviço
- Backward compatibility mantida individualmente

#### 2. **Eventos de Domínio**
- Cada serviço define seus próprios eventos
- Schema Registry para versionamento de eventos
- Eventos publicados via RabbitMQ
- Consumers lidam com versões diferentes

#### 3. **Comunicação HTTP**
- HttpClient com typed clients
- Polly para resiliência
- Service discovery via configuração
- Circuit breaker por serviço

#### 4. **Shared Nothing Architecture**
- Sem bibliotecas compartilhadas
- Duplicação intencional de código quando necessário
- Cada serviço tem suas próprias validações
- Modelos de dados independentes

## Fase 3: Mensageria e Comunicação

### 3.1 Configuração do RabbitMQ

**Objetivo**: Configurar sistema de mensageria

**Atividades**:
- [ ] Configurar RabbitMQ via Docker
- [ ] Criar filas e exchanges
- [ ] Implementar padrões de mensageria
- [ ] Configurar dead letter queues
- [ ] Implementar retry policies
- [ ] Configurar monitoramento

### 3.2 Implementação de Eventos

**Objetivo**: Implementar comunicação assíncrona

**Atividades**:
- [ ] Criar eventos de domínio
- [ ] Implementar publishers
- [ ] Implementar consumers
- [ ] Configurar serialização de mensagens
- [ ] Implementar idempotência
- [ ] Configurar logs de eventos

**Eventos**:
- `PagamentoConfirmado` - Pagamento foi confirmado
- `PagamentoRejeitado` - Pagamento foi rejeitado
- `MatriculaRealizada` - Matrícula foi efetivada
- `CursoFinalizado` - Curso foi concluído pelo aluno
- `CertificadoGerado` - Certificado foi gerado

## Fase 4: Desenvolvimento do Frontend

### 4.1 Configuração do Angular 18

**Objetivo**: Configurar aplicação frontend

**Atividades**:
- [ ] Migrar base do Angular v18
- [ ] Configurar roteamento
- [ ] Configurar interceptors HTTP
- [ ] Implementar guards de autenticação
- [ ] Configurar gerenciamento de estado
- [ ] Configurar ambiente de desenvolvimento/produção

### 4.2 Implementação de Componentes

**Objetivo**: Desenvolver interface do usuário

**Atividades**:
- [ ] Criar componente de login
- [ ] Criar componente de registro
- [ ] Criar dashboard do aluno
- [ ] Criar dashboard do administrador
- [ ] Criar componente de cursos
- [ ] Criar componente de aulas
- [ ] Criar componente de pagamentos
- [ ] Criar componente de certificados
- [ ] Implementar navegação responsiva
- [ ] Implementar notificações toast

### 4.3 Integração com APIs

**Objetivo**: Integrar frontend com BFF

**Atividades**:
- [ ] Criar serviços para consumir APIs
- [ ] Implementar autenticação JWT
- [ ] Implementar refresh token automático
- [ ] Configurar tratamento de erros
- [ ] Implementar loading states
- [ ] Configurar cache local
- [ ] Implementar retry automático

## Fase 5: Testes e Qualidade

### 5.1 Testes Automatizados

**Objetivo**: Garantir qualidade do código

**Atividades**:
- [ ] Testes unitários para cada API
- [ ] Testes de integração entre APIs
- [ ] Testes de contrato (Pact)
- [ ] Testes de performance
- [ ] Testes de segurança
- [ ] Testes de resiliência
- [ ] Testes end-to-end

### 5.2 Monitoramento e Observabilidade

**Objetivo**: Implementar monitoramento

**Atividades**:
- [ ] Configurar logs estruturados
- [ ] Implementar health checks
- [ ] Configurar métricas
- [ ] Implementar distributed tracing
- [ ] Configurar alertas
- [ ] Documentar troubleshooting

## Fase 6: Deployment e Operações

### 6.1 Containerização

**Objetivo**: Preparar aplicação para deploy

**Atividades**:
- [ ] Criar Dockerfiles para cada API
- [ ] Configurar Docker Compose
- [ ] Otimizar imagens Docker
- [ ] Configurar multi-stage builds
- [ ] Implementar health checks

### 6.2 Documentação

**Objetivo**: Documentar o sistema

**Atividades**:
- [ ] Documentar APIs com Swagger
- [ ] Criar README.md completo
- [ ] Documentar arquitetura
- [ ] Criar guia de instalação
- [ ] Documentar troubleshooting
- [ ] Criar diagrama de arquitetura

---

# Cards do Jira

## Epic: Plataforma Educacional Distribuída

### Sprint 1: Infraestrutura e Autenticação

#### CARD-001: Configuração do Ambiente de Desenvolvimento
**Tipo**: Task  
**Prioridade**: Highest  
**Story Points**: 3  
**Descrição**: Configurar ambiente de desenvolvimento com .NET 8, Docker, RabbitMQ, SQL Server e Angular 18  
**Critérios de Aceite**:
- [ ] Ambiente .NET 8 configurado
- [ ] Docker e Docker Compose instalados
- [ ] RabbitMQ rodando em container
- [ ] SQL Server configurado
- [ ] Angular 18 configurado
- [ ] Estrutura de pastas criada

#### CARD-002: Implementação da Auth API
**Tipo**: Story  
**Prioridade**: Highest  
**Story Points**: 8  
**Descrição**: Desenvolver API de autenticação com ASP.NET Core Identity e JWT  
**Critérios de Aceite**:
- [ ] Projeto Auth.API criado
- [ ] ASP.NET Core Identity configurado
- [ ] JWT Token generation implementado
- [ ] Endpoints de registro/login funcionando
- [ ] Refresh token implementado
- [ ] Testes unitários criados
- [ ] Swagger documentado

#### CARD-003: Configuração de Infraestrutura Base
**Tipo**: Task  
**Prioridade**: High  
**Story Points**: 5  
**Descrição**: Configurar infraestrutura externa independente dos microsserviços  
**Critérios de Aceite**:
- [ ] RabbitMQ configurado via Docker
- [ ] SQL Server configurado via Docker
- [ ] Redis configurado via Docker
- [ ] Scripts de inicialização criados
- [ ] Estrutura de pastas para cada microsserviço
- [ ] Documentação básica criada

### Sprint 2: APIs de Negócio

#### CARD-004: Implementação da Conteúdo API
**Tipo**: Story  
**Prioridade**: High  
**Story Points**: 8  
**Descrição**: Desenvolver API para gerenciar cursos, aulas e conteúdo programático  
**Critérios de Aceite**:
- [ ] Projeto Conteudo.API criado
- [ ] Modelos de Curso, Aula implementados
- [ ] Entity Framework configurado
- [ ] Endpoints CRUD implementados
- [ ] Autorização baseada em roles
- [ ] Validações de negócio implementadas
- [ ] Testes unitários e integração

#### CARD-005: Implementação da Alunos API
**Tipo**: Story  
**Prioridade**: High  
**Story Points**: 10  
**Descrição**: Desenvolver API para matrículas, progresso e certificados  
**Critérios de Aceite**:
- [ ] Projeto Alunos.API criado
- [ ] Sistema de matrículas implementado
- [ ] Tracking de progresso funcionando
- [ ] Geração de certificados implementada
- [ ] Consumer de eventos de pagamento
- [ ] Histórico do aluno disponível
- [ ] Testes completos

#### CARD-006: Implementação da Pagamentos API
**Tipo**: Story  
**Prioridade**: High  
**Story Points**: 8  
**Descrição**: Desenvolver API para processar pagamentos e emitir eventos  
**Critérios de Aceite**:
- [ ] Projeto Pagamentos.API criado
- [ ] Processamento de pagamentos implementado
- [ ] Consulta de status funcionando
- [ ] Publisher de eventos configurado
- [ ] Webhook de confirmação implementado
- [ ] Polly configurado para resiliência
- [ ] Logs de transações implementados

### Sprint 3: BFF e Mensageria

#### CARD-007: Implementação do BFF
**Tipo**: Story  
**Prioridade**: High  
**Story Points**: 10  
**Descrição**: Desenvolver Backend for Frontend para orquestrar chamadas  
**Critérios de Aceite**:
- [ ] Projeto BFF.API criado
- [ ] Cliente HTTP para todas as APIs
- [ ] HttpClientFactory configurado
- [ ] Orquestração de fluxos complexos
- [ ] Agregação de dados implementada
- [ ] Cache distribuído configurado
- [ ] Circuit breaker implementado
- [ ] CORS configurado para frontend

#### CARD-008: Configuração do Sistema de Mensageria
**Tipo**: Task  
**Prioridade**: High  
**Story Points**: 6  
**Descrição**: Configurar RabbitMQ e implementar sistema de eventos  
**Critérios de Aceite**:
- [ ] RabbitMQ configurado via Docker
- [ ] Filas e exchanges criadas
- [ ] Padrões de mensageria implementados
- [ ] Dead letter queues configuradas
- [ ] Retry policies implementadas
- [ ] Monitoramento configurado

#### CARD-009: Implementação de Eventos de Domínio
**Tipo**: Story  
**Prioridade**: High  
**Story Points**: 8  
**Descrição**: Implementar comunicação assíncrona entre microsserviços  
**Critérios de Aceite**:
- [ ] Eventos de domínio criados
- [ ] Publishers implementados
- [ ] Consumers implementados
- [ ] Serialização de mensagens configurada
- [ ] Idempotência implementada
- [ ] Logs de eventos configurados

### Sprint 4: Frontend Angular

#### CARD-010: Configuração do Frontend Angular
**Tipo**: Task  
**Prioridade**: High  
**Story Points**: 5  
**Descrição**: Configurar aplicação Angular 18 com base existente  
**Critérios de Aceite**:
- [ ] Base do Angular v18 migrada
- [ ] Roteamento configurado
- [ ] Interceptors HTTP implementados
- [ ] Guards de autenticação criados
- [ ] Gerenciamento de estado configurado
- [ ] Ambientes dev/prod configurados

#### CARD-011: Implementação de Componentes de Autenticação
**Tipo**: Story  
**Prioridade**: High  
**Story Points**: 6  
**Descrição**: Desenvolver componentes de login e registro  
**Critérios de Aceite**:
- [ ] Componente de login criado
- [ ] Componente de registro criado
- [ ] Validações de formulário implementadas
- [ ] Integração com Auth API
- [ ] Tratamento de erros implementado
- [ ] Design responsivo aplicado

#### CARD-012: Implementação de Dashboards
**Tipo**: Story  
**Prioridade**: High  
**Story Points**: 10  
**Descrição**: Desenvolver dashboards para aluno e administrador  
**Critérios de Aceite**:
- [ ] Dashboard do aluno criado
- [ ] Dashboard do administrador criado
- [ ] Componentes de cursos implementados
- [ ] Componentes de aulas implementados
- [ ] Componentes de pagamentos implementados
- [ ] Componentes de certificados implementados
- [ ] Navegação responsiva implementada

### Sprint 5: Integração e Testes

#### CARD-013: Integração Frontend com BFF
**Tipo**: Story  
**Prioridade**: High  
**Story Points**: 8  
**Descrição**: Integrar frontend Angular com Backend for Frontend  
**Critérios de Aceite**:
- [ ] Serviços para consumir APIs criados
- [ ] Autenticação JWT implementada
- [ ] Refresh token automático configurado
- [ ] Tratamento de erros implementado
- [ ] Loading states implementados
- [ ] Cache local configurado
- [ ] Retry automático implementado

#### CARD-014: Implementação de Testes Automatizados
**Tipo**: Task  
**Prioridade**: Medium  
**Story Points**: 13  
**Descrição**: Criar suíte completa de testes automatizados  
**Critérios de Aceite**:
- [ ] Testes unitários para todas as APIs
- [ ] Testes de integração entre APIs
- [ ] Testes de contrato implementados
- [ ] Testes de performance executados
- [ ] Testes de segurança implementados
- [ ] Testes de resiliência criados
- [ ] Testes end-to-end funcionando

#### CARD-015: Configuração de Monitoramento
**Tipo**: Task  
**Prioridade**: Medium  
**Story Points**: 8  
**Descrição**: Implementar monitoramento e observabilidade  
**Critérios de Aceite**:
- [ ] Logs estruturados configurados
- [ ] Health checks implementados
- [ ] Métricas configuradas
- [ ] Distributed tracing implementado
- [ ] Alertas configurados
- [ ] Troubleshooting documentado

### Sprint 6: Deployment e Documentação

#### CARD-016: Containerização e Deploy
**Tipo**: Task  
**Prioridade**: Medium  
**Story Points**: 8  
**Descrição**: Preparar aplicação para deployment  
**Critérios de Aceite**:
- [ ] Dockerfiles criados para cada API
- [ ] Docker Compose configurado
- [ ] Imagens Docker otimizadas
- [ ] Multi-stage builds implementados
- [ ] Health checks configurados

#### CARD-017: Documentação Completa
**Tipo**: Task  
**Prioridade**: Medium  
**Story Points**: 5  
**Descrição**: Criar documentação completa do sistema  
**Critérios de Aceite**:
- [ ] APIs documentadas com Swagger
- [ ] README.md completo criado
- [ ] Arquitetura documentada
- [ ] Guia de instalação criado
- [ ] Troubleshooting documentado
- [ ] Diagrama de arquitetura criado

#### CARD-018: Validação Final e Entrega
**Tipo**: Task  
**Prioridade**: Highest  
**Story Points**: 5  
**Descrição**: Validar funcionamento completo e preparar entrega  
**Critérios de Aceite**:
- [ ] Todos os fluxos de negócio funcionando
- [ ] Resiliência validada
- [ ] Performance validada
- [ ] Segurança validada
- [ ] Documentação completa
- [ ] Repositório GitHub organizado

---

## Definição de Pronto (Definition of Done)

Para cada card ser considerado "Pronto", deve atender:

### Código
- [ ] Código revisado por pelo menos 1 desenvolvedor
- [ ] Padrões de código seguidos (C# Conventions)
- [ ] Princípios SOLID aplicados
- [ ] Clean Code aplicado
- [ ] Sem code smells críticos

### Testes
- [ ] Testes unitários implementados (cobertura > 80%)
- [ ] Testes de integração implementados
- [ ] Todos os testes passando
- [ ] Testes de regressão executados

### Documentação
- [ ] API documentada no Swagger
- [ ] README atualizado
- [ ] Comentários de código adequados
- [ ] Guia de troubleshooting atualizado

### Deploy
- [ ] Build local executado com sucesso
- [ ] Aplicação rodando em ambiente de desenvolvimento
- [ ] Configurações de ambiente validadas
- [ ] Logs estruturados implementados

### Validação
- [ ] Critérios de aceite atendidos
- [ ] Demonstração para PO realizada
- [ ] Feedback incorporado
- [ ] Validação de segurança realizada

---

## Cronograma Sugerido

| Sprint | Duração | Foco Principal | Entregável |
|--------|---------|----------------|------------|
| Sprint 1 | 2 semanas | Infraestrutura e Auth | Auth API funcionando |
| Sprint 2 | 3 semanas | APIs de Negócio | Conteúdo, Alunos e Pagamentos APIs |
| Sprint 3 | 2 semanas | BFF e Mensageria | Comunicação entre serviços |
| Sprint 4 | 3 semanas | Frontend Angular | Interface completa |
| Sprint 5 | 2 semanas | Integração e Testes | Sistema integrado |
| Sprint 6 | 1 semana | Deploy e Documentação | Entrega final |

**Total**: 13 semanas (compatível com prazo do projeto)

---

## Riscos e Mitigações

| Risco | Impacto | Probabilidade | Mitigação |
|-------|---------|---------------|-----------|
| Complexidade da comunicação entre serviços | Alto | Média | Implementar BFF, usar patterns de resiliência |
| Problemas de performance | Médio | Baixa | Monitoramento, cache, otimização de queries |
| Dificuldade com mensageria | Alto | Média | Documentação detalhada, testes de integração |
| Prazo apertado | Alto | Média | Priorização de funcionalidades, MVP first |
| Problemas de sincronização | Médio | Média | Eventos bem definidos, idempotência |

---

Este guia serve como roadmap para o desenvolvimento da plataforma educacional distribuída. Cada card deve ser detalhado no momento de implementação, com tasks específicas e critérios de aceite refinados. 