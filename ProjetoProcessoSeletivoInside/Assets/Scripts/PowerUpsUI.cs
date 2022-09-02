using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PowerUpEvent : UnityEvent<Texture,float> { }

public class PowerUpsUI : MonoBehaviour
{
    [SerializeField]
    private GameObject powerUpDisplayTemplate;

    public List<GameObject> powerUpsActive;
    public int powerUpsCount;

    // Atributos relaciondos ao grid dos icones
    [Header("Icon Grid Settings")]
    private int iconRowSize = 2;
    private int iconColSize = 3;
    private float iconSpacing = .1f;
    private Vector3 topLeftCornerOffset;
    private float iconSize = 1;

    private void Awake()
    {
        topLeftCornerOffset = new Vector3(transform.position.x - iconSize/ 2, transform.position.y - iconSize / 2, transform.position.z);
    }

    public void OnUsePowerUp(Texture powerUpIcon, float powerUpDuration)
    {
        
        powerUpsCount++;

        int x = powerUpsCount;
        int y = 0;
        while(x > iconRowSize)
        {
            x -= iconRowSize;
            y++;
        }

        Vector3 position = topLeftCornerOffset + new Vector3((x * (iconSpacing + iconSize)), (-y * (iconSpacing + iconSize)), 0);

        GameObject pUDisplay = Instantiate(powerUpDisplayTemplate, position, transform.rotation, transform);

        MaterialPropertyBlock _propBlock = new MaterialPropertyBlock();
        Renderer _renderer = pUDisplay.GetComponent<Renderer>();

        _renderer.GetPropertyBlock(_propBlock);

        _propBlock.SetFloat("_StartTime", Time.time);
        _propBlock.SetFloat("_Duration", powerUpDuration);
        _propBlock.SetTexture("_MainTex", powerUpIcon);

        _renderer.SetPropertyBlock(_propBlock);

        pUDisplay.name = Time.time.ToString();
        powerUpsActive.Add(pUDisplay);
        StartCoroutine(RemovePUDisplay(pUDisplay, powerUpDuration));
    }

    IEnumerator RemovePUDisplay(GameObject pUDisplay, float duration)
    {
        yield return new WaitForSeconds(duration);
        powerUpsActive.Remove(pUDisplay);
        powerUpsCount--;
        Destroy(pUDisplay);
        RepositionActivePowerUps();
    }

    void RepositionActivePowerUps()
    {
        int x = 0;
        int y = 0;
        Vector3 position;

        foreach (GameObject powerUp in powerUpsActive)
        {
            x++;
            while (x > iconRowSize)
            {
                x -= iconRowSize;
                y++;
            }
            position = topLeftCornerOffset + new Vector3((x * (iconSpacing + iconSize)), (-y * (iconSpacing + iconSize)), 0);

            powerUp.transform.position = position;
        }
    }
}
