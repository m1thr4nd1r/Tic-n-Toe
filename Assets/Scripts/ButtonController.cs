using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public void Click(Button button) => GameManager.Instance.ProcessClick(button);

    public void ClickStart() => SceneController.StartGame();

    public void ClickExit() => SceneController.ExitGame();
}
