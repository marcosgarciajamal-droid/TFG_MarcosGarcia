using UnityEngine;
using TMPro;

public class DisplayCounter: MonoBehaviour
{
public TMP_Text counterText;
public PlantController plant;

public bool ShelfA, ShelfB, ShelfC;
void Update()
{
    if (ShelfA) counterText.text = plant.CountA.ToString();
    if (ShelfB) counterText.text = plant.CountB.ToString();
    if (ShelfC) counterText.text = plant.CountC.ToString();
}

}
