using Domain.AggregateModels.Client;


namespace Application.DTOs
{
    public class ClientResponseDto
    {
        public int Id { get; set; }
        public string BussinessName { get; set; }
    }


    public static class ClientExtension
    {
        public static ClientResponseDto ToClientResponseDto(this Client client)
        {
            return new ClientResponseDto
            {
                Id = client.Id,
                BussinessName = client.BusinessName,
            };
        }
    }
}
