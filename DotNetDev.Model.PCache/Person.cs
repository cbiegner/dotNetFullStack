namespace DotNetDev.Model
{
	public class Person
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }
		public Address Address { get; set; }
		public Communication Communication { get; set; }

		public Person()
		{
			Name = string.Empty;
			Surname = string.Empty;
		}
	}

	public enum AddressType
	{
		Private = 1, Business = 2, Other = 0
	}

	public class Address
	{
		public AddressType Type { get; set; }
		public string Street { get; set; }
		public string City { get; set; }
		public string ZIP { get; set; }
	}

	public enum CommunicationType
	{
		Phone = 1, Email = 2, Other = 0
	}

	public class Communication
	{
		public CommunicationType Type { get; set; }
		public string Value { get; set; }
	}
}
