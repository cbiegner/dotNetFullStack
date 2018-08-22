using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetDev.Client.PCache.Tests
{
	using Model;
	using Client;

	[TestClass]
	public class ValuesTests
	{
		private const string baseURI = "http://localhost:53600";

		[TestMethod]
		public void GetAll()
		{
			Values v = new Values(baseURI);

			var result = v.Read();

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void GetOne()
		{
			Values v = new Values(baseURI);

			var result = v.Read(1);

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void Create()
		{
			Values v = new Values(baseURI);

			Person p = new Person()
			{
				Id = 2,
				Name = "John",
				Surname = "Doe",
				Address = new Address() { Type = AddressType.Private, Street = "Exile Street", City = "Los Angeles", ZIP = "97531" },
				Communication = new Communication() { Type = CommunicationType.Other, Value = "http:www.foo.com" }
			};

			var result = v.Create(p);

			Assert.AreEqual(2, result);
		}

		[TestMethod]
		public void Update()
		{
			Values v = new Values(baseURI);

			Person p = new Person()
			{
				Id = 2,
				Name = "John",
				Surname = "Doe",
				Address = new Address() { Type = AddressType.Private, Street = "Exile Street", City = "Los Angeles", ZIP = "97531" },
				Communication = new Communication() { Type = CommunicationType.Other, Value = "http://www.foo.com" }
			};

			v.Update(p);

			var result = v.Read(p.Id);

			Assert.AreEqual(p.Communication.Value, result.Communication.Value);
		}

		[TestMethod]
		public void Delete()
		{
			Values v = new Values(baseURI);

			v.Delete(2);
			var result = v.Read(2);

			Assert.IsNull(result);
		}
	}
}
