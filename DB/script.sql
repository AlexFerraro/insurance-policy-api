create table apolice (
	id int not null primary key,
	descricao VARCHAR(50),
	cpf int,
	situacao VARCHAR (10),
	premio_total decimal(10, 2),
	data_criacao_registro date,
	data_alteracao_registro date,
	usuario_criacao_registro int,
	usuario_alteracao_registro int
)
create table parcela (
	id int primary key,
	id_apolice int,
	premio decimal(10, 2),
	forma_pagamento VARCHAR(50),
	data_pagamento date,
	data_pago date,
	juros decimal(10, 2),
	situacao VARCHAR (10),
	data_criacao_registro date,
	data_alteracao_registro date,
	usuario_criacao_registro int,
	usuario_alteracao_registro int,
	CONSTRAINT apolice_id_apolice__fkey
 FOREIGN KEY (id_apolice)
 REFERENCES apolice (id)
)