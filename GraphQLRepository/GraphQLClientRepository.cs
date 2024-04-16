using GraphQL.Client.Serializer.Newtonsoft;
using GraphQL.Client.Http;
using GraphQL;
using System.Text;

namespace GraphQLRepository
{
    public class GraphQLClientRepository<T> : IGraphQLRepository<T>
    {
        public string url { get; set; }

        public GraphQLQueryBuilder graphQLQueryBuilder { get; set; }    

        public GraphQLClientRepository(string url, GraphQLQueryBuilder graphQLQueryBuilder)
        {
            this.url = url;
            this.graphQLQueryBuilder = graphQLQueryBuilder;
        }

        public async Task<GraphQLResponse<T>> FindAll()
        {
            var url = new Uri(this.url);
            var graphqlHttpClient = new GraphQLHttpClient(url, new NewtonsoftJsonSerializer());

            Type classType = typeof(T);
            StringBuilder query = new StringBuilder();
            var param = new Dictionary<string, List<string>>();
            query.Append("query { ");
            query.Append(graphQLQueryBuilder.Build(classType, param));
            query.Append(" }");

            var request = new GraphQLRequest
            {
                Query = query.ToString()
            };
            var response = await graphqlHttpClient.SendQueryAsync<T>(request);
            return response;
        }

        public async Task<GraphQLResponse<T>> FindById(string id)
        {
            var url = new Uri(this.url);
            var graphqlHttpClient = new GraphQLHttpClient(url, new NewtonsoftJsonSerializer());

            Type classType = typeof(T);
            StringBuilder query = new StringBuilder();
            var param = new Dictionary<string, List<string>>
            {
                { "id", new List<string>() { id } }
            };
            query.Append("query { ");
            query.Append(graphQLQueryBuilder.Build(classType, param));
            query.Append(" }");

            var request = new GraphQLRequest
            {
                Query = query.ToString()
            };
            Console.WriteLine(query.ToString());
            var response = await graphqlHttpClient.SendQueryAsync<T>(request);
            return response;
        }

        public async Task<GraphQLResponse<T>> FindByParameters(Dictionary<string, List<string>> parameters)
        {
            var url = new Uri(this.url);
            var graphqlHttpClient = new GraphQLHttpClient(url, new NewtonsoftJsonSerializer());

            Type classType = typeof(T);
            StringBuilder query = new StringBuilder();
            query.Append("query { ");
            query.Append(graphQLQueryBuilder.Build(classType, parameters));
            query.Append(" }");

            var request = new GraphQLRequest
            {
                Query = query.ToString()
            };
            Console.WriteLine(query.ToString());
            var response = await graphqlHttpClient.SendQueryAsync<T>(request);
            return response;
        }

    }
}
