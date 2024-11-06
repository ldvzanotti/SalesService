## INSTRUÇÕES PARA O TESTE TÉCNICO

O cliente necessita criar uma API para manter vendas, com as seguintes regras:

1) Deve ser possível o registro de uma venda, que consiste nos dados do vendedor + itens vendidos; 
2) Uma venda contém informação sobre o vendedor que a efetivou, data, identificador do pedido e os itens que foram vendidos;
3) O vendedor deve possuir id, cpf, nome, e-mail e telefone;
4) A inclusão de uma venda deve possuir pelo menos 1 item;
5) Após o registro da venda ela deverá ficar com status "Aguardando Pagamento";
6) Deve ser possível obter uma venda através do seu ID;
7) Deve ser possível incluir novos itens ou remover itens, enquanto a venda ainda estiver com status "Aguardando Pagamento”, observando o item 4;
8) Deve ser possível atualizar o status de uma venda informando seu ID e algum dos status: 
`Pagamento aprovado` | `Enviado para transportadora` | `Entregue` | `Cancelada`;
 
9) Deve ser respeitada a seguinte regra de atualização de status:
 
De: `Aguardando pagamento` Para: `Pagamento Aprovado`
De: `Aguardando pagamento` Para: `Cancelada`
De: `Pagamento Aprovado` Para: `Enviado para Transportadora`
De: `Pagamento Aprovado` Para: `Cancelada`
De: `Enviado para Transportador` Para: `Entregue`

## ORIENTAÇÕES TÉCNICAS
 
## Obrigatório:
 
- Utilização do padrão REST;
- Linguagens de preferência: .Net Core e NodeJs (com Typescript);
- Testes de unidade;
- Persistir dados mesmo que "em memória";
 
## Desejável:
- Mecanismos de autenticação/autorização;
- Utilizar Micro ORM ou ORM (Ex. Dapper, Entity) para manipular os dados;
- Testes de integração;
- Middleware para tratamento de erros da aplicação;
- Docker para subir as dependências (Ex. banco de dados, Mensageria);
- Aplicar arquitetura limpa ou outro design de aplicação que separe as camadas de UI, orquestração e entidade de domínio;

## PONTOS QUE SERÃO AVALIADOS
- Uso correto do padrão REST;
- Arquitetura da aplicação, iremos avaliar como o projeto foi estruturada, bem como camadas e suas responsabilidades;
- Programação orientada a objetos;
- Boas práticas e princípios como Clean Code, SOLID, DRY, KISS;
- Se for utilizado DDD, clean architecture ou outro padrão arquitetural será avaliada a aplicação correta dos conceitos; 
- Qualidade dos Testes;
- Profissionalismo do código.

