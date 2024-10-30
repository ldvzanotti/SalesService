Funcionalidade: Criar um pedido

Criar um novo pedido com os itens selecionados e vendedor indicado.

Regra: Após o registro da venda ela deverá ficar com status "Aguardando pagamento"

Cenário: Pedido deve ser criado com status "Aguardando pagamento"
	Dado que fui atendido pelo representante de vendas de id '90B443C6-2711-4380-B101-F2DF684F473E'
	E que selecionei os produtos:
	| ProductId                            | Units |
	| AD96BD12-6A54-4CA4-B606-52BE1BEE160F | 20    |
	| A0557406-471A-4889-9D35-784A36DEC927 | 1     |
	Quando registro uma nova venda
	Então o pedido é criado com o status 'Aguardando pagamento'

Regra: Um pedido deve conter pelo menos 1 item

Cenário: Venda registrada sem itens
	Dado que fui atendido pelo representante de vendas de id '90B443C6-2711-4380-B101-F2DF684F473E'
	E que selecionei os produtos:
	| ProductId | Units |
	|           |       |
	Quando registro uma nova venda
	Então o pedido não é criado