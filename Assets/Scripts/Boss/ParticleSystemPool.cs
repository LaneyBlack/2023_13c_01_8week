using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemPool : MonoBehaviour
{
    public static ParticleSystemPool Instance;

    [SerializeField]
    private GameObject particlePrefab;
    [SerializeField]
    private int poolSize = 30;

    private List<GameObject> particles;

    private void Awake()
    {
        Instance = this;
        InitializePool();
    }

    private void InitializePool()
    {
        particles = new List<GameObject>(poolSize);
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(particlePrefab, transform);
            obj.SetActive(false);
            particles.Add(obj);
        }
    }

    public GameObject GetParticle()
    {
        foreach (var particle in particles)
        {
            if (!particle.activeInHierarchy)
            {
                particle.SetActive(true);
                return particle;
            }
        }

        GameObject newParticle = Instantiate(particlePrefab, transform);
        particles.Add(newParticle);
        return newParticle;
    }

    public void ReturnParticle(GameObject particle)
    {
        particle.SetActive(false);
    }
}