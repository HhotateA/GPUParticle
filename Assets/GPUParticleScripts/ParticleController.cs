using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [SerializeField]private ParticleGenerator lParticleGenerator;
    [SerializeField]private ParticleGenerator rParticleGenerator;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update(){
        lParticleGenerator.gameObject.SetActive(!OVRInput.Get(OVRInput.RawButton.LThumbstick));
        rParticleGenerator.gameObject.SetActive(!OVRInput.Get(OVRInput.RawButton.RThumbstick));
        Vector2 stickL = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);
        Vector2 stickR = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);
        lParticleGenerator.attractor = stickL.x+0.5f;
        lParticleGenerator.maxSpeed = (stickL.y+1.0f)*2.0f;
        rParticleGenerator.attractor = stickR.x+0.5f;
        rParticleGenerator.maxSpeed = (stickR.y+1.0f)*2.0f;
    }
}
