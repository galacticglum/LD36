using UnityEngine;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

public class Parser
{
    private static string[] names;
    private static string[] phrases;

    public static string GetName()
    {
        if(names == null)
        {
            Parse("Names");
        }
        return names[Random.Range(0, names.Length - 1)].Trim();
    }

    private static void Parse(string fileName)
    {
        names = (Resources.Load<TextAsset>(fileName).text).Trim().Split(',') ;
    }
}
