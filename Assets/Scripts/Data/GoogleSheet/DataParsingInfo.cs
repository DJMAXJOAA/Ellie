﻿using UnityEngine;

namespace Data.GoogleSheet
{
    public abstract class DataParsingInfo : ScriptableObject
    {
        [Header("Google SpreadSheets URL")] public string url;

        [Header("Google SpreadSheets Parsing Range")]
        public string startCell;

        public string endColumn;

        [Tooltip("Google SpreadSheets Parsing TSV file")] [HideInInspector]
        public string tsv;

        public string GetConvertedURL()
        {
            var docId = url.Split('/')[5];
            var gid = url.Split('=')[1];
            return
                $"https://docs.google.com/spreadsheets/d/{docId}/export?format=tsv&gid={gid}&range={startCell}:{endColumn}";
        }

        public abstract void Parse();
        public abstract T GetIndexData<T>(int index) where T : class;
    }
}