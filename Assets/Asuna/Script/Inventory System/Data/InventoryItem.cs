[System.Serializable]
public class InventoryItem
{
    public ItemData Data { get; }
    public int Amount { get; private set; }

    public ItemTypes ItemType => Data.ItemType;

    public InventoryItem(ItemData data, int amount)
    {
        Data = data;
        Amount = amount;
    }

    public void Add(int amount)
    {
        Amount += amount;
    }

    public bool Remove(int amount)
    {
        if (amount <= 0 || Amount < amount)
            return false;

        Amount -= amount;
        return true;
    }
}