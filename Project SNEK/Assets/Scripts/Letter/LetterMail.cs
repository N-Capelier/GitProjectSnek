﻿using UnityEngine;

namespace LetterMailManagement
{
    [CreateAssetMenu(fileName = "New LetterMail", menuName = "LetterMail", order = 50)]
    public class LetterMail : ScriptableObject
    {
        public string title;
        [TextArea(3, 10)] public string text;
    }
}