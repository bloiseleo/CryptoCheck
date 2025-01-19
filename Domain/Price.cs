namespace CryptoCheck.Domain
{
    public class Price
    {
        public Decimal Value { private set; get; }
        public Currency Currency { private set; get; }
        public Price(decimal value, Currency currency) 
        { 
            Value = value;
            Currency = currency;
        }
        public override string ToString()
        {
            return $"Price[value={Value},currency={Enum.GetName(typeof(Currency), Currency)}]";
        }
        public static Price Build(decimal price, string currency)
        {
            Currency c = (Currency) Enum.Parse(typeof(CryptoCheck.Domain.Currency), currency);
            return new Price(price, c);
        }
    }
}
