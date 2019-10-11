namespace OrdersClientSample.Models
{
    public class ProductViewModel
    {
        public string Name { get; set; }
        public string Quantity { get; set; }
        public string PartNumber { get; set; }
        public decimal Price { get; set; }

        public string Description
        {
            get
            {
                var desc = "";
                if (!string.IsNullOrWhiteSpace(Name))
                    desc += " " + Name;

                if (!string.IsNullOrWhiteSpace(Quantity))
                    desc += " " + Quantity;

                if (!string.IsNullOrWhiteSpace(PartNumber))
                    desc += " " + PartNumber;

                if (Price > 0)
                    desc += string.Format(" {0:#.##}", Price);

                return desc;
            }
        }
    }
}