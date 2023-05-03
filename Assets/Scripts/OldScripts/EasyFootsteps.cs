using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyFootsteps : MonoBehaviour
{
    public CharacterController controller;
    public CheckIfGrounded checkIfGrounded;
    public EasyTerrainTextureCheck terrainTextureCheck;
    public AudioSource audioSource;

    public AudioClip[] stoneClips;
    public AudioClip[] dirtClips;
    public AudioClip[] woodClips;

    float currentSpeed;
    bool walking;
    float distanceCovered;
    public float modifier = 0.5f;

    float airTime;
    AudioClip previousClip;
    float GetPlayerSpeed()
    {
        return controller.velocity.magnitude;
    }
    bool CheckIfWalking()
    {
        if (currentSpeed > 0 && checkIfGrounded.isGrounded)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Step(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5)
        {
            TriggerNextClip();
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentSpeed = GetPlayerSpeed();
        walking = CheckIfWalking();
        PlaySoundIfFalling();
       // if (walking)
      //  {
            //distanceCovered += (currentSpeed * Time.deltaTime) * modifier;
            //if(distanceCovered > 1)
           // {
               // TriggerNextClip();
               // distanceCovered = 0;
            //}
        //}
    }
    AudioClip GetClipFromArray(AudioClip[] clipArray)
    {
        int attemps = 3;
        AudioClip selectedClip = clipArray[Random.Range(0, clipArray.Length - 1)];

        while(selectedClip == previousClip && attemps > 0)
        {
            selectedClip = clipArray[Random.Range(0, clipArray.Length - 1)];
            attemps--;
        }
        previousClip = selectedClip;
        return selectedClip;
    }
   public void TriggerNextClip()
    {
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.volume = Random.Range(0.8f, 1f);
        if (checkIfGrounded.isOnTerrain)
        {
            terrainTextureCheck.GetTerrainTexture();
            if(terrainTextureCheck.textureValues[0] > 0)
            {
                audioSource.PlayOneShot(GetClipFromArray(dirtClips), terrainTextureCheck.textureValues[0]);
            }
            if (terrainTextureCheck.textureValues[1] > 0)
            {
                audioSource.PlayOneShot(GetClipFromArray(stoneClips), terrainTextureCheck.textureValues[1]);
            }
            if (terrainTextureCheck.textureValues[2] > 0)
            {
                audioSource.PlayOneShot(GetClipFromArray(woodClips), terrainTextureCheck.textureValues[2]);
            }
            if (terrainTextureCheck.textureValues[3] > 0)
            {
                audioSource.PlayOneShot(GetClipFromArray(stoneClips), terrainTextureCheck.textureValues[3]);
            }
        }
        else if (checkIfGrounded.isInside)
        {
            audioSource.PlayOneShot(GetClipFromArray(woodClips), 1);
        }
        else
        {
            audioSource.PlayOneShot(GetClipFromArray(stoneClips), 1);
        }
    }
    void PlaySoundIfFalling()
    {
        if (!checkIfGrounded.isGrounded)
        {
            airTime += Time.deltaTime;
        }
        else
        {
            if(airTime > 0.25f)
            {
                TriggerNextClip();
                airTime = 0;
            }
        }
    }
}
