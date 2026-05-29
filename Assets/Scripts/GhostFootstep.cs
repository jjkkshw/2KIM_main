using UnityEngine;
using UnityEngine.AI;

public class GhostFootstep : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource leftFootAudio;
    public AudioSource rightFootAudio;

    [Header("Footstep Sounds")]
    public AudioClip[] footstepClips;

    [Header("Step Settings")]
    private float roamStepInterval = 0.8f;
    private float chaseStepInterval = 0.35f;

    [Header("Movement Settings")]
    private float moveThreshold = 0.2f;
    private float chaseSpeedThreshold = 3f;

    private NavMeshAgent agent;

    private float stepTimer;
    private bool useLeftFoot;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        SetupAudio(leftFootAudio);
        SetupAudio(rightFootAudio);
    }

    void Update()
    {
        if (agent.velocity.magnitude > moveThreshold)
        {
            stepTimer += Time.deltaTime;

            bool isChasing =
                agent.velocity.magnitude > chaseSpeedThreshold;

            float currentInterval =
                isChasing ? chaseStepInterval : roamStepInterval;

            if (stepTimer >= currentInterval)
            {
                PlayFootstep();
                stepTimer = 0;
            }
        }
    }

    void PlayFootstep()
    {
        if (footstepClips.Length == 0)
            return;

        AudioClip clip =
            footstepClips[Random.Range(0, footstepClips.Length)];

        AudioSource currentAudio =
            useLeftFoot ? leftFootAudio : rightFootAudio;

        currentAudio.pitch =
            Random.Range(0.9f, 1.1f);

        currentAudio.PlayOneShot(clip);

        useLeftFoot = !useLeftFoot;
    }

    void SetupAudio(AudioSource source)
    {
        source.playOnAwake = false;
        source.loop = false;

        source.spatialBlend = 1f;
        source.minDistance = 1f;
        source.maxDistance = 15f;
    }
}