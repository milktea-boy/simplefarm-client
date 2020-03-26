using JsonFx.Json;

/// <summary>
/// use for external call
/// </summary>
public class ReadFileTool
{
	static public T JsonToClass<T>(string json) where T : class
	{
		T t = JsonReader.Deserialize<T>(json);
		return t;
	}

    static public string JsonByObject(object value)
    {
        string t = JsonWriter.Serialize(value);

        return t;
    }
}
