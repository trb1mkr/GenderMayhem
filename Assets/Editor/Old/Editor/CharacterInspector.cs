//using System.Linq;
//using UnityEditor;
//using UnityEngine;

//public class CharacterInspector : Editor
//{
//    [SerializeField]
//    string animationNameField;

//    private void Reset()
//    {
//        Character characterScript = (Character)target;
//        animationNameField = EditorPrefs.GetString("animationNameField");
//        characterScript.SetDefaultComponents();
//    }

//    public override void OnInspectorGUI()
//    {
//        Character characterScript = (Character)target;

//        GUILayout.BeginHorizontal();
//        animationNameField = GUILayout.TextField(animationNameField, GUILayout.Width(105), GUILayout.ExpandWidth(false));
//        EditorPrefs.SetString("animationNameField", animationNameField);
//        var createButton = GUILayout.Button("Create Frame");
//        var setButton = GUILayout.Button("Set Frame");
//        var getButton = GUILayout.Button("Get Frame");
//        GUILayout.EndHorizontal();

//        if (createButton)
//        {
//            var targetFrame = new AnimationFrame();
//            targetFrame.name = animationNameField;
//            SetTargetFrame(targetFrame);
//            characterScript.animationFrames.Add(targetFrame);
//        }

//        if (setButton)
//        {
//            AnimationFrame targetFrame = characterScript.animationFrames.Where( x => x.name == animationNameField.ToString()).ToList()[0];
//            SetTargetFrame(targetFrame);
//        }

//        if (getButton)
//        {
//            AnimationFrame targetFrame = characterScript.animationFrames.Where(x => x.name == animationNameField.ToString()).ToList()[0];
//            characterScript.spriteRenderer.sprite = targetFrame.sprite;
//            characterScript.polygonColliders[0].SetPath(0, targetFrame.torsoCollider);
//            characterScript.polygonColliders[1].SetPath(0, targetFrame.attackCollider);
//            characterScript.polygonColliders[2].SetPath(0, targetFrame.avoidCollider);
//            characterScript.weaponPoint.localPosition = targetFrame.weaponPoint[0];
//            characterScript.weaponPoint.localEulerAngles = targetFrame.weaponPoint[1];
//        }

//        void SetTargetFrame(AnimationFrame targetFrame)
//        {
//            targetFrame.sprite = characterScript.spriteRenderer.sprite;
//            targetFrame.torsoCollider = characterScript.polygonColliders[0].GetPath(0);
//            targetFrame.attackCollider = characterScript.polygonColliders[1].GetPath(0);
//            targetFrame.avoidCollider = characterScript.polygonColliders[2].GetPath(0);
//            targetFrame.weaponPoint[0] = characterScript.weaponPoint.transform.localPosition;
//            targetFrame.weaponPoint[1] = characterScript.weaponPoint.transform.localRotation.eulerAngles;
//        }

//        DrawDefaultInspector();
//    }
//}

//[CustomEditor(typeof(Player))]
//public class PlayerInspector : CharacterInspector { }

//[CustomEditor(typeof(Enemy))]
//public class EnemyInspector : CharacterInspector { }
