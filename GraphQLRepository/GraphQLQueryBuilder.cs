using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GraphQLRepository
{
    public class GraphQLQueryBuilder
    {
        public GraphQLQueryBuilder() { }

        public string Build(object objeto, Dictionary<string, List<string>> parameters)
        {
            StringBuilder queryBuilder = new StringBuilder();
            Type tipo = (Type)objeto;
            foreach (var campo in tipo.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                queryBuilder.Append($"{campo.Name.ToLower()} ");
                
                if (parameters.Count > 0)
                {
                    queryBuilder.Append("(");
                    foreach (KeyValuePair<string, List<string>> pair in parameters)
                    {
                        queryBuilder.Append($" {pair.Key}: ");
                        foreach (var value in pair.Value)
                        {
                            queryBuilder.Append($" {value}");
                        }
                        queryBuilder.Append(",");
                    }
                    queryBuilder.Remove(queryBuilder.Length - 1, 1);
                    queryBuilder.Append(")\n");
                    parameters.Clear();
                }

                //TODO Probar con Listas de datos primitivos
                if (campo.FieldType.IsGenericType && campo.FieldType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    Type tipoClaseLista = campo.FieldType.GetGenericArguments()[0];
                    queryBuilder.Append("{ ");
                    queryBuilder.Append(Build(tipoClaseLista, parameters));
                    queryBuilder.Append("} ");
                }
                else if (campo.FieldType.IsClass && campo.FieldType != typeof(string))
                {
                    Type valorCampo = campo.FieldType;
                    queryBuilder.Append("{ ");
                    queryBuilder.Append(Build(valorCampo, parameters));
                    queryBuilder.Append("} ");
                }
            }
            return queryBuilder.ToString();
        }
    }
}
