using System;

namespace Domain.AggregateModels.Client
{
    public class Client
    {
        public int Id { get; set; }                  
        public string BusinessName { get; set; } = default!;
        public int ClientTypeId { get; set; }       
        public DateTime CreatedDate { get; set; } 
        public string RFC { get; set; } = default!;


        public Client(int id, string businessName, int clientTypeId, DateTime createdDate, string rfc)
        {
            Id = id;
            BusinessName = businessName.Trim();
            ClientTypeId = clientTypeId;
            CreatedDate = createdDate.Date;
            RFC = rfc.Trim();
        }
    }
}
