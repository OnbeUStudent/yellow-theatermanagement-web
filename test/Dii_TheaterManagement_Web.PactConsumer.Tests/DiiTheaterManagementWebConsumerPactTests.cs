using Dii_TheaterManagement_Web.Clients;
using Microsoft.AspNetCore.Http;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Dii_TheaterManagement_Web.PactConsumer.Tests
{
    public class DiiTheaterManagementWebConsumerPactTests : IClassFixture<DiiTheaterManagementWebConsumerPactFixture>
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private IMockProviderService _mockProviderService;
        private string _mockProviderServiceBaseUri;

        public DiiTheaterManagementWebConsumerPactTests(DiiTheaterManagementWebConsumerPactFixture fixture, IHttpContextAccessor httpContextAccessor)
        {
            _mockProviderService = fixture.MockProviderService;
            _mockProviderService.ClearInteractions(); //NOTE: Clears any previously registered interactions before the test is run
            _mockProviderServiceBaseUri = fixture.MockProviderServiceBaseUri;
            this.httpContextAccessor = httpContextAccessor;
        }

        [Fact(Skip = "Shobhit To do later")]
        public async Task ClientForBcGreenNotificationSvc_EmailSend_GivenEmail()
        {
            // Arrange
            _mockProviderService
              .Given("There are NO movies")
              .UponReceiving("A GET request to retrieve the movies")
              .With(new ProviderServiceRequest
              {
                  Method = HttpVerb.Post,
                  Path = "/api/Movies",
                  Headers = new Dictionary<string, object>
                    {
                        { "Accept", "application/json" },
                       { "Content-Type", "application/json; charset=utf-8" }
                    }

              })
              .WillRespondWith(new ProviderServiceResponse
              {
                  Status = 200,
                  Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    }


              });
            var httpClient = new HttpClient { BaseAddress = new Uri(_mockProviderServiceBaseUri) };
            httpClient.DefaultRequestHeaders
              .Accept
              .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var theaterManagementBffClient = new TheaterManagementBffClient(httpClient, httpContextAccessor);

            // Act
            var result = await theaterManagementBffClient.ApiMoviesGetAsync();

            // Assert
            result.Equals(true);

            _mockProviderService.VerifyInteractions(); //NOTE: Verifies that interactions registered on the mock provider are called at least once
        }

    }
}
