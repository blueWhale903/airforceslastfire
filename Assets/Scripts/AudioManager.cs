using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Music")]
    [SerializeField] private AudioClip backgroundMusic;

    [Header("SFX")]
    [SerializeField] private AudioClip enemyHitSFX;
    [SerializeField] private AudioClip enemyExplodeSFX;
    [SerializeField] private AudioClip playerHurtSFX;
    [SerializeField] private AudioClip playerShootSFX;
    [SerializeField] private AudioClip bossWarningSFX;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (musicSource == null)
            musicSource = gameObject.AddComponent<AudioSource>();
        if (sfxSource == null)
            sfxSource = gameObject.AddComponent<AudioSource>();

        musicSource.loop = true;
        musicSource.playOnAwake = false;
        sfxSource.playOnAwake = false;
    }

    void Start()
    {
        PlayMusic();
    }

    public void PlayMusic()
    {
        if (backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.Play();
        }
    }

    public void PlayEnemyHitSFX(float volume = 1f)
    {
        PlaySFX(enemyHitSFX, volume);
    }

    public void PlayEnemyExplodeSFX(float volume = 1f)
    {
        PlaySFX(enemyExplodeSFX, volume);
    }

    public void PlayPlayerHurtSFX(float volume = 1f)
    {
        PlaySFX(playerHurtSFX, volume);
    }

    public void PlayPlayerShootSFX(float volume = 1f)
    {
        sfxSource.pitch = Random.Range(0.9f, 1.1f);
        PlaySFX(playerShootSFX, volume);
        sfxSource.pitch = 1;
    }

    public void PlayBossWarningSFX(float volume = 1f)
    {
        PlaySFX(bossWarningSFX, volume);
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip, volume);
    }
}
