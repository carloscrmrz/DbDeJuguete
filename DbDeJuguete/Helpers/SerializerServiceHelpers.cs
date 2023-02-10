using DbDeJuguete.Models;

namespace DbDeJuguete.Helpers;

public static class SerializerServiceHelpers
{
    /// <summary>
    /// Gets all the class' properties' contents and saves them in a <see cref="IEnumerable{T}"/> as <see cref="string"/>
    /// </summary>
    /// <param name="obj">The object which we will extract its properties' content.</param>
    /// <returns>A List with all the class' properties' contents.</returns>
    public static IEnumerable<string> GetPropertiesValues(this object obj)
    {
        var props = obj.GetType().GetProperties();

        return props
            .Select(p => p.GetValue(obj, null)?.ToString() ?? string.Empty)
            .ToList();
    }

    /// <summary>
    /// Gets all the class' properties and saves them in a <see cref="IEnumerable{T}"/> as <see cref="string"/>
    /// </summary>
    /// <typeparam name="T">The object which we will extract its properties.</typeparam>
    /// <returns>A List with all the class' properties.</returns>
    public static IEnumerable<string> GetObjectFields<T>() where T : ISerializable, new()
    {
        var instance = new T();
        var type = instance.GetType();

        return type.GetProperties().Select(propInfo => propInfo.Name).ToList();
    }
}