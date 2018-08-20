using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Web.Http;
using Swashbuckle.Swagger.Annotations;

namespace DotNetDev.Service.PCache.Controllers
{
	using Model;

	public class ValuesController : ApiController
	{
		[HttpGet]
		[SwaggerOperation("GetAll")]
		[SwaggerResponse(HttpStatusCode.OK, "OK", typeof(List<Strategiedepot>))]
		[SwaggerResponse(HttpStatusCode.NotFound, "No data", typeof(bool))]
		[SwaggerResponse(HttpStatusCode.InternalServerError, "An error occured", typeof(Exception))]
		public HttpResponseMessage Get()
		{
			try
			{
				var result = GetAllObjectsOfTypeFromCache<Person>();

				if (result != null)
				{
					return Request.CreateResponse(HttpStatusCode.OK, result);
				}
				else
				{
					return Request.CreateResponse(HttpStatusCode.NotFound, true);
				}
			}
			catch (Exception ex)
			{
				return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
			}
		}

		// GET api/values/5
		[SwaggerOperation("GetById")]
		[SwaggerResponse(HttpStatusCode.OK)]
		[SwaggerResponse(HttpStatusCode.NotFound)]
		public Person Get(int id)
		{
			return "value";
		}

		// POST api/values
		[SwaggerOperation("Create")]
		[SwaggerResponse(HttpStatusCode.Created)]
		public void Post([FromBody]string value)
		{
		}

		// PUT api/values/5
		[SwaggerOperation("Update")]
		[SwaggerResponse(HttpStatusCode.OK)]
		[SwaggerResponse(HttpStatusCode.NotFound)]
		public void Put(int id, [FromBody]string value)
		{
		}

		// DELETE api/values/5
		[SwaggerOperation("Delete")]
		[SwaggerResponse(HttpStatusCode.OK)]
		[SwaggerResponse(HttpStatusCode.NotFound)]
		public void Delete(int id)
		{
		}

		#region Private Methods
		//private IEnumerable<Person> GetAllPersonFromCache()
		//{
		//	ObjectCache cache = MemoryCache.Default;
		//	var result = cache.OfType<Person>();

		//	return result;
		//}
		private IEnumerable<T> GetAllObjectsOfTypeFromCache<T>()
		{
			ObjectCache cache = MemoryCache.Default;
			var result = cache.OfType<T>();

			return result;
		}

		/// <summary>
		/// Get stored object from cache
		/// </summary>
		/// <param name="id">Unique ID</param>
		/// <returns>Stored object (of any type)</returns>
		private T GetFromCache<T>(Guid id)
		{
			ObjectCache cache = MemoryCache.Default;
			var item = (T)cache[id.ToString()];

			return item;
		}

		/// <summary>
		/// Save object to cache
		/// </summary>
		/// <param name="data">object to store</param>
		/// <returns>Unique ID</returns>
		private static Guid SetToCache(object data)
		{
			var newGuid = Guid.NewGuid();

			ObjectCache cache = MemoryCache.Default;
			cache.Add(newGuid.ToString(), data, DateTime.Now.AddDays(1));

			return newGuid;
		}
		#endregion
	}
}
