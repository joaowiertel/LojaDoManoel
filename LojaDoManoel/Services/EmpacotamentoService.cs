public class EmpacotadorService
{
    private readonly List<Caixa> _caixasDisponiveis;

    public EmpacotadorService()
    {
        _caixasDisponiveis = new List<Caixa>
        {
            new Caixa { CaixaId = "Caixa 1", Dimensoes = new Dimensoes { Altura = 30, Largura = 40, Comprimento = 80 } },
            new Caixa { CaixaId = "Caixa 2", Dimensoes = new Dimensoes { Altura = 80, Largura = 50, Comprimento = 40 } },
            new Caixa { CaixaId = "Caixa 3", Dimensoes = new Dimensoes { Altura = 50, Largura = 80, Comprimento = 60 } }
        };
    }

    public List<CaixaResultado> EmpacotarPedidos(List<Pedido> pedidos)
    {
        var resultados = new List<CaixaResultado>();

        foreach (var pedido in pedidos)
        {
            var filaProdutos = pedido.Produtos.OrderByDescending(p => p.Dimensoes.Volume()).ToList();

            while (filaProdutos.Any())
            {
                bool algumProdutoAcomodado = false;

                foreach (var caixa in _caixasDisponiveis)
                {
                    double espacoRestante = caixa.Dimensoes.Volume();
                    var itensAcomodados = new List<Produto>();

                    for (int i = filaProdutos.Count - 1; i >= 0; i--)
                    {
                        var produto = filaProdutos[i];

                        if (ProdutoCabeNaCaixa(produto, caixa) && produto.Dimensoes.Volume() <= espacoRestante)
                        {
                            itensAcomodados.Add(produto);
                            espacoRestante -= produto.Dimensoes.Volume();
                            filaProdutos.RemoveAt(i);
                        }
                    }

                    if (itensAcomodados.Any())
                    {
                        resultados.Add(new CaixaResultado
                        {
                            CaixaId = caixa.CaixaId,
                            Produtos = itensAcomodados.Select(p => p.ProdutoId).ToList(),
                            Observacao = $"Pedido {pedido.PedidoId}: itens organizados dentro da {caixa.CaixaId}"
                        });
                        algumProdutoAcomodado = true;
                        break;
                    }
                }

                if (!algumProdutoAcomodado)
                {
                    var itemInviavel = filaProdutos.First();
                    resultados.Add(new CaixaResultado
                    {
                        CaixaId = null,
                        Produtos = new List<string> { itemInviavel.ProdutoId },
                        Observacao = $"Item {itemInviavel.ProdutoId} do pedido {pedido.PedidoId} excede o tamanho das caixas disponíveis."
                    });
                    filaProdutos.RemoveAt(0);
                }
            }
        }

        return resultados;
    }

    private bool ProdutoCabeNaCaixa(Produto produto, Caixa caixa)
    {
        return produto.Dimensoes.Altura <= caixa.Dimensoes.Altura &&
               produto.Dimensoes.Largura <= caixa.Dimensoes.Largura &&
               produto.Dimensoes.Comprimento <= caixa.Dimensoes.Comprimento;
    }
}