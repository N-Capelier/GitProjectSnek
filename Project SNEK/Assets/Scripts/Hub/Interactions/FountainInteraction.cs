using DialogueManagement;

namespace Hub.Interaction
{
    public class FountainInteraction : Interactor
    {
        protected override void Interact()
        {
            DialogueManager.Instance.OpenSkillTree();
        }
    }
}


