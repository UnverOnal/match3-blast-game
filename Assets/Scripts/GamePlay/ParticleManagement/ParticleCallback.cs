using System;
using GamePlay.CellManagement;
using UnityEngine;

namespace GamePlay.ParticleManagement
{
    public class ParticleCallback : MonoBehaviour
    {
        public event Action<ParticleType, ParticleSystem> OnStop;

        private ParticleType _poolType;
        private ParticleSystem _particle;
        
        public void SetParticle(ParticleType poolType, ParticleSystem particle)
        {
            SetStopAction(particle ,ParticleSystemStopAction.Callback);
            _poolType = poolType;
            _particle = particle;
        }
    
        private void SetStopAction(ParticleSystem particle ,ParticleSystemStopAction stopAction)
        {
            var main = particle.main;
            main.stopAction = stopAction;
        }

        private void OnParticleSystemStopped()
        {
            OnStop?.Invoke(_poolType, _particle);
            OnStop = null;
        }
    }
}
