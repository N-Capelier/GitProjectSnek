namespace Saving
{
    public class SaveState
    {
        public int bergamotState = 0;
        public int poppyState = 0;
        public int thistleState = 0;

        public int heartCoinAmount = 0;

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
    }
}