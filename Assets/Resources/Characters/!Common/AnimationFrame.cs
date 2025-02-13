using UnityEngine;
using System.IO;
#if UNITY_EDITOR
    using UnityEditor;
#endif

[System.Serializable]
public class AnimationFrame
{
    public string name;
    public Sprite sprite;
    public TextAsset weaponPoint;
    public TextAsset torsoCollider;
    public TextAsset attackCollider;
    public TextAsset avoidCollider;

#if UNITY_EDITOR
    public static string WritePath(Vector2[] points)
    {
        string pointsText = "";
        for (int i = 0; i < points.Length; i++)
        {
            pointsText = pointsText + points[i][0] + " " + points[i][1] + "\n";
        }
        return pointsText;
    }

    public static string WriteTransform(Transform transform)
    {
        string transformText = "";
        transformText = transform.localPosition[0].ToString() + " " + transform.localPosition[1].ToString() + " " + transform.localPosition[2].ToString() + "\n";
        transformText = transformText + transform.localRotation.eulerAngles[0].ToString() + " " + transform.localRotation.eulerAngles[1].ToString() + " " + transform.localRotation.eulerAngles[2].ToString() + "\n";
        return transformText;
    }

    public static TextAsset Save(string stringToWrite, string path)
    {
        var fullPath = Application.dataPath + "/Resources/" + path + ".txt";
        if (File.Exists(fullPath)) File.Delete(fullPath);
        //Debug.Log(fullPath);
        //Debug.Log(stringToWrite);
        File.WriteAllText(fullPath, stringToWrite);
        AssetDatabase.Refresh();
        TextAsset file = Resources.Load<TextAsset>(path);
        return file;
    }
#endif

    public static Vector2[] LoadPath(TextAsset file)
    {
        string[] pointsAsLines = file.text.Remove(file.text.Length - 1, 1).Split("\n");
        Vector2[] points = new Vector2[pointsAsLines.Length];
        for (int i = 0; i < pointsAsLines.Length; i++)
        {
            points[i] = new Vector2(float.Parse(pointsAsLines[i].Split(" ")[0]), float.Parse(pointsAsLines[i].Split(" ")[1]));
        }
        return points;
    }

    public static Vector3[] LoadTransform(TextAsset file)
    {
        string[] transformAsLines = file.text.Split("\n");
        Vector3 position = new Vector3();
        position.Set(float.Parse(transformAsLines[0].Split(" ")[0]), float.Parse(transformAsLines[0].Split(" ")[1]), float.Parse(transformAsLines[0].Split(" ")[2]));
        Vector3 rotation = new Vector3();
        rotation.Set(float.Parse(transformAsLines[0].Split(" ")[0]), float.Parse(transformAsLines[0].Split(" ")[1]), float.Parse(transformAsLines[0].Split(" ")[2]));
        return new Vector3[] { position, rotation };
    }
}
