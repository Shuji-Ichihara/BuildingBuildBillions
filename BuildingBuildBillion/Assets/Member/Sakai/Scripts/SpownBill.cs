using UnityEngine;
using System.Collections.Generic;

public class SpownBill : MonoBehaviour
{
    public GameObject[] Bills;
    public Vector3 BuildingPosition { get; set; }

    private List<int> randomIndices = new List<int>();

    private void Start()
    {
        GenerateRandomIndices(2); // 初回に2つの乱数を生成して配列に格納
    }

    public void NewBill()
    {
        if (GameManager.Instance.CountDownGameTime < 0.0f) { return; }

        if (randomIndices.Count > 0)
        {
            int indexToUse = randomIndices[0];
            GameObject newBill = Instantiate(Bills[indexToUse], transform.position, Quaternion.identity);
            GameManager.Instance.Obj = newBill;
            UIManager.Instance.Player1NextBuildingMaterial.sprite = UIManager.Instance.PreviewBuildingThumbnail(Bills[randomIndices[1]]);

            randomIndices.RemoveAt(0); // 配列の先頭の値を削除
            int newIndex = GenerateRandomIndex(); // 新しい乱数を生成
            SetSpriteInBills(newIndex); // 表示するスプライトを確定(Plane のみ)
            randomIndices.Add(newIndex); // 新しい乱数を配列の最後尾に追加
        }
    }

    private void GenerateRandomIndices(int count)
    {
        for (int i = 0; i < count; i++)
        {
            int newIndex = GenerateRandomIndex();
            SetSpriteInBills(newIndex);
            randomIndices.Add(newIndex);
        }
    }

    private int GenerateRandomIndex()
    {
        return Random.Range(0, Bills.Length);
    }

    private void SetSpriteInBills(int index)
    {
        NewBuildingcon newBuildingcon = Bills[index].GetComponent<NewBuildingcon>();
        if (newBuildingcon.sprites.Length > 0)
        {
            // 最初にスプライトをランダムに選択する
            SpriteRenderer spriteRenderer = newBuildingcon.GetComponent<SpriteRenderer>();
            newBuildingcon.ChangeSpriteRandomly(spriteRenderer);
        }
    }
}