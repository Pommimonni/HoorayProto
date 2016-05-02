using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;



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
public enum EffectsEnum : int { Hitting_wall,Wall_destrying, Finding_gem_while_moving,Finding_gem_movement_finished_to_middle, When_3_in_row_found,Gem_movement_finishing_combo,BonusGameEnds };



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

    public GameObject bombExplosionEffect;
    public GameObject bigBombExplosionEffect;
    public bool removeEffectSounds = false;
    public int modToCoinMove=2;


    // Update is called once per frame
    void Update () {
        //  FindEffect(EffectsEnum.Finding_gem);
        if (Input.GetKeyDown(KeyCode.C))
        {
            CreateCoinMoveEffectForPlayers(2f);
        }

	}

// public Dictionary<EffectsEnum, GameObject> myDictionary = new Dictionary<EffectsEnum, GameObject>();

    EffectDefinition FindEffect(EffectsEnum whatEffect)
    {
        List<EffectDefinition> found= definedEffects.Where(item => item.whatTypeOfEffect == whatEffect).ToList();
        if (found.Count == 1)
        {
          //  Debug.Log("Found effect " + whatEffect.ToString());
            return found[0];
        }
        else
        {
            Debug.LogError("DIDNT FIND effect for EffectEnum "+whatEffect.ToString());
            return null;
        }
    }

    public GameObject PlayBombExplosionEffect(Vector3 position)
    {
       return PlayNormalEffect(bombExplosionEffect, position);
    }
    public GameObject PlayBigBombExplosionEffect(Vector3 position)
    {
        return PlayNormalEffect(bigBombExplosionEffect,position);
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
        if (!removeEffectSounds)
        {
            CreateSoundEffect(effectToPlay, createdEffectParent);
        }
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

    public void SpawnEffectOnBothScreens(EffectsEnum whatEffect,Vector3 player1Position)
    {
        PlayEffect(whatEffect, player1Position);
        PlayEffect(whatEffect, Common.gameMaster.TransformPlayer1PositionToPlayer2Position(player1Position));
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
    public GameObject CoinGO;
    float coinmoveDuration = 0.5f;
    void CreateCoinMoveEffect(PlayerInformation player,float duration)
    {
        Vector3 startPos = player.myInformationGUI.moneyTotalBBResults.gameObject.transform.position;
        Vector3 endPos = player.myInformationGUI.moneyTotalText.transform.position;
        endPos.z -= 3f;
        StartCoroutine(CreateMultipleMovements(startPos,endPos,duration));
    }

    public void CreateCoinMoveEffectForPlayers(float duration)
    {
        CreateCoinMoveEffect(Common.gameMaster.player1,duration);
        CreateCoinMoveEffect(Common.gameMaster.player2,duration);
    }
    public void CreateOneCoinMove(float duration,Vector3 BBresults,Vector3 totalTextPos)
    {
        Vector3 startPos = BBresults;
        Vector3 endPos = totalTextPos;
        
       // endPos.z -= 4f;
        StartCoroutine(CoinEffect(startPos, endPos, duration));
    }

    IEnumerator CreateMultipleMovements(Vector3 startPos,Vector3 endPos,float duration)
    {
        float startTime = Time.time;
        while (duration > Time.time - startTime)
        {
            StartCoroutine(CoinEffect(startPos, endPos,coinmoveDuration));
            yield return new WaitForSeconds(coinmoveDuration / 2);
        }
    }

    IEnumerator CoinEffect(Vector3 startPos, Vector3 endPos,float duration)
    {
        GameObject createdObj = (GameObject)Instantiate(CoinGO, startPos, Quaternion.identity);
        Common.usefulFunctions.MoveObjectToPlaceNonFixed(createdObj.transform, endPos, duration);
        yield return new WaitForSeconds(coinmoveDuration);
        Destroy(createdObj);
    }
    

    GameObject PlayCFXEffect(GameObject effect, Vector3 location)
    {
        GameObject spawnedParticle = CFX_SpawnSystem.GetNextObject(effect);
        spawnedParticle.transform.position = location;
        return spawnedParticle;
    }
}
