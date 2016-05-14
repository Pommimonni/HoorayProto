using UnityEngine;
using System.Collections.Generic;
using FMODUnity;
using FMOD;

public class FMODSpeakerPositions : MonoBehaviour {

    public Vector2 backLeft;
    public Vector2 surroundLeft;
    public Vector2 frontLeft;
    public Vector2 frontCenter;
    public Vector2 frontRight;
    public Vector2 surroundRight;
    public Vector2 backRight;
    public Vector2 sub;
    
    

    // Use this for initialization
    void Start () {
        RepositionSpeakers();
    }

    void RepositionSpeakers()
    {
        var fmodLowLvlSystem = RuntimeManager.LowlevelSystem;
        fmodLowLvlSystem.setSpeakerPosition(SPEAKER.BACK_LEFT, backLeft.x, backLeft.y, true);
        fmodLowLvlSystem.setSpeakerPosition(SPEAKER.SURROUND_LEFT, surroundLeft.x, surroundLeft.y, true);
        fmodLowLvlSystem.setSpeakerPosition(SPEAKER.FRONT_LEFT, frontLeft.x, frontLeft.y, true);
        fmodLowLvlSystem.setSpeakerPosition(SPEAKER.FRONT_CENTER, frontCenter.x, frontCenter.y, true);
        fmodLowLvlSystem.setSpeakerPosition(SPEAKER.FRONT_RIGHT, frontRight.x, frontRight.y, true);
        fmodLowLvlSystem.setSpeakerPosition(SPEAKER.SURROUND_RIGHT, surroundRight.x, surroundRight.y, true);
        fmodLowLvlSystem.setSpeakerPosition(SPEAKER.BACK_RIGHT, backRight.x, backRight.y, true);
        fmodLowLvlSystem.setSpeakerPosition(SPEAKER.LOW_FREQUENCY, sub.x, sub.y, true);
    }
	
    public class SpeakerPlacement
    {
        public SPEAKER speaker;
        public float x;
        public float y;
        public bool is3D;
    }
}


