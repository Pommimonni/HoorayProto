using UnityEngine;
using System.Collections;

public class ControlGameMusics : MonoBehaviour {

    void Awake()
    {
        Common.controlGameMusic = this;
    }

    public FMODUnity.StudioEventEmitter bonusRoundSound;
    public FMODUnity.StudioEventEmitter gameMusic;
    public void StartBonusRound()
    {
        bonusRoundSound.Play();
        gameMusic.Stop();
        
    }
    public void EndBonusRound()
    {
        bonusRoundSound.Stop();
        gameMusic.Play();
    }

    public int hits = 0;

    public void PlayerHits()
    {
        hits++;
        float hitsf = (float)hits;
        gameMusic.SetParameter("Hits_played", hitsf);
    }
}
