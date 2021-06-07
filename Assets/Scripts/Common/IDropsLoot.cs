namespace CV
{

    internal interface IDropsLoot
    {
        void Drop(object loot);
        void SetLootValue(int value);
        void SetLootQuality(float value);
    }
}
