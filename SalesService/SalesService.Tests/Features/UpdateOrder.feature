Funcionalidade: Atualizar um pedido

Atualização de status ou de itens de um pedido.

Regra: Deve ser possível atualizar o status de um pedido, respeitando as seguintes regras:
De: "Aguardando pagamento" Para: "Pagamento aprovado"
De: "Aguardando pagamento" Para: "Cancelado"
De: "Pagamento aprovado" Para: "Enviado para transportadora"
De: "Pagamento aprovado" Para: "Cancelado"
De: "Enviado para transportadora" Para: "Entregue"

Cenário: Atualização de status válida
	Dado que tenho um pedido de id '<id>' e status '<status atual>'
	Quando atualizo o status do pedido de '<id>' para '<novo status>'
	Então o status do pedido de id '<id>' atualiza para '<novo status>'

	Exemplos:
	| id                                   | status atual                | novo status                 |
	| F0CBD493-6F48-46AD-8D69-2D4ECFFAD6D8 | Aguardando pagamento        | Pagamento aprovado          |
	| F0CBD493-6F48-46AD-8D69-2D4ECFFAD6D8 | Aguardando pagamento        | Cancelado                   |
	| 51922E52-C325-4F96-B548-01D8BEF5E475 | Pagamento aprovado          | Enviado para transportadora |
	| 51922E52-C325-4F96-B548-01D8BEF5E475 | Pagamento aprovado          | Cancelado                   |
	| FA0167FC-85BC-4039-B5B0-B9EAF525AB65 | Enviado para transportadora | Entregue                    |

Cenário: Atualização de status inválida
	Dado que tenho um pedido de id '<id>' e status '<status atual>'
	Quando atualizo o status do pedido de '<id>' para '<novo status>'
	Então o pedido de id '<id>' continua no status '<status atual>'

	Exemplos:
	| id                                   | status atual                | novo status                 |
	| F0CBD493-6F48-46AD-8D69-2D4ECFFAD6D8 | Aguardando pagamento        | Enviado para transportadora |
	| F0CBD493-6F48-46AD-8D69-2D4ECFFAD6D8 | Aguardando pagamento        | Entregue                    |
	| F0CBD493-6F48-46AD-8D69-2D4ECFFAD6D8 | Aguardando pagamento        | Aguardando pagamento        |
	| 51922E52-C325-4F96-B548-01D8BEF5E475 | Pagamento aprovado          | Entregue                    |
	| 51922E52-C325-4F96-B548-01D8BEF5E475 | Pagamento aprovado          | Aguardando pagamento        |
	| 51922E52-C325-4F96-B548-01D8BEF5E475 | Pagamento aprovado          | Pagamento aprovado          |
	| FA0167FC-85BC-4039-B5B0-B9EAF525AB65 | Enviado para transportadora | Cancelado                   |
	| FA0167FC-85BC-4039-B5B0-B9EAF525AB65 | Enviado para transportadora | Aguardando pagamento        |
	| FA0167FC-85BC-4039-B5B0-B9EAF525AB65 | Enviado para transportadora | Pagamento aprovado          |
	| FA0167FC-85BC-4039-B5B0-B9EAF525AB65 | Enviado para transportadora | Enviado para transportadora |
	| CDFD5449-23EE-472A-A0D7-0034BC3DA0D2 | Cancelado                   | Pagamento aprovado          |
	| CDFD5449-23EE-472A-A0D7-0034BC3DA0D2 | Cancelado                   | Aguardando pagamento        |
	| CDFD5449-23EE-472A-A0D7-0034BC3DA0D2 | Cancelado                   | Entregue                    |
	| CDFD5449-23EE-472A-A0D7-0034BC3DA0D2 | Cancelado                   | Enviado para transportadora |
	| CDFD5449-23EE-472A-A0D7-0034BC3DA0D2 | Cancelado                   | Cancelado                   |


Regra: Deve ser possível incluir novos itens ou remover itens, enquanto a venda ainda estiver com status "Aguardando pagamento", desde que o pedido continue contendo pelo menos 1 item.

Cenário: Pedido não está no status "Aguardando pagamento"
	Dado que tenho um pedido de id '<id>' e status '<status atual>'
	Quando atualizo os itens do pedido de id '<id>' para:
	| ProductId                            | Units |
	| AD96BD12-6A54-4CA4-B606-52BE1BEE160F | 20    |
	Então o pedido de id '<id>' não é atualizado

	Exemplos: 
	| id                                   | status atual                |
	| 51922E52-C325-4F96-B548-01D8BEF5E475 | Pagamento aprovado          |
	| FA0167FC-85BC-4039-B5B0-B9EAF525AB65 | Enviado para transportadora |
	| CDFD5449-23EE-472A-A0D7-0034BC3DA0D2 | Cancelado                   |

Cenário: Pedido no status "Aguardando pagamento" atualizado sem itens
	Dado que tenho um pedido de id 'F0CBD493-6F48-46AD-8D69-2D4ECFFAD6D8' e status 'Aguardando pagamento'
	Quando atualizo os itens do pedido de id 'F0CBD493-6F48-46AD-8D69-2D4ECFFAD6D8' para:
		| ProductId | Units |
		|           |       |
	Então o pedido de id 'F0CBD493-6F48-46AD-8D69-2D4ECFFAD6D8' não é atualizado

Cenário: Pedido no status "Aguardando pagamento" atualizado com itens
	Dado que tenho um pedido de id 'F0CBD493-6F48-46AD-8D69-2D4ECFFAD6D8' e status 'Aguardando pagamento'
	Quando atualizo os itens do pedido de id 'F0CBD493-6F48-46AD-8D69-2D4ECFFAD6D8' para:
	| ProductId                            | Units |
	| AD96BD12-6A54-4CA4-B606-52BE1BEE160F | 20    |
	Então o pedido de id 'F0CBD493-6F48-46AD-8D69-2D4ECFFAD6D8' é atualizado para:
	| ProductId                            | ProductName       | Units |
	| AD96BD12-6A54-4CA4-B606-52BE1BEE160F | Resma de Papel A4 | 20    |