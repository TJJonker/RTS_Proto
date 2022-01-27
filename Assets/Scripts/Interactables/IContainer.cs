public class IContainer
{
    private int amountOfResources = 1000;

    public int CollectResources(int amount)
    {
        if (amountOfResources > amount)
        {
            amountOfResources -= amount;
            return amount;
        }
        else
        {
            DestroyContainer();
            return amountOfResources;
        }
    }

    private void DestroyContainer()
    {

    }

}
