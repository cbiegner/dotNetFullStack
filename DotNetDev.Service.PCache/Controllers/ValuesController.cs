using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Web.Http;
using System.Web.Http.Description;

using Swashbuckle.Swagger.Annotations;

namespace DotNetDev.Service.PCache.Controllers
{
	using Model;

	public class ValuesController : ApiController
	{
		[HttpGet]
		[ResponseType(typeof(List<Person>))]
		[SwaggerOperation("GetAll")]
		[SwaggerResponse(HttpStatusCode.OK, "OK", typeof(List<Person>))]
		[SwaggerResponse(HttpStatusCode.NotFound, "No data", typeof(bool))]
		[SwaggerResponse(HttpStatusCode.InternalServerError, "An error occured", typeof(Exception))]
		public HttpResponseMessage Get()
		{
			try
			{
				var result = GetAllFromCache();

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

		[HttpGet]
		[ResponseType(typeof(Person))]
		[SwaggerOperation("GetById")]
		[SwaggerResponse(HttpStatusCode.OK)]
		[SwaggerResponse(HttpStatusCode.NotFound)]
		[SwaggerResponse(HttpStatusCode.InternalServerError, "An error occured", typeof(Exception))]
		public HttpResponseMessage Get(long id)
		{
			try
			{
				var result = GetFromCache(id);

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

		[HttpPost]
		[ResponseType(typeof(Guid))]
		[SwaggerOperation("Create")]
		[SwaggerResponse(HttpStatusCode.Created)]
		[SwaggerResponse(HttpStatusCode.NotFound)]
		[SwaggerResponse(HttpStatusCode.InternalServerError, "An error occured", typeof(Exception))]
		public HttpResponseMessage Post(Person data)
		{
			try
			{
				var result = SetToCache(data);

				if (result != null)
				{
					return Request.CreateResponse(HttpStatusCode.Created, result);
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

		[SwaggerOperation("Update")]
		[SwaggerResponse(HttpStatusCode.OK)]
		[SwaggerResponse(HttpStatusCode.NotFound)]
		[SwaggerResponse(HttpStatusCode.InternalServerError, "An error occured", typeof(Exception))]
		public HttpResponseMessage Put(long id, Person data)
		{
			try
			{
				if(UpdateCache(data))
				{
					return Request.CreateResponse(HttpStatusCode.OK, data);
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

		[SwaggerOperation("Delete")]
		[SwaggerResponse(HttpStatusCode.OK)]
		[SwaggerResponse(HttpStatusCode.NotFound)]
		[SwaggerResponse(HttpStatusCode.InternalServerError, "An error occured", typeof(Exception))]
		public HttpResponseMessage Delete(long id)
		{
			try
			{
				if(DeleteFromCache(id))
				{
					return Request.CreateResponse(HttpStatusCode.OK, id);
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

		#region Private Methods
		//private IEnumerable<Person> GetAllPersonFromCache()
		//{
		//	ObjectCache cache = MemoryCache.Default;
		//	var result = cache.OfType<Person>();

		//	return result;
		//}
		private static IEnumerable<Person> GetAllFromCache()
		{
			var result = new List<Person>();

			ObjectCache cache = MemoryCache.Default;
			foreach(var item in cache)
			{
				result.Add((Person)item.Value);
			}

			return result;
		}

		/// <summary>
		/// Get stored object from cache
		/// </summary>
		/// <param name="id">Unique ID</param>
		/// <returns>Stored object (of any type)</returns>
		private Person GetFromCache(long id)
		{
			var result = GetAllFromCache().First<Person>(i => i.Id == id);

			return result;
		}

		/// <summary>
		/// Save object to cache
		/// </summary>
		/// <param name="data">object to store</param>
		/// <returns>Unique ID</returns>
		private Person SetToCache(Person data)
		{
			ObjectCache cache = MemoryCache.Default;
			cache.Add(data.Id.ToString(), data, DateTime.Now.AddDays(1));

			return data;
		}

		/// <summary>
		/// Update object in cache
		/// </summary>
		/// <param name="data">object with updated data</param>
		/// <returns>Update successful</returns>
		private bool UpdateCache(Person data)
		{
			var result = GetFromCache(data.Id);

			if (result == null)
				return false;

			ObjectCache cache = MemoryCache.Default;
			cache.Set(data.Id.ToString(), data, DateTime.Now.AddDays(1));

			return true;
		}

		/// <summary>
		/// Remove item from cache
		/// </summary>
		/// <param name="id">Id of the item to be removed</param>
		/// <returns>Removing successful</returns>
		private bool DeleteFromCache(long id)
		{
			var result = GetFromCache(id);

			if (result == null)
				return false;

			ObjectCache cache = MemoryCache.Default;
			cache.Remove(id.ToString());

			return true;
		}
		#endregion
	}
}
