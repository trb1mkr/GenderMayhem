using UnityEngine;

public static class GetObject
{
    public static GameObject GetNearest(GameObject myObject, string tag)
    {
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag(tag);
        return PickNearest(myObject, allObjects);
    }

    // public static GameObject GetNearest(GameObject myObject, System.Type type)
    // {
    //     Object[] allObjects = Object.FindObjectsByType<>(FindObjectsSortMode.None);
    //     GameObject[] allGameObjects = new GameObject[allObjects.Length];
    //     for (int i = 0; i < allObjects.Length; i++)
    //     {
    //         allGameObjects[i] = allObjects[i].GameObject();
    //     }
    //     return PickNearest(myObject, allGameObjects);
    // }

    private static GameObject PickNearest(GameObject myObject, GameObject[] allObjects)
    {
        GameObject nearestObject = null;
        float nearestDistance = -1;
        foreach (var currentObject in allObjects)
        {
            if (CheckRelation(myObject, currentObject) == true || currentObject.activeInHierarchy == false) continue;
            var currentDistanse = Vector3.Distance(myObject.transform.position, currentObject.transform.position);
            if (nearestDistance > currentDistanse || nearestDistance == -1)
            {
                nearestDistance = currentDistanse;
                nearestObject = currentObject;
            }
        }
        return nearestObject;
    }

    public static bool CheckRelation(GameObject myObject, GameObject otherObject)
    {
        if (myObject == otherObject) return true;
        if (otherObject.transform.IsChildOf(myObject.transform)) return true;
        return false;
    }
}
