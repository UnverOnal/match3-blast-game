using System.Collections.Generic;
using Services.PoolingService;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;

namespace GamePlay.ParticleManagement
{
    public class ParticleManager
    {
        private readonly IPoolService _poolService;
        private readonly Dictionary<ParticleType, GameObject> _particles;
        private readonly Dictionary<ParticleType, ObjectPool<ParticleSystem>> _particlePools;

        private readonly Transform _parent;
        
        [Inject]
        public ParticleManager(ParticleData creationData, IPoolService poolService)
        {
            _poolService = poolService;
            _particles = new Dictionary<ParticleType, GameObject>();
            _particlePools = new Dictionary<ParticleType, ObjectPool<ParticleSystem>>();

            foreach (var particle in creationData.particles)
                _particles.Add(particle.type, particle.particle);

            _parent = new GameObject("Particles").transform;
        }

        public void Play(ParticleType type, Vector3 position)
        {
            var particle = GetParticle(type);
            particle.transform.position = position;
            particle.Play();
            
            var callBack = particle.GetComponent<ParticleCallback>();
            callBack.SetParticle(type, particle);
            callBack.OnStop += ReturnParticle;
        }

        private ParticleSystem GetParticle(ParticleType poolType)
        {
            var exist = _particlePools.TryGetValue(poolType, out var particlePool);
            if (!exist)
            {
                particlePool = _poolService.GetPoolFactory().CreatePool(() =>
                    Object.Instantiate(_particles[poolType], _parent).GetComponent<ParticleSystem>());
                _particlePools.Add(poolType, particlePool);
            }

            return particlePool.Get();
        }

        private void ReturnParticle(ParticleType poolType, ParticleSystem particleSystem)
        {
            var pool = _particlePools[poolType];
            pool.Return(particleSystem);
        }
    }
}