namespace Saving
{
    public class SaveState
    {
        public float bergamotState = 1f;
        public float poppyState = 1f;
        public float thistleState = 1f;

        public bool talkedOnceToPoppy = false;
        public bool talkedOnceToThistle = false;

        public int heartCoinAmount = 6;

        public int bonusHealth = 0;
        public int bonusRange = 0;
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

        public float soundVolume = 0.9f;
        public float musicVolume = 0.9f;

        /// <summary>
        /// 0 is low quality
        /// 1 is medium quality
        /// 2 is high quality
        /// </summary>
        public int quality = 1;


        public bool isDemoFinished = false;
    }
}