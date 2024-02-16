namespace AsteriskDotHMG.Common.Helpers;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class SwaggerUniqueAttribute : Attribute
{
    public string Description { get; set; }

    public SwaggerUniqueAttribute(string description = "")
    {
        Description = description;
    }
}

public class SwaggerUniqueSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        PropertyInfo[] properties = context.Type.GetProperties();

        foreach (PropertyInfo property in properties)
        {
            SwaggerUniqueAttribute attribute = property.GetCustomAttribute<SwaggerUniqueAttribute>();

            if (attribute != null)
            {
                string existingDescription = schema.Properties[property.Name.ToCamelCase()].Description;
                string uniqueDescription = string.IsNullOrEmpty(attribute.Description) ? "Must be unique" : attribute.Description;
                string newDescription = $"<i>({uniqueDescription})<i/>";

                if (!string.IsNullOrEmpty(existingDescription))
                {
                    newDescription = $"{existingDescription} {newDescription}";
                }

                schema.Properties[property.Name.ToCamelCase()].Description = newDescription;
            }
        }
    }
}