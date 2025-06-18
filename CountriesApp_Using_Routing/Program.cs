namespace CountriesApp_Using_Routing
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            app.UseRouting();

            //data
            Dictionary<int, string> countries = new Dictionary<int, string>()
            {
                {1, "United States" },
                {2, "Canada" },
                {3, "United Kingdom" },
                {4,"India" },
                {5, "Japan" }
            };

            //endpoints
            app.UseEndpoints(endpoints => {
                endpoints.MapGet("/countries", async context =>
                {
                    //read all countries
                    foreach (KeyValuePair<int, string> country in countries) 
                    {
                        await context.Response.WriteAsync($"{country.Key}, {country.Value}\n");
                    }
                });

                //When request path is "countries/{countryID}"
                endpoints.MapGet("/countries/{countryID:int:range(1, 100)}", async context =>
                {
                    //check if "countryID" was not submitted in the request
                    if (context.Request.RouteValues.ContainsKey("countryID") == false)
                    {
                        context.Response.StatusCode = 400; //Bad Request
                        await context.Response.WriteAsync("The CountryID should be between 1 and 100");
                    }
                    // read countryID from RouteValues ( route parameters)
                    int countryID = Convert.ToInt32(context.Request.RouteValues["countryID"]);

                    //if countryID exists in the countries dictionary
                    if (countries.ContainsKey(countryID))
                    {
                        string countryName = countries[countryID];
                        await context.Response.WriteAsync($"{countryName}");
                    }
                    else
                    {
                        context.Response.StatusCode = 404;
                        await context.Response.WriteAsync($"[No country]");
                    }
                });

                //When request path is "countries/{countryID}"
                endpoints.MapGet("/countries/{countruID:min(101)}", async context =>
                {
                    context.Response.StatusCode = 400; //Bad Request
                    await context.Response.WriteAsync("The CountryId should be between 1 and 100 - min ");
                });
            });

            //Default Middleware

            app.Run(async context =>
            {
                await context.Response.WriteAsync("No Response");
            });

           


            app.Run();
        }
    }
}
