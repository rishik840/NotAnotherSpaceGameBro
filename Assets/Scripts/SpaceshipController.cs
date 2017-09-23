using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceProject
{
    public class SpaceshipController : MonoBehaviour
    {
        [SerializeField]private const float m_SpeedMultiplyer = 1f;
        [SerializeField]
        private const float m_RotationMultiplyer = 1f;
        [SerializeField]
        float m_JumpPower = 12f;
        private Transform m_Cam;                  
        private Vector3 m_CamForward;             
        private Vector3 m_Move;
        private float m_TurnAmount;
        private float m_ForwardAmount;

        void Start()
        {
            // get the transform of the main camera
            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }
        }

        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            // read inputs
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v * m_CamForward + h * m_Cam.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = v * Vector3.forward + h * Vector3.right;
            }

            if (m_Move.magnitude > 1f)
                m_Move.Normalize();

            if(v>0)
            {
                m_Cam.position = new Vector3(m_Cam.position.x, m_Cam.position.y, Mathf.Lerp(m_Cam.position.z, m_Cam.position.z - 230, Time.deltaTime));
                if(v==0)
                    m_Cam.position = new Vector3(m_Cam.position.x, m_Cam.position.y, Mathf.Lerp(m_Cam.position.z, m_Cam.position.z + 230, Time.deltaTime));
            }
            m_Move = transform.InverseTransformDirection(m_Move);
            m_Move = Vector3.ProjectOnPlane(m_Move, Vector3.up);
            
            transform.Translate(m_Move * 100f);
            transform.Rotate(0,transform.eulerAngles.x * m_TurnAmount, 0);
        }
    }
}
