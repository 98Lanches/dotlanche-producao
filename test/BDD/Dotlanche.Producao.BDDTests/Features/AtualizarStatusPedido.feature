Funcionalidade: Atualizar status do pedido

Cenário: Pedido existe e é atualizado com sucesso
	Dados os seguintes pedidos estão cadastrados:
	| pedidoId                             | QueueKey | Status    | CreationTime        |
	| e516249a-93bc-439a-b004-0bf15e42c3ad | 3        | Recebido  | 2024-11-20 13:06:00 |
	| 4ecb63c5-45a3-4171-b3fb-80cbf065c3bb | 2        | EmPreparo | 2024-11-20 13:05:00 |
	Quando eu solicitar a atualização do pedido id 4ecb63c5-45a3-4171-b3fb-80cbf065c3bb para o status Pronto
	Então deve atualizar o status do pedido
	E retornar o novo status na resposta

Cenário: Pedido não existe
	Dados os seguintes pedidos estão cadastrados:
	| pedidoId                             | QueueKey | Status    | CreationTime        |
	| e516249a-93bc-439a-b004-0bf15e42c3ad | 3        | Recebido  | 2024-11-20 13:06:00 |
	| 4ecb63c5-45a3-4171-b3fb-80cbf065c3bb | 2        | EmPreparo | 2024-11-20 13:05:00 |
	Quando eu solicitar a atualização do pedido id 8abea682-b18c-4a83-8c01-86a8b60c87cc para o status Pronto
	Então deve retornar mensagem dizendo que pedido não existe
