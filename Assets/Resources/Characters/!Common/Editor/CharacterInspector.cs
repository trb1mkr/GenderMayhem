using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using System.Reflection;

public class CharacterInspector : Editor
{
    [SerializeField]
    string animationNameField;

    private void Reset()
    {
        Character characterScript = (Character)target;
        animationNameField = EditorPrefs.GetString("animationNameField");
        characterScript.SetDefaultComponents();
    }

    public override void OnInspectorGUI()
    {
        Character characterScript = (Character)target;

        GUILayout.BeginHorizontal();
        animationNameField = GUILayout.TextField(animationNameField, GUILayout.Width(105), GUILayout.ExpandWidth(false));
        EditorPrefs.SetString("animationNameField", animationNameField);
        var setButton = GUILayout.Button("Set Frame");
        var getButton = GUILayout.Button("Get Frame");
        var removeButton = GUILayout.Button("Remove Frame");
        GUILayout.EndHorizontal();

        if (setButton)
        {
            AnimationFrame targetFrame = FindTargetFrame();
            if (targetFrame != null) SetTargetFrame(targetFrame);
        }

        if (getButton)
        {
            AnimationFrame targetFrame = FindTargetFrame();
            if (targetFrame == null) return;
            characterScript.spriteRenderer.sprite = targetFrame.sprite;
            if (targetFrame.torsoCollider != null) characterScript.polygonColliders[0].SetPath(0, AnimationFrame.LoadPath(targetFrame.torsoCollider));
            if (targetFrame.attackCollider != null) characterScript.polygonColliders[1].SetPath(0, AnimationFrame.LoadPath(targetFrame.attackCollider));
            if (targetFrame.avoidCollider != null) characterScript.polygonColliders[2].SetPath(0, AnimationFrame.LoadPath(targetFrame.avoidCollider));
            if (targetFrame.weaponPoint != null) characterScript.weaponPoint.localPosition = AnimationFrame.LoadTransform(targetFrame.weaponPoint)[0];
            if (targetFrame.weaponPoint != null) characterScript.weaponPoint.localEulerAngles = AnimationFrame.LoadTransform(targetFrame.weaponPoint)[1];
        }

        if (removeButton)
        {
            AnimationFrame targetFrame = FindTargetFrame();
            if (targetFrame == null) return;
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(targetFrame.torsoCollider));
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(targetFrame.attackCollider));
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(targetFrame.avoidCollider));
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(targetFrame.weaponPoint));
            characterScript.animationFrames.Remove(targetFrame);
        }

        AnimationFrame FindTargetFrame()
        {
            try { return characterScript.animationFrames.Where(x => x.name == animationNameField.ToString()).ToList()[0]; }
            catch { Debug.Log("No such animation: " + animationNameField.ToString()); }
            return null;
        }

        void SetTargetFrame(AnimationFrame targetFrame)
        {
            targetFrame.sprite = characterScript.spriteRenderer.sprite;
            Regex regex = new Regex(@"(?<!^)(?=[A-Z])");
            string domain = "Characters/" + target.GetType().Name + "s/" + SceneManager.GetActiveScene().name + "/Animations/" + regex.Split(targetFrame.name)[0] + "/" + regex.Split(targetFrame.name)[1] + "/"; //Application.dataPath + "/Resources
            if (!AssetDatabase.IsValidFolder("Assets/Resources/" + domain))
            {
                Debug.Log("No such animation folder: " + domain);
                return;
            }
            //foreach (PolygonCollider2D collider in characterScript.polygonColliders) //ÂÑ¨ ËÎÌÀÅÒÑß
            //{
            //    if (collider == null) { Debug.Log("No collider"); return; }
            //    if (collider.pathCount > 1) Debug.Log("Where are more than 1 path"); return;
            //}
            var newTorsoCollider = AnimationFrame.WritePath(characterScript.polygonColliders[0].GetPath(0));
            var newAttackCollider = AnimationFrame.WritePath(characterScript.polygonColliders[1].GetPath(0));
            var newAvoidCollider = AnimationFrame.WritePath(characterScript.polygonColliders[2].GetPath(0));
            var newWeaponPoint = AnimationFrame.WriteTransform(characterScript.weaponPoint.transform);
            if (targetFrame.torsoCollider == null) targetFrame.torsoCollider = AnimationFrame.Save(newTorsoCollider, domain + "TorsoCollider");
            if (targetFrame.attackCollider == null) targetFrame.attackCollider = AnimationFrame.Save(newAttackCollider, domain + "AttackCollider");
            if (targetFrame.avoidCollider == null) targetFrame.avoidCollider = AnimationFrame.Save(newAvoidCollider, domain + "AvoidCollider");
            if (targetFrame.weaponPoint == null) targetFrame.weaponPoint = AnimationFrame.Save(newWeaponPoint, domain + "WeaponPoint");
            if (targetFrame.torsoCollider.text != newTorsoCollider) targetFrame.torsoCollider = AnimationFrame.Save(newTorsoCollider, domain + "TorsoCollider");
            if (targetFrame.attackCollider.text != newAttackCollider) targetFrame.attackCollider = AnimationFrame.Save(newAttackCollider, domain + "AttackCollider");
            if (targetFrame.avoidCollider.text != newAvoidCollider) targetFrame.avoidCollider = AnimationFrame.Save(newAvoidCollider, domain + "AvoidCollider");
            if (targetFrame.weaponPoint.text != newWeaponPoint) targetFrame.weaponPoint = AnimationFrame.Save(newWeaponPoint, domain + "WeaponPoint");
        }

        DrawDefaultInspector();
    }
}

[CustomEditor(typeof(Player))]
public class PlayerInspector : CharacterInspector { }

[CustomEditor(typeof(Enemy))]
public class EnemyInspector : CharacterInspector { }
