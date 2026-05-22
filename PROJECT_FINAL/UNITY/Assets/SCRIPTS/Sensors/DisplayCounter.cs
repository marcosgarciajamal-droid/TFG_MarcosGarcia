using UnityEngine;
using TMPro;

public class DisplayCounter: MonoBehaviour
{
public TMP_Text counterText;
public Counter shelfCounter;
void Update()
{
    counterText.text = shelfCounter.pieceCount.ToString();
}
}
