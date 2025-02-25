using System;
using System.Collections.Generic;

/// <summary>
/// ランキングのデータを格納するコンテナ
/// </summary>
[Serializable]
public class RankingData
{
    public List<(string name, int score)> Datas = new();
}
