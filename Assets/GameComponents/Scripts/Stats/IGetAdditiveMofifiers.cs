using System.Collections;
using System.Collections.Generic;


namespace RPG.Stats
{
    interface IGetAdditiveMofifiers
    {
        IEnumerable<float> GetAdditiveModifier(Stat stat);
    }
}
