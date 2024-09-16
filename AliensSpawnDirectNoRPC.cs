    public class AliensSpawner : NetworkBehaviour {
        [SerializeField] private DeadPool deadPool;
        [SerializeField] private Transform alienSpawnTransform;

        public override void OnNetworkSpawn() {
            if (IsServer) InvokeRepeating(nameof(SpawnAlien), 2, 5);
        }

        public void SpawnAlien() {
            var alienInstance = deadPool.GetAlienFromPool(alienSpawnTransform.position, alienSpawnTransform.rotation);
            if (!alienInstance.IsSpawned) alienInstance.Spawn(true);
        }
        
        public void DespawnAlien(NetworkObject networkObject) {
            networkObject.Despawn();
        }
    }

    public class DeadPool : NetworkObjectPool {
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private GameObject alienPrefab;
        
        public NetworkObject GetBulletFromPool(BulletType bulletType, Vector3 position, Quaternion rotation) {
            var bulletInstance = GetNetworkObject(bulletPrefab, position, rotation);
            
            bulletInstance.Spawn(true);
            
            bulletInstance.GetComponent<Bullet>().bulletType = bulletType;
            
            return bulletInstance;
        }
        
        public NetworkObject GetAlienFromPool(Vector3 position, Quaternion rotation) {
            var alienInstance = GetNetworkObject(alienPrefab, position, rotation);
            
            alienInstance.Spawn(true);
            
            return alienInstance;
        }

        public void ReturnAlienToPool(NetworkObject networkObject) {
            ReturnNetworkObject(networkObject, alienPrefab);
        }
    }
