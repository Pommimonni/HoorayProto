using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;


[System.Serializable]
public class EffectDefinition
{
    public EffectsEnum whatTypeOfEffect;
    public GameObject effectToMakePrefab;
    public AudioClip soundEffectClip;
    public int audioClipPriority = 100;
    // wrapper methods, serialization, etc...
}




[System.Serializable]
public enum EffectsEnum : int { Hitting_wall,Wall_destrying, Finding_gem_while_moving,Finding_gem_movement_finished_to_middle, Finding_gem_movement_finished_to_combo, ManyInRow, };



public class Effects : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Common.effects = this;
	}
    public List<EffectDefinition> definedEffects;

    public float durationToGemMoveMiddle;
    public float durationToGemMoveToGUI;
    public float gemStayDuration = 1f;
    public float whenInRowFoundItLasts = 1f;
    public float fixedZToMove = -1f;
    public MoneyCounParameters moneyCountParams;



    public GameObject test;

    // Update is called once per frame
    void Update () {
      //  FindEffect(EffectsEnum.Finding_gem);

	}

// public Dictionary<EffectsEnum, GameObject> myDictionary = new Dictionary<EffectsEnum, GameObject>();

    EffectDefinition FindEffect(EffectsEnum whatEffect)
    {
        List<EffectDefinition> found= definedEffects.Where(item => item.whatTypeOfEffect == whatEffect).ToList();
        if (found.Count == 1)
        {
            Debug.Log("Found effect " + whatEffect.ToString());
            return found[0];
        }
        else
        {
            Debug.LogError("DIDNT FIND effect for EffectEnum "+whatEffect.ToString());
            return null;
        }
    }

    public GameObject PlayEffect(EffectsEnum whatEffect,Vector3 location,Transform parent = null)
    {
        // GameObject effectToPlay = definedEffects.Where(item => item.whatTypeOfEffect == whatEffect);
        EffectDefinition effectToPlay = FindEffect(whatEffect);

        GameObject createdEffectParent = null;
        GameObject effectToPlayPrefab = effectToPlay.effectToMakePrefab;
        if (effectToPlayPrefab != null)
        {
            if (parent == null)
            {
                createdEffectParent= PlayNormalEffect(effectToPlayPrefab, location);
               
            }
            else
            {
                PlayEffectAndParent(effectToPlayPrefab, location, parent);
                createdEffectParent= parent.gameObject;
            }
        }
        CreateSoundEffect(effectToPlay, createdEffectParent);
        return createdEffectParent;
    }
    
    void CreateSoundEffect(EffectDefinition givenEffect,GameObject toObject)
    {
        if (givenEffect.soundEffectClip)
        {
            AudioClip newClip = givenEffect.soundEffectClip;
            AudioSource addedSource = toObject.AddComponent<AudioSource>();
            addedSource.clip = newClip;
            addedSource.priority = givenEffect.audioClipPriority;
            addedSource.Play();
        }
    }

    void PlayEffectAndParent(GameObject effect, Vector3 location, Transform parent)
    {
        // GameObject created = PlayCFXEffect(effect, location);
        GameObject created = PlayNormalEffect(effect, location);
        Common.usefulFunctions.SetObjectToParent(parent, effect.transform, Vector3.zero);
    }

    GameObject PlayNormalEffect(GameObject effect, Vector3 location)
    {
        GameObject createdObject = (GameObject)Instantiate(effect, location, Quaternion.identity);
        return createdObject;

    }

    

    GameObject PlayCFXEffect(GameObject effect, Vector3 location)
    {
        GameObject spawnedParticle = CFX_SpawnSystem.GetNextObject(effect);
        spawnedParticle.transform.position = location;
        return spawnedParticle;
    }
}
