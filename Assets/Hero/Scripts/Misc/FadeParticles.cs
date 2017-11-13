using UnityEngine;
using System.Collections;

public class FadeParticles : MonoBehaviour {
	public AnimationCurve particleFade;
	public Color color;
	public float minSize;
	public float maxSize;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Particle[] particles = particleEmitter.particles;
		for (int i = 0; i < particles.Length; i++){
			particles[i].color = new Color(color.r, color.g, color.b, particleFade.Evaluate(1.0f - (particles[i].energy / particles[i].startEnergy)));
			float energyVariation = particleEmitter.maxEnergy - particleEmitter.minEnergy;
			float particleEnergyVariation = particles[i].startEnergy - particleEmitter.minEnergy;
			float makeSize = particleEnergyVariation / energyVariation;
			particles[i].size = Mathf.Lerp(minSize, maxSize, makeSize);
		}
		particleEmitter.particles = particles;
	}
}
