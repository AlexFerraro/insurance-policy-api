create table policy (
	policy_id BIGINT GENERATED ALWAYS AS IDENTITY NOT NULL,
	description VARCHAR(50) NULL,
	cpf VARCHAR(12)  NOT NULL,
	status VARCHAR(10) NOT NULL,
	total_premium DECIMAL(10, 2) NOT NULL,
	creation_date DATE NOT NULL,
	modification_date DATE NULL,
	creation_user INT NOT NULL,
	modification_user INT NULL,
	
	CONSTRAINT pk_policy PRIMARY KEY (policy_id)
);

CREATE TABLE installment (
	installment_id BIGINT GENERATED ALWAYS AS IDENTITY NOT NULL,
	policy_fk BIGINT NOT NULL,
	premium DECIMAL(10, 2) NOT NULL,
	payment_method VARCHAR(10) NOT NULL,
	payment_date DATE NOT NULL,
	paid_date DATE NULL,
	interest DECIMAL(10, 2) NULL,
	status VARCHAR(10) NULL,
	creation_date DATE NOT NULL,
	modification_date DATE NULL,
	creation_user INT NOT NULL,
	modification_user INT NULL,
	
	CONSTRAINT pk_installment PRIMARY KEY (installment_id)
);

ALTER TABLE installment ADD CONSTRAINT fk_policy_installment FOREIGN KEY (policy_fk) REFERENCES policy (policy_id);