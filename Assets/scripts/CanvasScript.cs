using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/** interface of the whole canvas
 * information is updated here
 * no information is kept in canvas
 */
public class CanvasScript : MonoBehaviour
{
    public FloatingJoystick floatingJoystick;
    public Vector2 joystickDirection;
    public TextMeshProUGUI skullText;
    public TextMeshProUGUI coinText;
    public Image bg;
    public TextMeshProUGUI overText;
    public Button restartButton;
    [NonSerialized]
    public Action RestartAction;
    
    public void GameOverEnabled(bool b)
    {
        restartButton.gameObject.SetActive(b);
        overText.gameObject.SetActive(b);
        bg.gameObject.SetActive(b);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        restartButton.onClick.AddListener(RestartFunction);
    }

    // Update is called once per frame
    void Update()
    {
        joystickDirection = floatingJoystick.Direction;
    }

    public void UpdateText(string skull, string coin)
    {
        skullText.text = skull;
        coinText.text = coin;
    }

    private void RestartFunction()
    {
        RestartAction();
    }
}

