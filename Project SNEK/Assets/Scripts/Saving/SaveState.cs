using System.Collections.Generic;

namespace Saving
{
    public class SaveState
    {
        public float bergamotState = 1f;
        public float poppyState = 1f;
        public float thistleState = 1f;

        public bool talkedOnceToPoppy = false;
        public bool talkedOnceToThistle = false;

        public int heartCoinAmount = 0;
        public int spentHeartCoinAmount = 0;
        public int powerLevel = 0;

        public int bonusHealth = 0;
        public float bonusRange = 1;
        public int bonusPower = 0;

        /// <summary>
        /// 0 is never picked
        /// 1 is picked in a level
        /// 2 is trade with a NPC
        /// </summary>
        public int secretObject_1 = 0;
        public int secretObject_2 = 0;
        public int secretObject_3 = 0;

        /// <summary>
        /// 0 is null,
        /// 1 is Swift,
        /// 2 is Beam,
        /// 3 is Shield
        /// </summary>
        public int equipedTechnic = 0;

        public int unlockedLetters = 1;
        public List<int> readLetters = new List<int>();

        public int canvasCurrentState;

        public float soundVolume = 0.9f;
        public float musicVolume = 0.9f;

        /// <summary>
        /// 0 is low quality
        /// 1 is medium quality
        /// 2 is high quality
        /// </summary>
        public int quality = 1;


        public bool isDemoFinished = false;
        public int bossAnorexiaHp = 3;
        public int bossParanoiaHp = 3;
        public int bossDepressionHp = 3;

        /// <summary>
        /// 0 = none
        /// 1 = Level_1.1
        /// 2 = Level_1.2
        /// 3 = Level_1.3
        /// 4 = Anorexia
        /// ...
        /// </summary>
        public int unlockedLevels = 0;

        /// <summary>
        /// 0 = Left
        /// 1 = Center
        /// 2 = Right
        /// </summary>
        public int uiAccessibility = 0;

        public bool isTutorialFinished = false;

        /// <summary>
        /// 0 : Déplacement dans le hub
        /// 1 : Interaction avec les PNJ
        /// 2 : Boite aux lettres
        /// 3 : La fontaine
        /// </summary>
        public int tutorialState = 0;

        public bool useDarkBergamot = false;
        public bool useDarkPoppy = false;
        public bool useDarkThistle = false;
    }
}