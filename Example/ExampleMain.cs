using GraphQLRepository;

internal class ExampleMain
{
    private static async Task Main(string[] args)
    {

        var graphqlbuilder = new GraphQLQueryBuilder();
        var url = "https://rickandmortyapi.com/graphql";
        var connector = new GraphQLClientRepository<Characters>(url, graphqlbuilder);

        //var response = await connector.FindAll();
        //var response = await connector.FindById("121");
        var param = new Dictionary<string, List<string>>
            {
                { "id", new List<string>() { "303" } },
                { "heroname", new List<string>() { "\"Master Yi\"" } },
                { "class", new List<string>() { "\"Sword Master\"" } }
            };
        var response = await connector.FindByParameters(param);

        response.Data.characters.Results.ForEach(x => Console.WriteLine(x.Name));

    }


    public class Characters
    {
        public CharactersResult characters;
    }

    public class CharactersResult
    {
        public CharactersInfo Info;
        public List<Character> Results;
    }

    public class CharactersInfo
    {
        public int Count;
        public int Pages;
    }

    public class Character
    {
        public string Name;
        public string Id;
    }
}