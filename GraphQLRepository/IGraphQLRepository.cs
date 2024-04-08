using GraphQL;

namespace PpsGraphQLConnector
{
    internal interface IGraphQLRepository<T>
    {
        public string url { get; set; }

        Task<GraphQLResponse<T>> FindById(string id);

        Task<GraphQLResponse<T>> FindAll();

        Task<GraphQLResponse<T>> FindByParameters(Dictionary<string, List<string>> parameters);
    }
}
