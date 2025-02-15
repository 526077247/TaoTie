using UnityEditor;
using UnityEngine;

namespace TaoTie
{
    public class BuildSettings : ScriptableObject
    {
        public bool clearFolder = false;
        public bool isBuildExe = false;
        public bool buildHotfixAssembliesAOT = true;
        public bool isContainAB = false;
        public bool isBuildAll;
        public bool isPackAtlas;
        public BuildType buildType = BuildType.Release;
        public BuildAssetBundleOptions buildAssetBundleOptions = BuildAssetBundleOptions.None;
    }
}
