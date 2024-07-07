using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;

public class CreateNewBingoCard : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown cardSize;
    [SerializeField] private TMP_Dropdown dorama;
    [SerializeField] private Button GoButton;

    private TMP_Dropdown.OptionData default4x4 = new("4x4");
    private TMP_Dropdown.OptionData default5x5 = new("5x5");
    private TMP_Dropdown.OptionData default6x6 = new("6x6");
    private string randomDorama = "Random k-drama";

    private void Awake()
    {
        GoButton.onClick.AddListener(GenerateNewCard);
    }

    public void Initialize()
    {
        foreach(string doramasKey in DataBase.Doramas.Keys)
        {
            dorama.options.Add(new(doramasKey));
        }
    }

    public void OnDoramaPick()
    {
        string pickedDorama = dorama.itemText.text;
        if (pickedDorama == randomDorama)
        {
            int randomIndex = UnityEngine.Random.Range(0, DataBase.Doramas.Count);
            pickedDorama = DataBase.Doramas.ElementAt(randomIndex).Key;
        }
            
        string pickedSize = cardSize.itemText.text;
        int keyCount = DataBase.Doramas[pickedDorama].Count;

        switch (keyCount)
        {
            case > 36:
                cardSize.options = new() { default4x4, default5x5, default6x6 };
                break;
            case > 25:
                cardSize.options = new() { default4x4, default5x5 };
                break;
            case > 16:
                cardSize.options = new() { default4x4 };
                break;
            default:
                throw new Exception("Error when getting the number of tags of the drama!");
        }


    }

    private void GenerateNewCard()
    {
        BingoCard newCard = new();
        newCard.Dorama = dorama.itemText.text;
        int size;
        switch (cardSize.itemText.text)
        {
            case "4x4":
                size = 4;
                break;
            case "5x5":
                size = 5;
                break;
            case "6x6":
                size = 6;
                break;
            default:
                throw new Exception("Unknown card size!");
        }
        newCard.Size = new Vector2(size, size);
        newCard.Cells = GenetareNewCells(newCard.Dorama, size * size);
    }

    private BingoCell[] GenetareNewCells(string dorama, int count)
    {
        BingoCell[] result = new BingoCell[count];

        string[] tags = DataBase.Doramas[dorama].Keys.ToArray();

        GameMath.Shuffle(tags);

        for (int i = 0; i < count; i++)
        {
            result[i].Tag = tags[i];
        }

        return result;
    }

    private void OnDestroy()
    {
        GoButton.onClick.RemoveAllListeners();
    }
}

[Serializable]
public class BingoCell
{
    public string Tag;
    public bool IsPushed;
}

public class GameplayManager
{
    private BingoCard bingoCard;
}

public static class GameMath
{
    public static void Shuffle<T>(T[] array)
    {
        int n = array.Length;
        while (n > 1)
        {
            int k = UnityEngine.Random.Range(0, n--);
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    } 
}