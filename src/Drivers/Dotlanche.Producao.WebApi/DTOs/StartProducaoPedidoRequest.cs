namespace Dotlanche.Producao.WebApi.DTOs
{
    public record StartProducaoPedidoRequest
    {
        public required Guid PedidoId { get; set; }

        public required IEnumerable<ComboDTO> Combos { get; set; }

        public bool Validate(out List<string> problemDetails)
        {
            bool isValid = true;
            problemDetails = [];

            if (!Combos.Any())
            {
                isValid = false;
                problemDetails.Add($"{nameof(Combos)}: Pedido must have a combo to start producao!");
            }

            return isValid;
        }
    }
}