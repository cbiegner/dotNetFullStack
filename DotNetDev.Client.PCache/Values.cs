using System.Collections.Generic;

using RestSharp;

namespace DotNetDev.Client
{
	using Model;

	public class Values
	{
		private string _baseURI;

		public Values(string baseURI)
		{
			_baseURI = baseURI;
		}

		public long Create(Person item)
		{
			var client = new RestClient(_baseURI);
			var request = new RestRequest("/api/values/", Method.POST);
			request.AddJsonBody(item);

			IRestResponse<Person> response = client.Execute<Person>(request);

			return response.Data.Id;
		}

		public List<Person> Read()
		{
			var client = new RestClient(_baseURI);
			var request = new RestRequest("/api/values/", Method.GET);

			IRestResponse<List<Person>> response = client.Execute<List<Person>>(request);

			return response.Data;
		}

		public Person Read(long id)
		{
			var client = new RestClient(_baseURI);
			var request = new RestRequest("/api/values/{id}", Method.GET);
			request.AddUrlSegment("id", id);

			IRestResponse<Person> response = client.Execute<Person>(request);

			return response.Data;
		}

		public void Update(Person item)
		{
			var client = new RestClient(_baseURI);
			var request = new RestRequest("/api/values/{id}", Method.PUT);
			request.AddUrlSegment("id", item.Id);
			request.AddJsonBody(item);

			IRestResponse response = client.Execute(request);
		}

		public void Delete(long id)
		{
			var client = new RestClient(_baseURI);
			var request = new RestRequest("/api/values/{id}", Method.DELETE);
			request.AddUrlSegment("id", id);

			IRestResponse response = client.Execute(request);
		}
	}
}
