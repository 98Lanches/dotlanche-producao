Funcionalidade: Iniciar Producao Pedido

Cenário: Iniciar producao pedido válido
	Dado um pedido confirmado com id da753f8c-3871-49a1-a55f-c0404bc609bb
	E os seguintes produtos existam no servico de produto:
	| Id								   | Name			| Categoria		   | Price |
	| e8cb19fa-bc00-4f66-83fc-b60fe9b5c9f7 | Hambúrguer		| Lanches		   | 9,99  |
	| d9785d54-a0ea-4b99-a8c5-acb627f94c36 | Batata Frita	| Acompanhamento   | 5,99  |
	| 04e5850f-3cdd-40ad-9a63-2876fe270e6b | Coca-Cola		| Bebida		   | 5,99  |
	E o pedido possui os seguintes combos:
	| ComboId                              | ProdutoId                            |
	| 16d8ddb9-b0d7-4b5e-939f-5ca529a7aaff | e8cb19fa-bc00-4f66-83fc-b60fe9b5c9f7 |
	| 16d8ddb9-b0d7-4b5e-939f-5ca529a7aaff | d9785d54-a0ea-4b99-a8c5-acb627f94c36 |
	| 16d8ddb9-b0d7-4b5e-939f-5ca529a7aaff | 04e5850f-3cdd-40ad-9a63-2876fe270e6b |
	Quando eu solicitar o inicio da producao do pedido
	Então a producao do pedido deve ser iniciada com os produtos do pedido confirmado
	E deve gerar uma senha
	E o pedido deve ter o status Recebido
