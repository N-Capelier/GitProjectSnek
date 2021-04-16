namespace Saving
{
    public class SaveState
    {
        public float bergamotState = 1f;
        public float poppyState = 1f;
        public float thistleState = 1f;

        public int heartCoinAmount = 6;

        public int bonusHealth = 0;
        public int bonusRange = 0;
        public int bonusPower = 0;

        /// <summary>
        /// 0 is null,
        /// 1 is Swift,
        /// 2 is Beam,
        /// 3 is Shield
        /// </summary>
        public int equipedTechnic = 0;


        public bool isDemoFinished = false;
    }
}