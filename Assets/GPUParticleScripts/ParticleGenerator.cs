using UnityEngine;
using System.Runtime.InteropServices;

struct Particle{
	public Vector3 pos;
    public Vector3 vec;
	public Particle(Vector3 pos, Vector3 vec){
		this.pos = pos;
		this.vec = vec;
	}
}

public class ParticleGenerator : MonoBehaviour {
	[SerializeField]private Material particleMaterial;
	[SerializeField]private ComputeShader particleUpdatar;
    [SerializeField]private int particleCount = 10000;
    public float maxSpeed{get;set;}
    public float attractor{get;set;}
    [SerializeField]private float firstRadi = 1f;
    [SerializeField]private Transform targetTransform;

	private ComputeBuffer particleBuffer;
	void OnDisable() {
		particleBuffer.Release();
	}
	void OnEnable () {
		maxSpeed = 1f;
		attractor = 1f;
		InitializeComputeBuffer();
	}
    void Update() {
		particleUpdatar.SetBuffer(0, "Particles", particleBuffer);
		particleUpdatar.SetFloat("DeltaTime", Time.deltaTime);
		particleUpdatar.SetVector("targetPos", targetTransform.position);
		particleUpdatar.SetFloat("maxSpeed", maxSpeed);
		particleUpdatar.SetFloat("attractor", attractor);
		particleUpdatar.Dispatch(0, particleBuffer.count / 8 + 1, 1, 1);
	}
	void InitializeComputeBuffer() {
		particleBuffer = new ComputeBuffer(particleCount, Marshal.SizeOf(typeof(Particle)));
 
		Particle[] bullets = new Particle[particleBuffer.count];
		for(int i = 0; i < particleBuffer.count; i++){
			bullets[i] = new Particle(this.transform.position + new Vector3(Random.Range(-1f,1f), Random.Range(-1f,1f), Random.Range(-1f, 1f)).normalized * firstRadi, Vector3.zero);
		}
		particleBuffer.SetData(bullets);
	}
	void OnRenderObject() { 
		particleMaterial.SetBuffer("Particles", particleBuffer);
		particleMaterial.SetPass(0);
		Graphics.DrawProcedural(MeshTopology.Points, particleBuffer.count);
	}
 
}