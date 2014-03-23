namespace xBehave.example
{
    using Xbehave;
    using FluentAssertions;
    using Newtonsoft;
    using System.Net.Http;
    using xBehave.example.Infrastructure;
    using Xunit;
    using System.Net;

    [Trait("Type", "Integration")]
    public class GetRequests : BaseHttpFeature
    {
        [Background]
        public void Background()
        {
            "Given I am logged in"
                .Given(() =>
                {
                    bool result = Login("x@x.com", "x");
                    result.Should().BeTrue();
                });
        }

        [Scenario]
        [Example("/")]
        [Example("/dashboard")]
        [Example("/life-stories")]
        [Example("/lifestory/1")]
        public void SmokeTest(string route, HttpResponseMessage response)
        {
            "When I request the url"
                .When(() =>
                {
                    response = Get(route);
                });

            "Then I expect a 200 response"
                .Then(() =>
                {
                    response.StatusCode.Should().Be(HttpStatusCode.OK);
                });
        }
    }
}
