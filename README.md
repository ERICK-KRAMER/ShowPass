# ShowPass

ShowPass é uma aplicação web desenvolvida em ASP.NET Core para gerenciamento de eventos e venda de ingressos.

## Tecnologias Utilizadas

- ASP.NET Core 8.0
- Entity Framework Core
- PostgreSQL
- JWT para autenticação
- BCrypt para hash de senhas
- Swagger para documentação da API

## Funcionalidades Principais

1. Gerenciamento de Usuários
2. Autenticação e Autorização
3. Gerenciamento de Eventos
4. Venda de Ingressos
5. Gerenciamento de Pedidos
6. Envio de E-mails

## Configuração do Projeto

### Pré-requisitos

- .NET 8.0 SDK
- PostgreSQL

### Instalação

1. Clone o repositório
2. Navegue até a pasta do projeto
3. Execute `dotnet restore` para restaurar as dependências
4. Configure a string de conexão do banco de dados no arquivo `appsettings.json`
5. Execute as migrações do banco de dados com `dotnet ef database update`
6. Inicie a aplicação com `dotnet run`

## Estrutura do Projeto

- `Controllers/`: Contém os controladores da API
- `Data/`: Contexto do banco de dados e configurações do Entity Framework
- `Models/`: Entidades e DTOs
- `Services/`: Serviços da aplicação (e.g., TokenService, EmailService)
- `Repositories/`: Interfaces e implementações dos repositórios

## Endpoints da API

- `GET /User`: Lista todos os usuários
- `POST /User`: Cria um novo usuário
- `POST /User/SendToken`: Envia token para redefinição de senha
- `POST /User/UpdatePassword`: Atualiza a senha do usuário
- `GET /Event`: Lista todos os eventos
- `POST /Event`: Cria um novo evento
- `DELETE /Event/{id}`: Remove um evento
- `GET /Order`: Lista todos os pedidos
- `POST /Order`: Cria um novo pedido
- `POST /Login`: Realiza login e retorna um token JWT

## Contribuição

Contribuições são bem-vindas! Por favor, leia o guia de contribuição antes de submeter pull requests.

## Licença

Este projeto está licenciado sob a [MIT License](LICENSE).
