using GraphQL.Client.Serializer.Newtonsoft;
using GraphQL.Client.Http;
using GraphQL;
using System.Text;
using System.Reflection;

namespace PpsGraphQLConnector
{
    public class GraphQLClientRepository<T> : IGraphQLRepository<T>
    {
        public string url { get; set; }

        public GraphQLClientRepository(string url)
        {
            this.url = url;
        }

        public async Task<GraphQLResponse<T>> FindAll()
        {
            var url = new Uri(this.url);
            var graphqlHttpClient = new GraphQLHttpClient(url, new NewtonsoftJsonSerializer());

            Type classType = typeof(T);
            StringBuilder query = new StringBuilder();
            query.Append("query { ");
            query.Append(BuildGraphQLQuery(classType));
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
            //TODO Implementar findbyid
            throw new NotImplementedException();
        }

        public async Task<GraphQLResponse<T>> FindByParameters(Dictionary<string, List<string>> parameters)
        {
            var url = new Uri(this.url);
            var graphqlHttpClient = new GraphQLHttpClient(url, new NewtonsoftJsonSerializer());

            Type classType = typeof(T);
            StringBuilder query = new StringBuilder();
            query.Append("query { ");
            query.Append(BuildGraphQLQuery(classType));
            query.Append(" }");

            var query1 = @"
            query ($name: String) {
                characters(page: 2, filter: { name: $name }) {
                    info {
                      count,
                      pages
                    }
                    results {
                      name
                    }
                  }
            }
        ";
  
            var variables = new { name = "rick" };
            var request = new GraphQLRequest
            {
                Query = query.ToString(),
                Variables = variables
            };
            var response = await graphqlHttpClient.SendQueryAsync<T>(request);
            return response;
        }

        private string BuildGraphQLQuery(object objeto)
        {
            StringBuilder queryBuilder = new StringBuilder();
            Type tipo = (Type)objeto;
            foreach (var campo in tipo.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                queryBuilder.Append($"{campo.Name.ToLower()}\n ");

                //TODO Probar con Listas de datos primitivos
                if (campo.FieldType.IsGenericType && campo.FieldType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    Type tipoClaseLista = campo.FieldType.GetGenericArguments()[0];
                    queryBuilder.Append("{ ");
                    queryBuilder.Append(BuildGraphQLQuery(tipoClaseLista));
                    queryBuilder.Append("} ");
                }
                else if (campo.FieldType.IsClass && campo.FieldType != typeof(string))
                {
                    Type valorCampo = campo.FieldType;
                    queryBuilder.Append("{ ");
                    queryBuilder.Append(BuildGraphQLQuery(valorCampo));
                    queryBuilder.Append("} ");
                }
            }
            return queryBuilder.ToString();
        }

    }
}
