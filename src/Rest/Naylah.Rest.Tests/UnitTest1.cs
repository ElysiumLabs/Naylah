using Naylah.Rest.Tests.JsonPlaceholder;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Naylah.Rest.Tests
{
    public class RestClientCallsTests
    {
        private RestClient client;

        [SetUp]
        public void Setup()
        {
            var settings = new RestClientSettings();
            settings.BaseUri = new Uri("https://jsonplaceholder.typicode.com/");
            client = new RestClient(settings);

        }

        [Test]
        public async Task GetPostsAsObject()
        {
            var posts = await client.ExecuteAsync<IEnumerable<Post>>("posts", HttpMethod.Get);
            Assert.NotZero(posts.Count());
        }

        [Test]
        public async Task GetPostsAsString()
        {
            var posts = await client.ExecuteAsync<string>("posts", HttpMethod.Get);
            Assert.IsTrue(!string.IsNullOrEmpty(posts));
        }


        [Test]
        public async Task NotFoundShouldRiseRestException()
        {
            IEnumerable<Post> posts = null;

            try
            {
                posts = await client.ExecuteAsync<IEnumerable<Post>>("DQWWQDpostsSADQWQWD", HttpMethod.Get);
            }
            catch (RestException rex)
            {
                Assert.IsNotNull(rex);
                Assert.IsTrue(rex.HttpStatusCode == HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
            }
            
            Assert.IsNull(posts);
        }
    }
}