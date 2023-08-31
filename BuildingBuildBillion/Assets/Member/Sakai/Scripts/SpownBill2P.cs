using UnityEngine;
using System.Collections.Generic;

public class SpownBill2P : MonoBehaviour
{
    public GameObject[] Bills2P;
    public Vector3 BuildingPosition { get; set; }

    private List<int> randomIndices2P = new List<int>();

    private void Start()
    {
        GenerateRandomIndices2P(2); // 初回に2つの乱数を生成して配列に格納
    }

    public void NewBill2P()
    {
        if (GameManager.Instance.CountDownGameTime < 0.0f) { return; }

        if (randomIndices2P.Count > 0)
        {
            int indexToUse = randomIndices2P[0];
            GameObject newBill2P = Instantiate(Bills2P[indexToUse], transform.position, Quaternion.identity);
            GameManager.Instance.Obj2 = newBill2P;
            UIManager.Instance.Player2NextBuildingMaterial.sprite = UIManager.Instance.PreviewBuildingThumbnail(Bills2P[randomIndices2P[1]]);

            randomIndices2P.RemoveAt(0); // 配列の先頭の値を削除
            int newIndex = GenerateRandomIndex2P(); // 新しい乱数を生成
            SetSpriteInBills2P(newIndex);   // 表示するスプライトを確定(Plane のみ
            randomIndices2P.Add(newIndex); // 新しい乱数を配列の最後尾に追加
        }
    }

    private void GenerateRandomIndices2P(int count)
    {
        for (int i = 0; i < count; i++)
        {
            int newIndex = GenerateRandomIndex2P();
            SetSpriteInBills2P(newIndex);
            randomIndices2P.Add(newIndex);
        }
    }

    private int GenerateRandomIndex2P()
    {
        return Random.Range(0, Bills2P.Length);
    }

    private void SetSpriteInBills2P(int index)
    {
        NewBuildingcon newBuildingcon = Bills2P[index].GetComponent<NewBuildingcon>();
        if (newBuildingcon.sprites.Length > 0)
        {
            // 最初にスプライトをランダムに選択する
            SpriteRenderer spriteRenderer = newBuildingcon.GetComponent<SpriteRenderer>();
            newBuildingcon.ChangeSpriteRandomly(spriteRenderer);
        }
    }
}