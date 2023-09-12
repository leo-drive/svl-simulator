/**
 * Copyright (c) 2019-2021 LG Electronics, Inc.
 *
 * This software contains code licensed as described in LICENSE.
 *
 */

using UnityEngine;
using UnityEditor;

namespace Simulator.Map
{
    public class MapLane : MapDataPoints, IMapType, ISpawnable
    {
        [SerializeField]
        private string _id;
        public string id
        {
            get {return _id; }
            set
            {
#if UNITY_EDITOR
                Undo.RecordObject(this, "Changed MapLane ID");
#endif
                _id = value;
            }       
        }

#region UniqueIdGeneration

        // Prefix used during auto id generation.
        [System.NonSerialized]
        public static string idPrefix = "lane_";
        
#if UNITY_EDITOR

        // Autoassign id for a newely created item.
        private void Reset()
        {
            var mapHolder = UnityEngine.Object.FindObjectOfType<MapHolder>();
            id = IdGenerator.AutogenerateNextId<MapLane>(idPrefix);
        }

        [InitializeOnLoadMethod]
        static void Initialize()
        {
            UnityEditor.SceneManagement.EditorSceneManager.sceneOpened += OnEditorSceneManagerSceneOpened;
        }

        // When scene is opened, go over all objects of same type and assign ids if missing.
        static void OnEditorSceneManagerSceneOpened(UnityEngine.SceneManagement.Scene scene, UnityEditor.SceneManagement.OpenSceneMode mode)
        {
            var mapHolder = UnityEngine.Object.FindObjectOfType<MapHolder>();
            if (mapHolder == null)
                return;

            // If there is no ID set, set autogenerated ones.
            foreach (var sign in mapHolder.transform.GetComponentsInChildren<MapLane>())
            {
                if(string.IsNullOrEmpty(sign.id))
                    sign.id = IdGenerator.AutogenerateNextId<MapLane>(idPrefix);
            }
        }
#endif
#endregion

        public bool Spawnable { get; set; } = false;
        public bool DenySpawn = false; // to deny spawns in odd lanes on ramps etc.

    }

    public interface ISpawnable
    {
        bool Spawnable { get; set; }
    }
}
