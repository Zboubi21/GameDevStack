using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevStack.Pooling
{
    public class PoolTracker : MonoBehaviour
    {
        PoolType m_PoolType;
        EnemyType m_EnemyType;
        ProjectileType m_ProjectileType;
        FxType m_FxType;
        ObjectType m_ObjectType;

        public PoolType PoolType { get { return m_PoolType; } set { m_PoolType = value; } }
        public EnemyType EnemyType { get { return m_EnemyType; } set { m_EnemyType = value; } }
        public ProjectileType ProjectileType { get { return m_ProjectileType; } set { m_ProjectileType = value; } }
        public FxType FxType { get => m_FxType; set => m_FxType = value; }
        public ObjectType ObjectType { get => m_ObjectType; set => m_ObjectType = value; }

        public void ResetTrackedObject()
        {
            switch (m_PoolType)
            {
                case PoolType.EnemyType:
                    ObjectPooler.Instance.ReturnEnemyToPool(m_EnemyType, gameObject);
                break;
                case PoolType.ProjectileType:
                    ObjectPooler.Instance.ReturnProjectileToPool(m_ProjectileType, gameObject);
                break;
                case PoolType.ObjectType:
                    ObjectPooler.Instance.ReturnObjectToPool(m_ObjectType, gameObject);
                break;
                case PoolType.FxType:
                    ObjectPooler.Instance.ReturnFXToPool(m_FxType, gameObject);
                break;
            }
            Destroy(this);
        }
    }
}