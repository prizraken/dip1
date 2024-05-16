using System;

internal class JSONObject
{
    private string json;

    public JSONObject(string json)
    {
        this.json = json;
    }

    internal object GetField(string v)
    {
        throw new NotImplementedException();
    }

    internal bool HasField(string v)
    {
        throw new NotImplementedException();
    }
}