namespace AsteriskDotHMG.Common.Helpers;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class SwaggerRequiredAttribute : Attribute
{
}

public class SwaggerRequiredSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        schema.Nullable = false;

        PropertyInfo[] properties = context.Type.GetProperties();

        foreach (PropertyInfo property in properties)
        {
            Attribute attribute = property.GetCustomAttribute(typeof(SwaggerRequiredAttribute));

            if (attribute != null)
            {
                if (schema.Required == null)
                {
                    HashSet<string> set = new()
                {
                    property.Name.ToCamelCase()
                };

                    schema.Required = set;
                }
                else
                {
                    schema.Required.Add(property.Name.ToCamelCase());
                }
            }
        }
    }
}

public class SwaggerRequiredOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {

        ParameterInfo fromFormParameter = context.MethodInfo.GetParameters().FirstOrDefault(p => p.IsDefined(typeof(FromFormAttribute), true));

        if (fromFormParameter != null)
        {
            foreach (string key in operation.RequestBody.Content.Keys)
            {
                operation.RequestBody.Content[key].Schema = context.SchemaGenerator.GenerateSchema(fromFormParameter.ParameterType, context.SchemaRepository);
            }
        }
    }
}
