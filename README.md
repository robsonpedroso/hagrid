**Hagrid**

## Introdução

Projeto para gerenciar e centralizar os dados dos usuários, permissões e empresas.

### Prerequisitos

* Framework 4.7.2 instalado
* SQL Server
* Criar o banco de dados local ex.: **DBHAccounts**
* Alterar a connection string para o banco - caso haja necessidade
* Buildar o projeto
* Executar o Migration **Update-database** no `PMC`


### Pontos de observação

* Existe um Seed que é executado validando a chave no web.config `RunSeed`, se ela estiver `true` ele executa, se não existir ou estiver `false` ele não executa
* A `Empresa` Hagrid é a `Main` por isso existe metodos que só ela (ou funcionários dela) pode executar
* Usuário principal: `robson.pedroso@hagrid.com.br` senha: `12345678`
* É utilizado o método `string Encrypt(this string field)` namespace `Hagrid.Core.Domain` para encriptar a senha

### Instalação

Após a execução do pre requisitos, segue um passo a passo de como rodar localmente.

Clonar o repositório

```
git clone https://github.com/robsonpedroso/hagrid.git Hagrid
```

**npm**

Para começar a desenvolver o UI você precisa instalar o [node.js](https://nodejs.org/en/) na sua maquina

Depois do node.js instalado, instale as dependências

```
cd \Web

npm install
```
Depois das dependências instale o grunt-cli globalmente:

```
npm install -g grunt-cli
```

Agora execute as tarefas para gerar os arquivos:

Para especificar uma tafera basta informa-la `["ngconstant:localhost", "concat:app", "cssmin:app"]`

```
grunt
```

Ou simplesmente:

```
grunt concat:app
```

Para não ficar executando a cada momento o grunt, basta deixar em observação:

```
grunt watch
```

## Configurações dos endpoints

Os endpoints estão configurado conforme abaixo, lembrando que se for alterar, é necessário ajustar as liberações de permissão no Banco de dados para permitir a leitura dos novos endpoints

A Api esta configurada na url: `http://localhost:55888/`

O login esta configurado na url: `http://localhost:55777/`

O Admin esta configurado na url: `http://localhost:4201/`



As configurações das chaves de transferências (transfer token)

* No Hagrid-UI-Admin esta no arquivo de `Enviroment`
* No Hagrid-UI-Login esta no arquivo de `grunta\ngconstant.js`
* Na Api esta no arquivo `Web.config`

## Publicação
------------

Para publicar a aplicação você pode seguir os seguintes passos


**Grunt**

Para publicar em dev:

```
grunt pub-dev
```

Para publicar em homologação:

```
grunt pub-hom
```

Para publicar em produção:

```
grunt pub-prod
```

Os arquivos vão ficar na pasta `\Accounts\publish\`.

![](/accounts-publish-folder.jpg)

Para executar os migrations do banco
------------

Para rodar o migraions do Entity Framework, você tem que gerar primeiro os arquivos para o ambiente desejado com o `grunt`.

####Usando linha de comando:

**MigSharp**  

Para atualizar o banco em dev, homologação ou produção:
```
cd /publish/api/bin

./Migrate.bat
```

Para efetuar o rollback do banco em dev, homologação ou produção:
```
cd /publish/api/bin

./Migrate_rollback.bat
```

## Autores

* **Robson Pedroso** - *Projeto inicial* - [RobsonPedroso](https://github.com/robsonpedroso)
