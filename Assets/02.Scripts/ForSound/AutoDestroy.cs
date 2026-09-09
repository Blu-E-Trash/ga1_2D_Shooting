using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AutoDestroy : MonoBehaviour
{
    private void Start()
    {
        AudioSource audioSource = GetComponent<AudioSource>();

        if (audioSource != null && audioSource.clip != null)
        {
            // 사운드 클립의 재생 길이(초)만큼 기다렸다가 자기 자신을 파괴
            Destroy(gameObject, audioSource.clip.length);
        }
        else
        {
            Destroy(gameObject, 2f); // 사운드가 없을 경우 안전장치 (2초 후 파괴)
        }
    }
}