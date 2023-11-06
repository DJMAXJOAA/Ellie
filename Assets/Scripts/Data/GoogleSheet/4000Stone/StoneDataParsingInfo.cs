using Assets.Scripts.StatusEffects;
using Codice.Client.BaseCommands.Ls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

namespace Assets.Scripts.Data.GoogleSheet
{
    [Serializable]
    public class StoneData
    {
        public int index;
        public string name;
        public string description;
        public int appearanceStage;
        public int tier;
        public Element element;
        public int damage;
        public StatusEffectName statusEffect;
        public float statusEffectDuration;
        public StatusEffectName debuff;
        public List<int> conditions = new();
        public string specialEffectName;
        public int specialEffectIndex;
        public int combineCost;
        public int sellCost;
        public string imageName;
    }

    [CreateAssetMenu(fileName = "StoneData", menuName = "GameData List/StoneData")]
    public class StoneDataParsingInfo : DataParsingInfo
    {
        public List<StoneData> stones;

        public override T GetIndexData<T>(int index)
        {
            if (typeof(T) == typeof(StoneData))
            {
                return stones.Find(m => m.index == index) as T;
            }

            return default(T);
        }
        public override void Parse()
        {
            stones.Clear();

            string[] lines = tsv.Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                if (string.IsNullOrEmpty(lines[i])) continue;
                string[] entries = lines[i].Split('\t');

                StoneData data = new();
                try
                {
                    //�ε���
                    data.index = int.Parse(entries[0].Trim());
                    //�̸�
                    data.name = entries[1].Trim();
                    //Description
                    data.description = entries[2].Trim();
                    //ó�� �����ϴ� ��������
                    data.appearanceStage = int.Parse(entries[3].Trim());
                    //Ƽ��
                    data.tier = int.Parse(entries[4].Trim());
                    //�Ӽ�
                    data.element = (Element)Enum.Parse(typeof(Element), entries[5].Trim());
                    //������
                    data.damage = int.Parse(entries[6].Trim());
                    //�����ϴ� �����̻�
                    data.statusEffect = (StatusEffectName)Enum.Parse(typeof(StatusEffectName), entries[7].Trim());
                    //�����̻��� ���ӽð�
                    data.statusEffectDuration = float.Parse(entries[8].Trim());
                    //�����ϴ� �����
                    data.debuff = (StatusEffectName)Enum.Parse(typeof(StatusEffectName), entries[9].Trim());
                    //Ư�� ȿ���� �߻��ϴ� ������ list
                    string[] conditions = entries[10].Trim().Split(',');
                    if (entries[10] != "None")
                    {
                        foreach (string condition in conditions)
                        {
                            data.conditions.Add(int.Parse(condition.Trim()));
                        }
                    }
                    //Ư�� ȿ�� �̸�
                    data.specialEffectName = entries[11].Trim();
                    //Ư�� ȿ���� �ε���
                    data.specialEffectIndex = int.Parse(entries[12].Trim());
                    //���� ���
                    data.combineCost = int.Parse(entries[13].Trim());
                    //���� �Ǹ� ���
                    data.sellCost = int.Parse(entries[14].Trim());
                    //�̹��� �̸�
                    data.imageName = entries[15].Trim();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error parsing line {i + 1}: {entries[i]}");
                    Debug.LogError(e);
                    continue;
                }
                stones.Add(data);
            }
        }
    }
}