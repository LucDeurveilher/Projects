using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXManager : MonoBehaviour
{
    [SerializeField] GameObject[] prefabVFX;

    [SerializeField] GameObject prefabBleed;
    [SerializeField] GameObject prefabBloodDrop;
    [SerializeField] GameObject prefabFreeze;
    [SerializeField] GameObject prefabPoison;

    [SerializeField] GameObject[] textEffectVFX;
    public void PlayVFX(int vfxIndex, Vector3 position, float duration = 0f)
    {
        // Vérifie que l'index est valide
        if (vfxIndex < 0 || vfxIndex >= prefabVFX.Length)
        {
            Debug.LogWarning("VFX index out of range.");
            return;
        }
        GameObject vfxInstance = Instantiate(prefabVFX[vfxIndex], position, Quaternion.identity);
        GameManager.Instance.AudioManager.PlayAttackAudio(vfxIndex);

        if (duration > 0)
        {
            Destroy(vfxInstance, duration);
        }
    }

    public void PlayBleed(Vector3 position, float duration = 0f)
    {
        GameObject vfxInstance = Instantiate(prefabBleed, position, Quaternion.identity);

        if (!GameManager.Instance.OptionsManager.mute)
        {
            vfxInstance.GetComponent<AudioSource>().mute = false;
            vfxInstance.GetComponent<AudioSource>().volume = GameManager.Instance.AudioManager.GetVolume();
        }
        else
        {
            vfxInstance.GetComponent<AudioSource>().mute = true;
        }
        
        if (duration > 0)
        {
            Destroy(vfxInstance, duration);
        }
    }

    public GameObject PlayBloodDrop(Transform parent)
    {
        Vector3 offset = Vector3.up/4;
        GameObject vfxInstance = Instantiate(prefabBloodDrop, parent,true);
        vfxInstance.transform.position = parent.position + offset;

        GameObject temp = Instantiate(textEffectVFX[0],parent.transform.position + Vector3.up, Quaternion.identity);
        Destroy(temp,3);

        return vfxInstance;
    }

    public GameObject PlayFreeze(Transform parent)
    {
        Vector3 offset = Vector3.up/2;
        GameObject vfxInstance = Instantiate(prefabFreeze, parent, true);
        vfxInstance.transform.position = parent.position + offset;

        if (!GameManager.Instance.OptionsManager.mute)
        {
            vfxInstance.GetComponent<AudioSource>().mute = false;
            vfxInstance.GetComponent<AudioSource>().volume = GameManager.Instance.AudioManager.GetVolume();
        }
        else
        {
            vfxInstance.GetComponent<AudioSource>().mute = true;
        }

        GameObject temp = Instantiate(textEffectVFX[1], parent.transform.position + Vector3.up, Quaternion.identity);
        Destroy(temp, 3);

        return vfxInstance;
    }

    public GameObject PlayPoison(Transform parent)
    {
        Vector3 offset = Vector3.up/2;
        GameObject vfxInstance = Instantiate(prefabPoison, parent, true);
        vfxInstance.transform.position = parent.position + offset;

        if (!GameManager.Instance.OptionsManager.mute)
        {
            vfxInstance.GetComponent<AudioSource>().mute = false;
            vfxInstance.GetComponent<AudioSource>().volume = GameManager.Instance.AudioManager.GetVolume();
        }
        else
        {
            vfxInstance.GetComponent<AudioSource>().mute = true;
        }

        GameObject temp = Instantiate(textEffectVFX[2], parent.transform.position + Vector3.up, Quaternion.identity);
        Destroy(temp, 3);

        return vfxInstance;
    }
}

