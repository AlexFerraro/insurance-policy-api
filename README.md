# Insurance Policy API

API para gerenciamento de apólices de seguros e parcelas de pagamento.

## Políticas de Uso

A API Insurance Policy API fornece endpoints para criar, recuperar, atualizar e pagar parcelas de apólices de seguro. Os endpoints aceitam e retornam dados no formato JSON.

## URL Base

A URL base para todos os endpoints da API é:

```
http://seu-host/v1/api/
```

## Endpoints da API

### Políticas de Seguro

#### Criar uma apólice

Cria uma nova apólice de seguro juntamente com suas parcelas de pagamento.

- Método: `POST`
- URL: `/apolice`
- Content-Type: `application/json`

##### Parâmetros da Solicitação

O corpo da solicitação deve conter um objeto JSON contendo os detalhes da apólice de seguro a ser criada. Os seguintes campos são obrigatórios:

- `descricao` (string): Descrição da apólice.
- `cpf` (long): Número de CPF do titular da apólice.
- `parcelas` (array): Lista de parcelas de pagamento.

Cada parcela deve conter os seguintes campos:

- `premio` (decimal): Valor do prêmio da parcela.
- `formaPagamento` (string): Forma de pagamento da parcela. Valores permitidos: CARTAO, BOLETO, DINHEIRO.
- `dataPagamento` (string, formato ISO 8601): Data de pagamento da parcela.

Exemplo de corpo de solicitação:

```json
{
  "descricao": "Apólice de seguro de automóvel",
  "cpf": 12345678901,
  "parcelas": [
    {
      "premio": 1000.00,
      "formaPagamento": "CARTAO",
      "dataPagamento": "2023-07-05T00:00:00Z"
    },
    {
      "premio": 800.00,
      "formaPagamento": "BOLETO",
      "dataPagamento": "2023-07-10T00:00:00Z"
    }
  ]
}
```

##### Resposta

- Status: 201 Created
- Content-Type: `application/json`

Exemplo de resposta:

```json
{
  "data": {
    "id": 1,
    "descricao": "Apólice de seguro de automóvel",
    "cpf": 12345678901,
    "parcelas": [
      {
        "id": 1,
        "premio": 1000.00,
        "formaPagamento": "CARTAO",
        "dataPagamento": "2023-07-05T00:00:00Z"
      },
      {
        "id": 2,
        "premio": 800.00,
        "formaPagamento": "BOLETO",
        "dataPagamento": "2023-07-10T00:00:00Z"
      }
    ]
  },
  "links": [
    {
      "href": "http://seu-host/v1/api/apolice/1",
      "rel": "self",
      "method": "GET"
    }
  ]
}
```

#### Obter todas as apólices

Recupera todas as apólices de seguro registradas.

- Método: `GET`
- URL: `/apolice`

##### Parâmetros da Solicitação

- `skip` (int): Número de registros a serem ignorados na consulta (utilizado para paginação).

- `take` (int): Número máximo de registros a serem retornados na consulta (utilizado para paginação).

Exemplo de URL de solicitação:

```
http://seu-host/v1/api/apolice?skip=0&take=10
```

##### Resposta

- Status: 200 OK
- Content-Type: `application/json`

Exemplo de resposta:

```json
{
  "data": [
    {
      "id": 1,
      "descricao": "Apólice de seguro de automóvel",
      "cpf": 12345678901,
      "parcelas": [
        {
          "id": 1,
          "premio": 1000.00,
          "formaPagamento": "CARTAO",
          "dataPagamento": "2023-07-05T00:00:00Z"
        },
        {
          "id": 2,
          "premio": 800.00,
          "formaPagamento": "BOLETO",
          "dataPagamento": "2023-07-10T00:00:00Z"
        }
      ]
    },
    {
      "id": 2,
      "descricao": "Apólice de seguro residencial",
      "cpf": 98765432109,
      "parcelas": [
        {
          "id": 3,
          "premio": 500.00,
          "formaPagamento": "CARTAO",
          "dataPagamento": "2023-07-12T00:00:00Z"
        }
      ]
    }
  ],
  "links": [
    {
      "href": "http://seu-host/v1/api/apolice?skip=0&take=10",
      "rel": "self",
      "method": "GET"
    }
  ]
}
```

#### Obter uma apólice por ID

Recupera uma apólice de seguro pelo seu ID.

- Método: `GET`
- URL: `/apolice/{id}`

##### Parâmetros da Solicitação

- `id` (long): ID da apólice de seguro.

Exemplo de URL de solicitação:

```
http://seu-host/v1/api/apolice/1
```

##### Resposta

- Status: 200 OK
- Content-Type: `application/json`

Exemplo de resposta:

```json
{
  "data": {
    "id": 1,
    "descricao": "Apólice de seguro de automóvel",
    "cpf": 12345678901,
    "parcelas": [
      {
        "id": 1,
        "premio": 1000.00,
        "formaPagamento": "CARTAO",
        "dataPagamento": "2023-07-05T00:00:00Z"
      },
      {
        "id": 2,
        "premio": 800.00,
        "formaPagamento": "BOLETO",
        "dataPagamento": "2023-07-10T00:00:00Z"
      }
    ]
  },
  "links": [
    {
      "href": "http://seu-host/v1/api/apolice/1",
      "rel": "self",
      "method": "GET"
    }
  ]
}
```

#### Atualizar uma apólice

Atualiza uma apólice de seguro e suas parcelas.

- Método: `PATCH`
- URL: `/apolice

##### Parâmetros da Solicitação

O corpo da solicitação deve conter um objeto JSON contendo os detalhes atualizados da apólice de seguro. O campo `id` é obrigatório e não pode ser zero. Os demais campos são opcionais e representam os novos valores dos campos da apólice.

Exemplo de corpo de solicitação:

```json
{
  "id": 1,
  "descricao": "Nova descrição da apólice",
  "cpf": 12345678901
}
```

##### Resposta

- Status: 200 OK
- Content-Type: `application/json`

Exemplo de resposta:

```json
{
  "data": {
    "id": 1,
    "descricao": "Nova descrição da apólice",
    "cpf": 12345678901,
    "parcelas": [
      {
        "id": 1,
        "premio": 1000.00,
        "formaPagamento": "CARTAO",
        "dataPagamento": "2023-07-05T00:00:00Z"
      },
      {
        "id": 2,
        "premio": 800.00,
        "formaPagamento": "BOLETO",
        "dataPagamento": "2023-07-10T00:00:00Z"
      }
    ]
  },
  "links": [
    {
      "href": "http://seu-host/v1/api/apolice/1",
      "rel": "self",
      "method": "GET"
    }
  ]
}
```

### Parcelas de Pagamento

#### Registrar pagamento de uma parcela

Registra o pagamento de uma parcela de uma apólice de seguro.

- Método: `POST`
- URL: `/parcela/{id}/pagamento`
- Content-Type: `application/json`

##### Parâmetros da Solicitação

- `id` (long): ID da parcela a ser paga.
- `paidDate` (string, formato ISO 8601): Data de pagamento da parcela.

Exemplo de URL de solicitação:

```
http://seu-host/v1/api/parcela/1/pagamento?paidDate=2023-07-05T00:00:00Z
```

##### Resposta

- Status: 200 OK
- Content-Type: `application/json`

Exemplo de resposta:

```json
{
  "links": [
    {
      "href": "http://seu-host/v1/api/apolice",
      "rel": "self",
      "method": "GET"
    }
  ]
}
```

# Padrões de Projetos Utilizados

A aplicação foi desenvolvida utilizando os seguintes padrões de projetos:

## Factory

O padrão Factory foi utilizado para a criação de objetos relacionados à apólice de seguro e suas parcelas de pagamento. Ele permite encapsular a lógica de criação desses objetos em uma classe ou método separado, fornecendo uma forma mais flexível e desacoplada de criar instâncias.

## Unit of Work

O padrão Unit of Work foi utilizado para gerenciar transações e operações de persistência de dados relacionadas à apólice de seguro e suas parcelas. Ele permite agrupar várias operações em uma única transação e garantir a consistência dos dados ao persisti-los no banco de dados.

## Repository

O padrão Repository foi utilizado para abstrair o acesso e a manipulação dos dados da apólice de seguro e suas parcelas. Ele fornece uma camada de abstração entre a lógica de negócio e a camada de persistência de dados, permitindo que a aplicação se torne independente do mecanismo de armazenamento utilizado.

## DTO (Data Transfer Object)

O padrão DTO foi utilizado para transferir dados entre os diferentes componentes da aplicação. Ele permite definir objetos simples que encapsulam os dados necessários para uma determinada operação ou comunicação, evitando o acoplamento excessivo entre os componentes.

# Arquitetura Onion

A aplicação foi construída seguindo a arquitetura Onion (ou Clean Architecture). Essa arquitetura propõe uma separação clara e modular das responsabilidades da aplicação, visando a manutenibilidade, testabilidade e independência de frameworks externos.

Na arquitetura Onion, a aplicação é dividida em camadas, cada uma com uma responsabilidade específica:

- Camada de Domínio (Core): Contém as entidades de domínio, regras de negócio e interfaces dos serviços utilizados pela aplicação.
- Camada de Aplicação: Contém os casos de uso da aplicação, que implementam as regras de negócio e orquestram a interação entre as camadas.
- Camada de Infraestrutura: Contém a implementação dos serviços externos, como acesso a banco de dados, APIs externas, entre outros.
- Camada de Interface: Contém as interfaces de interação com o usuário, como APIs REST, interfaces gráficas, entre outros.

Essa arquitetura permite que a aplicação seja independente de frameworks externos, facilita a realização de testes automatizados e proporciona uma maior flexibilidade e manutenibilidade ao longo do tempo.

# Considerações Finais

Esta API permite a criação, recuperação, atualização e pagamento de parcelas de apólices de seguro. Certifique-se de fornecer os dados corretos ao fazer solicitações aos endpoints correspondentes. Em caso de dúvidas ou problemas, entre em contato com o suporte técnico.