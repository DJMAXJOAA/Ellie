﻿using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Equipments
{
    public class Pickaxe : MonoBehaviour
    {
        public enum Tier
        {
            Tier5 = 9000,
            Tier4,
            Tier3,
            Tier2,
            Tier1,
        }
        public PickaxeData data;
        public Material[] pickaxeMaterials;
        public GameObject[] smithingEffects;
        public Transform effectPos;
        public Tier tier;
        [SerializeField] private int durability = 0;
        public int Durability { get { return durability; } set { durability = value; } }
        public int MinSmithPower { get { return data.minSmithPower; } }
        public int MaxSmithPower { get { return data.maxSmithPower; } }
        private void LoadPickaxeData(Tier tier)
        {
            this.tier = tier;
            data = DataManager.Instance.GetIndexData<PickaxeData, PickaxeDataParsingInfo>((int)tier);
            durability = data.durability;  
            int materialIdx = (int)tier % pickaxeMaterials.Length;
            GetComponent<Renderer>().material = pickaxeMaterials[materialIdx];
        }
        private void Start()
        {
            pickaxeMaterials = Resources.LoadAll<Material>("Materials/PickaxeMaterials");
            //일단 이펙트 다 꺼주기
            foreach(GameObject obj in smithingEffects)
            {
                obj.GetComponent<ParticleSystem>().Stop();
                obj.SetActive(false);
            }
        }
        private void Update()
        {
            //테스트
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                LoadPickaxeData(Tier.Tier5);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                LoadPickaxeData(Tier.Tier4);

            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                LoadPickaxeData(Tier.Tier3);

            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                LoadPickaxeData(Tier.Tier2);

            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                LoadPickaxeData(Tier.Tier1);

            }
        }

        public void HandleSmithingAnimationEvent()
        {
            int idx = Random.Range(0, smithingEffects.Length);
            GameObject effect = smithingEffects[idx];
            effect.SetActive(true);
            effect.GetComponent<ParticleSystem>().Play();
        }
    }
}