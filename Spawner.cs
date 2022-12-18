using Random = UnityEngine.Random;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace PrefabSpawner {
    public class Spawner : MonoBehaviour {
        [SerializeField] private GenericDictionary<GameObject, bool> prefabsWithAlignedInformation;
        [SerializeField] private GameObject prefabParent;
        
        public int quantity;
    
        private Mesh _navMesh;
        private NavMeshTriangulation _triangulatedNavMesh;

        private Vector3 _point1;
        private Vector3 _point2;
    
        private Vector3 _randomPosition;
    
        public void SpawnThemAll() {
            _navMesh = new Mesh();
            _triangulatedNavMesh = NavMesh.CalculateTriangulation();
            _navMesh.vertices = _triangulatedNavMesh.vertices;

            for (var i = 0; i < quantity; i++) {
                var dictionnaryRandomEntry = prefabsWithAlignedInformation
                    .ElementAt(Random.Range(0, prefabsWithAlignedInformation.Count)).Key; 
            
                SpawnPrefab(dictionnaryRandomEntry, prefabsWithAlignedInformation[dictionnaryRandomEntry]);
            }
        }

        private void SpawnPrefab(GameObject prefab, bool isAligned) {
            _point1 = _navMesh.vertices[Random.Range(0,_navMesh.vertexCount)];
            _point2 = _navMesh.vertices[Random.Range(0, _navMesh.vertexCount)];

            _randomPosition = Vector3.Lerp(_point1, _point2, Random.value);
        
            if (NavMesh.SamplePosition(_randomPosition, out NavMeshHit hit, 2f, -1)) {
                var spawnedPrefab = Instantiate(prefab,  hit.position, Quaternion.identity, prefabParent.transform);
                spawnedPrefab.transform.rotation = isAligned ? Quaternion.LookRotation(hit.normal) : new Quaternion(0, Random.Range(-1, 1), 0, 0);
            }
        }
    }
}

