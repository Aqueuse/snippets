using Spaceships.Shoot;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Spaceships {
    public class Spaceship : NetworkBehaviour {
        public InputActionReference shootInputActionReference;
        public Transform playerTurretTransform;

        private GameObject missile;
        
        private void OnEnable() {
            shootInputActionReference.action.Enable();
            shootInputActionReference.action.performed += Shoot;
        }

        private void OnDisable() {
            shootInputActionReference.action.Disable();
            shootInputActionReference.action.performed += Shoot;
        }

        
        private void Shoot(InputAction.CallbackContext callBackContext) {
            if (IsLocalPlayer) ServerFireRpc();
        }
        
        [Rpc(SendTo.Server)]
        private void ServerFireRpc() {
            missile = ObjectsReference.instance.deadPool.GetBulletFromPool(BulletType.BASIC, playerTurretTransform.position, playerTurretTransform.rotation).gameObject;
            missile.GetComponent<Bullet>().PropulseMe();
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
}
