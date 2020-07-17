using System;
using System.Collections.Generic;
using UnityEngine;
using GameDevStack.Patterns;

namespace GameDevStack.Pooling
{
    public class ObjectPooler : SingletonMonoBehaviour<ObjectPooler>
    {

		[Header("Enemy pools")]
		[SerializeField] private List<EnemyPool> m_EnemyPools = null;
		[Serializable] public class EnemyPool
		{
			public string m_Name;
			public EnemyType m_EnemyType;
			public GameObject m_Prefab;
			public int m_Size;
		}

		[Header("Projectile pools")]
		[SerializeField] private List<ProjectilPool> m_projectilPools = null;
		[Serializable] public class ProjectilPool
		{
			public string m_Name;
			public ProjectileType m_ProjectileType;
			public GameObject m_Prefab;
			public int m_Size;
		}
		[Header("Fx pools")]
		[SerializeField] private List<FXPool> m_FXPools = null;
		[Serializable] public class FXPool
		{
			public string m_Name;
			public FxType m_FxType;
			public GameObject m_Prefab;
			public int m_Size;
		}

		[Header("Object pools")]
		[SerializeField] private List<ObjectPool> m_objectPools = null;
		[Serializable] public class ObjectPool
		{
			public string m_Name;
			public ObjectType m_ObjectType;
			public GameObject m_Prefab;
			public int m_Size;
		}

		[Space]

		[Header("Pool test")]
		[SerializeField] private bool m_UsePoolTest = true;
		[SerializeField] private Transform m_SpawnPool = null;
		[SerializeField] private PoolTest[] m_PoolTest = null;
		[Serializable] public class PoolTest
		{
			public KeyCode m_Input;
			public EnemyType m_ObjectToSpawn;
		}

		[SerializeField] private ObjectPoolTest[] m_ObjectPoolTest = null;
		[Serializable] public class ObjectPoolTest
		{
			public KeyCode m_Input;
			public ObjectType m_ObjectToSpawn;
		}

		private Dictionary<EnemyType, Queue<GameObject>> m_EnemyPoolDictionary;
		private Dictionary<ProjectileType, Queue<GameObject>> m_ProjectilePoolDictionary;
		private Dictionary<FxType, Queue<GameObject>> m_FxPoolDictionary;
		private Dictionary<ObjectType, Queue<GameObject>> m_ObjectPoolDictionary;

		private Queue<PoolTracker> m_TrackedObject = new Queue<PoolTracker>();

		protected override void Awake()
		{
			base.Awake();
			CreateAllPools();
		}

		private void CreateAllPools()
		{
			m_EnemyPoolDictionary = new Dictionary<EnemyType, Queue<GameObject>>();
			foreach(EnemyPool pool in m_EnemyPools)
			{
				Queue<GameObject> objectPool = new Queue<GameObject>();
				for(int i = 0, l = pool.m_Size; i < l; ++i)
				{
					GameObject obj = Instantiate(pool.m_Prefab, transform, this);
					obj.SetActive(false);
					obj.name = obj.name + "_" + i;
					objectPool.Enqueue(obj);
				}
				m_EnemyPoolDictionary.Add(pool.m_EnemyType, objectPool);
			}

			m_ProjectilePoolDictionary = new Dictionary<ProjectileType, Queue<GameObject>>();
			foreach(ProjectilPool pool in m_projectilPools)
			{
				Queue<GameObject> objectPool = new Queue<GameObject>();
				for(int i = 0, l = pool.m_Size; i < l; ++i)
				{
					GameObject obj = Instantiate(pool.m_Prefab, transform, this);
					obj.SetActive(false);
					obj.name = obj.name + "_" + i;
					objectPool.Enqueue(obj);
				}
				m_ProjectilePoolDictionary.Add(pool.m_ProjectileType, objectPool);
			}

			m_FxPoolDictionary = new Dictionary<FxType, Queue<GameObject>>();
			foreach (FXPool pool in m_FXPools)
			{
				Queue<GameObject> objectPool = new Queue<GameObject>();
				for (int i = 0, l = pool.m_Size; i < l; ++i)
				{
					GameObject obj = Instantiate(pool.m_Prefab, transform, this);
					obj.SetActive(false);
					obj.name = obj.name + "_" + i;
					objectPool.Enqueue(obj);
				}
				m_FxPoolDictionary.Add(pool.m_FxType, objectPool);
			}

			m_ObjectPoolDictionary = new Dictionary<ObjectType, Queue<GameObject>>();
			foreach(ObjectPool pool in m_objectPools)
			{
				Queue<GameObject> objectPool = new Queue<GameObject>();
				for(int i = 0, l = pool.m_Size; i < l; ++i)
				{
					GameObject obj = Instantiate(pool.m_Prefab, transform, this);
					obj.SetActive(false);
					obj.name = obj.name + "_" + i;
					objectPool.Enqueue(obj);
				}
				m_ObjectPoolDictionary.Add(pool.m_ObjectType, objectPool);
			}
		}

		void Update()
		{
			if(m_UsePoolTest)
			{
				for (int i = 0, l = m_PoolTest.Length; i < l; ++i)
				{
					if(Input.GetKeyDown(m_PoolTest[i].m_Input))
					{
						if(m_SpawnPool != null)
							SpawnEnemyFromPool(m_PoolTest[i].m_ObjectToSpawn, m_SpawnPool.position, m_SpawnPool.rotation);
						else
							SpawnEnemyFromPool(m_PoolTest[i].m_ObjectToSpawn, Vector3.zero, Quaternion.identity);
					}
				}
				for (int i = 0, l = m_ObjectPoolTest.Length; i < l; ++i)
				{
					if(Input.GetKeyDown(m_ObjectPoolTest[i].m_Input))
					{
						if(m_SpawnPool != null)
							SpawnObjectFromPool(m_ObjectPoolTest[i].m_ObjectToSpawn, m_SpawnPool.position, m_SpawnPool.rotation);
						else
							SpawnObjectFromPool(m_ObjectPoolTest[i].m_ObjectToSpawn, Vector3.zero, Quaternion.identity);
					}
				}
			}
		}

		public GameObject SpawnEnemyFromPool(EnemyType enemyType, Vector3 position, Quaternion rotation)
		{

			if(!m_EnemyPoolDictionary.ContainsKey(enemyType))
			{
				Debug.LogWarning("Pool of " + enemyType + " dosen't exist.");
				return null;
			}

			if(m_EnemyPoolDictionary[enemyType].Count == 0)
			{
				Debug.LogError(enemyType.ToString() + " pool is empty!");
				return null;
			}

			GameObject objectToSpawn = m_EnemyPoolDictionary[enemyType].Dequeue();

			objectToSpawn.transform.position = position;
			objectToSpawn.transform.rotation = rotation;
			objectToSpawn.SetActive(true);

			PoolTracker poolTracker = AddPoolTrackerComponent(objectToSpawn, PoolType.EnemyType);
			poolTracker.EnemyType = enemyType;
			m_TrackedObject.Enqueue(poolTracker);

			return objectToSpawn;
		}
		public void ReturnEnemyToPool(EnemyType enemyType, GameObject objectToReturn)
		{
			CheckPoolTrackerOnResetObject(objectToReturn);
			objectToReturn.SetActive(false);
			m_EnemyPoolDictionary[enemyType].Enqueue(objectToReturn);
		}


		public GameObject SpawnProjectileFromPool(ProjectileType projectileType, Vector3 position, Quaternion rotation)
		{

			if(!m_ProjectilePoolDictionary.ContainsKey(projectileType))
			{
				Debug.LogError("Pool of " + projectileType + " dosen't exist.");
				return null;
			}

			if(m_ProjectilePoolDictionary[projectileType].Count == 0)
			{
				Debug.LogError(projectileType.ToString() + " pool is empty!");
				return null;
			}

			GameObject objectToSpawn = m_ProjectilePoolDictionary[projectileType].Dequeue();

			objectToSpawn.transform.position = position;
			objectToSpawn.transform.rotation = rotation;
			objectToSpawn.SetActive(true);

			PoolTracker poolTracker = AddPoolTrackerComponent(objectToSpawn, PoolType.ProjectileType);
			poolTracker.ProjectileType = projectileType;
			m_TrackedObject.Enqueue(poolTracker);

			return objectToSpawn;
		}
		public void ReturnProjectileToPool(ProjectileType objectType, GameObject objectToReturn)
		{
			CheckPoolTrackerOnResetObject(objectToReturn);
			objectToReturn.SetActive(false);
			m_ProjectilePoolDictionary[objectType].Enqueue(objectToReturn);
		}

		public GameObject SpawnFXFromPool(FxType FXType, Vector3 position, Quaternion rotation)
		{

			if (!m_FxPoolDictionary.ContainsKey(FXType))
			{
				Debug.LogError("Pool of " + FXType + " dosen't exist.");
				return null;
			}

			if (m_FxPoolDictionary[FXType].Count == 0)
			{
				Debug.LogError(FXType.ToString() + " pool is empty!");
				return null;
			}

			GameObject objectToSpawn = m_FxPoolDictionary[FXType].Dequeue();

			objectToSpawn.transform.position = position;
			objectToSpawn.transform.rotation = rotation;
			objectToSpawn.SetActive(true);

			PoolTracker poolTracker = AddPoolTrackerComponent(objectToSpawn, PoolType.FxType);
			poolTracker.FxType = FXType;
			m_TrackedObject.Enqueue(poolTracker);

			return objectToSpawn;
		}
		public void ReturnFXToPool(FxType objectType, GameObject objectToReturn)
		{
			CheckPoolTrackerOnResetObject(objectToReturn);
			objectToReturn.SetActive(false);
			m_FxPoolDictionary[objectType].Enqueue(objectToReturn);
		}

		public GameObject SpawnObjectFromPool(ObjectType objectType, Vector3 position, Quaternion rotation, Transform parent = null)
		{

			if(!m_ObjectPoolDictionary.ContainsKey(objectType))
			{
				Debug.LogError("Pool of " + objectType + " dosen't exist.");
				return null;
			}

			if(m_ObjectPoolDictionary[objectType].Count == 0)
			{
				Debug.LogError(objectType.ToString() + " pool is empty!");
				return null;
			}

			GameObject objectToSpawn = m_ObjectPoolDictionary[objectType].Dequeue();

			if (objectToSpawn == null)
				return null;

			objectToSpawn.transform.position = position;
			objectToSpawn.transform.rotation = rotation;

			if (parent != null)
				if( objectToSpawn.transform.parent != parent)
					objectToSpawn.transform.SetParent(parent);

			objectToSpawn.SetActive(true);

			PoolTracker poolTracker = AddPoolTrackerComponent(objectToSpawn, PoolType.ObjectType);
			poolTracker.ObjectType = objectType;
			m_TrackedObject.Enqueue(poolTracker);

			return objectToSpawn;
		}
		public void ReturnObjectToPool(ObjectType objectType, GameObject objectToReturn)
		{
			CheckPoolTrackerOnResetObject(objectToReturn);
			objectToReturn.SetActive(false);
			m_ObjectPoolDictionary[objectType].Enqueue(objectToReturn);
		}

		PoolTracker AddPoolTrackerComponent(GameObject objectToSpawn, PoolType poolType)
		{
			// PoolTracker poolTracker = objectToSpawn.GetComponent<PoolTracker>();
			// if(poolTracker == null){
				PoolTracker poolTracker = objectToSpawn.AddComponent<PoolTracker>().GetComponent<PoolTracker>();
			// }
			poolTracker.PoolType = poolType;
			return poolTracker;
		}
		
		public void On_ReturnAllInPool()
		{
			for (int i = 0, l = m_TrackedObject.Count; i < l; ++i) 
			{
				PoolTracker poolTracker = m_TrackedObject.Dequeue();
				poolTracker?.ResetTrackedObject();
			}
		}

		void CheckPoolTrackerOnResetObject(GameObject objectToReturn)
		{
			PoolTracker poolTracker = objectToReturn.GetComponent<PoolTracker>();
			if(poolTracker != null)
				Destroy(poolTracker);
		}

	}
}