Funcionalidade: Obter fila de pedidos em produção

Cenário: Fila de pedidos com pedidos em recebidos, em preparo e prontos
	Dados os seguintes pedidos estão cadastrados:
	| pedidoId                             | QueueKey | Status    | CreationTime        |
	| e516249a-93bc-439a-b004-0bf15e42c3ad | 3        | Recebido  | 2024-11-20 13:06:00 |
	| 4ecb63c5-45a3-4171-b3fb-80cbf065c3bb | 2        | EmPreparo | 2024-11-20 13:05:00 |
	| 8abea682-b18c-4a83-8c01-86a8b60c87cc | 6        | EmPreparo | 2024-11-20 13:06:00 |
	| b645406e-aba7-4206-8c68-6927d31eb13a | 1        | Pronto    | 2024-11-20 13:01:00 |
	| 39b66a58-8e2d-42d3-bde3-d16bd5282b6c | 5        | Pronto    | 2024-11-20 13:06:00 |
	| 21257157-99de-4675-93a0-b4210821fd8b | 4        | Recebido  | 2024-11-20 13:07:00 |
	Quando eu solicitar a fila de pedidos em producao
	Então a fila deve ser retornada na seguinte ordem:
	| pedidoId                             | QueueKey | Status    | 
	| b645406e-aba7-4206-8c68-6927d31eb13a | 1        | Pronto    | 
	| 39b66a58-8e2d-42d3-bde3-d16bd5282b6c | 5        | Pronto    | 
	| 4ecb63c5-45a3-4171-b3fb-80cbf065c3bb | 2        | EmPreparo | 
	| 8abea682-b18c-4a83-8c01-86a8b60c87cc | 6        | EmPreparo | 
	| e516249a-93bc-439a-b004-0bf15e42c3ad | 3        | Recebido  | 
	| 21257157-99de-4675-93a0-b4210821fd8b | 4        | Recebido  | 

Cenário: Fila de pedidos deve ignorar pedidos finalizados
	Dados os seguintes pedidos estão cadastrados:
	| pedidoId                             | QueueKey | Status     | CreationTime        |
	| d7b4a407-77c9-4d03-aa77-0af01302e7ee | 2        | EmPreparo  | 2024-11-20 13:05:00 |
	| c5b32602-1260-4a0a-8c74-939d1a57a71b | 6        | EmPreparo  | 2024-11-20 13:06:00 |
	| a667b176-d94e-4319-b863-c13efab59744 | 1        | Finalizado | 2024-11-20 12:00:00 |
	Quando eu solicitar a fila de pedidos em producao
	Então a fila deve ser retornada na seguinte ordem:
	| pedidoId                             | QueueKey | Status    | 
	| d7b4a407-77c9-4d03-aa77-0af01302e7ee | 2        | EmPreparo | 
	| c5b32602-1260-4a0a-8c74-939d1a57a71b | 6        | EmPreparo | 

Cenário: Fila de pedidos deve ignorar pedidos cancelados
	Dados os seguintes pedidos estão cadastrados:
	| pedidoId                             | QueueKey | Status     | CreationTime        |
	| 4c50c1cb-df56-41d8-bb68-63f104d10a80 | 2        | EmPreparo  | 2024-11-20 13:05:00 |
	| a977e674-6806-496e-8305-39fde8418a48 | 6        | EmPreparo  | 2024-11-20 13:06:00 |
	| 4cfd47bb-91bb-40f9-939d-25d222d3952c | 2        | Cancelado  | 2024-11-20 12:00:00 |
	Quando eu solicitar a fila de pedidos em producao
	Então a fila deve ser retornada na seguinte ordem:
	| pedidoId                             | QueueKey | Status    | 
	| 4c50c1cb-df56-41d8-bb68-63f104d10a80 | 2        | EmPreparo | 
	| a977e674-6806-496e-8305-39fde8418a48 | 6        | EmPreparo | 

Cenário: Fila de pedidos vazia
	Dados os seguintes pedidos estão cadastrados:
	| pedidoId                             | QueueKey | Status     | CreationTime        |
	Quando eu solicitar a fila de pedidos em producao
	Então a fila deve ser retornada na seguinte ordem:
	| pedidoId                             | QueueKey | Status    | 
