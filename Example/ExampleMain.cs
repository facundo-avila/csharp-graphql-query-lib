using PpsGraphQLConnector;

internal class ExampleMain
{
    private static async Task Main(string[] args)
    {
        var connector = new GraphQLClientRepository<Characters>("https://rickandmortyapi.com/graphql");

        var response = await connector.FindAll();

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