using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Player;
using UnityEditor.Compilation;

namespace TaoTie
{
    public static class BuildAssemblyEditor
    {
        private static bool IsBuildCodeAuto;
        [MenuItem("Tools/Build/EnableAutoBuildCodeDebug _F1")]
        public static void SetAutoBuildCode()
        {
            EditorPrefs.SetInt("AutoBuild", 1);
            ShowNotification("AutoBuildCode Enabled");
        }
        
        [MenuItem("Tools/Build/DisableAutoBuildCodeDebug _F2")]
        public static void CancelAutoBuildCode()
        {
            EditorPrefs.DeleteKey("AutoBuild");
            ShowNotification("AutoBuildCode Disabled");
        }
        
        [MenuItem("Tools/Build/BuildCodeDebug _F5")]
        public static void BuildCodeDebug()
        {
            string jstr = File.ReadAllText("Assets/AssetsPackage/config.bytes");
            var config = JsonHelper.FromJson<PackageConfig>(jstr);
            string assemblyName = "Code" + config.GetPackageMaxVersion(Define.DefaultName);
            BuildMuteAssembly(assemblyName, new []
            {
                "Assets/Scripts/Code",
            }, Array.Empty<string>(), CodeOptimization.Debug);

            AfterCompiling(assemblyName);
            
        }
        
        [MenuItem("Tools/Build/BuildCodeRelease _F6")]
        public static void BuildCodeRelease()
        {
            string jstr = File.ReadAllText("Assets/AssetsPackage/config.bytes");
            var config = JsonHelper.FromJson<PackageConfig>(jstr);
            string assemblyName = "Code" + config.GetPackageMaxVersion(Define.DefaultName);
            BuildMuteAssembly(assemblyName, new []
            {
                "Assets/Scripts/Code",
            }, Array.Empty<string>(),CodeOptimization.Release);

            AfterCompiling(assemblyName);

        }

        /// <summary>
        /// 获取裁剪后的系统AOT
        /// </summary>
//         public static void BuildSystemAOT()
//         {
//             PlatformType activePlatform = PlatformType.None;
// #if UNITY_ANDROID
// 			activePlatform = PlatformType.Android;
// #elif UNITY_IOS
// 			activePlatform = PlatformType.IOS;
// #elif UNITY_STANDALONE_WIN
//             activePlatform = PlatformType.Windows;
// #elif UNITY_STANDALONE_OSX
// 			activePlatform = PlatformType.MacOS;
// #elif UNITY_STANDALONE_LINUX
//             activePlatform = PlatformType.Linux;
// #elif UNITY_WEBGL
//             activePlatform = PlatformType.WebGL;
// #else
// 			activePlatform = PlatformType.None;
// #endif
//             
//             BuildTarget buildTarget = BuildHelper.buildmap[activePlatform];
//             BuildTargetGroup group = BuildHelper.buildGroupmap[activePlatform];
//
//             string programName = "TaoTie";
//             PlayerSettings.SetScriptingBackend(group,ScriptingImplementation.IL2CPP);
//             string relativeDirPrefix = "../Temp";
//             BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions()
//             {
//                 scenes = new string[] { "Assets/AssetsPackage/Scenes/InitScene/Init.unity" },
//                 locationPathName = $"{relativeDirPrefix}/{programName}",
//                 options = BuildOptions.None,
//                 target = buildTarget,
//                 targetGroup = group,
//             };
//
//             UnityEngine.Debug.Log("开始EXE打包");
//             
//             if (Directory.Exists(relativeDirPrefix))
//             {
//                 Directory.Delete(relativeDirPrefix,true);
//             }
//             Directory.CreateDirectory(relativeDirPrefix);
//             BuildPipeline.BuildPlayer(buildPlayerOptions);
//             UnityEngine.Debug.Log("完成exe打包");
//             try
//             {
//                 if (!Directory.Exists(Define.AOTDir)) Directory.CreateDirectory(Define.AOTDir);
//                 for (int i = 0; i < CodeLoader.SystemAotDllList.Length; i++)
//                 {
//                     var assemblyName = CodeLoader.SystemAotDllList[i];
//                     File.Copy(
//                         Path.Combine(HybridCLR.Editor.SettingsUtil.GetAssembliesPostIl2CppStripDir(buildTarget),
//                             $"{assemblyName}"), Path.Combine(Define.AOTDir, $"{assemblyName}.bytes"), true);
//                 }
//             }
//             catch (Exception ex)
//             {
//                 //检查是否已开启IL2CPP
//                 Debug.LogError(ex);
//             }
//         }

        /// <summary>
        /// 获取没裁剪的AOT
        /// </summary>
        // [MenuItem("Tools/Build/BuildUserAOT _F9")]
        // public static void BuildUserAOT()
        // {
        //     try
        //     {
        //         var target = EditorUserBuildSettings.activeBuildTarget;
        //         var buildDir = HybridCLR.Editor.SettingsUtil.GetHotUpdateDllsOutputDirByTarget(target);
        //         var group = BuildPipeline.GetBuildTargetGroup(target);
        //
        //         ScriptCompilationSettings scriptCompilationSettings = new ScriptCompilationSettings();
        //         scriptCompilationSettings.group = group;
        //         scriptCompilationSettings.target = target;
        //         Directory.CreateDirectory(buildDir);
        //         ScriptCompilationResult scriptCompilationResult = PlayerBuildInterface.CompilePlayerScripts(scriptCompilationSettings, buildDir);
        //         // foreach (var ass in scriptCompilationResult.assemblies)
        //         // {
        //         //     //Debug.LogFormat("compile assemblies:{1}/{0}", ass, buildDir);
        //         // }
        //         Debug.Log("compile finish!!!");
        //         if (!Directory.Exists(Define.AOTDir)) Directory.CreateDirectory(Define.AOTDir);
        //         for (int i = 0; i < CodeLoader.UserAotDllList.Length; i++)
        //         {
        //             var assemblyName = CodeLoader.UserAotDllList[i];
        //             File.Copy(
        //                 Path.Combine(buildDir,
        //                     $"{assemblyName}"), Path.Combine(Define.AOTDir, $"{assemblyName}.bytes"), true);
        //         }
        //
        //         StripAOTDll();
        //         Debug.Log("Build Code Success");
        //     }
        //     catch (Exception ex)
        //     {
        //         //檢查是否已開啟IL2CPP
        //         Debug.LogError(ex);
        //     }
        // }

        // [MenuItem("Tools/Build/BuildSystemAOT _F10")]
        // public static void BuildAOTCode()
        // {
        //     BuildSystemAOT();
        //     StripAOTDll();
        //     Debug.Log("Build Code Success");
        // }

        // [MenuItem("Tools/Build/StripAOTDll")]
        // public static void StripAOTDll()
        // {
        //     foreach (var aotDllName in CodeLoader.AllAotDllList)
        //     {
        //         string dllName = Path.GetFileName(aotDllName);
        //         string dstFile = $"{Define.AOTDir}/{dllName}.bytes";
        //         HybridCLR.Editor.AOT.AOTAssemblyMetadataStripper.Strip(dstFile, dstFile);
        //     }
        //     AssetDatabase.Refresh();
        // }
        
        private static void BuildMuteAssembly(string assemblyName, string[] CodeDirectorys, string[] additionalReferences, CodeOptimization codeOptimization,bool isAuto = false)
        {
            List<string> scripts = new List<string>();
            for (int i = 0; i < CodeDirectorys.Length; i++)
            {
                DirectoryInfo dti = new DirectoryInfo(CodeDirectorys[i]);
                FileInfo[] fileInfos = dti.GetFiles("*.cs", System.IO.SearchOption.AllDirectories);
                for (int j = 0; j < fileInfos.Length; j++)
                {
                    scripts.Add(fileInfos[j].FullName);
                }
            }
            if (!Directory.Exists(Define.BuildOutputDir))
                Directory.CreateDirectory(Define.BuildOutputDir);

            string dllPath = Path.Combine(Define.BuildOutputDir, $"{assemblyName}.dll");
            string pdbPath = Path.Combine(Define.BuildOutputDir, $"{assemblyName}.pdb");
            File.Delete(dllPath);
            File.Delete(pdbPath);

            AssemblyBuilder assemblyBuilder = new AssemblyBuilder(dllPath, scripts.ToArray());
            
            //启用UnSafe
            //assemblyBuilder.compilerOptions.AllowUnsafeCode = true;

            BuildTargetGroup buildTargetGroup = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);

            assemblyBuilder.compilerOptions.CodeOptimization = codeOptimization;
            assemblyBuilder.compilerOptions.ApiCompatibilityLevel = PlayerSettings.GetApiCompatibilityLevel(buildTargetGroup);
            // assemblyBuilder.compilerOptions.ApiCompatibilityLevel = ApiCompatibilityLevel.NET_4_6;

            assemblyBuilder.additionalReferences = additionalReferences;

            assemblyBuilder.flags = AssemblyBuilderFlags.None;
            //AssemblyBuilderFlags.None                 正常发布
            //AssemblyBuilderFlags.DevelopmentBuild     开发模式打包
            //AssemblyBuilderFlags.EditorAssembly       编辑器状态
            assemblyBuilder.referencesOptions = ReferencesOptions.UseEngineModules;

            assemblyBuilder.buildTarget = EditorUserBuildSettings.activeBuildTarget;

            assemblyBuilder.buildTargetGroup = buildTargetGroup;

            assemblyBuilder.buildStarted += delegate(string assemblyPath) { Debug.LogFormat("build start：" + assemblyPath); };

            assemblyBuilder.buildFinished += delegate(string assemblyPath, CompilerMessage[] compilerMessages)
            {
                IsBuildCodeAuto = false;
                int errorCount = compilerMessages.Count(m => m.type == CompilerMessageType.Error);
                int warningCount = compilerMessages.Count(m => m.type == CompilerMessageType.Warning&&!m.message.Contains("CS0436"));

                Debug.LogFormat("Warnings: {0} - Errors: {1}", warningCount, errorCount);

                if (warningCount > 0)
                {
                    Debug.LogFormat("有{0}个Warning!!!", warningCount);
                }

                if (errorCount > 0||warningCount > 0)
                {
                    for (int i = 0; i < compilerMessages.Length; i++)
                    {
                        if (compilerMessages[i].type == CompilerMessageType.Error ||
                            compilerMessages[i].type == CompilerMessageType.Warning)
                        {
                            if (!compilerMessages[i].message.Contains("CS0436")
                                && !compilerMessages[i].message.Contains("CS0618"))
                                Debug.LogError(compilerMessages[i].message);
                            else
                                Debug.LogWarning(compilerMessages[i].message);

                        }
                    }
                }
            };
            if (isAuto)
            {
                IsBuildCodeAuto = true;
                EditorApplication.CallbackFunction Update = null;
                Update = () =>
                {
                    if(IsBuildCodeAuto||EditorApplication.isCompiling) return;
                    EditorApplication.update -= Update;
                    AfterBuild(assemblyName);
                };
                EditorApplication.update += Update;
            }
            //开始构建
            if (!assemblyBuilder.Build())
            {
                Debug.LogErrorFormat("build fail：" + assemblyBuilder.assemblyPath);
                return;
            }
        }

        private static void AfterCompiling(string assemblyName)
        {
            while (!File.Exists(Path.Combine(Define.BuildOutputDir, $"{assemblyName}.dll"))
                   && EditorApplication.isCompiling)
            {
                Debug.Log("Compiling wait1");
                // 主线程sleep并不影响编译线程
                Thread.Sleep(1000);
                Debug.Log("Compiling wait2");
            }
            AfterBuild(assemblyName);
            //反射获取当前Game视图，提示编译完成
            Debug.Log("Build Code Success");
        }
        
        public static void AfterBuild(string assemblyName)
        {
            Debug.Log("Compiling finish");
            Directory.CreateDirectory(Define.HotfixDir);
            FileHelper.CleanDirectory(Define.HotfixDir);
            File.Copy(Path.Combine(Define.BuildOutputDir, $"{assemblyName}.dll"), Path.Combine(Define.HotfixDir, $"{assemblyName}.dll.bytes"), true);
            File.Copy(Path.Combine(Define.BuildOutputDir, $"{assemblyName}.pdb"), Path.Combine(Define.HotfixDir, $"{assemblyName}.pdb.bytes"), true);
            AssetDatabase.Refresh();

            Debug.Log("build success!");
        }

        public static void ShowNotification(string tips)
        {
            var game = EditorWindow.GetWindow(typeof(EditorWindow).Assembly.GetType("UnityEditor.GameView"));
            game?.ShowNotification(new GUIContent($"{tips}"));
        }
    }
    
}