using System.Collections.Generic;

namespace RPG.Stats
{
    interface IGetPercentageModifiers
    {
        IEnumerable<float> GetPercentageModifier(Stat stat);
    }
}