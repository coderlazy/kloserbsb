using UnityEngine;
using System.Collections;

public class EffectController : MonoSingleton<EffectController> 
{
	public AudioClip m_AudioEat;

	private ParticleSystem[] m_HitEffects;
	private ParticleSystem[] m_CrystalEffects;
	private ParticleSystem m_CrushEffect;
	private int hitEffectIndex;
	private int crystalEffectIndex;

	private AudioSource Audio;

	// Use this for initialization
	void Start () 
	{
		Audio = GetComponent<AudioSource>();

		// Ball crush effect
		m_CrushEffect = (GameObject.Instantiate(
			Resources.Load(Constants.PATH_EFFECTS + "ball-effect")) as GameObject).GetComponent<ParticleSystem>();
		
		// Hit effects
		m_HitEffects = new ParticleSystem[ModelLevel.CurrentLevel.maxBounce < 10 ? ModelLevel.CurrentLevel.maxBounce : 10];
		GameObject psys = Resources.Load(Constants.PATH_EFFECTS + "hit") as GameObject;
		for (int i = 0; i < m_HitEffects.Length; i++)
		{
			m_HitEffects[i] = (GameObject.Instantiate(psys) as GameObject).GetComponent<ParticleSystem>();
		}
		
		hitEffectIndex = 0;

		// Crystal effects
		m_CrystalEffects = new ParticleSystem[ModelLevel.CurrentLevel.minCrystal < 10 ? ModelLevel.CurrentLevel.minCrystal : 10];
		psys = Resources.Load(Constants.PATH_EFFECTS + "crystal-effect") as GameObject;
		for (int i = 0; i < m_CrystalEffects.Length; i++)
		{
			m_CrystalEffects[i] = (GameObject.Instantiate(psys) as GameObject).GetComponent<ParticleSystem>();
		}
		
		crystalEffectIndex = 0;
	}

	public void PlayCrushEffect()
	{
		m_CrushEffect.transform.localPosition = GameValue.Ball.transform.localPosition;
		m_CrushEffect.Play();
	}

	public void PlayHitEffect(Vector3 pos)
	{
		m_HitEffects[hitEffectIndex].transform.localPosition = pos;
		m_HitEffects[hitEffectIndex].Play();
		
		hitEffectIndex++;
		if (hitEffectIndex >= m_HitEffects.Length)
			hitEffectIndex = 0;
	}

	public void PlayCrystalEffect(Vector3 pos)
	{
		m_CrystalEffects[crystalEffectIndex].transform.localPosition = pos;
		m_CrystalEffects[crystalEffectIndex].Play();

		crystalEffectIndex++;
		if (crystalEffectIndex >= m_CrystalEffects.Length)
			crystalEffectIndex = 0;

		Audio.PlayOneShot(m_AudioEat);
	}
}
