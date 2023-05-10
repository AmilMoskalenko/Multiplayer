using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool SharedInstance;
    
    [SerializeField] private GameObject _objectToPool;
    [SerializeField] private int _amountToPool;
    
    private List<GameObject> _pooledObjects;

    private void Awake()
    {
        SharedInstance = this;
    }

    private void Start()
    {
        _pooledObjects = new List<GameObject>();
        for (int i = 0; i < _amountToPool; i++)
        {
            var tmp = Instantiate(_objectToPool);
            tmp.SetActive(false);
            _pooledObjects.Add(tmp);
        }
    }
    
    public GameObject GetPooledObject()
    {
        for (int i = 0; i < _amountToPool; i++)
        {
            if (!_pooledObjects[i].activeInHierarchy)
            {
                return _pooledObjects[i];
            }
        }
        return null;
    }
}
