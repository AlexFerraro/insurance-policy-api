# Insurance Policy API

API para gerenciamento de ap�lices de seguros e parcelas de pagamento.

## Pol�ticas de Uso

A API Insurance Policy API fornece endpoints para criar, recuperar, atualizar e pagar parcelas de ap�lices de seguro. Os endpoints aceitam e retornam dados no formato JSON.

## URL Base

A URL base para todos os endpoints da API �:

```
http://seu-host/v1/api/
```

## Endpoints da API

### Pol�ticas de Seguro

#### Criar uma ap�lice

Cria uma nova ap�lice de seguro juntamente com suas parcelas de pagamento.

- M�todo: `POST`
- URL: `/apolice`
- Content-Type: `application/json`

##### Par�metros da Solicita��o

O corpo da solicita��o deve conter um objeto JSON contendo os detalhes da ap�lice de seguro a ser criada. Os seguintes campos s�o obrigat�rios:

- `descricao` (string): Descri��o da ap�lice.
- `cpf` (long): N�mero de CPF do titular da ap�lice.
- `parcelas` (array): Lista de parcelas de pagamento.

Cada parcela deve conter os seguintes campos:

- `premio` (decimal): Valor do pr�mio da parcela.
- `formaPagamento` (string): Forma de pagamento da parcela. Valores permitidos: CARTAO, BOLETO, DINHEIRO.
- `dataPagamento` (string, formato ISO 8601): Data de pagamento da parcela.

Exemplo de corpo de solicita��o:

```json
{
  "descricao": "Ap�lice de seguro de autom�vel",
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
    "descricao": "Ap�lice de seguro de autom�vel",
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

#### Obter todas as ap�lices

Recupera todas as ap�lices de seguro registradas.

- M�todo: `GET`
- URL: `/apolice`

##### Par�metros da Solicita��o

- `skip` (int): N�mero de registros a serem ignorados na consulta (utilizado para pagina��o).

- `take` (int): N�mero m�ximo de registros a serem retornados na consulta (utilizado para pagina��o).

Exemplo de URL de solicita��o:

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
      "descricao": "Ap�lice de seguro de autom�vel",
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
      "descricao": "Ap�lice de seguro residencial",
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

#### Obter uma ap�lice por ID

Recupera uma ap�lice de seguro pelo seu ID.

- M�todo: `GET`
- URL: `/apolice/{id}`

##### Par�metros da Solicita��o

- `id` (long): ID da ap�lice de seguro.

Exemplo de URL de solicita��o:

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
    "descricao": "Ap�lice de seguro de autom�vel",
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

#### Atualizar uma ap�lice

Atualiza uma ap�lice de seguro e suas parcelas.

- M�todo: `PATCH`
- URL: `/apolice

##### Par�metros da Solicita��o

O corpo da solicita��o deve conter um objeto JSON contendo os detalhes atualizados da ap�lice de seguro. O campo `id` � obrigat�rio e n�o pode ser zero. Os demais campos s�o opcionais e representam os novos valores dos campos da ap�lice.

Exemplo de corpo de solicita��o:

```json
{
  "id": 1,
  "descricao": "Nova descri��o da ap�lice",
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
    "descricao": "Nova descri��o da ap�lice",
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

Registra o pagamento de uma parcela de uma ap�lice de seguro.

- M�todo: `POST`
- URL: `/parcela/{id}/pagamento`
- Content-Type: `application/json`

##### Par�metros da Solicita��o

- `id` (long): ID da parcela a ser paga.
- `paidDate` (string, formato ISO 8601): Data de pagamento da parcela.

Exemplo de URL de solicita��o:

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

# Padr�es de Projetos Utilizados

A aplica��o foi desenvolvida utilizando os seguintes padr�es de projetos:

## Factory

O padr�o Factory foi utilizado para a cria��o de objetos relacionados � ap�lice de seguro e suas parcelas de pagamento. Ele permite encapsular a l�gica de cria��o desses objetos em uma classe ou m�todo separado, fornecendo uma forma mais flex�vel e desacoplada de criar inst�ncias.

## Unit of Work

O padr�o Unit of Work foi utilizado para gerenciar transa��es e opera��es de persist�ncia de dados relacionadas � ap�lice de seguro e suas parcelas. Ele permite agrupar v�rias opera��es em uma �nica transa��o e garantir a consist�ncia dos dados ao persisti-los no banco de dados.

## Repository

O padr�o Repository foi utilizado para abstrair o acesso e a manipula��o dos dados da ap�lice de seguro e suas parcelas. Ele fornece uma camada de abstra��o entre a l�gica de neg�cio e a camada de persist�ncia de dados, permitindo que a aplica��o se torne independente do mecanismo de armazenamento utilizado.

## DTO (Data Transfer Object)

O padr�o DTO foi utilizado para transferir dados entre os diferentes componentes da aplica��o. Ele permite definir objetos simples que encapsulam os dados necess�rios para uma determinada opera��o ou comunica��o, evitando o acoplamento excessivo entre os componentes.

# Arquitetura Onion

A aplica��o foi constru�da seguindo a arquitetura Onion (ou Clean Architecture). Essa arquitetura prop�e uma separa��o clara e modular das responsabilidades da aplica��o, visando a manutenibilidade, testabilidade e independ�ncia de frameworks externos.

Na arquitetura Onion, a aplica��o � dividida em camadas, cada uma com uma responsabilidade espec�fica:

- Camada de Dom�nio (Core): Cont�m as entidades de dom�nio, regras de neg�cio e interfaces dos servi�os utilizados pela aplica��o.
- Camada de Aplica��o: Cont�m os casos de uso da aplica��o, que implementam as regras de neg�cio e orquestram a intera��o entre as camadas.
- Camada de Infraestrutura: Cont�m a implementa��o dos servi�os externos, como acesso a banco de dados, APIs externas, entre outros.
- Camada de Interface: Cont�m as interfaces de intera��o com o usu�rio, como APIs REST, interfaces gr�ficas, entre outros.

Essa arquitetura permite que a aplica��o seja independente de frameworks externos, facilita a realiza��o de testes automatizados e proporciona uma maior flexibilidade e manutenibilidade ao longo do tempo.

# Considera��es Finais

Esta API permite a cria��o, recupera��o, atualiza��o e pagamento de parcelas de ap�lices de seguro. Certifique-se de fornecer os dados corretos ao fazer solicita��es aos endpoints correspondentes. Em caso de d�vidas ou problemas, entre em contato com o suporte t�cnico.