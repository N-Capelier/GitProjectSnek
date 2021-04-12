namespace Saving
{
    public class NPCManager : Singleton<NPCManager>
    {
        public BergamotHUB bergamot;
        public PoppyHUB poppy;
        public ThistleHUB thistle;

        public void RefreshNPCs()
        {
            bergamot.Refresh();
            poppy.Refresh();
            thistle.Refresh();
        }

    }
}