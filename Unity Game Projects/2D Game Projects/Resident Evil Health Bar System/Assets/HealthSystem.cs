using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    Sprite healthStatus;
    Sprite healthRate;
    Image m_Image;
    [SerializeField] float scrollSpeed;
    [SerializeField] Sprite[] healthStatusSprites;
    [SerializeField] Sprite[] healthRateSprites;
    [SerializeField] GameObject StatusGameObj;
    [SerializeField] GameObject healthRateGameObj;
    [SerializeField] bool isPosioned;
    [SerializeField] string currentState;

    private void Start()
    {
        StatusGameObj.GetComponent<Image>().sprite = healthStatus;
        healthRateGameObj.GetComponent<Image>().sprite = healthRate;
        m_Image = GetComponent<Image>();
        m_Image.color = new Color32(0, 255, 0, 255);
        healthStatus = healthStatusSprites[0];
        healthRate = healthRateSprites[0];
        isPosioned = false;
        currentState = "G";
    }

    private void Update()
    {
        StatusGameObj.GetComponent<Image>().sprite = healthStatus;
        healthRateGameObj.GetComponent<Image>().sprite = healthRate;

        m_Image.material.mainTextureOffset = m_Image.material.mainTextureOffset + new Vector2(Time.deltaTime * (-scrollSpeed / 10), 0f);

        // fine
        if(Input.GetKeyDown(KeyCode.G))
        {
            currentState = "G";
        }

        // caution
        if (Input.GetKeyDown(KeyCode.Y))
        {
            currentState = "Y";
        }

        // danger
        if (Input.GetKeyDown(KeyCode.R))
        {
            currentState = "R";
        }

        // posion
        if (Input.GetKeyDown(KeyCode.P))
        {
            isPosioned = true;
        }

        // blue herb
        if (Input.GetKeyDown(KeyCode.B))
        {
            isPosioned = false;
        }

        UpdateCondition(currentState, isPosioned);
    }

    private void UpdateCondition(string state, bool posion)
    {
        if(state == "G")
        {
            if (!posion)
            {
                m_Image.color = new Color32(0, 255, 0, 255);
                healthStatus = healthStatusSprites[0];
            }
            else
            {
                m_Image.color = new Color32(119, 0, 255, 255);
                healthStatus = healthStatusSprites[3];
            }
            healthRate = healthRateSprites[0];
        }
        else if(state == "Y")
        {
            if (!posion)
            {
                m_Image.color = new Color32(255, 255, 0, 255);
                healthStatus = healthStatusSprites[1];
            }
            else
            {
                m_Image.color = new Color32(119, 0, 255, 255);
                healthStatus = healthStatusSprites[3];
            }
            healthRate = healthRateSprites[1];
        }
        else if(state == "R")
        {
            if (!posion)
            {
                m_Image.color = new Color32(255, 0, 0, 255);
                healthStatus = healthStatusSprites[2];
            }
            else
            {
                m_Image.color = new Color32(119, 0, 255, 255);
                healthStatus = healthStatusSprites[3];
            }
            healthRate = healthRateSprites[2];
        }
    }
}
