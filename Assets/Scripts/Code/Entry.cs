using System;
using System.Threading;
using UnityEngine;
using YooAsset;

namespace TaoTie
{
    public static class Entry
    {
        public static void Start()
        {
            StartAsync().Coroutine();
        }

        private static async ETTask StartAsync()
        {
            try
            {
                ManagerProvider.RegisterManager<Messager>();
                ManagerProvider.RegisterManager<LogManager>();
                
                ManagerProvider.RegisterManager<AttributeManager>();
                
                ManagerProvider.RegisterManager<CoroutineLockManager>();
                ManagerProvider.RegisterManager<TimerManager>();
                
                ManagerProvider.RegisterManager<CacheManager>();

                var cm = ManagerProvider.RegisterManager<ConfigManager>();
                await cm.LoadAsync();
                
                ManagerProvider.RegisterManager<ResourcesManager>();
                ManagerProvider.RegisterManager<GameObjectPoolManager>();
                ManagerProvider.RegisterManager<ImageLoaderManager>();
                ManagerProvider.RegisterManager<MaterialManager>();
                
                ManagerProvider.RegisterManager<I18NManager>();
                ManagerProvider.RegisterManager<UIManager>();

                ManagerProvider.RegisterManager<CameraManager>();
                ManagerProvider.RegisterManager<SceneManager>();
                
                ManagerProvider.RegisterManager<ServerConfigManager>();

                ManagerProvider.RegisterManager<InputManager>();
                if(PackageManager.Instance.PlayMode == EPlayMode.HostPlayMode && (Define.Networked||Define.ForceUpdate))
                    await UIManager.Instance.OpenWindow<UIUpdateView,Action>(UIUpdateView.PrefabPath,StartGame);//下载热更资源
                else
                    StartGame();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
        static void StartGame()
        {
            StartGameAsync().Coroutine();
        }

        static async ETTask StartGameAsync()
        {
            ManagerProvider.RegisterManager<SoundManager>();
            var sm = ManagerProvider.RegisterManager<SoundManager>();

            GameObjectPoolManager.GetInstance().AddPersistentPrefabPath(UIToast.PrefabPath);
            using (ListComponent<ETTask> tasks = ListComponent<ETTask>.Create())
            {
                tasks.Add(sm.InitAsync());
                tasks.Add(GameObjectPoolManager.GetInstance().PreLoadGameObjectAsync(UIToast.PrefabPath, 1));
                await ETTaskHelper.WaitAll(tasks);
            }
            await PackageManager.Instance.UnloadUnusedAssets(Define.DefaultName);
            SceneManager.Instance.SwitchScene<LoginScene>().Coroutine();
        }
    }
    
}