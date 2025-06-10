using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.CodeBase.Cam
{
    public class Cameras
    {
        public Camera ScreenCamera => _screenCamera;

        private Camera _screenCamera;

        public Cameras(Camera screenCamera)
        {
            _screenCamera = screenCamera;
        }
    }

    public class CameraControlInstaller : MonoInstaller<CameraControlInstaller>
    {
        [SerializeField] private Camera _screenCamera;

        public override void InstallBindings()
        {
            Container.Bind<Cameras>()
                .FromMethod(() => new Cameras(_screenCamera))
                .AsSingle();
        }
    }
}
