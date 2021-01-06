namespace Assets.Scripts.Items
{
    /// <summary>
    /// Item Grade - used to determine quality of item
    /// * Base attributes are set for SS type grade
    /// * E grade would only get 10% of base attributes
    /// </summary>
    public enum ItemGrade
    {
        SS = 6,
        S = 5,
        A = 4,
        B = 3,
        C = 2,
        D = 1,
        E = 0
    }
}