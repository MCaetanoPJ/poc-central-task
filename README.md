# Criando a rede Docker:
Como o projeto utiliza diversos serviços que precisam se comunicar entre si, todos os containers devem estar na mesma rede.
Para isso, execute:

**docker network create app_network**

# Para iniciar o projeto, basta executar o comando docker:
**docker-compose up --build**

# (Front)🏗️ Arquitetura do Projeto Vue - CentralTask

Este documento descreve a arquitetura do projeto **CentralTask**, desenvolvido com Vue.js, Vite, Vue Router e Pinia.

---

## 📁 Estrutura Principal

centralTask/
├── public/
├── src/
│ ├── assets/
│ ├── components/ # Componentes reutilizáveis
│ ├── views/ # Páginas principais (roteadas)
│ ├── stores/ # Gerenciamento de estado com Pinia
│ ├── services/ # Chamadas à API (Axios)
│ ├── router/ # Definição de rotas com Vue Router
│ ├── App.vue # Componente raiz
│ └── main.js # Ponto de entrada da aplicação
├── .env # Variáveis de ambiente
├── package.json
├── vite.config.js
└── README.md


---

## 🔄 Fluxo de Funcionamento

1. `main.js` inicia o app, configurando Vue Router e Pinia.
2. `App.vue` serve como ponto de montagem visual.
3. `router/` define as rotas e associa as `views/`.
4. `views/` renderizam páginas conforme a navegação.
5. `components/` são usados dentro das views para funcionalidades específicas.
6. `stores/` armazenam e controlam o estado global (ex: lista de tarefas).
7. `services/` fazem chamadas HTTP usando Axios e fornecem dados para as stores.

---

## ✅ Boas Práticas Aplicadas

- **Separação de responsabilidades**: UI, lógica de estado e chamadas HTTP estão em camadas distintas.
- **Componentização**: Reutilização de componentes pequenos e específicos.
- **Gerenciamento de estado com Pinia**: Simples, escalável e reativo.
- **Serviços centralizados**: Evita duplicação de lógica de requisição HTTP.
- **Estrutura pronta para escalar**: Diretórios organizados e separados por contexto.

---

## 📋 Resumo em Tabela

| Camada        | Papel principal                                   | Localização         |
|---------------|----------------------------------------------------|---------------------|
| UI / Views    | Renderização das páginas via rotas                 | `src/views/`        |
| UI / Components| Elementos reutilizáveis dentro das views           | `src/components/`   |
| Rotas         | Define caminhos e quais views carregar             | `src/router/`       |
| Store         | Estado global e lógica de negócio                  | `src/stores/`       |
| Serviços      | Chamadas HTTP à API, abstração e tratamento        | `src/services/`     |
| Config/Env    | Variáveis e build (Vite config, chaves, endpoints) | `.env`, `vite.config.js` |

---

## 🛠️ Tecnologias Utilizadas

- **Vue 3**
- **Vite**
- **Vue Router**
- **Pinia**
- **Axios**

---

# (API)🏗️ Arquitetura da API - CentralTask

## 1. Descrição do Projeto

CentralTask é uma aplicação para cadastro e gerenciamento de tarefas, onde usuários autenticados podem criar e atribuir tarefas a outros usuários. A comunicação em tempo real entre o servidor e os clientes é feita via SignalR, permitindo que os usuários recebam notificações instantâneas quando uma tarefa é atribuída a eles.

A aplicação utiliza arquitetura CQRS para separar as operações de leitura e escrita, RabbitMQ para mensageria assíncrona, PostgreSQL como banco de dados relacional, e é toda executada dentro de containers Docker para garantir isolamento e facilidade de deploy.

---

## 2. Tecnologias Utilizadas

- **.NET 8**: Plataforma principal para desenvolvimento da aplicação.
- **CQRS (Command Query Responsibility Segregation)**: Arquitetura para separar comandos (escrita) e queries (leitura).
- **RabbitMQ**: Broker de mensageria para comunicação assíncrona entre componentes.
- **SignalR**: Biblioteca para comunicação em tempo real entre servidor e front-end.
- **PostgreSQL**: Banco de dados relacional para persistência dos dados.
- **Docker & Docker Compose**: Containerização e orquestração dos serviços da aplicação.

---

## 3. Arquitetura e Fluxo do Sistema

### 3.1 Fluxo Geral

1. O usuário realiza login na aplicação.
2. O usuário cria uma tarefa, atribuindo-a a um usuário específico autenticado.
3. A tarefa é persistida no banco de dados PostgreSQL.
4. Um comando CQRS é emitido para registrar a criação da tarefa.
5. RabbitMQ é utilizado para encaminhar eventos e comandos entre os componentes do sistema de forma assíncrona.
6. Ao atribuir a tarefa, uma mensagem é enviada via SignalR para o usuário destinatário em tempo real, notificando-o da nova tarefa.
7. O front-end recebe essa notificação instantaneamente e atualiza a interface do usuário.

### 3.2 Detalhes Técnicos

- **CQRS**:  
  - Commands tratam a lógica de alteração (criação/atualização/exclusão de tarefas).  
  - Queries tratam as consultas de dados (listagem de tarefas, usuários, etc).

- **RabbitMQ**:  
  - Atua como middleware para eventos e comandos, desacoplando produtores e consumidores e garantindo escalabilidade.

- **SignalR**:  
  - Garante comunicação bidirecional e instantânea, enviando notificações ao usuário específico via conexões persistentes.

- **Docker**:  
  - Cada componente (API, RabbitMQ, PostgreSQL) roda em containers isolados, facilitando desenvolvimento, testes e implantação.
