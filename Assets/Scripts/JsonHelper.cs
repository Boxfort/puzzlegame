using UnityEngine;
using LitJson;

public static class JsonHelper
{
	// TODO: Investigate if android will throw a shitfit
    public static JsonData LoadJsonResource(string path)
	{
		TextAsset jsonString = Resources.Load<TextAsset>(path);
        return JsonMapper.ToObject(jsonString.text);
	}
}

