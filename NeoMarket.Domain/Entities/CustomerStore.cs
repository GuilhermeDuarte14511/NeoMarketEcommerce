namespace NeoMarket.Domain.Entities
{
    public class CustomerStore
    {
        public int Id { get; set; }
        public int UserId { get; set; }  // Cliente que comprou na loja
        public int StoreId { get; set; }  // Loja onde o cliente comprou
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public User User { get; set; }
        public Store Store { get; set; }
    }

}
