using System.Text.Json.Serialization;

public class RetornoPedidos
{
	[JsonPropertyName("pedidos")]
	public List<Pedido> Pedidos { get; set; }
}